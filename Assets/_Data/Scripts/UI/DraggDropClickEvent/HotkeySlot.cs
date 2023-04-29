using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HotkeySlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    public WeaponSO WeaponSO;
    public Image displayWeapon;

    private bool clickedOnce;
    private void Awake()
    {
        this.displayWeapon = transform.Find("Image").GetComponent<Image>();
        if (this.WeaponSO == null)
        {
            this.displayWeapon.gameObject.SetActive(false);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        DraggableInventory draggableItem = eventData.pointerDrag.GetComponent<DraggableInventory>();

        if (draggableItem != null)
        {
            //if (PlayerManager.Instance.IsHotkey(draggableItem.IndexWeapon))
            //{
            //    Vector2 vector = PlayerManager.Instance.SwapHotkey(transform.GetSiblingIndex(), draggableItem.IndexWeapon);
            //    AddToHotkeys(draggableItem.GetComponent<WeaponTransferInfor>().WeaponSO);
            //    //Debug.Log(vector.x + " " + vector.y);
            //    if (vector.y != transform.GetSiblingIndex())
            //    {
            //        transform.parentModel.GetChild((int)vector.y).GetComponent<HotkeySlot>().RemoveFromHotkeys();
            //        Debug.Log("Remove");
            //    }
            //}
            //else
            //{
            //    AddToHotkeys(draggableItem.GetComponent<WeaponTransferInfor>().WeaponSO);
            //    PlayerManager.Instance.SetHotkeyWeapon(transform.GetSiblingIndex(), draggableItem.IndexWeapon);
            //}

            if (PlayerManager.Instance.Hotkeys.Contains(draggableItem.IndexWeapon))
            {
                int index = PlayerManager.Instance.Hotkeys.IndexOf(draggableItem.IndexWeapon);
                PlayerManager.Instance.Hotkeys[index] = -1;
                transform.parent.GetChild(index).GetComponent<HotkeySlot>().RemoveFromHotkeys();
            }

            AddToHotkeys(draggableItem.GetComponent<WeaponTransferInfor>().WeaponSO);
            PlayerManager.Instance.Hotkeys[transform.GetSiblingIndex()] = draggableItem.IndexWeapon;
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.WeaponSO != null)
        {
            InventorySystem.instance.TransferInfor(this.WeaponSO);
        }

        if (!this.clickedOnce)
        {
            this.clickedOnce = true;
            StartCoroutine(this.ResetClick());
        }
        else
        {
            Debug.Log("Double-clicked!");
            this.clickedOnce = false;
            StopAllCoroutines();

            if (this.WeaponSO != null)
            {
                this.RemoveFromHotkeys();
                PlayerManager.Instance.Hotkeys[transform.GetSiblingIndex()] = -1;
            }
        }
    }

    private IEnumerator ResetClick()
    {
        yield return new WaitForSeconds(0.3f);
        this.clickedOnce = false;
    }

    public void AddToHotkeys(WeaponSO weaponSO)
    {
        this.WeaponSO = weaponSO;
        this.displayWeapon.gameObject.SetActive(true);
        this.displayWeapon.sprite = this.WeaponSO.image;
        this.displayWeapon.SetNativeSize();
    }

    private void RemoveFromHotkeys()
    {
        this.WeaponSO = null;
        this.displayWeapon.gameObject.SetActive(false);
    }
}
