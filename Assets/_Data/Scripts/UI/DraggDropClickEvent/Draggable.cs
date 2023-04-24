using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    protected Transform parentOnDrag;
    protected RectTransform rectTransform;
    protected Canvas canvas;
    protected Camera mainCamera;

    protected virtual void Awake()
    {
        this.rectTransform = GetComponent<RectTransform>();
        this.canvas = GetComponentInParent<Canvas>();
        this.mainCamera = Camera.main;
        this.parentOnDrag = this.canvas.transform.Find("Empty");
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        //For overrite
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        this.ConvertPosition();
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        //For overrite
    }

    protected virtual void ConvertPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = this.rectTransform.position.z - this.mainCamera.transform.position.z;
        Vector3 newPos = this.mainCamera.ScreenToWorldPoint(mousePos);
        newPos.z = this.rectTransform.position.z;

        Vector3 clampedPos = newPos;
        Vector3 screenPos = this.mainCamera.WorldToScreenPoint(clampedPos);
        float minX = this.rectTransform.rect.width / 2;
        float maxX = this.canvas.pixelRect.width - this.rectTransform.rect.width / 2;
        float minY = this.rectTransform.rect.height / 2;
        float maxY = this.canvas.pixelRect.height - this.rectTransform.rect.height / 2;
        clampedPos.x = Mathf.Clamp(screenPos.x, minX, maxX);
        clampedPos.y = Mathf.Clamp(screenPos.y, minY, maxY);
        clampedPos = this.mainCamera.ScreenToWorldPoint(clampedPos);
        clampedPos.z = this.rectTransform.position.z;

        rectTransform.position = clampedPos;
    }

}
