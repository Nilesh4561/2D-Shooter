using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySampleAssets._2D;

[RequireComponent(typeof(Platformer2DUserControl))]
public class Player : MonoBehaviour {
  
    public int fallBoundry = -20;

    [SerializeField]
    public string playerDead = "PlayerDead";

    [SerializeField]
    public string playerHit = "PlayerHit";

    AudioManager audioManager;

    private PlayerStats stats;

    [SerializeField]
    private StatusIndicator sI;

    void Start()
    {
        stats = PlayerStats.instance;
        stats.currentHealth = stats.maxHealth;

        if (sI == null)
        {
            Debug.LogError("No Status indicator on the player");
        }
        else
        {
            sI.SetHealth(stats.currentHealth, stats.maxHealth);
        }
        GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;

        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("no audioManager");
        }

        InvokeRepeating("RegenHealth", 1f/stats.healthRegenRate, 1f/stats.healthRegenRate);
    }

    void RegenHealth()
    {
        stats.currentHealth += 1;
        sI.SetHealth(stats.currentHealth, stats.maxHealth);
    }

    void Update()
    {
        if (transform.position.y <= fallBoundry)
        {
            DamagePlayer(9999);
        }
    }


    void OnUpgradeMenuToggle(bool active)
    {
        Weapon _weapon = GetComponentInChildren<Weapon>();
        if (_weapon != null)
            _weapon.enabled = !active;

        WeaponSwitching _weaponSwitching = GetComponentInChildren<WeaponSwitching>();
        if (_weaponSwitching != null)
            _weaponSwitching.enabled = !active;
    }

    public void DamagePlayer(int damage)
    {
        stats.currentHealth -= damage;
        if (stats.currentHealth <= 0)
        {
            audioManager.PlaySound(playerDead);

            GameMaster.KillPlayer(this);
        }
        else
        {
            audioManager.PlaySound(playerHit);
        }

        sI.SetHealth(stats.currentHealth, stats.maxHealth);
    }
}
