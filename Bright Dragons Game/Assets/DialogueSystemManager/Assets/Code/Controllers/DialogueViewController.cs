using UnityEngine;
using System.Collections;
using DialogueSystem.Models;
using System.Collections.Generic;
using DialogueSystem.Views;
using DialogueSystem.Services;

namespace DialogueSystem.Controllers
{
    public class DialogueViewController : MonoBehaviour
    {
        public DialogueView dialogueView { get; set; }

        public Dialogue currentDialogue { get; set; }

        void Awake()
        {
            dialogueView = this.gameObject.GetComponent<DialogueView>();
            DialogueViewManager.Instance.setDialogueInView += SetDialogue;
            DialogueViewManager.Instance.setCharacterPortrait += SetCharacterPortrait;
        }

        private void Start()
        {
             dialogueView.SetUpDialogueView(DialogueSystemManager.Instance.dialogueType);
        }

        public void ContinueToNextDialogue(Response response)
        {
            if (response == null)
                DialogueService.Instance.LoadNextDialogue();
            else //The only time a continue button should have a response is esstentially to take advantage of the ActionTaken on the continue button
                DialogueService.Instance.DetermineActionBasedOnResponse(response);
        }

        public void ActOnResponse(Response response)
        {
            DialogueService.Instance.DetermineActionBasedOnResponse(response);
        }

        private void SetCharacterPortrait(Sprite portrait,string displayName)
        {
            dialogueView.SetCharacterPortrait(portrait,displayName);
        }

        private void SetDialogue(Dialogue dialogue)
        {
            currentDialogue = dialogue;
            dialogueView.SetDialoguePanel(currentDialogue);
        }
    }
}