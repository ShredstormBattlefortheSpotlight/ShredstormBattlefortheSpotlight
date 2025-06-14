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
    private int attackPower;
    [SerializeField]
    private int mouseButton;
    [SerializeField]
    private Player player;
    [SerializeField]
    private Animation anims;
    private float attackTimer = 0;
    private float delayTimer = 0;
    [SerializeField]
    private int attackSwitch;
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
                player.SetState(1);
            }
        }
        else if (delayTimer > 0)
        {
            delayTimer -= Time.deltaTime;
        }
        if (Input.GetMouseButton(mouseButton) && (attackTimer <= 0 && delayTimer <= 0)){
            attackArea.SetActive(true);
            switch (attackSwitch)
            {
                case 0:
                    anims.Play("rig|PlayerAttack");
                    break;
                case 1:
                    anims.Play("rig|PlayerAttack2");
                    break;
                default:
                    Debug.Log("attackswitch invalid value");
                    break;
            }
            attackTimer = attackLength;
            player.SetState(3);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
       
    }
    
}