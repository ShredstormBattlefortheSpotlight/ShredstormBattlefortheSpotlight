using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Transform spawnPoint;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        if (player != null && spawnPoint != null)
        {
            player.transform.position = spawnPoint.position;
            player.transform.rotation = spawnPoint.rotation;
        }
        else
        {
            Debug.LogError("PlayerSpawner: Missing player reference or spawn point!");
        }
    }
}