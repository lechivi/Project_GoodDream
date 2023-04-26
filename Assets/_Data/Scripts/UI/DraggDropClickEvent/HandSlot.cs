using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem itemSelect = eventData.pointerDrag.GetComponent<DraggableItem>();
        DraggableItem itemToSwap = GetComponentInChildren<DraggableItem>();

        if (itemSelect.ParentPanel == itemToSwap.ParentPanel)
        {
            if (itemToSwap.WeaponNormalSO != null)
            {
                itemToSwap.SetParentAndPosition(itemSelect.ParentAfterDrag);
            }

            itemSelect.ParentAfterDrag = transform;
        }
        else
        {
            //Remove WeaponNormalSO
            itemSelect.ParentPanel.RemoveItem(itemSelect.WeaponNormalSO);
            itemSelect.WeaponNormalSO = null;
            itemSelect.ImageItem.gameObject.SetActive(false);

            if (itemToSwap.WeaponNormalSO != null)
            {
                itemSelect.ParentPanel.AddItem(itemToSwap.WeaponNormalSO);
                itemSelect.ImageItem.gameObject.SetActive(true);
            }

            itemToSwap.WeaponNormalSO = itemSelect.WeaponNormalSO;
            itemToSwap.ImageItem.gameObject.SetActive(true);
        }
    }
}
    
