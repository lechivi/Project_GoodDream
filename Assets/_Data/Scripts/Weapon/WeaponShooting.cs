using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShooting : Weapon
{
    [Header("SHOOTING SETTING")]
    [SerializeField] protected List<Transform> shootPoints = new List<Transform>();
    [SerializeField] protected List<GameObject> bulletClone = new List<GameObject>();
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected int maxAmmo;
    [SerializeField] protected float reloadTime;
    [SerializeField] protected float bulletSpeed = 25f;
    [SerializeField] protected float fireRate = 4;

    [SerializeField] protected AudioClip fireAudio;
    [SerializeField] protected AudioClip reloadAudio;

    protected int currentAmmo;
    protected float timerReload;
    protected float waitForNextShot;
    protected bool isReloading;
    protected Vector2 direction;

    public int CurrentAmmo => this.currentAmmo;
    public int MaxAmmo => this.maxAmmo;
    public bool IsReloading => this.isReloading;

    protected override void Awake()
    {
        base.Awake();
        this.WeaponType = WeaponType.Shooting;
        this.currentAmmo = this.maxAmmo;
        foreach (Transform child in transform)
        {
            if (child.gameObject.name == "ShootPoint")
                this.shootPoints.Add(child);
        }
    }

    protected override void Update()
    {
        if (GameManager.HasInstance)
        {
            if (!GameManager.Instance.IsPlaying) return;
        }

        if (this.IsUsing && gameObject.CompareTag("PlayerWeapon") && this.weaponParent.PlayerCtrl.PlayerLife.Health > 0) 
        {
            if (this.isReloading)
            {
                this.Reload();
                return;
            }

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
                        this.isReloading = true;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.R) && this.currentAmmo < this.maxAmmo)
            {
                this.isReloading = true;
            }

            if (UIManager.HasInstance)
            {
                GamePanel gamePanel = UIManager.Instance.GamePanel;
                gamePanel.SecondMove.gameObject.SetActive(false);
                gamePanel.FirstMove.Icon.sprite = gamePanel.Shooting;
            }
        }

       
    }

    private void Reload()
    {
        //if (AudioManager.HasInstance)
        //{
        //    AudioManager.Instance.PlaySE(this.reloadAudio);
        //}

        this.timerReload += Time.deltaTime;
        if (UIManager.HasInstance)
        {
            UIManager.Instance.GamePanel.FirstMove.StartFillMove(this.timerReload, this.reloadTime);
        }
        if (this.timerReload < this.reloadTime) return;

        this.timerReload = 0;
        this.currentAmmo = this.maxAmmo;
        this.isReloading = false;

        if (UIManager.HasInstance)
        {
            WeaponParent.playerAmmoDelegate(this.currentAmmo, this.maxAmmo, true);
        }
    }

    //private void InvokeReload()
    //{
    //    this.currentAmmo = this.maxAmmo;
    //    this.isReloading = false;

    //    if (UIManager.HasInstance)
    //    {
    //        WeaponParent.playerAmmoDelegate(this.currentAmmo, this.maxAmmo, true);
    //    }

    //}

    private void Shoot()
    {
        //if (AudioManager.HasInstance)
        //{
        //    AudioManager.Instance.PlaySE(this.fireAudio);
        //}

        foreach (Transform child in this.shootPoints)
        {
            this.GetBullet(child);
        }

        GetComponentInChildren<Animator>().SetTrigger("Recoil");
        if (UIManager.HasInstance)
        {
            WeaponParent.playerAmmoDelegate(this.currentAmmo, this.maxAmmo, true);
        }
    }

    private GameObject GetBullet(Transform shootPoint)
    {
        int direction = 1 * (this.weaponParent.PlayerCtrl.PlayerMovement.IsFacingRight ? 1 : -1);
        int damage = this.GetRandomDamage();
        //for (int i = 0; i < this.bulletClone.Count; i++)
        //{
        //    if (!this.bulletClone[i].activeInHierarchy)
        //    {
        //        this.bulletClone[i].name = "BulletClone_" + i;
        //        this.bulletClone[i].SetActive(true);
        //        this.bulletClone[i].transform.position = this.ShootPoint.position;
        //        this.bulletClone[i].transform.rotation = this.weaponHolder.rotation;
        //        this.bulletClone[i].transform.parent = this.spawnPool;
        //        Rigidbody2D rbClone = bulletClone[i].GetComponent<Rigidbody2D>();
        //        rbClone.bodyType = RigidbodyType2D.Dynamic;
        //        rbClone.AddForce(direction * this.bulletClone[i].transform.right * this.bulletSpeed, ForceMode2D.Impulse);
        //        return this.bulletClone[i];
        //    }
        //}
        GameObject bulletObj = Instantiate(this.bulletPrefab, shootPoint.position, shootPoint.transform.rotation, this.weaponParent.SpawnPool);
        bulletObj.tag = "PlayerWeapon";
        bulletObj.layer = LayerMask.NameToLayer("Player");

        BulletScript bulletScript = bulletObj.GetComponent<BulletScript>();
        bulletScript.WeaponParent = this.weaponParent;
        bulletScript.Damage = damage;
        bulletScript.IsCritical = damage == this.maxDamage;

        Rigidbody2D rb = bulletObj.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(direction * bulletObj.transform.right * this.bulletSpeed, ForceMode2D.Impulse);

        this.bulletClone.Add(bulletObj);

        return bulletObj;
    }

    public override void EnemyUseWeapon()
    {
        base.EnemyUseWeapon();

    }
}
