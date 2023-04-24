using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : BaseManager<PlayerManager>
{
    [SerializeField] private List<GameObject> listWeaponObj = new List<GameObject>();

    public List<GameObject> ListWeaponObj { get => this.listWeaponObj; set => this.listWeaponObj = value; }
    public int CurrentWeapon = 0;

    public List<int> Hotkeys = new List<int>();

    protected override void Awake()
    {
        base.Awake();

        //if (this.Hotkeys.Count == 0)
        //{
        //    for (int i =0; i <5; i++)
        //    {
        //        this.Hotkeys.Add(-1);
        //    }
        //}
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
