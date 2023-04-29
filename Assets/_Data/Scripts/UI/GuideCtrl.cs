using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Guide
{
    PressE,
    DragToHand,
    FullHand,
    OpenInv,
    InvLeft,
    InvRight,
}

[System.Serializable]
public class GuidePopup
{
    public GameObject GuideObj;
    public Guide EnumGuide;
    public bool First;
    public bool GuideShow;
}

public class GuideCtrl : MonoBehaviour
{
    [SerializeField] private List<GuidePopup> listGuide = new List<GuidePopup>();
    [SerializeField] private GameObject dimed;

    public List<GuidePopup> ListGuide => this.listGuide;

    private int activeGuide;

    public GuidePopup GetGuide(Guide guide)
    {
        foreach (GuidePopup guidePopup in listGuide)
        {
            if (guidePopup.EnumGuide == guide)
            {
                return guidePopup;
            }
        }

        return null;
    }
    public void SetTrueGuide(Guide guide)
    {
        foreach(GuidePopup guidePopup in listGuide)
        {
            if (guidePopup.EnumGuide == guide)
            {
                guidePopup.GuideObj.SetActive(true);
                guidePopup.GuideShow = true;
                this.activeGuide = this.listGuide.IndexOf(guidePopup);
                this.dimed.SetActive(true);
                break;
            }
        }
    }  
    
    public void SetTrueGuide(GuidePopup guidePopup)
    {
        foreach(GuidePopup guide in listGuide)
        {
            if (guide == guidePopup)
            {
                guide.GuideObj.SetActive(true);
                guidePopup.GuideShow = true;
                this.activeGuide = this.listGuide.IndexOf(guide);
                this.dimed.SetActive(true);
                break;
            }
        }
    }

    public void SetFalseGuide()
    {
        this.listGuide[this.activeGuide].GuideObj.SetActive(false);
        this.listGuide[this.activeGuide].GuideShow = false;
        this.dimed.SetActive(false);
        this.activeGuide = -1;
    }
}
