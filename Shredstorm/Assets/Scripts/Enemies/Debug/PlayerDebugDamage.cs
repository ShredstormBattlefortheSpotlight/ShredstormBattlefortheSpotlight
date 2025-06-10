using UnityEngine;

public class PlayerDebugDamage : MonoBehaviour, IDamageable
{
    private Renderer playerRenderer;
    private Color originalColor;

    void Start()
    {
        playerRenderer = GetComponentInChildren<Renderer>();

        if (playerRenderer != null)
        {
            // own copy of the material
            playerRenderer.material = new Material(playerRenderer.material);
            originalColor = playerRenderer.material.color;
        }
    }

    public void TakeDamage(int amount, Vector3 knockbackDir, float knockbackForce)
    {
        Debug.Log($"player took {amount} damage!");

        if (playerRenderer != null)
        {
            playerRenderer.material.color = Color.red;
            Invoke(nameof(ResetColor), 0.2f); 
        }
    }

    private void ResetColor()
    {
        if (playerRenderer != null)
            playerRenderer.material.color = originalColor;
    }
}