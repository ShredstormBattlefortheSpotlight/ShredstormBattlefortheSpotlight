using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private float attackDelay;
    [SerializeField]
    private GameObject attackArea;
    [SerializeField]
    private float attackLength;
    [SerializeField]
    private int mouseButton;
    private float attackTimer = 0;
    private float delayTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        attackArea.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                delayTimer = attackDelay;
                attackArea.SetActive(false);
            }
        }
        else if (delayTimer > 0)
        {
            delayTimer -= Time.deltaTime;
        }
        if (Input.GetMouseButton(mouseButton) && (attackTimer <= 0 && delayTimer <= 0)){
            attackArea.SetActive(true);
            attackTimer = attackLength;
        }
    }
}