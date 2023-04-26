using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationTextStatic : MonoBehaviour
{
    public static NotificationTextStatic instance;

    private Animator animator;
    private CanvasGroup canvasGroup;
    private TMP_Text notiText;

    private void Awake()
    {
        this.animator = GetComponent<Animator>();
        this.canvasGroup = GetComponent<CanvasGroup>();
        this.notiText = GetComponentInChildren<TMP_Text>();

        this.HideText();
        NotificationTextStatic.instance = this;
    }

    public void SetNotiText(string text, float time)
    {
        //this.canvasGroup
        //this.animator.Rebind();
        this.notiText.enabled = true;
        this.notiText.SetText(text);
        Invoke("HideText", time);
    }

    //public void SetTriggerDisable()
    //{
    //    this.animator.SetTrigger("Disable");
    //}

    public void HideText()
    {
        this.notiText.enabled = false;
    }
}
