using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("identity")]
    public string enemyName = "default enemy"; // name shown in UI or logs

    [Header("movement")]
    public float moveSpeed = 2f;                      // how fast they chase
    public Vector2 moveSpeedRange = new(1f, 3f);      // NEW: random speed range
    public float agroRange = 5f;                      // player spotted within this distance
    public float stopDistance = 3f;                   // how close before stopping to attack
    public float wanderRadius = 2f;                   // roam radius when idle
    public bool alwaysAlert = false;                  // if true, ignore agroRange and always chase

    [Header("health")]
    public int maxHealth = 5;                         // starting hp

    [Header("combat")]
    public int damage = 1;                            // how much hp they remove from player
    public Vector2 damageRange = new(1, 5);           // NEW: random damage range
    public float attackRange = 2f;                    // must be within this to swing
    public float attackCooldown = 1.25f;              // time between swings
    public Vector2 cooldownRange = new(0.8f, 2f);     // NEW: random cooldown range
    public float attackWindup = 0.5f;                 // delay before damage is applied
    public bool isRanged = false;                     // toggle ranged vs melee behavior
    public float knockbackResistance = 0f;            // 0 = full push, 1 = immune

    [Header("drops")]
    public int xpDrop = 10;                           // xp given when they die
    public Vector2Int xpRange = new(5, 25);           // NEW: random XP range
    public GameObject xpPickupPrefab;                 // XP‐pickup prefab here

    [Header("optional vfx/sfx")]
    public ParticleSystem hitEffect;                  // plays on takedamage()
    public AudioClip hitSound;                        // plays on takedamage()
    public ParticleSystem deathEffect;                // plays on die()
    public AudioClip deathSound;                      // plays on die()

    [Header("animation")]
    public Animator animator;                         // drag your Animator here
    public string walkTrigger = "Walk";               // trigger to play walk anim
    public string hitTrigger = "Hit";                 // trigger on takedamage()
    public string attackTrigger = "Attack";           // trigger when attacking
    public string deathTrigger = "Die";               // trigger on death

    [Header("ui")]
    public GameObject healthBarPrefab;                // optional healthbar over head

    // Called at spawn to randomize values
    public void RandomizeStats()
    {
        moveSpeed = Random.Range(moveSpeedRange.x, moveSpeedRange.y);
        damage = Mathf.RoundToInt(Random.Range(damageRange.x, damageRange.y));
        attackCooldown = Random.Range(cooldownRange.x, cooldownRange.y);
        xpDrop = Random.Range(xpRange.x, xpRange.y);
    }

    // Called from spawner to apply scaling over time
    public void ScaleStats(float multiplier)
    {
        maxHealth = Mathf.RoundToInt(maxHealth * multiplier);
        damage = Mathf.RoundToInt(damage * multiplier);
        moveSpeed *= multiplier;
    }
}
