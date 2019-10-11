 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAI))]
public class Enemy : MonoBehaviour {

    [System.Serializable]
    public class EnemyStats
    {
        public int maxHealth = 100;

        public int damage = 40;

        private int _currentHealth;
        public int currentHealth
        {
            get { return _currentHealth; }
            set { _currentHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public void Init()
        {
            currentHealth = maxHealth;
        }
    }

    public Transform deathparticle;

    public float shakeAmount = 0.1f;
    public float shakeLength = 0.1f;

    [SerializeField]
    public string deathSoundName = "Explosion";

    [Header("Optional")]
    [SerializeField]
    private StatusIndicator sI;

    public EnemyStats stats = new EnemyStats();

    public int moneyDrop = 50;

    void Start()
    {
        stats.Init();

        if (sI != null)
        {
            sI.SetHealth(stats.currentHealth, stats.maxHealth);
        }
        GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;
        if (deathparticle == null)
        {
            Debug.LogError("no particle found");
        }

    }

    void OnUpgradeMenuToggle(bool active)
    {
        EnemyAI _enemyAI = GetComponent<EnemyAI>();
        if (_enemyAI != null)
        {
            _enemyAI.enabled = !active;
        }
    }


    public void DamageEnemy(int damage)
    {
        stats.currentHealth -= damage;
        if (stats.currentHealth <= 0)
        {
            GameMaster.KillEnemy(this);
        }

        if (sI != null)
        {
            sI.SetHealth(stats.currentHealth, stats.maxHealth);
        }
    }

    void OnCollisionEnter2D(Collision2D _colInfo)
    {
        Player _player = _colInfo.collider.GetComponent<Player>();
        if (_player != null)
        {
            _player.DamagePlayer(stats.damage);
            DamageEnemy(99999);
        }
    }

    void OnDestroy()
    {
        GameMaster.gm.onToggleUpgradeMenu -= OnUpgradeMenuToggle;
    }
}
