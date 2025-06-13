using UnityEngine;

public class AttackArea : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float knockbackForce = 0.2f;

    private void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponent<IDamageable>();
        if (target == null) return;

        Vector3 knDir = (other.transform.position - transform.root.position).normalized;
        DebugManager.Log($"player hits {other.name} for {damage} dmg");
        target.TakeDamage(damage, knDir, knockbackForce);
    }
}