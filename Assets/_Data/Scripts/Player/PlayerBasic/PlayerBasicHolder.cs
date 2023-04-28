using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicHolder : MonoBehaviour
{
    [SerializeField] private List<WeaponNormalSO> holderItems;

    public List<WeaponNormalSO> HolderItems { get => this.holderItems; set => this.holderItems = value; }


    public void TransferItem(int index)
    {
        if (this.holderItems[index] == null) return;
        if (DreamBookScript.instance.AddWeapon(this.holderItems[index]))
        {
            if (UIManager.HasInstance)
            {
                UIManager.Instance.HomeScenePanel.PanelHandHolderCtrl.ClearItem(this.holderItems[index]);
            }
            this.holderItems[index] = null;
        }
    }
}
