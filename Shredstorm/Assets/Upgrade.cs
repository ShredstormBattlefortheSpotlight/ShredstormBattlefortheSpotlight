using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private float healthUpgradeAmount;
    [SerializeField]
    private float speedUpgradeAmount;
    [SerializeField]
    private float damageUpgradeAmount;
    // Start is called before the first frame update
    public void FinishUpgrade()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        player.CheckLevelUp();
    }
    public void UpgradeHealth()
    {
        player.UpgradeHealth(healthUpgradeAmount);
        FinishUpgrade();
    }
    public void UpgradeSpeed()
    {
        player.UpgradeSpeed(speedUpgradeAmount);
        FinishUpgrade();
    }
    public void UpgradeDamage()
    {
        player.UpgradeDamage(damageUpgradeAmount);
        FinishUpgrade();
    }
}
