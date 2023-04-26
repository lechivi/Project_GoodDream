using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelHandHolderCtrl : PanelItemParent
{
    [SerializeField] private PlayerBasicHolder playerBasicHolder;
    [SerializeField] private List<DraggableItem> items = new List<DraggableItem>();

    public override bool CheckCanAddItem()
    {
        foreach (WeaponSO item in this.playerBasicHolder.HolderItems)
        {
            if (item == null) return true;
        }
        return false;
    }

    public override void AddItem(WeaponNormalSO item)
    {
        base.AddItem(item);
        for (int i = 0; i < 2; i++)
        {
            if (this.playerBasicHolder.HolderItems[i] != null)
            {
                continue;
            }
            else
            {
                this.playerBasicHolder.HolderItems[i] = item;
                break;
            }
        }
    }

    public override void RemoveItem(WeaponNormalSO item)
    {
        base.RemoveItem(item);
        int index = this.playerBasicHolder.HolderItems.IndexOf(item);
        this.playerBasicHolder.HolderItems[index] = null;
    }

    public void ClearItem(WeaponNormalSO item)
    {
        bool checkExist = false;
        int index = -1;
        foreach (DraggableItem draggItem in this.items)
        {
            if (draggItem.WeaponNormalSO == item)
            {
                checkExist = true;
                index = this.items.IndexOf(draggItem);
            }
        }

        if (!checkExist || index == -1) return;
        this.items[index].WeaponNormalSO = null;
        this.items[index].ImageItem.gameObject.SetActive(false);
    }
}
