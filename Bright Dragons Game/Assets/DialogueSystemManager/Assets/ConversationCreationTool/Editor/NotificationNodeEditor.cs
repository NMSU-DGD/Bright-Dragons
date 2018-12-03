using UnityEngine;
using System.Collections;
using UnityEditor;
using DialogueSystem.Models;
using DialogueSystem.Data;
using System;
using System.IO;
using System.Collections.Generic;

public partial class NodeEditor : EditorWindow
{

    private void CreateNotificationNode()
    {
        notificationNode = CreateInstance<NotificationNode>();
        notificationNode.window = new Rect(20, 20, 200, 200);
        notificationNode.notification.id = (notificationContainer.notifications.Count + 1);
        notificationNode.nodeId = currentNodeIndex;
        currentNodeIndex++;
        windows.Add(notificationNode);
    }

    private void SaveNotificationToXml()
    {
        GenericXmlLoader<NotificationsContainer> loader = new GenericXmlLoader<NotificationsContainer>();
        Debug.Log("Saving out Notification");

        notificationContainer.notifications.Add(notificationNode.notification);

        loader.SaveXMLFile(notificationContainer, "Assets/DialogueSystemManager/Resources/XML/Notifications.xml"); //TODO: Put string in Constants file
         
        AssetDatabase.Refresh();

        InitializeBlankEditor();
    }
}
