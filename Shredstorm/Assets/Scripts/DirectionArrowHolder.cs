using System.Collections.Generic;
using UnityEngine;

public class BandmateArrow : MonoBehaviour
{
    public List<string> bandmateTags = new List<string> { "Drummer", "Singer" }; // Add more as needed
    public Transform player; // Drag Player here if needed

    private Transform target;

    void Update()
    {
        FindClosestBandmate();

        if (target == null) return;

        // rotate to face target
        Vector3 direction = target.position - transform.position;
        direction.y = 0f; // Keep arrow flat

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion lookRot = Quaternion.LookRotation(direction);
            transform.rotation = lookRot * Quaternion.Euler(90, 0, 0);  // Tilt upward for better visibility
        }

        // arrow to always hover above the player's head
        if (player != null)
        {
            transform.position = player.position + new Vector3(0, 2.5f, 0); // adjust Y height here
        }
    }

    void FindClosestBandmate()
    {
        float minDist = Mathf.Infinity;
        Transform closest = null;

        foreach (string tag in bandmateTags)
        {
            GameObject[] bandmates = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject bm in bandmates)
            {
                if (!bm.activeInHierarchy) continue; // skip collected/inactive
                float dist = Vector3.Distance(transform.position, bm.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = bm.transform;
                }
            }
        }

        target = closest;
    }
}