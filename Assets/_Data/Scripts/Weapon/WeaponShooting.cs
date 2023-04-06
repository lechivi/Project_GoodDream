using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShooting : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;

    [Header("BULLET SETTING")]
    [SerializeField] private Transform WeaponHolder;
    [SerializeField] private Transform ShootPoint;
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
        this.weaponCtrl = transform.parent.GetComponent<WeaponCtrl>();
        this.spawnPool = GameObject.Find("SpawnPool").transform;
        this.currentAmmo = this.maxAmmo;
    }

    private void FixedUpdate()
    {
        //if (GameManager.HasInstance)
        //{
        //    if (!GameManager.Instance.IsPlaying) return;
        //}

        this.FaceGun();

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

                    //this.weaponCtrl.SetSliderValue();
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

        //if (AudioManager.HasInstance)
        //{
        //    AudioManager.Instance.PlaySE(this.reloadAudio);
        //}

        Invoke("InvokeReload", this.reloadTime);
    }

    private void InvokeReload()
    {
        this.currentAmmo = this.maxAmmo;
        //this.weaponCtrl.SetSliderValue();
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
        //    if (AudioManager.HasInstance)
        //    {
        //        AudioManager.Instance.PlaySE(this.fireAudio);
        //    }

        this.GetBullet();

        //GetComponentInChildren<Animator>().SetTrigger("Recoil");
    }

    private GameObject GetBullet()
    {
        int direction = 1 * (this.playerMovement.IsFacingRight ? 1 : -1);

        //for (int i = 0; i < this.bulletClone.Count; i++)
        //{

        //    if (!this.bulletClone[i].activeInHierarchy)
        //    {
        //        this.bulletClone[i].name = "BulletClone_" + i;
        //        this.bulletClone[i].SetActive(true);
        //        this.bulletClone[i].transform.position = this.ShootPoint.position;
        //        this.bulletClone[i].transform.rotation = this.WeaponHolder.rotation;
        //        this.bulletClone[i].transform.parent = this.spawnPool;

        //        Rigidbody2D rbClone = bulletClone[i].GetComponent<Rigidbody2D>();
        //        rbClone.bodyType = RigidbodyType2D.Dynamic;
        //        rbClone.AddForce(direction * this.bulletClone[i].transform.right * this.bulletSpeed, ForceMode2D.Impulse);

        //        return this.bulletClone[i];
        //    }
        //}
        GameObject obj = Instantiate(this.bulletPrefab, this.ShootPoint.position, this.WeaponHolder.rotation, this.spawnPool);
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
