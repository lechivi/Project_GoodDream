using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationPanel : MonoBehaviour
{
    [SerializeField] private NotificationHUD notificationHUD;

    public NotificationHUD NotificationHUD => this.notificationHUD;
}
