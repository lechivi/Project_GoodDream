using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : Draggable 
{ 
    public PanelItemParent ParentPanel;
    public WeaponNormalSO WeaponNormalSO;
    public Image ImageItem;
    public Transform ParentAfterDrag;

    protected override void Awake()
    {
        base.Awake();
        //this.parentOnDrag = GetComponentInParent<Canvas>().transform.Find("Empty");
        this.ImageItem.gameObject.SetActive(false);
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        if (this.WeaponNormalSO == null) return;

        this.ParentAfterDrag = transform.parent;
        transform.SetParent(this.parentOnDrag);
        transform.SetAsLastSibling();

        this.ImageItem.raycastTarget = false;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        //base.OnDrag(eventData);
        transform.position = Input.mousePosition;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        this.SetParentAndPosition(this.ParentAfterDrag);

        this.ImageItem.raycastTarget = true;

    }

    protected override void ConvertPosition()
    {
        base.ConvertPosition();
    }

    public void SetParentAndPosition(Transform parent)
    {
        transform.SetParent(parent);
        transform.GetComponent<RectTransform>().localPosition = Vector3.zero;
    }

    public void SetWeaponNormalSO(WeaponNormalSO weaponNormalSO)
    {
        this.WeaponNormalSO = weaponNormalSO;
        this.ImageItem.gameObject.SetActive(true);
        this.ImageItem.sprite = weaponNormalSO.image;
        this.ImageItem.SetNativeSize();
    }
}
