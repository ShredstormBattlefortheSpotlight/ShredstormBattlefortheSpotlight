using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAttackHit : MonoBehaviour
{
    [SerializeField]
    private int attackPower;
    private bool hasDamaged = false;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && !hasDamaged)
        {
            hasDamaged = true;
            other.gameObject.GetComponent<IDamageable>().TakeDamage(attackPower, Vector3.zero, 0);
            
        }
    }

    private void OnEnable()
    {
        hasDamaged = false;
    }
}
