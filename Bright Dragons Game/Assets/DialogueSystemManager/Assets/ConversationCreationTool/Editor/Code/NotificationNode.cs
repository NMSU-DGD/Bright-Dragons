using UnityEngine;
using System.Collections;
using DialogueSystem.Models;
using System;
using UnityEditor;
using System.Collections.Generic;

public class NotificationNode : BaseNode
{
    public Notification notification { get; set; }

    public NotificationNode()
    {
        notification = new Notification();
        windowTitle = "Notification";
        notification.id = 0;
        notification.name = "";
        notification.starter = "";
        notification.text = "";
        notification.description = "";
    }

    public override void DrawWindow()
    {
        base.DrawWindow();
        GUI.skin.textArea.wordWrap = true;
        notification.name = base.TextField("Name: ", notification.name);
        notification.starter = base.TextField("Starter: ", notification.starter);
        EditorGUILayout.LabelField("Description: ");
        notification.description = EditorGUILayout.TextArea(notification.description);
        EditorGUILayout.LabelField("Text: "); //TODO: Add a guilayout skin that will allow for word wrap
        notification.text = EditorGUILayout.TextArea(notification.text, GUILayout.ExpandHeight(true));
    }
}
