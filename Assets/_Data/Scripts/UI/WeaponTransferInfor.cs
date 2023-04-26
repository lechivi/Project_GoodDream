using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponTransferInfor : MonoBehaviour
{
    public WeaponSO WeaponSO;
    public Image ImageWeapon;
    public Image ImageSelect;
    public Image ImageEquip;

    public bool IsSelect;
    public bool IsEquip;

    private void Awake()
    {
        this.ImageWeapon = transform.Find("Weapon").GetComponent<Image>();
        this.ImageSelect = transform.Find("Select").GetComponent<Image>();
        this.ImageEquip = transform.Find("Equip").GetComponent<Image>();
    }

    private void Start()
    {
        if (this.WeaponSO.image != null)
        {
            this.ImageWeapon.sprite = this.WeaponSO.image;
            this.ImageWeapon.SetNativeSize();
        }
        else
        {
            this.ImageWeapon.sprite = null;
        }

        this.ImageSelect.gameObject.SetActive(this.IsSelect);
        this.ImageEquip.gameObject.SetActive(this.IsEquip);
    }

    public void SetImageRaycastTarget(bool isBlock)
    {
        this.ImageWeapon.raycastTarget = isBlock;
        this.ImageSelect.raycastTarget = isBlock;
        this.ImageEquip.raycastTarget = isBlock;
    }

    public void SetSelectWeapon(bool isSelect)
    {
        this.IsSelect = isSelect;
        this.ImageSelect.gameObject.SetActive(isSelect);
    }

    public void SetEquipWeapon(bool isEquip)
    {
        this.IsEquip = isEquip;
        this.ImageEquip.gameObject.SetActive(isEquip);
    }

}
