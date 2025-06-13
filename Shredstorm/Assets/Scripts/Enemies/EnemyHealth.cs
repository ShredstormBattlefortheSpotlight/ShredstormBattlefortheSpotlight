using System.Collections;
using UnityEngine;

/*
 * EnemyHealth.cs
 *  – handles all damage, hit feedback, knockback, death VFX/SFX
 *  – player’s AttackArea will call TakeDamage()
 *  – uses rigidbody impulse for knockback, then resets to kinematic
 */
public class EnemyHealth : MonoBehaviour, IDamageable
{
    private int currentHealth;
    private EnemyStats stats;

    private Renderer rend;
    private Color origColor;
    private Coroutine flashRoutine;

    private Rigidbody rb;

    void Start()
    {
        stats = GetComponent<EnemyStats>();
        currentHealth = stats.maxHealth;

        // get or add rigidbody for knockback only
        rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;             // no falling
        rb.isKinematic = true;             // movement by script, not physics
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        // cache renderer for hit-flash
        rend = GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            // make a unique material instance so we can tint it
            rend.material = new Material(rend.material);
            origColor = rend.material.color;
        }
    }

    // called by player AttackArea.OnTriggerEnter(...)
    public void TakeDamage(int amount, Vector3 knockDir, float knockForce)
    {
        currentHealth -= amount;

        // play hit VFX/SFX
        if (stats.hitEffect) Instantiate(stats.hitEffect, transform.position, Quaternion.identity);
        if (stats.hitSound) AudioSource.PlayClipAtPoint(stats.hitSound, transform.position);

        // trigger hit animation if set
        if (stats.animator) stats.animator.SetTrigger(stats.hitTrigger);

        // apply physics knockback
        if (knockForce > 0f && rb != null)
        {
            float force = knockForce * (1f - stats.knockbackResistance);
            rb.isKinematic = false;
            rb.AddForce(knockDir * force, ForceMode.Impulse);
            StartCoroutine(ResetKinematic());
        }

        // flash gray
        if (rend != null)
        {
            if (flashRoutine != null) StopCoroutine(flashRoutine);
            flashRoutine = StartCoroutine(FlashHit());
        }

        // die if out of HP
        if (currentHealth <= 0)
            Die();
    }

    // re-enable kinematic so movement code works again
    private IEnumerator ResetKinematic()
    {
        yield return new WaitForSeconds(0.1f);
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
    }

    // quick gray flash
    private IEnumerator FlashHit()
    {
        rend.material.color = Color.gray;
        yield return new WaitForSeconds(0.1f);
        rend.material.color = origColor;
    }

    private void Die()
    {
        // play death VFX/SFX
        if (stats.deathEffect) Instantiate(stats.deathEffect, transform.position, Quaternion.identity);
        if (stats.deathSound) AudioSource.PlayClipAtPoint(stats.deathSound, transform.position);

        // trigger death anim if available
        if (stats.animator) stats.animator.SetTrigger(stats.deathTrigger);

        // spawn XP pickup
        if (stats.xpPickupPrefab != null)
        {
            var xpObj = Instantiate(
                stats.xpPickupPrefab,
                transform.position,
                Quaternion.identity
            );
            // set the amount on the pickup
            var xp = xpObj.GetComponent<XP_Pickup>();
            if (xp != null) xp.amount = stats.xpDrop;
        }

        var spawner = FindObjectOfType<EnemySpawner>();
        if (spawner != null)
            spawner.ReturnToPool(gameObject);
        else
            Destroy(gameObject);
        
    }
}
