using System.Collections;
using UnityEngine;

/*
 * EnemyAttack.cs
 *  - checks distance to player each frame
 *  - only attacks when off cooldown
 *  - supports melee vs ranged (projectile stub)
 *  - triggers anim/sfx, then after windup deals damage + knockback
 *  - needs: EnemyStats, Animator (optional), hitEffect/hitSound (optional)
 */

public class EnemyAttack : MonoBehaviour
{
    private EnemyStats stats;
    private Transform player;
    private float lastAttackTime = 0f;
    private bool isAttacking = false;

    void Start()
    {
        stats = GetComponent<EnemyStats>();
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (player == null || stats == null || isAttacking) return;

        float dist = Vector3.Distance(transform.position, player.position);
        

        // decide attack range: melee stops at stopDistance, ranged uses attackRange
        //float range = stats.isRanged ? stats.attackRange : stats.stopDistance;
        float range = stats.attackRange;
        float cdRemaining = (lastAttackTime + stats.attackCooldown) - Time.time;


        // DebugManager.Log(
        //     $"[{stats.enemyName}] dist={dist:F1}, range={range}, cdRem={Mathf.Max(cdRemaining, 0f):F1}"
        // );

        // close enough + cooldown ready?
        //Debug.Log("Player in range? " + (dist <= range));
        if (dist <= range && Time.time >= lastAttackTime + stats.attackCooldown)
        {
            lastAttackTime = Time.time;
            StartCoroutine(PerformAttack());
        }

    }

    // windup -> damage
    private IEnumerator PerformAttack()
    {
        isAttacking = true;

        // trigger attack animation if assigned
        if (stats.animator != null)
            stats.animator.SetTrigger(stats.attackTrigger);

        // optional attack sound
        if (stats.hitSound != null)
            AudioSource.PlayClipAtPoint(stats.hitSound, transform.position, 0.8f);

        // wait for attackWindup
        yield return new WaitForSeconds(stats.attackWindup);

        // melee: directly hurt player; ranged: stub for projectile
        if (!stats.isRanged)
        {
            HitPlayer();
        }
        else
        {
            // TODO: instantiate projectile prefab here
            // e.g. Instantiate(stats.projectilePrefab, muzzle.position, muzzle.rotation);
            HitPlayer(); // fallback in case no projectile
        }

        isAttacking = false;
    }

    // does the actual damage + knockback
    private void HitPlayer()
    {
        if (player == null) return;

        var dmg = player.GetComponent<IDamageable>();
        if (dmg != null)
        {
            Vector3 knockDir = (player.position - transform.position).normalized;
            dmg.TakeDamage(stats.damage, knockDir, 0.2f);

            DebugManager.Log($"enemy dealt {stats.damage} dmg. cooldown={stats.attackCooldown}s");
        }
        else
        {
            DebugManager.Log($"[{stats.enemyName}] no IDamageable on player!");
        }
    }
}
