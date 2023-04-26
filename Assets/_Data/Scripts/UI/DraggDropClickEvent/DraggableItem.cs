using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public PanelItemParent ParentPanel;
    public WeaponNormalSO WeaponNormalSO;
    public Image ImageItem;
    public Transform ParentAfterDrag;

    private Transform parentOnDrag;

    private void Awake()
    {
        this.parentOnDrag = GetComponentInParent<Canvas>().transform.Find("Empty");
        this.ImageItem.gameObject.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (this.WeaponNormalSO == null) return;

        this.ParentAfterDrag = transform.parent;
        transform.SetParent(this.parentOnDrag);
        transform.SetAsLastSibling();

        this.ImageItem.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.SetParentAndPosition(this.ParentAfterDrag);

        this.ImageItem.raycastTarget = true;

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
