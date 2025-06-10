using UnityEngine;

public class EnemyDebugDamage : MonoBehaviour
{
    private IDamageable damageable;
    private bool isDead = false;

    void Start()
    {
        damageable = GetComponent<IDamageable>();
    }

    void Update()
    {
        // press spacebar to deal damage to this enemy
        if (!isDead && Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 knockbackDir = new Vector3(-1f, 0, -1f).normalized; // fake knockback direction
            float knockbackForce = 0.2f; // small nudge

            damageable?.TakeDamage(1, knockbackDir, knockbackForce);
            Debug.Log("enemy took 1 damage with knockback!");
        }

        // check if the object was destroyed (null)
        if (!isDead && damageable == null)
        {
            isDead = true;
            Debug.Log("enemy died!");
        }
    }
}