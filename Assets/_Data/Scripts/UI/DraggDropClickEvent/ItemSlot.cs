using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem itemSelect = eventData.pointerDrag.GetComponent<DraggableItem>();
        if (itemSelect.ParentAfterDrag == transform) return;
        DraggableItem itemToSwap = GetComponentInChildren<DraggableItem>();

        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_STOREITEM);
        }

        if (itemSelect.ParentPanel == itemToSwap.ParentPanel)
        {
            itemToSwap.SetParentAndPosition(itemSelect.ParentAfterDrag);
            itemSelect.ParentAfterDrag = transform;
        }
        else
        {
            //if (!itemSelect.ParentPanel.CheckCanAddItem()) return;
            WeaponNormalSO tempItemSelect = itemSelect.WeaponNormalSO;
            WeaponNormalSO tempItemmToSwapt = itemToSwap.WeaponNormalSO;



            if (itemToSwap.WeaponNormalSO != null)
            {
                itemSelect.WeaponNormalSO = tempItemmToSwapt;
                itemSelect.ImageItem.sprite = itemSelect.WeaponNormalSO.image;
                itemSelect.ImageItem.SetNativeSize();
                itemSelect.ImageItem.gameObject.SetActive(true);

                itemToSwap.WeaponNormalSO = tempItemSelect;
                itemToSwap.ImageItem.sprite = itemToSwap.WeaponNormalSO.image;
                itemToSwap.ImageItem.SetNativeSize();
                itemToSwap.ImageItem.gameObject.SetActive(true);

                itemToSwap.ParentPanel.RemoveItem(tempItemmToSwapt);
                itemSelect.ParentPanel.RemoveItem(tempItemSelect);
                itemToSwap.ParentPanel.AddItem(tempItemSelect);
                itemSelect.ParentPanel.AddItem(tempItemmToSwapt);
            }
            else
            {
                itemToSwap.WeaponNormalSO = itemSelect.WeaponNormalSO;
                itemToSwap.ImageItem.sprite = itemToSwap.WeaponNormalSO.image;
                itemToSwap.ImageItem.SetNativeSize();
                itemToSwap.ImageItem.gameObject.SetActive(true);

                itemSelect.WeaponNormalSO = null;
                itemSelect.ImageItem.gameObject.SetActive(false);

                itemSelect.ParentPanel.RemoveItem(tempItemSelect);
                itemToSwap.ParentPanel.AddItem(tempItemSelect);
            }

        }
    }
}
