using DialogueSystem.Controllers;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem.Views
{
    public class NotificationView : MonoBehaviour
    {
        private NotificationViewController myController { get; set; }
        private Text notificationText { get; set; }
        private Image notificationImage { get; set; }
        private float displaySpeed { get; set; }

        void Awake()
        {
            myController = this.gameObject.AddComponent<NotificationViewController>();
            notificationText = this.gameObject.GetComponentInChildren<Text>();
            notificationImage = this.gameObject.GetComponent<Image>();
            SetNotificationSettings();
        }

        void OnDisable()
        {
            notificationText.text = "";
        }

        public void DisplayNotificationText(string textToDisplay)
        {
           StartCoroutine(WriteTextToView(textToDisplay, notificationText));
        }

        private IEnumerator WriteTextToView(string textToDisplay,Text textToWriteTo)
        {
            for (int i = 0; i < textToDisplay.Length; i++)
            {
                textToWriteTo.text += textToDisplay[i];
                yield return new WaitForSeconds(displaySpeed);
            }
        }

        private void SetNotificationSettings()
        {
            displaySpeed = Constants.SetDialogueDisplaySpeed();
            if (DialogueSystemManager.Instance.dialogueFont != null)
                notificationText.font = DialogueSystemManager.Instance.dialogueFont;

            var notificationColor = new Color();
            notificationColor = notificationImage.color;
            notificationColor.a = DialogueSystemManager.Instance.alphaOfDialoguePanel;

            notificationImage.color = notificationColor;

            notificationImage.sprite = DialogueSystemManager.Instance.notificationBoxBackGroundImage;
            if ((int)DialogueSystemManager.Instance.notificationImageType == (int)Image.Type.Simple)
                notificationImage.type = Image.Type.Simple;
            else if ((int)DialogueSystemManager.Instance.notificationImageType == (int)Image.Type.Sliced)
                notificationImage.type = Image.Type.Sliced;
        }
    }
}