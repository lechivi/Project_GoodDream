using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGuide : MonoBehaviour
{
    private bool isRead;
    private void Start()
    {
        if (UIManager.HasInstance)
        {
            //UIManager.Instance.NotificationPanel.NotificationHUD.SetNotiText("Press Pause Button to open your inventory!", 30f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (!this.isRead && UIManager.HasInstance)
            {
                this.isRead = true;
                //UIManager.Instance.NotificationPanel.NotificationHUD.HideText();
            }
        }


    }
}
