using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicHolder : MonoBehaviour
{
    [SerializeField] private PanelHandHolderCtrl panelHandHolderCtrl;
    [SerializeField] private List<WeaponNormalSO> holderItems;

    public List<WeaponNormalSO> HolderItems { get => this.holderItems; set => this.holderItems = value; }


    public void TransferItem(int index)
    {
        if (this.holderItems[index] == null) return;
        if (DreamBookScript.instance.AddWeapon(this.holderItems[index]))
        {
            this.panelHandHolderCtrl.ClearItem(this.holderItems[index]);
            this.holderItems[index] = null;
        }
    }

    //public void TransferAllItem()
    //{
    //    if (this.holderItems.Count == 0) return;
    //    for (int i = 0; i < this.holderItems.Count; i++)
    //    {
    //        if (DreamBookScript.instance.AddWeapon(this.holderItems[i]))
    //        {
    //            this.holderItems[i] = null;
    //        }
    //        else return;
    //    }
    //}
}
