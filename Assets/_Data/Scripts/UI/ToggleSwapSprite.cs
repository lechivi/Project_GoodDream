using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSwapSprite : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite spriteOn;
    [SerializeField] private Sprite spriteOff;

    private bool isOn;

    public void OnClickedToggle()
    {
        this.isOn = !this.isOn;
        this.image.sprite = this.isOn ? this.spriteOn : this.spriteOff;
    }
}
