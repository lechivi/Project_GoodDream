using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlurringSliderFill : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image fillImage;
    [SerializeField] private Image symbol;
    [SerializeField] private TMP_Text valueText;
    [SerializeField] private float alpha = 0.6f;

    private Color activeColor = Color.white;
    private Color inactiveColor = Color.white;

    private void Awake()
    {
        this.inactiveColor.a = this.alpha;
    }

    public void SetActiveSlider(int value, int maxValue, bool active)
    {
        this.slider.maxValue = maxValue;
        this.slider.value = value;

        if (active)
        {   
            this.fillImage.color = this.activeColor;
            this.symbol.color = this.activeColor;
            this.valueText.SetText(value + "/" + maxValue);
            this.valueText.alpha = 1f;
        }
        else
        {
            this.fillImage.color = this.inactiveColor;
            this.symbol.color = this.inactiveColor;
            this.valueText.alpha = 0f;
        }
    }

}
