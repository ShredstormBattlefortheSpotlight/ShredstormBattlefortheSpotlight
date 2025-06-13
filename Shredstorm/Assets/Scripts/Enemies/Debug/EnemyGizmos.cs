using UnityEngine;

public class EnemyGizmos : MonoBehaviour
{
    private EnemyStats stats;

    void OnDrawGizmosSelected()
    {
        if (stats == null) stats = GetComponent<EnemyStats>();
        if (stats == null) return;

        // agro range (when they spot you)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stats.agroRange);

        // stop distance (how close melee stops)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stats.stopDistance);

        // attack range (when they swing)
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, stats.attackRange);

        // wander radius (idle roam)
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, stats.wanderRadius);
    }
}