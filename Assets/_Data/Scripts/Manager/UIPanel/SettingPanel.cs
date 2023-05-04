using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    [Header("MAIN MENU")]
    [SerializeField] private GameObject dimed;
    [SerializeField] private GameObject popup;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Toggle bgmMute;
    [SerializeField] private Toggle sfxMute;

    [Header("ON GAME")]
    [SerializeField] private Slider bgmSliderOG;
    [SerializeField] private Slider sfxSliderOG;
    [SerializeField] private Toggle bgmMuteOG;
    [SerializeField] private Toggle sfxMuteOG;

    private float bgmValue;
    private float sfxValue;
    private bool isBGMMute;
    private bool isSFXMute;

    private void Awake()
    {
        this.SetupValue();
    }

    private void OnEnable()
    {
        this.SetupValue();
    }

    private void SetupValue()
    {
        if (AudioManager.HasInstance)
        {
            this.bgmValue = AudioManager.Instance.AttachBGMSource.volume;
            this.sfxValue = AudioManager.Instance.AttachSFXSource.volume;
            this.bgmSlider.value = this.bgmValue;
            this.sfxSlider.value = this.sfxValue;

            this.isBGMMute = AudioManager.Instance.AttachBGMSource.mute;
            this.isSFXMute = AudioManager.Instance.AttachSFXSource.mute;
            this.bgmMute.isOn = this.isBGMMute;
            this.sfxMute.isOn = this.isSFXMute;
        }
    }

    public void SetupValueOG()
    {
        if (AudioManager.HasInstance)
        {
            this.bgmValue = AudioManager.Instance.AttachBGMSource.volume;
            this.sfxValue = AudioManager.Instance.AttachSFXSource.volume;
            this.bgmSliderOG.value = this.bgmValue;
            this.sfxSliderOG.value = this.sfxValue;

            this.isBGMMute = AudioManager.Instance.AttachBGMSource.mute;
            this.isSFXMute = AudioManager.Instance.AttachSFXSource.mute;
            this.bgmMuteOG.isOn = this.isBGMMute;
            this.sfxMuteOG.isOn = this.isSFXMute;
        }
    }

    public void SettingMainMenu(bool active)
    {
        this.dimed.SetActive(active);
        this.popup.SetActive(active);
    }

    public void OnSliderChangeBGMValue(float value)
    {
        this.bgmValue = value;
    }

    public void OnSliderChangeSFXValue(float value)
    {
        this.sfxValue = value;
    }

    public void OnToggelMuteBGM(bool value)
    {
        this.isBGMMute = value;
    }

    public void OnToggelMuteSFX(bool value)
    {
        this.isSFXMute = value;
    }

    public void OnClickedCancelButton()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_BUTTON);
        }

        if (UIManager.HasInstance)
        {
            UIManager.Instance.ActiveSettingPanel(false);
        }

        if (GameManager.HasInstance)
        {
            if (!GameManager.Instance.IsPlaying && !UIManager.Instance.MenuPanel.gameObject.activeSelf)
            {
                UIManager.Instance.ActivePausePanel(true);
            }
        }
    }

    public void OnClickedSubmitButton()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_BUTTON);
        }

        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.ChangeBGMVolume(this.bgmValue);
            AudioManager.Instance.ChangeSFXVolume(this.sfxValue);
            AudioManager.Instance.MuteBGM(this.isBGMMute);
            AudioManager.Instance.MuteSFX(this.isSFXMute);
        }

        if (UIManager.HasInstance)
        {
            UIManager.Instance.SettingPanel.SettingMainMenu(false);
            UIManager.Instance.ActiveSettingPanel(false);
        }

        if (GameManager.HasInstance)
        {
            if (!GameManager.Instance.IsPlaying && !UIManager.Instance.MenuPanel.gameObject.activeSelf)
            {
                UIManager.Instance.ActivePausePanel(true);
            }
        }
    }  
    
    public void OnClickedSubmitButtonOG()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_BUTTON);
        }

        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.ChangeBGMVolume(this.bgmValue);
            AudioManager.Instance.ChangeSFXVolume(this.sfxValue);
            AudioManager.Instance.MuteBGM(this.isBGMMute);
            AudioManager.Instance.MuteSFX(this.isSFXMute);
        }

        //if (UIManager.HasInstance)
        //{
        //    UIManager.Instance.SettingPanel.SettingMainMenu(false);
        //    UIManager.Instance.ActiveSettingPanel(false);
        //}

        //if (GameManager.HasInstance)
        //{
        //    if (!GameManager.Instance.IsPlaying && !UIManager.Instance.MenuPanel.gameObject.activeSelf)
        //    {
        //        UIManager.Instance.ActivePausePanel(true);
        //    }
        //}
    }
}
