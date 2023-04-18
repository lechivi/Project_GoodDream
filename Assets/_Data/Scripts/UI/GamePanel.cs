using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    [Header("TOP_LEFT")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private TMP_Text manaText;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider ammoSlider;
    [SerializeField] private Slider manaSlider;

    [Header("BOTTOM_MIDDLE]")]
    [SerializeField] private Sprite melee1;
    [SerializeField] private Sprite melee2;
    [SerializeField] private Sprite shooting;
    [SerializeField] private Sprite magic1;
    [SerializeField] private Sprite magic2;
    [SerializeField] private FillCooldownMoveImage firstMove;
    [SerializeField] private FillCooldownMoveImage secondMove;

    public TMP_Text HealthText { get => this.healthText; set => this.healthText = value; }
    public TMP_Text AmmoText { get => this.ammoText; set => this.ammoText = value; }
    public TMP_Text ManaText { get => this.manaText; set => this.manaText = value; }
    public Sprite Melee1 => this.melee1;
    public Sprite Melee2 => this.melee2;
    public Sprite Shooting => this.shooting;
    public Sprite Magic1 => this.magic1;
    public Sprite Magic2 => this.magic2;
    public FillCooldownMoveImage FirstMove => this.firstMove;
    public FillCooldownMoveImage SecondMove => this.secondMove;

    private int maxMana;

    private void Awake()
    {
        if (GameManager.HasInstance)
        {
            this.maxMana = GameManager.Instance.MaxMana;
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
        this.healthSlider.maxValue = maxValue;
        this.healthSlider.value = value;
        this.healthText.SetText(value.ToString() + "/" + maxValue.ToString());
    }

    private void OnPlayerAmmo(int value, int maxValue, bool isShooting)
    {
        this.ammoSlider.maxValue = maxValue;
        this.ammoSlider.value = value;
        if (isShooting)
        {
            this.ammoText.alpha = 1;
            this.ammoText.SetText(value.ToString() + "/" + maxValue.ToString());
        }
        else
        {
            this.ammoText.alpha = 0;
        }
    }

    private void OnPlayerMana(int value, bool isMagic)
    {
        this.manaSlider.value = value;
        if (isMagic)
        {
            this.manaText.alpha = 1;
            this.manaText.SetText(value.ToString() + "/" + this.maxMana.ToString());
        }
        else
        {
            this.manaText.alpha = 0;
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
