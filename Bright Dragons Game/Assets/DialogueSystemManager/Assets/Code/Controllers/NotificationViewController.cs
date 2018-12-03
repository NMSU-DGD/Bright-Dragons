using DialogueSystem.Views;
using UnityEngine;

namespace DialogueSystem.Controllers
{
    public class NotificationViewController : MonoBehaviour
    {
        private NotificationView myView { get; set; }

        void Awake()
        {
            myView = this.gameObject.GetComponent<NotificationView>();
        }

        public void DisplayNotificiation(Notification notification)
        {
            myView.DisplayNotificationText(notification.text);
        }
    }
}