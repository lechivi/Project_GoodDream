using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider seSlider;
    [SerializeField] private Toggle bgmMute;
    [SerializeField] private Toggle seMute;

    private float bgmValue;
    private float seValue;
    private bool isBGMMute;
    private bool isSEMute;

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
            this.seValue = AudioManager.Instance.AttachSESource.volume;
            this.bgmSlider.value = this.bgmValue;
            this.seSlider.value = this.seValue;

            this.isBGMMute = AudioManager.Instance.AttachBGMSource.mute;
            this.isSEMute = AudioManager.Instance.AttachSESource.mute;
            this.bgmMute.isOn = this.isBGMMute;
            this.seMute.isOn = this.isSEMute;
        }
    }

    public void OnSliderChangerBGMValue(float value)
    {
        this.bgmValue = value;
    }

    public void OnSliderChangeSEValue(float value)
    {
        this.seValue = value;
    }

    public void OnToggelMuteBGM(bool value)
    {
        this.isBGMMute = value;
    }

    public void OnToggelMuteSE(bool value)
    {
        this.isSEMute = value;
    }

    public void OnClickedCancelButton()
    {
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
            AudioManager.Instance.ChangeBGMVolume(this.bgmValue);
            AudioManager.Instance.ChangeSEVolume(this.seValue);
            AudioManager.Instance.MuteBGM(this.isBGMMute);
            AudioManager.Instance.MuteSE(this.isSEMute);
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
}
