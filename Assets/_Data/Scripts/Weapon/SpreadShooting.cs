using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpreadShooting : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;

    [Header("BULLET SETTING")]
    [SerializeField] private Transform WeaponHolder;
    [SerializeField] private Transform[] ShootPoints;
    [SerializeField] private List<GameObject> bulletClone = new List<GameObject>();
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int maxAmmo;
    [SerializeField] private float reloadTime;
    [SerializeField] private float bulletSpeed = 25f;
    [SerializeField] private float fireRate = 4;

    [SerializeField] private AudioClip fireAudio;
    [SerializeField] private AudioClip reloadAudio;

    private WeaponCtrl weaponCtrl;
    private Transform spawnPool;
    private int currentAmmo;
    private float waitForNextShot;
    private bool isReloading;
    private Vector2 direction;

    private void Awake()
    {
        this.WeaponHolder = transform.parent;
        this.weaponCtrl = transform.parent.GetComponent<WeaponCtrl>();
        this.currentAmmo = this.maxAmmo;
        this.spawnPool = GameObject.Find("SpawnPool").transform;
    }

    private void FixedUpdate()
    {
        if (this.isReloading) return;

        if (Input.GetMouseButton(0))
        {
            if (Time.time > this.waitForNextShot)
            {
                this.waitForNextShot = Time.time + 1f / this.fireRate;

                if (this.currentAmmo > 0)
                {
                    this.currentAmmo--;
                    this.Shoot();
                }
                else
                {
                    this.Reload();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && this.currentAmmo < this.maxAmmo)
        {
            this.Reload();
        }
    }

    private void Reload()
    {
        this.isReloading = true;

        Invoke("InvokeReload", this.reloadTime);
    }

    private void InvokeReload()
    {
        this.currentAmmo = this.maxAmmo;
        this.isReloading = false;
    }

    private void FaceGun()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.direction = mousePos - (Vector2)this.WeaponHolder.position;
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        this.WeaponHolder.rotation = Quaternion.Euler(0f, 0f, this.playerMovement.IsFacingRight ? rotZ : rotZ - 180);
        //this.WeaponHolder.transform.right = this.direction;
    }

    private void Shoot()
    {
        foreach (Transform child in this.ShootPoints)
        {
            this.GetBullet(child);
        }

    }

    private GameObject GetBullet(Transform shootPoint)
    {
        int direction = 1 * (this.playerMovement.IsFacingRight ? 1 : -1);
        GameObject obj = Instantiate(this.bulletPrefab, shootPoint.position, shootPoint.rotation, this.spawnPool);

        this.bulletClone.Add(obj);

        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(direction * obj.transform.right * this.bulletSpeed, ForceMode2D.Impulse);

        return obj;
    }

    public int CurrentAmmo() => this.currentAmmo;
    public int MaxAmmo() => this.maxAmmo;
    public bool IsReloading() => this.isReloading;
}
