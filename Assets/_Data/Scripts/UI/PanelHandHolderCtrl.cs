using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelHandHolderCtrl : PanelItemParent
{
    [SerializeField] private List<DraggableItem> items = new List<DraggableItem>();

    public override bool CheckCanAddItem()
    {
        foreach (WeaponSO item in PlayerBasicCtrl.instance.PlayerHolder.HolderItems)
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
            if (PlayerBasicCtrl.instance.PlayerHolder.HolderItems[i] != null)
            {
                continue;
            }
            else
            {
                PlayerBasicCtrl.instance.PlayerHolder.HolderItems[i] = item;
                break;
            }
        }
    }

    public override void RemoveItem(WeaponNormalSO item)
    {
        base.RemoveItem(item);
        int index = PlayerBasicCtrl.instance.PlayerHolder.HolderItems.IndexOf(item);
        PlayerBasicCtrl.instance.PlayerHolder.HolderItems[index] = null;
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
