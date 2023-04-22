using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using UnityEngine.Events;

//[RequireComponent(typeof(Image))]
public class TabButtonNavigation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public TabGroup tabGroup;
    public Image highlightBackground;

    public UnityEvent onTabSelected;
    public UnityEvent onTabDeselected;

    private void Start()
    {
        //this.highlightBackground = GetComponent<Image>();
        this.tabGroup.Subscribe(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        this.tabGroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.tabGroup.OnTabExit(this);
    }

    public void Select()
    {
        if (this.onTabSelected != null)
        {
            this.onTabSelected.Invoke();
        }
    }

    public void Deselect()
    {
        if (this.onTabDeselected != null)
        {
            this.onTabDeselected.Invoke();
        }
    }
}
