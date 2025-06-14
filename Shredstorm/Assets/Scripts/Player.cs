using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float maximumHealth;
    [SerializeField]
    private float currentHealth;
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
    [SerializeField]
    private GameObject levelUpUI;
    [SerializeField]
    private int level = 1;
    [SerializeField]
    private int experienceRequired = 100;
    [SerializeField]
    private GameObject drummerAbility = null;
    [SerializeField]
    private GameObject singerAbility;
    [SerializeField]
    private Animation walkAnimation;
    [SerializeField]
    private GameObject spawnPoint;
    
    public float CurrentHealth => currentHealth;
    public float MaximumHealth => maximumHealth;
    public float Damage => damage;
    public int Experience => experience;
    public int ExperienceRequired => experienceRequired;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maximumHealth;
        transform.position = spawnPoint.transform.position;
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
            //walkAnimation.Play("root|PlayerRun");

        }

        movement.y = gravity;
        controller.Move(speed * Time.deltaTime * movement);

        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        if (Physics.Raycast(transform.position, fwd, 20))
        {
            print("There is something in front of the object!");
            Debug.DrawRay(transform.position, transform.forward, Color.green);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Drummer")){
            Debug.Log("Contact with drummer");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.gameObject.CompareTag("Experience"))
        {
            experience += experiencePerObject;
            CheckLevelUp();
        }*/
        if (other.gameObject.CompareTag("Drummer"))
        {
            Debug.Log("Contact with drummer");
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Singer"))
        {
            Debug.Log("Contact with singer");
            singerAbility.SetActive(true);
            Destroy(other.gameObject);
        }
    }

    public void AddExperience(int amount)
    {
        experience += amount;
        CheckLevelUp();
    }

    public void CheckLevelUp()
    {
        if (experience >= experienceRequired)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        experience -= experienceRequired;
        level++;
        experienceRequired += level * 100;
        levelUpUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void UpgradeHealth(float upgrade)
    {
        maximumHealth += upgrade;
        currentHealth += upgrade;
    }

    public void UpgradeSpeed(float upgrade)
    {
        speed += upgrade;
    }

    public void UpgradeDamage(float upgrade)
    {
        damage += upgrade;
    }
    public void TakeDamage(int amount, Vector3 knockDir, float knockForce)
    {
        currentHealth -= amount;

        // apply physics knockback

        // flash gray
        
    }
}
