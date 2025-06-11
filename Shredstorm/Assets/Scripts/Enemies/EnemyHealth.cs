using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    private int currentHealth;
    private EnemyStats stats;

    private Renderer enemyRenderer;
    private Color originalColor;
    private Coroutine flashCoroutine;

    private Rigidbody rb;

    // start with max health
    void Start()
    {
        stats = GetComponent<EnemyStats>();
        currentHealth = stats.maxHealth;

        rb = GetComponent<Rigidbody>();

        // cache visual data
        enemyRenderer = GetComponentInChildren<Renderer>();
        if (enemyRenderer != null)
        {
            originalColor = enemyRenderer.material.color;
        }
    }

    // take damage from attacks
    public void TakeDamage(int amount, Vector3 knockbackDir, float knockbackForce)
    {
        currentHealth -= amount;

        // apply knockback using rigidbody force
        if (knockbackForce > 0f && rb != null)
        {
            float adjustedForce = knockbackForce * (1f - stats.knockbackResistance);
            rb.AddForce(knockbackDir * adjustedForce, ForceMode.Impulse);
        }

        // flash + shake
        if (enemyRenderer != null)
        {
            if (flashCoroutine != null)
                StopCoroutine(flashCoroutine);

            flashCoroutine = StartCoroutine(DamageFlash());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // grayed out and shake when hit
    private IEnumerator DamageFlash()
    {
        enemyRenderer.material.color = Color.gray;

        Vector3 originalPos = transform.position;
        Vector3 shakeOffset = new Vector3(0.05f, 0, 0.05f);
        transform.position = originalPos + shakeOffset;

        yield return new WaitForSeconds(0.1f);

        enemyRenderer.material.color = originalColor;
        transform.position = originalPos;
    }

    // destroy enemy on death
    private void Die()
    {
        // if (stats.deathEffect != null)
        //     Instantiate(stats.deathEffect, transform.position, Quaternion.identity);

        // if (stats.deathSound != null)
        //     AudioSource.PlayClipAtPoint(stats.deathSound, transform.position);

        // later: xp drop here

        Destroy(gameObject);
    }
}
