using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            Vector3 knockbackDir = (other.transform.position - transform.position).normalized;
            damageable.TakeDamage(1, knockbackDir, 0.2f);
            Debug.Log("Player hit enemy!");
        }
    }
}