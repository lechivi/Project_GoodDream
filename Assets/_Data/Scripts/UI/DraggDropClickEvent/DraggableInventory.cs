using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableInventory : Draggable, IPointerClickHandler
{
    public Transform ParentAfterDrag;
    public int IndexWeapon;

    private WeaponTransferInfor weaponTransferInfor;
    private bool clickedOnce = false;

    protected override void Awake()
    {
        base.Awake();
        this.weaponTransferInfor = GetComponent<WeaponTransferInfor>();
    }

    #region DRAG_SYSTEM
    public override void OnBeginDrag(PointerEventData eventData)
    {
        this.ParentAfterDrag = transform.parent;
        transform.SetParent(this.parentOnDrag);
        transform.SetAsLastSibling();

        this.weaponTransferInfor.SetImageRaycastTarget(false);
        if (this.weaponTransferInfor.IsSelect)
        {
            this.weaponTransferInfor.ImageSelect.gameObject.SetActive(false);
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        //transform.position = Input.mousePosition;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        this.SetParentAndPosition(this.ParentAfterDrag);

        this.weaponTransferInfor.SetImageRaycastTarget(true);
        if (this.weaponTransferInfor.IsSelect)
        {
            this.weaponTransferInfor.ImageSelect.gameObject.SetActive(true);
        }
    }
    #endregion

    #region CLICK_SYSTEM
    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.weaponTransferInfor.WeaponSO != null)
        {
            InventorySystem.instance.SelectWeaponInventory(this.IndexWeapon);
        }

        if (!this.clickedOnce)
        {
            this.clickedOnce = true;
            StartCoroutine(this.ResetClick());
        }
        else
        {
            //Debug.Log("Double-clicked!");
            this.clickedOnce = false;
            StopAllCoroutines();

            //Equip Weapon
            //this.weaponTransferInfor.SetEquipWeapon(!this.weaponTransferInfor.IsEquip);
            if (!this.weaponTransferInfor.IsEquip)
            {
                InventorySystem.instance.EquipWeaponInventory(this.IndexWeapon);
            }
        }
    }

    private IEnumerator ResetClick()
    {
        yield return new WaitForSeconds(0.3f);
        this.clickedOnce = false;
    }
    #endregion

    public void SetParentAndPosition(Transform parent)
    {
        transform.SetParent(parent);
        transform.GetComponent<RectTransform>().localPosition = Vector3.zero;
    }
}
