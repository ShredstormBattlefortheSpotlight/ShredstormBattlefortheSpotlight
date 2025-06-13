using UnityEngine;

// anything that can take damage implements this
public interface IDamageable
{
    // amount = how much hp to lose
    // knockDir + force = pushback when hit
    void TakeDamage(int amount, Vector3 knockDir, float knockForce);
}