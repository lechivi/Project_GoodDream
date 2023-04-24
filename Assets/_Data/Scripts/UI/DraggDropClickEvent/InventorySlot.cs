using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        //Swap weapon index
        DraggableInventory weaponSelect = eventData.pointerDrag.GetComponent<DraggableInventory>();
        DraggableInventory weaponToSwap = GetComponentInChildren<DraggableInventory>();

        if (weaponToSwap != null)
        {
            if (weaponSelect.IndexWeapon == weaponToSwap.IndexWeapon) return;
            PlayerManager.Instance.SwapWeapon(weaponSelect.IndexWeapon, weaponToSwap.IndexWeapon);

            int temp = weaponSelect.IndexWeapon;
            weaponSelect.IndexWeapon = weaponToSwap.IndexWeapon;
            weaponToSwap.IndexWeapon = temp;
        }

        if (weaponToSwap != null)
        {
            weaponToSwap.SetParentAndPosition(weaponSelect.ParentAfterDrag);
        }
        weaponSelect.ParentAfterDrag = transform;

    }

}
