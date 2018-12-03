using DialogueSystem.Controllers;
using UnityEngine;

namespace DialogueSystem.Managers
{
    public class NotificationViewManager : Singleton<NotificationViewManager>
    {
        NotificationViewController notificationViewController { get; set; }

        void Start()
        {
            notificationViewController = GameObject.FindObjectOfType<NotificationViewController>();
            notificationViewController.gameObject.SetActive(false);
        }

        public void LoadNotificationToView(Notification notification)
        {
            notificationViewController.gameObject.SetActive(true);
            notificationViewController.DisplayNotificiation(notification);
        }

        public void CloseNotificiation()
        {
            notificationViewController.gameObject.SetActive(false);
        }
    }
}