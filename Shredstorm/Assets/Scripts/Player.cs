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
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maximumHealth;
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

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.gameObject.CompareTag("Experience"))
    //     {
    //         experience += experiencePerObject;
    //         CheckLevelUp();
    //     }
    // }

    public void AddExperience(int xp)
    {
        experience += xp;
        DebugManager.Log($"player gained {xp} xp (total {experience})");
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
    public void TakeDamage(int amount, Vector3 knockbackDir, float knockbackForce)
    {
        currentHealth -= amount;
        
        controller.Move(knockbackDir * knockbackForce);
        
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            // TODO: game over logic
        }
    }
}
