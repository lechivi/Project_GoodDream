using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideCtrl : MonoBehaviour
{
    [SerializeField] private GameObject dimed;
    [SerializeField] private GameObject pressE;
    [SerializeField] private GameObject dragToHand;
    [SerializeField] private GameObject fullHand;
    [SerializeField] private GameObject openInventory;

    public bool GuideShow;
    public bool FirstPressE;
    public bool FirstDragToHand;
    public bool FirstFullHand;
    public bool FirstOpenInventory;

    private int activeGuide;

    private void Awake()
    {
        //this.dimed.SetActive(false);
        //this.SetActiveGuidePressE(false);
        //this.SetActiveGuideDragToHand(false);
        //this.SetActiveGuideFullHand(false);
        //this.SetActiveGuideOpenInventory(false);
    }

    public void SetActiveGuidePressE()
    {
        this.GuideShow = true;
        this.activeGuide = 0;
        this.dimed.SetActive(true);
        this.pressE.SetActive(true);
    }

    public void SetActiveGuideDragToHand()
    {
        this.GuideShow = true;
        this.activeGuide = 1;
        this.dimed.SetActive(true);
        this.dragToHand.SetActive(true);
    }

    public void SetActiveGuideFullHand()
    {
        this.GuideShow = true;
        this.activeGuide = 2;
        this.dimed.SetActive(true);
        this.fullHand.SetActive(true);
    }

    public void SetActiveGuideOpenInventory()
    {
        this.GuideShow = true;
        this.activeGuide = 3;
        this.dimed.SetActive(true);
        this.openInventory.SetActive(true);
    }

    public void SetFalseGuide()
    {
        switch (this.activeGuide)
        {
            case 0:
                this.pressE.SetActive(false);
                break;

            case 1:
                this.dragToHand.SetActive(false);
                break;

            case 2:
                this.fullHand.SetActive(false);
                break;

            case 3:
                this.openInventory.SetActive(false);
                break;

            case -1:
                break;
        }
        this.activeGuide = -1;
        this.dimed.SetActive(false);
    }
}
