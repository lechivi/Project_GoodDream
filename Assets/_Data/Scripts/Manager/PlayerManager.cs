using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : BaseManager<PlayerManager>
{
    [Header("PLAYER DATA")]
    public int CurrentHealth = 0;
    public int MaxHealth = 100;
    public int CurrentMana = 0;
    public int MaxMana = 100;
    public float ReloadSpeed = 1f;
    public CharacterSO CharacterSO;

    public int CurrentHealthSave;
    public int CurrentManaSave;

    [Header("WEAPON DATA")]
    //[SerializeField] private List<WeaponSO> listWeapon = new List<WeaponSO>();
    [SerializeField] private List<GameObject> listWeaponObj = new List<GameObject>();

    //public List<WeaponSO> ListWeapon { get => this.listWeapon; set => this.listWeapon = value; }
    public List<GameObject> ListWeaponObj { get => this.listWeaponObj; set => this.listWeaponObj = value; }

    public List<WeaponNormalSO> ListWeaponNormalSO = new List<WeaponNormalSO>();
    public List<int> Hotkeys = new List<int>();
    public int CurrentWeapon = 0;
    public bool TestDefaultWeapon;

    public void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex >= 3 && SceneManager.GetActiveScene().buildIndex <= 5)
        {
            this.CurrentHealthSave = this.CurrentHealth;
            this.CurrentManaSave = this.CurrentMana;
        }
    }

    public void UpdateHealth(int value)
    {
        this.CurrentHealth = value;
    }

    public void UpdateMaxHealth(int value)
    {
        this.MaxHealth = value;
    }

    public void UpdateMana(int value)
    {
        this.CurrentMana = value;
    }

    protected override void Awake()
    {
        base.Awake();
        if (!this.TestDefaultWeapon)
        {
            this.listWeaponObj.Clear();
            this.ListWeaponNormalSO.Clear();
        }

        this.SetupPlayerData();
    }

    public void SetupPlayerData()
    {
        this.CurrentHealth = this.MaxHealth;
        this.CurrentMana = this.MaxMana;

        if (UIManager.HasInstance)
        {
            UIManager.Instance.GamePanel.HealthSlider.SetActiveSlider(this.CurrentHealth, this.MaxHealth, true);
            UIManager.Instance.GamePanel.ManaSlider.SetActiveSlider(this.CurrentMana, this.MaxMana, true);
        }

    }

    public void SetPlayerModel(CharacterSO characterSO)
    {
        this.CharacterSO = characterSO;
        this.MaxHealth = characterSO.maxHealth;
        this.MaxMana = characterSO.maxMana;
        this.ReloadSpeed = characterSO.reloadSpeed;

        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null)
        {
            playerObj.GetComponent<PlayerCtrl>().PlayerModel.SetPlayerModel(characterSO);
        }

        this.SetupPlayerData();
    }

    public void CreateListWeapon()
    {
        if (this.ListWeaponNormalSO.Count == 0) return;
        foreach (WeaponNormalSO item in this.ListWeaponNormalSO) 
        {
            GameObject weaponObj = EvolutionWeapon(item);
            if (weaponObj != null)
            {
                this.listWeaponObj.Add(weaponObj);
                //this.listWeapon.Add(weaponObj.GetComponent<Weapon>().WeaponSO);
            }
        }

    }

    private GameObject EvolutionWeapon(WeaponNormalSO item)
    {
        float rateEvo = Random.value;
        bool canEvo = rateEvo < item.rateEvo ? true : false;

        if (canEvo)
        {
            float[] percentages = { item.rateMagicFire, item.rateMagicLightning, item.rateMagicIce, item.rateMagicPoison };
            int indexMagic = RandomPercent(percentages);

            switch (indexMagic)
            {
                case 0:
                    return this.MagicWeapon(item, MagicType.Fire);
                case 1:
                    return this.MagicWeapon(item, MagicType.Lightning);     
                case 2:
                    return this.MagicWeapon(item, MagicType.Ice);
                case 3:
                    return this.MagicWeapon(item, MagicType.Poison);
                case 4:
                    return this.MagicWeapon(item, MagicType.None);
                case -1:
                    Debug.Log("Error evo magic");
                    break;
            }
        }
        return item.weaponPrefab;
    }

    private int RandomPercent(float[] percentages)
    {
        float sum = 0;
        for (int i = 0; i < percentages.Length; i++)
        {
            if (percentages[i] < 0) return -1;
            sum += percentages[i];
        }
        if (sum > 1) return -1;

        float cumulativeProbability = 0;
        float random = Random.value;
        for (int i = 0; i < percentages.Length; i++)
        {
            cumulativeProbability += percentages[i];
            if (random < cumulativeProbability) return i;
        }
        return percentages.Length;
    }

    private GameObject MagicWeapon(WeaponNormalSO item, MagicType type)
    {
        List<WeaponPowerSO> powers = new List<WeaponPowerSO>();
        foreach (WeaponPowerSO powerSO in item.evolutionWeapon)
        {
            if (powerSO.magic == type)
                powers.Add(powerSO);
        }

        if (powers.Count == 0)
        {
            Debug.Log("Error evo magic");
            return null;
        }
        else
        {
            int random = Random.Range(0, powers.Count);
            return powers[random].weaponPrefab;
        }
    }

    public void SwapWeapon(int weaponSelect, int weaponToSwap)
    {
        GameObject temp = this.listWeaponObj[weaponSelect];
        this.listWeaponObj[weaponSelect] = this.listWeaponObj[weaponToSwap];
        this.listWeaponObj[weaponToSwap] = temp;

        if (PlayerCtrl.instance != null)
        {
            Transform weaponA = PlayerCtrl.instance.WeaponParent.transform.GetChild(weaponSelect);
            Transform weaponB = PlayerCtrl.instance.WeaponParent.transform.GetChild(weaponToSwap);

            int siblingIndexA = weaponA.GetSiblingIndex();
            int siblingIndexB = weaponB.GetSiblingIndex();

            weaponA.SetSiblingIndex(siblingIndexB);
            weaponB.SetSiblingIndex(siblingIndexA);
        }
        Debug.Log("Swap: " + weaponSelect + " <=> " + weaponToSwap);

    }

    public void EquipWeaponFromInventoryToPlayer(int index)
    {
        if (PlayerCtrl.instance != null)
        {
            PlayerCtrl.instance.WeaponParent.SetWeapon(index);
        }
        //Debug.Log("Weapon: " + index);
    }

}
