using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Transform player;
    private EnemyStats stats;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        stats = GetComponent<EnemyStats>();
    }

    void Update()
    {
        if (player == null || stats == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (!stats.isRanged)
        {
            // melee tries to get close
            if (distance > 1f) // not overlapping
            {
                MoveTowardPlayer();
            }
        }
        else
        {
            // ranged stays back unless too far
            if (distance > stats.attackRange)
            {
                MoveTowardPlayer();
            }
            // else hold position (ranged attacking from distance)
        }
    }

    void MoveTowardPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;

        // move forward
        transform.position += direction * stats.moveSpeed * Time.deltaTime;

        // rotate to face player
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(-90f, lookRotation.eulerAngles.y, 0f);
        }
    }

}