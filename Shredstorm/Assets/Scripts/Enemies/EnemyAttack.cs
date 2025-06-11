using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private EnemyStats stats;
    private Transform player;
    private float lastAttackTime = 0f;

    void Start()
    {
        stats = GetComponent<EnemyStats>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null || stats == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // close enough + cooldown ready
        if (distanceToPlayer <= stats.attackRange && Time.time >= lastAttackTime + stats.attackCooldown)
        {
            lastAttackTime = Time.time;
            PerformAttack();
        }
    }

    // enemy starts attack
    private void PerformAttack()
    {
        Debug.Log("enemy started attack!");

        // damage happens after a delay (attack speed)
        Invoke(nameof(ApplyDamage), stats.attackSpeed);
    }

    // does damage
    private void ApplyDamage()
    {
        if (player == null) return;

        IDamageable dmg = player.GetComponent<IDamageable>();
        if (dmg != null)
        {
            Vector3 knockDir = (player.position - transform.position).normalized;
            dmg.TakeDamage(stats.damage, knockDir, 0.2f); // does damage + knockback
            Debug.Log($"enemy dealt {stats.damage} damage to player. cooldown = {stats.attackCooldown}s");
        }
    }
}