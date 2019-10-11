using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    public static GameMaster gm;

    private int maxLife = 3;

    [SerializeField]
    string GameOver = "GameOver";

    public AudioManager audioManager;

    private static int _playerLife = 3;
    public static int PlayerLife
    {
        get { return _playerLife; }
    }

    [SerializeField]
    private int startingMoney = 100;
    public static int Money;
    
    void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
        audio = GetComponent<AudioSource>();
    }

    public Transform playerPrefab;
    public Transform spawnPoint;
    public Transform spawnPrefab;
    public int spawnDelay = 2;
    private AudioSource audio;

    public CameraShake camShake;

    [SerializeField]
    private GameObject gameOverUI;

    [SerializeField]
    private GameObject upgradeMenu;

    [SerializeField]
    private WaveSpawner wave;

    public IEnumerator RespawnPlayer()
    {
        audioManager.PlaySound("PlayerRespawnTime");
        yield return new WaitForSeconds(spawnDelay);

        audioManager.PlaySound("RespawnPlayer");
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        Transform clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation) as Transform;
        Destroy(clone.gameObject, 3f);
    }

    public delegate void UpgradeMenuCallBack(bool active);
    public UpgradeMenuCallBack onToggleUpgradeMenu;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            ToggleUpgradeMenu();
        }
    }

    private void ToggleUpgradeMenu()
    {
        upgradeMenu.SetActive(!upgradeMenu.activeSelf);
        wave.enabled = !upgradeMenu.activeSelf;
        onToggleUpgradeMenu.Invoke(upgradeMenu.activeSelf);
    }

    void Start()
    {
        if (camShake == null)
        {
            Debug.LogError("no reference is found");
        }

        _playerLife = maxLife;
        Money = startingMoney;

        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("more than one audio source");
        }
    }

    public void EndGame()
    {
        audioManager.PlaySound(GameOver);
        // Debug.Log("Game Over");
        gameOverUI.SetActive(true);

    }

    public static void KillPlayer(Player player)
    {
        Destroy(player.gameObject);

        _playerLife -= 1;
        if (_playerLife <= 0)
        {
            gm.EndGame();
        }
        else
        {
            gm.StartCoroutine(gm.RespawnPlayer());
        }
    }

    public static void KillEnemy (Enemy enemy)
    {
        gm._KillEnemy(enemy);
    }

    public void _KillEnemy(Enemy _enemy)
    {
        Transform _clone = Instantiate(_enemy.deathparticle, _enemy.transform.position, Quaternion.identity) as Transform;
        Destroy(_clone.gameObject, 1f);

        camShake.Shake(_enemy.shakeAmount, _enemy.shakeLength);
        Destroy(_enemy.gameObject);

        audioManager.PlaySound(_enemy.deathSoundName);

        Money += _enemy.moneyDrop;
        audioManager.PlaySound("Money");
    }
}
