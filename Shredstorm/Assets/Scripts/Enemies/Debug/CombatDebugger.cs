using UnityEngine;

public class CombatDebugger : MonoBehaviour, IDamageable
{
    [SerializeField] private string actorName = "enemy"; // "player" or "enemy"
    [SerializeField] private int maxHealth = 5;
    private int currentHealth;

    private Renderer rend;
    private Color originalColor;

    void Start()
    {
        currentHealth = maxHealth;

        rend = GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            rend.material = new Material(rend.material); 
            originalColor = rend.material.color;
        }
    }

    public void TakeDamage(int amount, Vector3 knockDir, float knockbackForce)
    {
        currentHealth -= amount;

        if (actorName == "enemy")
        {
            Debug.Log($"player hit enemy! enemy took {amount} damage. enemy health = {currentHealth}");
        }
        else
        {
            Debug.Log($"enemy hit player! player took {amount} damage. player health = {currentHealth}");
        }

        // visual flash
        if (rend != null)
        {
            Color flashColor = actorName == "enemy" ? Color.gray : Color.red;
            rend.material.color = flashColor;
            Invoke(nameof(ResetColor), 0.2f);
        }

        if (currentHealth <= 0)
        {
            Debug.Log($"{actorName} has died!");
            Destroy(gameObject);
        }

        if (knockbackForce > 0)
        {
            Debug.Log($"{actorName} was knocked back by {knockbackForce}");
        }
    }

    private void ResetColor()
    {
        if (rend != null)
        {
            rend.material.color = originalColor;
        }
    }
}