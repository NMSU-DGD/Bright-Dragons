using UnityEngine;
using System.Collections;
using DialogueSystem.Models;
using DialogueSystem.Views;
using DialogueSystem;

public class DialogueViewManager : Singleton<DialogueViewManager>
{
    public DialogueView dialogueView { get; set; }

    public delegate void DialogueEventHandler(Dialogue dialogue);
    public event DialogueEventHandler setDialogueInView;

    public delegate void SetCharacterPortraitHandler(Sprite portrait,string displayName);
    public event SetCharacterPortraitHandler setCharacterPortrait;

    private void Awake()
    {
        dialogueView = GameObject.Find("DialogueView").GetComponent<DialogueView>();
    }

    public void Start()
    {
        SetDialogueViewActive(false);
    }

    public void LoadDialogue(Dialogue dialogue,Sprite portrait = null,string displayName = null)
    {
        SetDialogueViewActive(true);
        setCharacterPortrait(portrait,displayName);
        setDialogueInView(dialogue);
    }

    public void CloseDialogueView()
    {
        SetDialogueViewActive(false);
    }

    #region Helper Methods
    private void SetDialogueViewActive(bool isActive)
    {
        dialogueView.gameObject.SetActive(isActive);
    }
    #endregion
}
