using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float health;
    [SerializeField]
    private float damage;
    [SerializeField]
    private int experience;
    [SerializeField]
    private float speed;
    [SerializeField]
    private CharacterController controller;
    [SerializeField]
    private float gravity;
    [SerializeField]
    private int experiencePerObject;
    private int level;
    private int experienceRequired;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        movement = Vector3.ClampMagnitude(movement, 1f);
        if (movement != Vector3.zero)
        {
            transform.forward = movement;
            transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation(transform.forward), Time.deltaTime * 40f);
            transform.rotation = Quaternion.Euler(-90, transform.eulerAngles.y, transform.eulerAngles.z);

        }

        movement.y = gravity;
        controller.Move(speed * Time.deltaTime * movement);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Experience")){
            experience += experiencePerObject;
            if (experience >= experienceRequired)
            {
                LevelUp();
            }
        }
    }

    private void LevelUp()
    {
        experience -= experienceRequired;
        level++;
        experienceRequired = experiencePerObject * 10 * level;
        if (experience >= experienceRequired)
        {
            LevelUp();
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            //gameover logic goes here
        }
    }
}
