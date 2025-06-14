using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcanimate : MonoBehaviour
{
    [SerializeField]
    private Animation anims;
    [SerializeField]
    private float idleTime;
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0)
        {
            if (gameObject.CompareTag("Drummer"))
            {
                anims.Play("rig|GoggleIdle");
            }
            if (gameObject.CompareTag("Singer"))
            {
                anims.Play("VigorIdle.001");
            }
            timer = idleTime;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
}
