//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParent : PlayerAbstract
{
    public delegate void PlayerAmmo(int ammo, int maxAmmo, bool isShooting);
    public static PlayerAmmo playerAmmoDelegate;

    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private List<Weapon> listWeapon = new List<Weapon>();

    public Transform SpawnPool { get; private set; }
    private Vector3 direction;
    private int currentWeapon;

    protected override void Awake()
    {
        base.Awake();
        this.SpawnPool = GameObject.Find("SpawnPool").transform;    
    }
    
    private void Start()
    {
        foreach (Transform child in transform)
        {
            Weapon weapon = child.GetComponent<Weapon>();
            if (weapon != null)
            {
                this.listWeapon.Add(weapon);
                weapon.SetActiveWeapon(false);
            }
        }

        if (this.listWeapon.Count > 0)
        {
            this.SetWeapon(0); //TODO: choose the weapon used at the last play
        }
    }

    private void Update()
    {
        this.FaceWeapon();

        if (this.listWeapon[currentWeapon].GetComponent<WeaponShooting>() != null)
        {
            if (this.listWeapon[currentWeapon].GetComponent<WeaponShooting>().IsReloading) return;
        }

        this.SwapWeapon();
    }

    private void FaceWeapon()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.direction = mousePos - (Vector2) transform.position;

        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, this.playerCtrl.PlayerMovement.IsFacingRight ? rotZ : rotZ - 180);
    }

    private void SwapWeapon()
    {
        //Use mouse scroll wheel to swap weapon
        if (Input.mouseScrollDelta.y > 0)
        {
            this.SetWeapon(this.currentWeapon - 1);
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            this.SetWeapon(this.currentWeapon + 1);
        }

        //use number button from 1-5 to swap weapon
        if (Input.GetKeyDown(KeyCode.Alpha1) && this.listWeapon.Count >= 1)
        {
            this.SetWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && this.listWeapon.Count >= 2)
        {
            this.SetWeapon(1);
        }        
        if (Input.GetKeyDown(KeyCode.Alpha3) && this.listWeapon.Count >= 3)
        {
            this.SetWeapon(2);
        }        
        if (Input.GetKeyDown(KeyCode.Alpha4) && this.listWeapon.Count >= 4)
        {
            this.SetWeapon(3);
        }       
        if (Input.GetKeyDown(KeyCode.Alpha5) && this.listWeapon.Count >= 5)
        {
            this.SetWeapon(4);
        }
    }

    private void SetWeapon(int indexWeapon)
    {
        //if (AudioManager.HasInstance)
        //{
        //    AudioManager.Instance.PlaySE(this.swapWeaponAudio);
        //}

        if (indexWeapon >= this.listWeapon.Count)
            indexWeapon = 0;
        else if (indexWeapon < 0)
            indexWeapon = this.listWeapon.Count - 1;

        this.listWeapon[currentWeapon].SetActiveWeapon(false);
        this.currentWeapon = indexWeapon;
        this.listWeapon[indexWeapon].SetActiveWeapon(true);

        if (UIManager.HasInstance)
        {
            if (this.listWeapon[currentWeapon].WeaponType == WeaponType.Shooting)
            {
                WeaponShooting weaponShooting = this.listWeapon[currentWeapon].GetComponent<WeaponShooting>();

                playerAmmoDelegate(weaponShooting.CurrentAmmo, weaponShooting.MaxAmmo, true);
                PlayerMagic.playerManaDelegate(this.playerCtrl.PlayerMagic.MaxMana, false);
            }

            else if (this.listWeapon[currentWeapon].WeaponType == WeaponType.Magic)
            {
                playerAmmoDelegate(1, 1, false);
                PlayerMagic.playerManaDelegate(this.playerCtrl.PlayerMagic.Mana, true);
            }

            else
            {
                playerAmmoDelegate(1, 1, false);
                PlayerMagic.playerManaDelegate(this.playerCtrl.PlayerMagic.MaxMana, false);
            }

            UIManager.Instance.GamePanel.FirstMove.ResetFillMove();
            UIManager.Instance.GamePanel.SecondMove.ResetFillMove();
        }
       
    }


    public void SpawnDamageText(int damage, Collider2D collision, Transform parent, bool isCritical)
    {
        Vector3 position = new Vector3(collision.transform.position.x + Random.Range(-0.5f, 0.5f), collision.transform.position.y + 1.4f, collision.transform.position.z);
        GameObject damageTextObject = Instantiate(this.damageTextPrefab, position, Quaternion.identity, parent);

        damageTextObject.GetComponentInChildren<TextMesh>().text = "-" + damage.ToString();
        damageTextObject.GetComponentInChildren<TextMesh>().color = isCritical ? Color.red : Color.white;
        damageTextObject.GetComponentInChildren<TextMesh>().fontSize = isCritical ? 75 : 50;

        //Debug.Log(damage, damageTextObject.gameObject);
        StartCoroutine(DestroyDamageText(damageTextObject));
    }

    protected virtual IEnumerator DestroyDamageText(GameObject damageTextObject)
    {
        yield return new WaitForSeconds(1f);
        Destroy(damageTextObject);
    }
}
