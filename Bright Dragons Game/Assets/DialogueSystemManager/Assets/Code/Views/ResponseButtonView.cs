using UnityEngine;
using System.Collections;
using DialogueSystem.Models;
using UnityEngine.UI;

namespace DialogueSystem.Views
{
    public class ResponseButtonView : MonoBehaviour
    {
        public Response response { get; set; }
        public Text responseText { get; set; }

        private Image imageComponent { get; set; }
        private Button buttonComponent { get; set; }

        private void Awake()
        {
            responseText = this.gameObject.GetComponentInChildren<Text>();
            imageComponent = this.gameObject.GetComponent<Image>();
            buttonComponent = this.gameObject.GetComponent<Button>();

            if (DialogueSystemManager.Instance.turnOffResponseButtonBackGroundImage)
                TurnOffBackGroundImage();

            SetTargetGraphicForHighlight();
            SetHighlightedAndTextColor();
            SetResponseButtonBGImage();
            SetImageType();
            SetResponseTextFont();

        }

        public void SetResponse(Response resp)
        {
            response = resp;
            responseText.text = response.text;
        }

        #region Helper Methods
        private void TurnOffBackGroundImage()
        {
            Color transparent = new Color(imageComponent.color.r, imageComponent.color.g, imageComponent.color.b, 0);
            imageComponent.color = transparent;
        }

        private void SetTargetGraphicForHighlight()
        {
            //This will set the target graphic to the text or button component depending on which one the user wants to highlight
            if (DialogueSystemManager.Instance.highlightedResponseArea == Constants.HighlightableType.text)
                buttonComponent.targetGraphic = responseText;
            else if (DialogueSystemManager.Instance.highlightedResponseArea == Constants.HighlightableType.button)
                buttonComponent.targetGraphic = imageComponent;
        }

        private void SetHighlightedAndTextColor()
        {
            //Sets up the highlighted value the user set. User needs to make sure the Alpha is not 0
            ColorBlock cb = buttonComponent.colors;
            if(DialogueSystemManager.Instance.highlightedResponseArea == Constants.HighlightableType.button)
            {
                cb.highlightedColor = DialogueSystemManager.Instance.highlightedResponseColor;
                cb.normalColor = DialogueSystemManager.Instance.responseButtonColor;
                responseText.color = DialogueSystemManager.Instance.responseButtonTextColor;
            }
            else if (DialogueSystemManager.Instance.highlightedResponseArea == Constants.HighlightableType.text)
            {
                cb.normalColor = DialogueSystemManager.Instance.responseButtonTextColor;
                cb.highlightedColor = DialogueSystemManager.Instance.highlightedResponseColor;
                imageComponent.color = DialogueSystemManager.Instance.responseButtonColor;
                cb.colorMultiplier = 3;
            }
            buttonComponent.colors = cb;
        }

        private void SetResponseButtonBGImage()
        {
            if (DialogueSystemManager.Instance.responseButtonBackGroundImage != null)
                imageComponent.sprite = DialogueSystemManager.Instance.responseButtonBackGroundImage;
        }

        private void SetImageType()
        {
            if ((int)DialogueSystemManager.Instance.responseButtonImageType == (int)Image.Type.Simple)
                imageComponent.type = Image.Type.Simple;
            else if ((int)DialogueSystemManager.Instance.responseButtonImageType == (int)Image.Type.Sliced)
                imageComponent.type = Image.Type.Sliced;
        }

        private void SetResponseTextFont()
        {
            if (DialogueSystemManager.Instance.responseButtonFont != null)
                responseText.font = DialogueSystemManager.Instance.responseButtonFont;
        }
        #endregion
    }
}