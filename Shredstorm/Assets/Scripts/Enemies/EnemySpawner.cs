using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Wave
{
    public string    name;           // for easy debug in waves list
    public GameObject enemyPrefab;  
    public int       count;          // how many to spawn this wave
    public float     spawnInterval;  // seconds between each spawn
}

/*
 * EnemySpawner.cs
 *  – runs through a list of Waves
 *  – for each wave, spawns `count` enemies at `spawnInterval`
 *  – can spawn at fixed spawnPoints OR randomly around the Player
 *  – automatically parents spawned enemies under this GameObject
 */

public class EnemySpawner : MonoBehaviour
{
    [Header("Wave Configuration")]
    public List<Wave> waves;
    public bool       prePool          = true;   // instantiate all at Start (disabled) then reuse
    [Header("Spawn Options")]
    public bool       useRandomRadius  = true;   // if true, ignore spawnPoints and pick random around player
    public float      minSpawnRadius   = 20f;    // if random, minimum distance from player
    public float      maxSpawnRadius   = 50f;    // if random, maximum distance from player
    [Header("Fixed Spawn Points")]
    public Transform[] spawnPoints;              // optional: drag empty Transforms here

    private Dictionary<GameObject, Queue<GameObject>> pools 
        = new Dictionary<GameObject, Queue<GameObject>>();
    private Transform player;                     // cache player transform

    void Start()
    {
        // find the Player by tag
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
        else Debug.LogError("EnemySpawner: no GameObject tagged 'Player'!");

        if (prePool)
            PrePoolAllWaves();

        StartCoroutine(RunAllWaves());
    }

    private void PrePoolAllWaves()
    {
        foreach (var w in waves)
        {
            if (w.enemyPrefab == null || w.count <= 0) continue;
            var q = new Queue<GameObject>();
            for (int i = 0; i < w.count; i++)
            {
                // parent to this spawner so hierarchy stays clean
                var go = Instantiate(w.enemyPrefab, Vector3.zero, Quaternion.identity, transform);
                go.SetActive(false);
                q.Enqueue(go);
            }
            pools[w.enemyPrefab] = q;
        }
    }

    private IEnumerator RunAllWaves()
    {
        for (int i = 0; i < waves.Count; i++)
        {
            var wave = waves[i];
            DebugManager.Log($"--- Starting wave {i+1}: {wave.name} ({wave.count} enemies) ---");

            for (int j = 0; j < wave.count; j++)
            {
                SpawnOne(wave.enemyPrefab);
                yield return new WaitForSeconds(wave.spawnInterval);
            }

            // wait for all of this prefab’s active instances to be gone
            yield return new WaitUntil(() =>
            {
                foreach (var es in FindObjectsOfType<EnemyStats>())
                {
                    if (es.gameObject.activeInHierarchy && es.gameObject.CompareTag(wave.enemyPrefab.tag))
                        return false;
                }
                return true;
            });
        }

        DebugManager.Log("All waves complete!");
    }

    private void SpawnOne(GameObject prefab)
    {
        GameObject go;

        // pop from pool if available
        if (prePool && pools.TryGetValue(prefab, out var q) && q.Count > 0)
        {
            go = q.Dequeue();
            go.SetActive(true);
        }
        else
        {
            // instantiate and parent under this spawner
            go = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
            if (prePool)
                pools[prefab].Enqueue(go);
        }

        // choose spawn position
        Vector3 spawnPos;
        if (useRandomRadius && player != null)
        {
            // pick a random direction on the XZ plane
            Vector2 circle = Random.insideUnitCircle.normalized;
            float radius = Random.Range(minSpawnRadius, maxSpawnRadius);
            Vector3 offset = new Vector3(circle.x, 0, circle.y) * radius;
            spawnPos = player.position + offset;
        }
        else if (spawnPoints != null && spawnPoints.Length > 0)
        {
            // pick one of the fixed points
            var sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
            spawnPos = sp.position;
        }
        else
        {
            // fallback: spawn at spawner’s position
            spawnPos = transform.position;
        }

        // apply position  (keep prefab’s own Y if needed)
        go.transform.position = new Vector3(spawnPos.x, spawnPos.y, spawnPos.z);
        go.transform.rotation = Quaternion.identity;
    }

    /// <summary>
    /// Call this from EnemyHealth.Die() instead of Destroy to reuse in pool.
    /// </summary>
    public void ReturnToPool(GameObject go)
    {
        Debug.Log("Returning to Pool");
        go.SetActive(false);
        if (prePool && pools.TryGetValue(go, out var q))
            q.Enqueue(go);
        else
            Destroy(go);
    }
}
