using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider seSlider;

    private float bgmValue;
    private float seValue;

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
