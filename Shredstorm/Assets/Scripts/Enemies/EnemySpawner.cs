using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * EnemySpawner.cs
 *  – spawns enemies at random intervals near the player
 *  – optionally scales enemy difficulty over time
 *  – spawns boss after timer runs out
 *  – can spawn at fixed spawnPoints OR randomly around the Player
 *  – automatically parents spawned enemies under this GameObject
 */

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Options")]
    public GameObject[] enemyPrefabs;           // array of enemy types to randomly spawn
    public float spawnInterval = 5f;            // how often to spawn
    public float spawnVariance = 1f;            // random +/- variance per spawn

    public bool useRandomRadius = true;         // if true, ignore spawnPoints and pick random around player
    public float minSpawnRadius = 20f;          // minimum distance from player
    public float maxSpawnRadius = 50f;          // maximum distance from player

    [Header("Fixed Spawn Points")]
    public Transform[] spawnPoints;             // optional: drag empty Transforms here

    [Header("Scaling Settings")]
    public float difficultyInterval = 15f;      // how often to scale enemies (in seconds)
    public float scaleMultiplier = 1.1f;        // how much stronger per scale tick

    [Header("Boss Settings")]
    public GameObject bossPrefab;               // assign boss prefab
    public float gameDuration = 300f;           // seconds until boss spawns

    private float elapsedTime = 0f;             // time since start
    private float nextDifficultyTime = 0f;      // tracks scaling interval
    private bool bossSpawned = false;

    private float currentScale = 1f;            // current enemy scaling multiplier
    private Transform player;

    void Start()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
        else Debug.LogError("EnemySpawner: no GameObject tagged 'Player'!");

        StartCoroutine(SpawnEnemiesOverTime());
    }

    private IEnumerator SpawnEnemiesOverTime()
    {
        while (elapsedTime < gameDuration)
        {
            elapsedTime += Time.deltaTime;

            // scale difficulty if time
            if (elapsedTime >= nextDifficultyTime)
            {
                currentScale *= scaleMultiplier;
                nextDifficultyTime += difficultyInterval;
                Debug.Log($"[EnemySpawner] Difficulty scaled to {currentScale:F2}");
            }

            // spawn one enemy
            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            SpawnOne(prefab);

            float nextDelay = spawnInterval + Random.Range(-spawnVariance, spawnVariance);
            nextDelay = Mathf.Max(1f, nextDelay); // minimum delay
            yield return new WaitForSeconds(nextDelay);
        }

        // Boss Spawn after timer ends
        if (!bossSpawned && bossPrefab != null && player != null)
        {
            bossSpawned = true;

            Vector3 bossSpawnPos = player.position + player.forward * 10f;

            if (Physics.Raycast(bossSpawnPos + Vector3.up * 10f, Vector3.down, out RaycastHit hit, 30f))
            {
                bossSpawnPos.y = hit.point.y + 1.5f;
            }

            Instantiate(bossPrefab, bossSpawnPos, Quaternion.identity);
            Debug.Log("Boss spawned!");
        }
    }

    private void SpawnOne(GameObject prefab)
    {
        GameObject go = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);

        var stats = go.GetComponent<EnemyStats>();
        if (stats != null)
        {
            stats.RandomizeStats();
            stats.ScaleStats(currentScale); // apply difficulty scaling
        }

        Vector3 spawnPos;
        if (useRandomRadius && player != null)
        {
            Vector2 circle = Random.insideUnitCircle.normalized;
            float radius = Random.Range(minSpawnRadius, maxSpawnRadius);
            Vector3 offset = new Vector3(circle.x, 0, circle.y) * radius;
            spawnPos = player.position + offset;
        }
        else if (spawnPoints != null && spawnPoints.Length > 0)
        {
            var sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
            spawnPos = sp.position;
        }
        else
        {
            spawnPos = transform.position;
        }

        if (Physics.Raycast(spawnPos + Vector3.up * 10f, Vector3.down, out RaycastHit hit, 30f))
        {
            float yOffset = 1.5f;
            spawnPos.y = hit.point.y + yOffset;
        }

        go.transform.position = spawnPos;
        go.transform.rotation = Quaternion.identity;
    }

    // Call this from EnemyHealth.Die() instead of Destroy to reuse in pool.
    public void ReturnToPool(GameObject go)
    {
        Destroy(go); // pooling not used yet
    }
}
