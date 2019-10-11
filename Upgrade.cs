using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour {

    [SerializeField]
    private Text health;

    [SerializeField]
    private Text speed;

    [SerializeField]
    private float healthMultiplier = 1.3f;

    [SerializeField]
    private int upgradeCost = 50;

    PlayerStats stats;

    void OnEnable()
    {
        stats = PlayerStats.instance;
        UpdateValues();
    }

    void UpdateValues()
    {
        health.text = "Health : " +stats.maxHealth.ToString();
        speed.text = "Speed : " +stats.movementSpeed.ToString();
    }

    public void UpgradeHealth()
    {
        if(GameMaster.Money < upgradeCost)
        {
            AudioManager.instance.PlaySound("NoMoney");
            return;
        }
        stats.maxHealth = (int)(stats.maxHealth * healthMultiplier);
        GameMaster.Money -= upgradeCost;
        AudioManager.instance.PlaySound("Money");
        UpdateValues();
    }

    public void SpeedUpgrade()
    {
        if (GameMaster.Money < upgradeCost)
        {
            AudioManager.instance.PlaySound("NoMoney");
            return;
        }
        stats.movementSpeed = Mathf.Round (stats.movementSpeed * 1.1f);
        GameMaster.Money -= upgradeCost;
        AudioManager.instance.PlaySound("Money");
        UpdateValues();
    }
}
