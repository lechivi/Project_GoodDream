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
        //if (this.Hotkeys.Count == 0)
        //{
        //    for (int i =0; i <5; i++)
        //    {
        //        this.Hotkeys.Add(-1);
        //    }
        //}
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
        int rateEvo = Random.Range(0, 100);
        bool canEvo = rateEvo < item.rateEvo ? true : false;

        if (canEvo)
        {
            int rateTotal1 = (int)item.rateMagicFire + (int)item.rateMagicLightning;
            int rateTotal2 = (int)item.rateMagicFire + (int)item.rateMagicLightning + (int)item.rateMagicPoison;
            int rateTotal3 = (int)item.rateMagicFire + (int)item.rateMagicLightning + (int)item.rateMagicPoison + (int)item.rateMagicIce;

            int rateMagic = Random.Range(0, 100);

            if (item.rateMagicFire != 0 && rateMagic < item.rateMagicFire)
            {
                Debug.Log($"Fire: {rateMagic}/{item.rateMagicFire}");
                return this.MagicWeapon(item, MagicType.Fire);
            }

            if (item.rateMagicLightning != 0 && (item.rateMagicFire <= rateMagic && rateMagic < rateTotal1))
            {
                Debug.Log($"Lightning: {rateMagic}/{rateTotal1}");
                return this.MagicWeapon(item, MagicType.Lightning);
            }

            if (item.rateMagicPoison != 0 && (rateTotal1 <= rateMagic && rateMagic < rateTotal2))
            {
                Debug.Log($"Poison: {rateMagic}/{rateTotal2}");
                return this.MagicWeapon(item, MagicType.Poison);
            }

            if (item.rateMagicIce != 0 && (rateTotal2 <= rateMagic && rateMagic < rateTotal3))
            {
                Debug.Log($"Ice: {rateMagic}/{rateTotal3}");
                return this.MagicWeapon(item, MagicType.Ice);
            }

            if (rateTotal3 <= rateMagic)
            {
                Debug.Log($"Normal: {rateMagic} > {rateTotal3}");
                return this.MagicWeapon(item, MagicType.None);
            }
        }

        Debug.Log("_____________" + item.weaponName);
        return item.weaponPrefab;
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
            Debug.Log("_____________" + powers[random].weaponName);
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
