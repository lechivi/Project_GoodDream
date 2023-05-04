using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponShooting : Weapon
{
    [Header("SHOOTING SETTING")]
    [SerializeField] protected List<Transform> shootPoints = new List<Transform>();
    [SerializeField] protected List<GameObject> bulletClone = new List<GameObject>();
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected int maxAmmo;
    [SerializeField] protected float reloadTime;
    [SerializeField] protected float bulletSpeed = 25f;
    [SerializeField] protected float bulletLifeTime = 2f;
    [SerializeField] protected float fireRate = 4;

    [SerializeField] protected AudioClip fireAudio;
    [SerializeField] protected AudioClip reloadAudio;

    protected int currentAmmo;
    protected int direction;
    protected float timerReload;
    protected float originalReloadTime;
    protected float waitForNextShot;
    protected bool isReloading;

    public int CurrentAmmo => this.currentAmmo;
    public int MaxAmmo => this.maxAmmo;
    public int Direction { get => this.direction; set => this.direction = value; }
    public bool IsReloading => this.isReloading;

    protected override void Awake()
    {
        base.Awake();
        this.WeaponType = WeaponType.Shooting;
        this.currentAmmo = this.maxAmmo;
        this.originalReloadTime = this.reloadTime;

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
                if (UIManager.HasInstance)
                {
                    if (EventSystem.current.IsPointerOverGameObject()) return;
                }

                if (Time.time > this.waitForNextShot)
                {
                    this.waitForNextShot = Time.time + 1f / this.fireRate;

                    if (this.currentAmmo > 0)
                    {
                        this.currentAmmo--;
                        this.SetupShoot();
                    }
                    else
                    {
                        if (AudioManager.HasInstance && this.reloadAudio != null)
                        {
                            AudioManager.Instance.PlaySFX(this.reloadAudio);
                        }

                        this.isReloading = true;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.R) && this.currentAmmo < this.maxAmmo)
            {
                if (AudioManager.HasInstance && this.reloadAudio != null)
                {
                    AudioManager.Instance.PlaySFX(this.reloadAudio);
                }

                this.isReloading = true;
            }

            if (UIManager.HasInstance)
            {
                GamePanel gamePanel = UIManager.Instance.GamePanel;
                gamePanel.FirstMove.Icon.sprite = gamePanel.Shooting;
                gamePanel.SecondMove.Icon.sprite = gamePanel.ShootingReload;
            }
        }
    }

    protected void Reload1()
    {
        this.isReloading = true;

        if (AudioManager.HasInstance && this.reloadAudio != null)
        {
            AudioManager.Instance.PlaySFX(this.reloadAudio);
        }

        if (UIManager.HasInstance)
        {
            UIManager.Instance.GamePanel.SecondMove.StartFillMove(this.timerReload, this.reloadTime);
        }

        Invoke("InvokeReload", this.reloadTime);
    }

    protected void InvokeReload()
    {
        this.currentAmmo = this.maxAmmo;
        this.isReloading = false;

        if (UIManager.HasInstance)
        {
            WeaponParent.playerAmmoDelegate(this.currentAmmo, this.maxAmmo, true);
        }
    }

    protected void Reload()
    {
        if (PlayerManager.HasInstance)
        {
            this.reloadTime = this.originalReloadTime / (PlayerManager.Instance.MulReloadSpeed);
        }

        this.timerReload += Time.deltaTime;
        if (UIManager.HasInstance && this.IsUsing && gameObject.CompareTag("PlayerWeapon"))
        {
            UIManager.Instance.GamePanel.SecondMove.StartFillMove(this.timerReload, this.reloadTime);
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

    protected virtual void SetupShoot()
    {
        GetComponentInChildren<Animator>().SetTrigger("Recoil");
        this.Shoot();
    }

    public virtual void Shoot()
    {
        if (AudioManager.HasInstance && this.fireAudio != null)
        {
            AudioManager.Instance.PlaySFX(this.fireAudio);
        }

        foreach (Transform child in this.shootPoints)
        {
            this.GetBullet(child, true);
        }

        if (UIManager.HasInstance)
        {
            WeaponParent.playerAmmoDelegate(this.currentAmmo, this.maxAmmo, true);
        }
    }

    protected GameObject GetBullet(Transform shootPoint, bool isPlayer)
    {
        int damage = this.GetRandomDamage();

        if (isPlayer)
        {
            this.direction = 1 * (this.weaponParent.PlayerCtrl.PlayerMovement.IsFacingRight ? 1 : -1);
        }
        else
        {
            this.direction = transform.parent.parent.parent.localScale.x > 0 ? -1 : 1;
        }
        
        for (int i = 0; i < this.bulletClone.Count; i++)
        {
            if (!this.bulletClone[i].activeInHierarchy)
            {
                this.bulletClone[i].transform.position = shootPoint.position;
                this.bulletClone[i].transform.rotation = shootPoint.rotation;
                this.bulletClone[i].transform.parent = isPlayer ? this.weaponParent.SpawnPool : GameObject.Find("SpawnPool").transform;
                
                BulletScript bulletScriptClone = this.bulletClone[i].GetComponent<BulletScript>();
                bulletScriptClone.LifeTime = this.bulletLifeTime;
                bulletScriptClone.Damage = damage;
                bulletScriptClone.IsCritical = damage == this.maxDamage;

                Vector2 scale = new Vector2(Mathf.Abs(bulletScriptClone.transform.localScale.x), bulletScriptClone.transform.localScale.y);

                if (isPlayer)
                {
                    bulletScriptClone.WeaponParent = this.weaponParent;
                    bulletScriptClone.transform.localScale = this.weaponParent.PlayerCtrl.PlayerMovement.IsFacingRight ? new Vector2(-scale.x, scale.y) : scale;
                }

                this.bulletClone[i].SetActive(true);

                Rigidbody2D rbClone = bulletClone[i].GetComponent<Rigidbody2D>();
                rbClone.bodyType = RigidbodyType2D.Dynamic;
                rbClone.AddForce(this.direction * this.bulletClone[i].transform.right * this.bulletSpeed, ForceMode2D.Impulse);

                return this.bulletClone[i];
            }
        }

        GameObject obj = Instantiate(this.bulletPrefab, shootPoint.position, shootPoint.rotation, isPlayer ? this.weaponParent.SpawnPool : GameObject.Find("SpawnPool").transform);
        obj.tag = isPlayer ? "PlayerWeapon" : "EnemyWeapon";
        obj.layer = LayerMask.NameToLayer(isPlayer ? "Player" : "Enemy");

        BulletScript bullet = obj.GetComponent<BulletScript>();
        bullet.LifeTime = this.bulletLifeTime;
        bullet.Damage = damage;
        bullet.IsCritical = damage == this.maxDamage;
        
        if (isPlayer)
        {
            bullet.WeaponParent = this.weaponParent;
            bullet.transform.localScale = this.weaponParent.PlayerCtrl.PlayerMovement.IsFacingRight ? new Vector2(-bullet.transform.localScale.x, bullet.transform.localScale.y) : bullet.transform.localScale;
        }

        obj.SetActive(true);

        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(this.direction * obj.transform.right * this.bulletSpeed, ForceMode2D.Impulse);

        this.bulletClone.Add(obj);

        return obj;
    }

    public override void EnemyUseWeapon()
    {
        base.EnemyUseWeapon();
        foreach (Transform child in this.shootPoints)
        {
            this.GetBullet(child, false);

        }
    }
}
