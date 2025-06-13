using UnityEngine;

[RequireComponent(typeof(Collider))]
public class XP_Pickup : MonoBehaviour
{
    [Tooltip("How much XP this gives")]
    public int amount = 10;

    void Start()
    {
        // ensure this collider is a trigger
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // only pick up by the Player
        if (!other.CompareTag("Player")) return;

        // find the Player script
        var player = other.GetComponent<Player>();
        if (player != null)
        {
            player.AddExperience(amount);
        }

        Destroy(gameObject);
    }
}