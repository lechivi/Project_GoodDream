using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : BaseManager<PlayerManager>
{
    [SerializeField] private List<GameObject> listWeaponObj = new List<GameObject>();

    public List<GameObject> ListWeaponObj { get => this.listWeaponObj; set => this.listWeaponObj = value; }

    public List<WeaponNormalSO> ListWeaponNormalSO = new List<WeaponNormalSO>();
    public List<int> Hotkeys = new List<int>();

    public int CurrentWeapon = 0;

    public bool Test;
    protected override void Awake()
    {
        base.Awake();
        if (!this.Test)
        {
            this.listWeaponObj.Clear();
            this.ListWeaponNormalSO.Clear();
        }
        
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
            }
        }

    }

    private GameObject EvolutionWeapon(WeaponNormalSO item)
    {
        float rateEvo = Random.value;
        bool canEvo = rateEvo < item.rateEvo ? true : false;

        if (canEvo)
        {
            //int rateTotal1 = (int)item.rateMagicFire + (int)item.rateMagicLightning;
            //int rateTotal2 = (int)item.rateMagicFire + (int)item.rateMagicLightning + (int)item.rateMagicPoison;
            //int rateTotal3 = (int)item.rateMagicFire + (int)item.rateMagicLightning + (int)item.rateMagicPoison + (int)item.rateMagicIce;

            //int rateMagic = Random.Range(0, 100);

            //if (item.rateMagicFire != 0 && rateMagic < item.rateMagicFire)
            //{
            //    Debug.Log($"Fire: {rateMagic}/{item.rateMagicFire}");
            //    return this.MagicWeapon(item, MagicType.Fire);
            //}

            //if (item.rateMagicLightning != 0 && (item.rateMagicFire <= rateMagic && rateMagic < rateTotal1))
            //{
            //    Debug.Log($"Lightning: {rateMagic}/{rateTotal1}");
            //    return this.MagicWeapon(item, MagicType.Lightning);
            //}

            //if (item.rateMagicPoison != 0 && (rateTotal1 <= rateMagic && rateMagic < rateTotal2))
            //{
            //    Debug.Log($"Poison: {rateMagic}/{rateTotal2}");
            //    return this.MagicWeapon(item, MagicType.Poison);
            //}

            //if (item.rateMagicIce != 0 && (rateTotal2 <= rateMagic && rateMagic < rateTotal3))
            //{
            //    Debug.Log($"Ice: {rateMagic}/{rateTotal3}");
            //    return this.MagicWeapon(item, MagicType.Ice);
            //}

            //if (rateTotal3 <= rateMagic)
            //{
            //    Debug.Log($"Normal: {rateMagic} > {rateTotal3}");
            //    return this.MagicWeapon(item, MagicType.None);
            //}

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
