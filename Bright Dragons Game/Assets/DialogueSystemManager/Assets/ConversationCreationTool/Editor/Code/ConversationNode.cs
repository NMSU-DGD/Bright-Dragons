using UnityEngine;
using System.Collections;
using DialogueSystem.Models;
using System;
using UnityEditor;
using System.Collections.Generic;

public class ConversationNode : BaseNode
{
    public Conversation conversation { get; set; }

    public delegate void AddDialogueHandler(BaseNode myBaseNode);
    public event AddDialogueHandler addDialogueEvent;

    public bool startDialogueAttached { get; set; }

    public ConversationNode()
    {
        conversation = new Conversation();
        windowTitle = "Conversation";
        conversation.name = "";
        conversation.starter = "";
        conversation.description = "";
        conversation.isRepeatable = false;
        conversation.isCompleted = false;
        conversation.isLocked = false;
        conversation.phase = 0;
        startDialogueAttached = false;
        conversation.dialogues = new List<Dialogue>();
    }

    public override void DrawWindow()
    {
        base.DrawWindow();
        conversation.name = base.TextField("Name: ", conversation.name);
        conversation.starter = base.TextField("Starter: ", conversation.starter);
        EditorGUILayout.LabelField("Description: ");
        conversation.description = EditorGUILayout.TextArea(conversation.description);
        conversation.isRepeatable = base.ToggleField("Repeatable: ", conversation.isRepeatable);
        conversation.isLocked = base.ToggleField("Locked: ", conversation.isLocked);

        if (conversation.isLocked)
        {
            conversation.phase = base.IntField("Phase to unlock: ", conversation.phase);
        }

        if(!startDialogueAttached)
        {
            if (GUILayout.Button("Add Dialogue"))
            {
                EditorConstants.windowsToAttach.Add(nodeId);
                isParentToAttachedNode = true;

                if (addDialogueEvent != null)
                    addDialogueEvent(this);

                startDialogueAttached = true;
            }
        }
    }

    public void AddDialogue(Dialogue dialogue)
    {
        conversation.dialogues.Add(dialogue);
    }

    public void Reset()
    {
        startDialogueAttached = false;
    }
}
