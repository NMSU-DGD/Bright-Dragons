using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DialogueSystem.Models;
using System;
using DialogueSystem.Controllers;
using System.Linq;
using UnityEngine.EventSystems;

namespace DialogueSystem.Views
{
    public class DialogueView : MonoBehaviour
    {
        private Text dialogueText { get; set; }
        private Image characterPortrait { get; set; }
        private Text characterName { get; set; }
        private List<ResponseButtonView> responseButtonViews { get; set; }
        private DialogueViewController controller { get; set; }
        private ContinueButtonView continueButtonView { get; set; }
        private float displaySpeed { get; set; }
        private int buttonIndex { get; set; }
        private Button selectedButton { get; set; }
        private EventSystem eventSystem { get; set; }
        private bool alreadyContinued { get; set; }


        private void Start()
        {
            eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        }

        private void Update()
        {
            if(responseButtonViews[0].gameObject.activeInHierarchy)
            {
                UseCorrectInputsBasedOnResponseInputType();
            }
            if(continueButtonView.gameObject.activeInHierarchy)
            {
                selectedButton = continueButtonView.GetComponent<Button>();
                selectedButton.Select();
            }
        }

        public void SetUpDialogueView(Constants.DialogueType dialogueType)
        {
            InitalizeView(dialogueType);
        }

        public void SetDialoguePanel(Dialogue dialogue)
        {
            if (DialogueSystemManager.Instance.displaySpeedForDialogue == Constants.DialogueDisplaySpeed.instant)
            {
                dialogueText.text = dialogue.text;
                ShowCorrectResponseButtonSet(dialogue.responses);
            }
            else
                StartCoroutine(DisplayDialogueInPanel(dialogue));
        }

        public void SetCharacterPortrait(Sprite portrait,string displayName)
        {
            if (portrait != null)
                characterPortrait.sprite = portrait;
            else
                characterPortrait.sprite = null;
            Color col = characterPortrait.color;
            col.a = DialogueSystemManager.Instance.alphaOfDialoguePanel;
            characterPortrait.color = col;
            SetCharacterName(displayName);
        }

        #region Helper Methods
        private void SetCharacterName(string name)
        {
            characterName.text = name;
            characterName.color = DialogueSystemManager.Instance.dialogueTextColor;
            if (DialogueSystemManager.Instance.dialogueFont != null)
                characterName.font = DialogueSystemManager.Instance.dialogueFont;
        }

        private void InitalizeView(Constants.DialogueType dialogueType)
        {
            DetermineDialogueConfigurations(dialogueType);
            SetUpDialoguePanel();
            //Setting the response buttons and then turning them off
            SetUpResponseButtons();
            TurnOffResponseButtons();
            SetUpContinueButton();
            SetButtonListeners();
            displaySpeed = Constants.SetDialogueDisplaySpeed();

            controller = this.gameObject.GetComponent<DialogueViewController>();
        }
        #region Set Up Methods
        private void DetermineDialogueConfigurations(Constants.DialogueType dialogueType)
        {
            Text largerDialogueTextBox = GameObject.Find("LargerDialogueText").GetComponent<Text>();
            Text smallerDialogueTextBox = GameObject.Find("SmallerDialogueText").GetComponent<Text>();
            characterName = GameObject.Find("CharacterName").GetComponent<Text>();
            characterPortrait = GameObject.Find("CharacterPortrait").GetComponent<Image>();
            if (dialogueType == Constants.DialogueType.responsesWithPortrait)
            {
                dialogueText = smallerDialogueTextBox;
                largerDialogueTextBox.gameObject.SetActive(false);
                if (!DialogueSystemManager.Instance.showCharacterName)
                    characterName.gameObject.SetActive(false);
            }
            if (dialogueType == Constants.DialogueType.responsesWithNoPortrait)
            {
                dialogueText = largerDialogueTextBox;
                characterName.gameObject.SetActive(false);
                characterPortrait.gameObject.SetActive(false);
                smallerDialogueTextBox.gameObject.SetActive(false);
            }

            dialogueText.color = DialogueSystemManager.Instance.dialogueTextColor;
            if (DialogueSystemManager.Instance.dialogueFont != null)
                dialogueText.font = DialogueSystemManager.Instance.dialogueFont;
        }

        private void SetUpDialoguePanel()
        {
            Image dialoguePanelImage = GameObject.Find("DialoguePanel").GetComponent<Image>();
            if (DialogueSystemManager.Instance.dialogueBoxBackGroundImage != null)
                dialoguePanelImage.sprite = DialogueSystemManager.Instance.dialogueBoxBackGroundImage;

            if ((int)DialogueSystemManager.Instance.dialogueImageType == (int)Image.Type.Simple)
                dialoguePanelImage.type = Image.Type.Simple;
            else if ((int)DialogueSystemManager.Instance.dialogueImageType == (int)Image.Type.Sliced)
                dialoguePanelImage.type = Image.Type.Sliced;
            Color col = dialoguePanelImage.color;
            col.a = DialogueSystemManager.Instance.alphaOfDialoguePanel;
            dialoguePanelImage.color = col;
        }

        private void SetUpContinueButton()
        {
            continueButtonView = GameObject.FindObjectOfType<ContinueButtonView>();
            //If value is override to make continue button bigger, do that here
            float viewWidth = this.gameObject.GetComponent<RectTransform>().rect.width;
            continueButtonView.SetScaleOfContinueButton(viewWidth);
            continueButtonView.gameObject.SetActive(false);
        }

        private void SetUpResponseButtons()
        {
            responseButtonViews = GameObject.FindObjectsOfType<ResponseButtonView>().OrderBy(x => x.transform.GetSiblingIndex()).ToList();
        }

        private void SetButtonListeners()
        {
            for (int i = 0; i <= 2; i++)
            {
                ResponseButtonView buttonView = responseButtonViews[i];
                Button button = responseButtonViews[i].GetComponent<Button>();
                button.onClick.AddListener(delegate { GetResponse(buttonView); });
            }
            continueButtonView.buttonComponent.onClick.AddListener(delegate { ContinueDialogue(); });
        }

        #endregion

        #region Coroutines
        private IEnumerator DisplayDialogueInPanel(Dialogue dialogue)
        {
            for (int i = 0; i < dialogue.text.Length; i++)
            {
                dialogueText.text += dialogue.text[i];
                yield return new WaitForSeconds(displaySpeed);
            }
            ShowCorrectResponseButtonSet(dialogue.responses);
        }

        private IEnumerator ConintueDialogueAutomatically()
        {
            yield return new WaitForSeconds(DialogueSystemManager.Instance.timetillProgress);
            ContinueDialogue();
        }
        #endregion

        private void SetResponses(List<Response> responses)
        {
            buttonIndex = 0;
            eventSystem.SetSelectedGameObject(null);
            for (int numOfResponse = 0; numOfResponse < responses.Count; numOfResponse++)
            {
                if (numOfResponse <= 2)
                {
                    responseButtonViews[numOfResponse].gameObject.SetActive(true);
                    responseButtonViews[numOfResponse].SetResponse(responses[numOfResponse]);
                }
            }
        }

        private void ShowContinueButton(bool isShown)
        {
            if (DialogueSystemManager.Instance.haveConversationsAutoContinueIfNoResponses && !alreadyContinued)
            {
                alreadyContinued = true;
                StartCoroutine(ConintueDialogueAutomatically());
            }
            else
                continueButtonView.gameObject.SetActive(isShown);
        }

        private void SetResponseForContinueButton(Response response)
        {
            continueButtonView.SetResponse(response);
        }

        private void ShowCorrectResponseButtonSet(List<Response> responses)
        {
            alreadyContinued = false;
            if (responses.Count == 0)
                ShowContinueButton(true);
            else if (responses.Count == 1 && responses[0].text.Trim() == "")
            {
                ShowContinueButton(true);
                SetResponseForContinueButton(responses[0]);
            }
            else if (responses.Count > 0)
                SetResponses(responses);
        }

        private void GetResponse(ResponseButtonView responseButtonPressed)
        {
            ResetDialogueAndResponses();
            TurnOffResponseButtons();
            controller.ActOnResponse(responseButtonPressed.response);
        }

        private void TurnOffResponseButtons()
        {
            foreach(var button in responseButtonViews)
            {
                button.gameObject.SetActive(false);
            }
        }

        private void ContinueDialogue()
        {
            ResetDialogueAndResponses();
            ShowContinueButton(false);
            Response response = continueButtonView.response;
            SetResponseForContinueButton(null);
            controller.ContinueToNextDialogue(response);
            StopCoroutine(ConintueDialogueAutomatically());
        }

        private void ResetDialogueAndResponses()
        {
            dialogueText.text = "";
            foreach(var button in responseButtonViews)
            {
                button.responseText.text = "";
            }
        }

        private void UseCorrectInputsBasedOnResponseInputType()
        {
            if (DialogueSystemManager.Instance.dialogueSelectType == Constants.DialogueSelectType.UpAndDown)
            {
                if (Input.GetKeyDown(KeyCode.DownArrow) && buttonIndex < (responseButtonViews.Count - 1))
                    buttonIndex++;
                else if (Input.GetKeyDown(KeyCode.UpArrow) && buttonIndex > 0)
                    buttonIndex--;
                if (responseButtonViews[buttonIndex].gameObject.activeInHierarchy)
                {
                    selectedButton = responseButtonViews[buttonIndex].GetComponent<Button>();
                    selectedButton.Select();
                }
            }
            else if (DialogueSystemManager.Instance.dialogueSelectType == Constants.DialogueSelectType.MouseAndKeyboard)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                    GetResponse(responseButtonViews[0]);
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                    GetResponse(responseButtonViews[1]);
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                    GetResponse(responseButtonViews[2]);
            }
        }
        #endregion
    }
}