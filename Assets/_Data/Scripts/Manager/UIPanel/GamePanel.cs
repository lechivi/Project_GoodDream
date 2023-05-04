using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using EasyMobileInput;

public class GamePanel : MonoBehaviour
{
    [Header("TOP_LEFT")]
    [SerializeField] private BlurringSliderFill healthSlider;
    [SerializeField] private BlurringSliderFill ammoSlider;
    [SerializeField] private BlurringSliderFill manaSlider;

    [Header("BOTTOM_MIDDLE]")]
    [SerializeField] private Sprite melee1;
    [SerializeField] private Sprite melee2;
    [SerializeField] private Sprite shooting;
    [SerializeField] private Sprite shootingReload;
    [SerializeField] private Sprite magic1;
    [SerializeField] private Sprite magic2;
    [SerializeField] private WeaponImageHandler firstMove;
    [SerializeField] private WeaponImageHandler secondMove;

    [Header("MOBILE PHONE CTRL")]
    public bool IsMobile;
    [SerializeField] private Joystick movementJoystick;
    [SerializeField] private UnityEngine.UI.Button buttonA;
    [SerializeField] private UnityEngine.UI.Button buttonB;
    [SerializeField] private GameObject swapZone;

    public BlurringSliderFill HealthSlider => this.healthSlider;
    public BlurringSliderFill ManaSlider => this.manaSlider;
    public Sprite Melee1 => this.melee1;
    public Sprite Melee2 => this.melee2;
    public Sprite Shooting => this.shooting;
    public Sprite ShootingReload => this.shootingReload;
    public Sprite Magic1 => this.magic1;
    public Sprite Magic2 => this.magic2;
    public WeaponImageHandler FirstMove => this.firstMove;
    public WeaponImageHandler SecondMove => this.secondMove;
    public Joystick MovementJoystick => this.movementJoystick;
    public UnityEngine.UI.Button ButtonA => this.buttonA;
    public UnityEngine.UI.Button ButtonB => this.buttonB;

    private void Awake()
    {
        if (!this.IsMobile)
        {
            this.movementJoystick.gameObject.SetActive(false);
            this.buttonA.gameObject.SetActive(false);
            this.buttonB.gameObject.SetActive(false);
            this.swapZone.gameObject.SetActive(false);
        }
    }

    public virtual void InitializeWeapon()
    {
        if (PlayerManager.HasInstance)
        {
            if (PlayerManager.Instance.ListWeaponObj != null)
            {
                Weapon weapon = PlayerManager.Instance.ListWeaponObj[PlayerManager.Instance.CurrentWeapon].GetComponent<Weapon>();
                if (weapon == null) return;

                this.buttonA.onClick.AddListener(weapon.OnClickedFirstMoveButton);
                this.buttonB.onClick.AddListener(weapon.OnClickedSecondMoveButton);
                Debug.Log("asasdasdas");
            }
        }
    }

    private void OnEnable() //or use Start()
    {
        PlayerLife.playerHealthDelegate += OnPlayerHealth;
        WeaponParent.playerAmmoDelegate += OnPlayerAmmo;
        PlayerMagic.playerManaDelegate += OnPlayerMana;
    }

    private void OnDisable() //or use OnDestroy()
    {
        PlayerLife.playerHealthDelegate -= OnPlayerHealth;
        WeaponParent.playerAmmoDelegate -= OnPlayerAmmo;
        PlayerMagic.playerManaDelegate -= OnPlayerMana;
    }

    private void OnPlayerHealth(int value, int maxValue)
    {
        this.healthSlider.SetActiveSlider(value, maxValue, true);
    }

    private void OnPlayerAmmo(int value, int maxValue, bool isShooting)
    {
        if (isShooting)
        {
            this.ammoSlider.SetActiveSlider(value, maxValue, true);
        }
        else
        {
            this.ammoSlider.SetActiveSlider(1, 1, false);
        }
    }

    private void OnPlayerMana(int value, int maxValue)
    {
        this.manaSlider.SetActiveSlider(value, maxValue, true);
    }

    public void OnClickedPauseButton()
    {
        if (GameManager.HasInstance && UIManager.HasInstance)
        {
            GameManager.Instance.PauseGame();
            UIManager.Instance.PausePanel.FullAllTab();
            UIManager.Instance.ActivePausePanel(true);
        }
    }
}
