using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] private GameObject howToPlayButton;
    [SerializeField] private GameObject howToPlayImage;
    public void OnStartButtonClick()
    {
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ActiveMenuPanel(false);
            UIManager.Instance.ActiveLoadingPanel(true);
        }

        if (AudioManager.HasInstance)
        {
            //AudioManager.Instance.PlayBGM(AUDIO.BGM_BGM_02, 0.5f);
        }
    }

    public void OnSettingButtonClick()
    {
        if (UIManager.HasInstance)
        {
            UIManager.Instance.ActiveSettingPanel(true);
        }
    }

    public void OnClickedHowToPlayButton()
    {
        this.howToPlayImage.SetActive(true);
        this.howToPlayButton.SetActive(false);
    }

    public void OnClickedOnHowToPlayButtonImage()
    {
        this.howToPlayImage.SetActive(false);
        this.howToPlayButton.SetActive(true);
    }
}
