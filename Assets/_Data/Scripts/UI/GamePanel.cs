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
    [SerializeField] private Sprite magic1;
    [SerializeField] private Sprite magic2;
    [SerializeField] private WeaponImageHandler firstMove;
    [SerializeField] private WeaponImageHandler secondMove;

    [Header("MOBILE PHONE CTRL")]
    [SerializeField] private Joystick movementJoystick;
    public bool IsJoystick;

    public BlurringSliderFill HealthSlider => this.healthSlider;
    public Sprite Melee1 => this.melee1;
    public Sprite Melee2 => this.melee2;
    public Sprite Shooting => this.shooting;
    public Sprite Magic1 => this.magic1;
    public Sprite Magic2 => this.magic2;
    public WeaponImageHandler FirstMove => this.firstMove;
    public WeaponImageHandler SecondMove => this.secondMove;
    public Joystick MovementJoystick => this.movementJoystick;

    private int maxMana;

    private void Awake()
    {
        if (GameManager.HasInstance)
        {
            this.maxMana = GameManager.Instance.MaxMana;
        }
    }

    private void Start()
    {
        if (!this.IsJoystick)
            this.movementJoystick = null;
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

    private void OnPlayerMana(int value, bool isMagic)
    {
        if (isMagic)
        {
            this.manaSlider.SetActiveSlider(value, this.maxMana, true);
        }
        else
        {
            this.manaSlider.SetActiveSlider(1, 1, false);
        }
    }

    private void Update()
    {

    }

    public void OnClickedPauseButton()
    {
        if (GameManager.HasInstance && UIManager.HasInstance)
        {
            GameManager.Instance.PauseGame();
            UIManager.Instance.ActivePausePanel(true);
        }
    }
}
