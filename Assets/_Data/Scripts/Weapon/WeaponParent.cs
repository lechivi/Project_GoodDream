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

    public bool Test;
    private Vector3 direction;
    private int currentWeapon;

    protected override void Awake()
    {
        base.Awake();
        this.SpawnPool = GameObject.Find("SpawnPool").transform;

        if (this.Test)
        {
            this.listWeapon.Clear();
            foreach (Transform child in transform)
            {
                Weapon weapon = child.GetComponent<Weapon>();
                if (weapon != null)
                {
                    this.listWeapon.Add(weapon);
                    weapon.gameObject.SetActive(true);
                    weapon.SetActiveWeapon(false);
                }
            }
        }
        else
        {
            if (PlayerManager.HasInstance)
            {
                foreach (GameObject weaponOnList in PlayerManager.Instance.ListWeaponObj)
                {
                    GameObject weaponObj = Instantiate(weaponOnList, transform);
                    Weapon weapon = weaponObj.GetComponent<Weapon>();
                    if (weapon != null)
                    {
                        this.listWeapon.Add(weapon);
                        weapon.gameObject.SetActive(true);
                        weapon.SetActiveWeapon(false);
                    }
                }
            }
        }
        
    }

    private void Start()
    {
        if (this.listWeapon.Count > 0)
        {
            if (PlayerManager.HasInstance)
            {
                this.SetWeapon(PlayerManager.Instance.CurrentWeapon);
            }
            else
            {
                this.SetWeapon(0);
            }
        }
    }

    private void Update()
    {
        if (this.playerCtrl.PlayerLife.Health <= 0) return;

        this.FaceWeapon();
        this.SwapWeapon();
    }

    private void FaceWeapon()
    {
        //Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //this.direction = mousePos - (Vector2)transform.position;

        //float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0f, 0f, this.playerCtrl.PlayerMovement.IsFacingRight ? rotZ : rotZ - 180);
        if (UIManager.HasInstance && UIManager.Instance.GamePanel.IsMobile)
        {
            this.direction = (Vector2)this.playerCtrl.MovementJoystick.CurrentProcessedValue;
            float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, this.playerCtrl.PlayerMovement.IsFacingRight ? rotZ : rotZ - 180);
        }
        else
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            this.direction = mousePos - (Vector2)transform.position;

            float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, this.playerCtrl.PlayerMovement.IsFacingRight ? rotZ : rotZ - 180);
        }
    }

    private void SwapWeapon()
    {
        //Use mouse scroll wheel to swap weapon (just for test)
        if (this.Test)
        {
            if (Input.mouseScrollDelta.y > 0)
            {
                this.SetWeapon(this.currentWeapon - 1);
            }
            if (Input.mouseScrollDelta.y < 0)
            {
                this.SetWeapon(this.currentWeapon + 1);
            }
        }

        if (PlayerManager.HasInstance)
        {
            if (this.listWeapon[currentWeapon].GetComponent<WeaponShooting>() != null)
            {
                if (this.listWeapon[currentWeapon].GetComponent<WeaponShooting>().IsReloading) return;
            }
        }

        if (PlayerManager.HasInstance)
        {
            List<int> hotkeys = PlayerManager.Instance.Hotkeys;

            //Use number button from 1-5 to swap weapon
            if (hotkeys.Count == 0) return;
            if (Input.GetKeyDown(KeyCode.Alpha1) && this.listWeapon.Count >= 1 && hotkeys[0] != -1)
            {
                this.SetWeapon(hotkeys[0]);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && this.listWeapon.Count >= 2 && hotkeys[1] != -1)
            {
                this.SetWeapon(hotkeys[1]);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && this.listWeapon.Count >= 3 && hotkeys[2] != -1)
            {
                this.SetWeapon(hotkeys[2]);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4) && this.listWeapon.Count >= 4 && hotkeys[3] != -1)
            {
                this.SetWeapon(hotkeys[3]);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5) && this.listWeapon.Count >= 5 && hotkeys[4] != -1)
            {
                this.SetWeapon(hotkeys[4]);
            }

            //Use Tab to swap weapon
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                List<int> existHotkeys = new List<int>();
                foreach (int key in hotkeys)
                {
                    if (key != -1)
                    {
                        existHotkeys.Add(key);
                    }
                }
                if (existHotkeys.Count == 0) return;

                if (existHotkeys.Contains(this.currentWeapon))
                {
                    if (existHotkeys.Count == 1) return;

                    int currentHotkey = existHotkeys.IndexOf(this.currentWeapon);
                    if (currentHotkey == existHotkeys.Count - 1)
                    {
                        this.SetWeapon(existHotkeys[0]);
                    }
                    else
                    {
                        this.SetWeapon(existHotkeys[currentHotkey + 1]);
                    }
                }
                else
                {
                    this.SetWeapon(existHotkeys[0]);
                }
            }
        }


    }

    public void SetWeapon(int indexWeapon)
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_EQUIP);
        }

        if (indexWeapon >= this.listWeapon.Count)
            indexWeapon = 0;
        else if (indexWeapon < 0)
            indexWeapon = this.listWeapon.Count - 1;

        this.listWeapon[currentWeapon].SetActiveWeapon(false);
        this.currentWeapon = indexWeapon;
        this.listWeapon[indexWeapon].SetActiveWeapon(true);

        if (PlayerManager.HasInstance)
        {
            PlayerManager.Instance.CurrentWeapon = this.currentWeapon;
        }

        if (UIManager.HasInstance)
        {
            if (UIManager.Instance.GamePanel.IsMobile)
            {
                UIManager.Instance.GamePanel.InitializeWeapon();
            }

            if (this.listWeapon[currentWeapon].WeaponType == WeaponType.Shooting)
            {
                WeaponShooting weaponShooting = this.listWeapon[currentWeapon].GetComponent<WeaponShooting>();
                playerAmmoDelegate(weaponShooting.CurrentAmmo, weaponShooting.MaxAmmo, true);

            }
            else
            {
                playerAmmoDelegate(1, 1, false);
            }

            //PlayerMagic.playerManaDelegate(this.playerCtrl.PlayerMagic.Mana, this.playerCtrl.PlayerMagic.MaxMana);
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

    }

}
