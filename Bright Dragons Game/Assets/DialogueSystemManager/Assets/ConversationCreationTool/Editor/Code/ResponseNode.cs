using UnityEngine;
using System.Collections;
using System;
using DialogueSystem.Models;
using UnityEditor;

public class ResponseNode : BaseNode
{
    public Response response { get; set; }

    public bool continueResponse { get; set; }

    public int parentDialogueId { get; set; }

    public delegate void AddDialogueEventHandler(BaseNode baseNode);
    public event AddDialogueEventHandler addDialogueEvent;
    public delegate void AddExisitingDialogueEventHandler(int id);
    public event AddExisitingDialogueEventHandler addExistingDialogueEvent;

    public bool dialogueHookedUp { get; set; }


    public ResponseNode()
    {
        response = new Response();
        windowTitle = "Response";
        response.actionTaken = "";
        response.conversationParentId = 0;
        response.Id = 0;
        response.nextDialogueId = 0;
        response.text = "";
        continueResponse = false;
        dialogueHookedUp = false;
    }

    public override void DrawWindow()
    {
        base.DrawWindow();
        EditorGUILayout.LabelField("Id: " + response.Id);
        if(!continueResponse)
        {
            response.text = base.TextField("Text: ", response.text);
        }
        response.actionTaken = base.TextField("ActionTaken: ", response.actionTaken);
        if (!dialogueHookedUp && !continueResponse)
            response.nextDialogueId = base.IntField("Route to existing dialogue Id: ", response.nextDialogueId);
        else
            GUILayout.Label("Next Dialouge Id: " + response.nextDialogueId.ToString());

        if (response.nextDialogueId == 0 && !dialogueHookedUp)
        {
            if (GUILayout.Button("Add New Dialogue"))
            {
                response.nextDialogueId = EditorConstants.dialogueIdIndex;
                dialogueHookedUp = true;
                EditorConstants.windowsToAttach.Add(nodeId);
                isParentToAttachedNode = true;
                if (addDialogueEvent != null)
                    addDialogueEvent(this);
            }
        }
        else if(response.nextDialogueId != 0 && !dialogueHookedUp)
        {
            if (GUILayout.Button("Route To"))
            {
                dialogueHookedUp = true;
                EditorConstants.windowsToAttach.Add(nodeId);
                if (addExistingDialogueEvent != null)
                    addExistingDialogueEvent(response.nextDialogueId);
            }
        }
    }

    public void SetId(int id)
    {
        response.Id = id;
    }
}
