using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelItemCtrl : PanelItemParent
{
    [SerializeField] private List<ItemSlot> slots = new List<ItemSlot>();

    public ItemHolderZone Zone;

    private void Awake()
    {
        transform.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Debug.Log("asdad");
    }

    public void SetItemFromZone(List<WeaponNormalSO> list)
    {
        foreach(ItemSlot slot in slots)
        {
            DraggableItem draggItem = slot.GetComponentInChildren<DraggableItem>();
            draggItem.WeaponNormalSO = null;
            draggItem.ImageItem.gameObject.SetActive(false);
        }

        if (list == null) return;

        int index = 0;
        foreach (WeaponNormalSO item in list)
        {
            this.slots[index].GetComponentInChildren<DraggableItem>().SetWeaponNormalSO(item);
            index++;
        }
    }

    public override bool CheckCanAddItem()
    {
        return this.Zone.SelectedItems.Count < 5;
    }

    public override void AddItem(WeaponNormalSO item)
    {
        base.AddItem(item);
        this.Zone.SelectedItems.Add(item);
    }

    public override void RemoveItem(WeaponNormalSO item)
    {
        base.RemoveItem(item);
        for (int i = 0; i < this.Zone.SelectedItems.Count; i++)
        {
            if (this.Zone.SelectedItems[i] == item)
            {
                this.Zone.SelectedItems.RemoveAt(i);
                break;
            }
        }
    }
}
