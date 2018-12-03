using UnityEngine;
using System.Collections;
using DialogueSystem.Models;
using UnityEngine.UI;

namespace DialogueSystem.Views
{
    public class ContinueButtonView : MonoBehaviour
    {
        public Response response { get; set; }
        public Button buttonComponent { get; set; }
        private Image imageComponent { get; set; }
        private Text continueButtonText { get; set; }


        private void Awake()
        {
            buttonComponent = this.gameObject.GetComponent<Button>();
            imageComponent = buttonComponent.GetComponent<Image>();
            continueButtonText = buttonComponent.GetComponentInChildren<Text>();
        }

        private void Start()
        {
            SetContinueButtonBGImage();
            SetContinueButtonTextAndColor();
            SetContinueButtonFont();
        }

        public void SetResponse(Response resp)
        {
            response = resp;
        }

       #region Helper Methods
        private void SetContinueButtonBGImage()
        {
            if (DialogueSystemManager.Instance.continueDialogueButtonSprite != null)
                imageComponent.sprite = DialogueSystemManager.Instance.continueDialogueButtonSprite;
        }

        private void SetContinueButtonTextAndColor()
        {
            continueButtonText.text = DialogueSystemManager.Instance.continueButtonText;
            continueButtonText.color = DialogueSystemManager.Instance.responseButtonTextColor;
        }

        public void SetScaleOfContinueButton(float viewWidth)
        {
            if (DialogueSystemManager.Instance.widthOfContinueButton != 0)
                buttonComponent.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left,
                    (viewWidth - DialogueSystemManager.Instance.widthOfContinueButton - 20), DialogueSystemManager.Instance.widthOfContinueButton);
        }

        private void SetContinueButtonFont()
        {
            if (DialogueSystemManager.Instance.continueButtonFont != null)
                continueButtonText.font = DialogueSystemManager.Instance.continueButtonFont;
        }
        #endregion 
    }
}