using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("identity")]
    public string enemyName = "default enemy";

    [Header("movement")]
    public float moveSpeed = 2f;

    [Header("health")]
    public int maxHealth = 5;

    [Header("combat")]
    public int damage = 1;
    public float attackRange = 2f;
    public float attackCooldown = 1.25f;
    public float attackSpeed = 1.0f;

    public bool isRanged = false;

    [Header("drops")]
    public int xpDropAmount = 10;

    [Header("behavior modifiers")]
    [Range(0f, 1f)]
    public float knockbackResistance = 0f; // 0 = full knockback, 1 = immune

    [Header("optional")]
    public AudioClip deathSound;
    public ParticleSystem deathEffect;
}