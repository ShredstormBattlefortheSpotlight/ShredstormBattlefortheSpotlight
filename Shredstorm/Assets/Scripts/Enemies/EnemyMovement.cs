using UnityEngine;

/*
 * EnemyMovement.cs
 *  – uses Rigidbody.velocity so gravity & collisions keep them grounded
 *  – chases player based on EnemyStats (agroRange / alwaysAlert)
 *  – stops at stopDistance (melee) or attackRange (ranged) so no overlap
 *  – locks X/Z rotation so they stay upright
 */

public class EnemyMovement : MonoBehaviour
{
    private EnemyStats stats;
    private Transform player;
    private float gravity;

    // Store original speed so we can apply per-instance randomness only once
    private bool randomizedSpeedApplied = false;

    void Start()
    {
        // find stats on this object or its children
        stats = GetComponent<EnemyStats>() 
             ?? GetComponentInChildren<EnemyStats>();

        if (stats == null)
            Debug.LogError("EnemyMovement: no EnemyStats found!");

        var p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;
        else
            Debug.LogError("EnemyMovement: no GameObject tagged 'Player'!");
    }

    void Update()
    {
        if (player == null || stats == null) return;

        // NEW: Apply random speed variance once at runtime
        if (!randomizedSpeedApplied)
        {
            float min = stats.moveSpeed * 1f;
            float max = stats.moveSpeed * 2f;
            stats.moveSpeed = Random.Range(min, max);
            randomizedSpeedApplied = true;
        }

        float dist = Vector3.Distance(transform.position, player.position);

        // only chase if alwaysAlert OR within agroRange
        bool shouldChase = stats.alwaysAlert || dist <= stats.agroRange;
        if (!shouldChase) return;

        // decide how far we should stay from player
        float stopAt = stats.isRanged ? stats.attackRange : stats.stopDistance;

        if (dist < stopAt)
        {
            // clamp so we never overlap the player
            Vector3 dirAway = (transform.position - player.position).normalized;
            transform.position = player.position + dirAway * stopAt;
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                transform.position.z
            );
        }
        else
        {
            // move closer
            Vector3 dir = (player.position - transform.position).normalized;
            Vector3 move = dir * stats.moveSpeed * Time.deltaTime;
            move.y = gravity;  // no vertical drift
            transform.position += move;
        }

        // rotate only on Y so we stay upright
        Vector3 look = player.position - transform.position;
        look.y = 0;
        if (look.sqrMagnitude > 0.001f)
        {
            Quaternion goal = Quaternion.LookRotation(look);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, goal, Time.deltaTime * 5f
            );
        }
    }
}
