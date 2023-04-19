using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponImageHandler : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Image readyFrame;
    [SerializeField] private Image fill;

    public Image Icon { get => this.icon; set => this.icon = value; }

    private void Update()
    {
        this.readyFrame.enabled = this.fill.fillAmount == 0 ? true : false;
    }

    public void StartFillMove(float timer, float delay)
    {
        this.fill.fillAmount = (delay - timer) / delay;
    }

    public void ResetFillMove()
    {
        this.fill.fillAmount = 0;
    }
}
