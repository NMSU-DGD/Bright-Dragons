using UnityEngine;
using System.Collections;
using System;
using DialogueSystem.Models;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class DialogueNode : BaseNode
{
    public Dialogue dialogue { get; set; }

    public delegate void AddDialogueNodeHandler(BaseNode baseNode);
    public event AddDialogueNodeHandler addDialogueEvent;
    public delegate void AddResponseNodeHandler(int responseId, int dialogueId,bool isContinueResponse);
    public event AddResponseNodeHandler addResponseEvent;

    public int attachedResponses { get; set; }

    public bool attachedDialogue { get; set; }

    public bool continueResponseAttached { get; set; }

    public List<int> ResponseIDs { get; set; }

    public DialogueNode()
    {
        dialogue = new Dialogue();
        windowTitle = "Dialogue";
        dialogue.id = 0;
        dialogue.responses = new List<Response>();
        dialogue.text = "";
        attachedDialogue = false;
        attachedResponses = 0;
        continueResponseAttached = false;
        ResponseIDs = new List<int>() { 1, 2, 3 };
    }
    public override void DrawWindow()
    {
        base.DrawWindow();
        EditorGUILayout.LabelField("Id: " + dialogue.id);
        EditorGUILayout.LabelField("Text: ");
        dialogue.text = EditorGUILayout.TextArea(dialogue.text);
        AddResponseButtonIfAble();
        AddContinueResponseButtonIfAble();
        AddDialogueButtonIfNoAttachedDialoguesOrResponses();
    }

    public void SetID(int id)
    {
        dialogue.id = id;
    }

    public void AddResponse(Response response)
    {
        dialogue.responses.Add(response);
    }

    public void AddEndingResponseIfDialogueHasNoResponses()
    {
        if (!attachedDialogue && attachedResponses == 0)
        {
            Response response = new Response();
            response.nextDialogueId = 0;
            response.text = " ";
            response.Id = 1;

            this.AddResponse(response);
        }
    }

    #region Helper Methods
    private void AddResponseButtonIfAble()
    {
        if (attachedResponses < 3 && !attachedDialogue && !continueResponseAttached)
        {
            if (GUILayout.Button("Add Response"))
            {
                EditorConstants.windowsToAttach.Add(nodeId);
                isParentToAttachedNode = true;

                if (addResponseEvent != null)
                    addResponseEvent(ResponseIDs.First(),dialogue.id,false);
                ResponseIDs.RemoveAt(0);
                attachedResponses++;
            }
        }
    }

    private void AddContinueResponseButtonIfAble()
    {
        if(attachedResponses == 0 && !attachedDialogue && !continueResponseAttached)
        {
            if(GUILayout.Button("Add Continue Response"))
            {
                EditorConstants.windowsToAttach.Add(nodeId);
                isParentToAttachedNode = true;

                if (addResponseEvent != null)
                    addResponseEvent((attachedResponses + 1), dialogue.id,true);
                attachedResponses++;
                continueResponseAttached = true;
            }
        }
    }

    private void AddDialogueButtonIfNoAttachedDialoguesOrResponses()
    {
        if (!attachedDialogue && attachedResponses == 0)
        {
            if (GUILayout.Button("Add Dialogue"))
            {
                EditorConstants.windowsToAttach.Add(nodeId);
                isParentToAttachedNode = true;

                if (addDialogueEvent != null)
                    addDialogueEvent(this);

                attachedDialogue = true;
            }
        }
    }
    #endregion
}
