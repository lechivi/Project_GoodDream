using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<TabButtonNavigation> tabButtons;
    public Color tabIdle;
    public Color tabHover;
    //public Color tabActive;
    public TabButtonNavigation selectedTab;
    public List<GameObject> objectToSwap;
    public void Subscribe(TabButtonNavigation button)
    {
        if (this.tabButtons == null)
        {
            this.tabButtons = new List<TabButtonNavigation>();
        }

        this.tabButtons.Add(button);
    }

    public void OnTabEnter(TabButtonNavigation button)
    {
        this.ResetTabs();
        if (this.selectedTab == null || button != this.selectedTab)
        {
            button.highlightBackground.color = this.tabHover;

        }
    }

    public void OnTabExit(TabButtonNavigation button)
    {
        this.ResetTabs();

    }

    public void OnTabSelected(TabButtonNavigation button)
    {
        if(this.selectedTab != null)
        {
            this.selectedTab.Deselect(); //Deselect the previous select tab
        }

        this.selectedTab = button;
        this.selectedTab.Select();

        this.ResetTabs();
        //button.highlightBackground.color = this.tabActive;

        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < this.objectToSwap.Count; i++)
        {
            if (i == index)
            {
                this.objectToSwap[i].SetActive(true);
            }
            else
            {
                this.objectToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTabs()
    {
        foreach (TabButtonNavigation button in tabButtons)
        {
            if (this.selectedTab != null && button == this.selectedTab) continue;
            button.highlightBackground.color = this.tabIdle;
        }
    }

}
