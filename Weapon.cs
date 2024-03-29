﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public float fireRate = 0f;
    public int Damage = 10;
    public LayerMask whatToHit;

    public Transform hitPrefab;

    public Transform bulletTrailPrefab;
    public Transform muzzleFlashPrefab;
    public float effectSpawnRate = 10;


    float timeToSpawnEffect = 0;
    float timeToFire = 0f;
    Transform firePoint;

    public float camShakeAmt = 0.01f;
    public float camShakeLength = 0.1f;

    CameraShake camShake;

    [SerializeField]
    string pistolSound = "Pistol";

    [SerializeField]
    string machineGunSound = "MachineGun";

    AudioManager audioManager;


    // Use this for initialization
    void Awake () {
        firePoint = transform.Find("Fire");
        if (firePoint == null)
        {
            Debug.LogWarning("Not a fire object present in the pistol");
        }
    }

    void Start()
    {
        camShake = GameMaster.gm.GetComponent<CameraShake>();

        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("no audioManager");
        }
    }

    // Update is called once per frame
    void Update () {
        if (fireRate == 0)    
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButton("Fire1") && Time.time > timeToFire)     
            {
                timeToFire = Time.time + 1 / fireRate;      
                Shoot();
            }
        }
    }

    void Shoot()
    {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);     
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);

        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition - firePointPosition, 100, whatToHit);

        Debug.DrawLine(firePointPosition, (mousePosition - firePointPosition) * 100, Color.cyan);

        if (hit.collider != null)
        {
            Debug.DrawLine(firePointPosition, hit.point, Color.red);
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.DamageEnemy(Damage);
                // Debug.Log("we hit" + hit.collider.name + "and did" + Damage + "damage");
            }
        }

        if (Time.time >= timeToSpawnEffect)
        {
            Vector3 hitPos;
            Vector3 hitNormal;

            if (hit.collider == null)
            {
                hitPos = (mousePosition - firePointPosition) * 30;
                hitNormal = new Vector3(999, 999, 999);
            }
            else
            {
                hitPos = hit.point;
                hitNormal = hit.normal;
            }

            Effect(hitPos, hitNormal);
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }
    }

    void Effect(Vector3 hitPos, Vector3 hitNormal)
    {
        Transform trail = Instantiate(bulletTrailPrefab, firePoint.position, firePoint.rotation) as Transform;
        LineRenderer lr = trail.GetComponent<LineRenderer>();

        if (lr != null)
        {
            lr.SetPosition(0, firePoint.position);
            lr.SetPosition(1, hitPos);
        }
        Destroy(trail.gameObject, 0.02f);

        if (hitNormal != new Vector3(999, 999, 999))
        {
            Transform hParticle = Instantiate(hitPrefab, hitPos, Quaternion.FromToRotation(Vector3.right, hitNormal)) as Transform;
            Destroy(hParticle.gameObject, 0.5f);
        }

        Transform clone = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation) as Transform;
        clone.parent = firePoint;
        float size = Random.Range(0.6f, 0.9f);
        clone.localScale = new Vector3(size, size, size);
        Destroy(clone.gameObject, 0.02f);

        camShake.Shake(camShakeAmt, camShakeLength);

        audioManager.PlaySound(pistolSound);
        audioManager.PlaySound(machineGunSound);
    }
}
