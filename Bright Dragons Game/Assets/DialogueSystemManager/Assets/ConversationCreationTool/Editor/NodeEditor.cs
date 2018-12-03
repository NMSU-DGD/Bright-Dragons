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
    private List<BaseNode> windows { get; set; }
    private List<int> attachedWindows { get; set; }

    private int currentNodeIndex { get; set; }
    private ConversationNode conversationNode { get; set; }
    private NotificationNode notificationNode { get; set; }
    private List<DialogueNode> dialogueNodes { get; set; }
    private List<ResponseNode> responseNodes { get; set; }
    private ConversationsContainer conversationContainer { get; set; }
    private NotificationsContainer notificationContainer { get; set; }
    private bool conversationsLoaded = false;
    private bool notificationsLoaded = false;

    private BaseNode selectedNode { get; set; }

    private Vector2 mousePos { get; set; }

    [MenuItem("Window/Conversation Node Editor")]
    static void ShowEditor()
    {
        NodeEditor editor = EditorWindow.GetWindow<NodeEditor>();
        editor.InitializeBlankEditor();
    }

    #region Initializing the Editor
    public void InitializeBlankEditor()
    {
        GenericXmlLoader<ConversationsContainer> conversationLoader = new GenericXmlLoader<ConversationsContainer>();
        GenericXmlLoader<NotificationsContainer> notificationLoader = new GenericXmlLoader<NotificationsContainer>();
        if(!conversationsLoaded)
        {
            conversationContainer = conversationLoader.LoadXMLFromResourcesFolder(Constants.ResourcesPathToConversationsXML,"No Conversation.XML found, will create one after saving first conversation");
            if (conversationContainer.Conversations == null)
                conversationContainer.Conversations = new List<Conversation>();
            conversationsLoaded = true;
        }
        if(!notificationsLoaded)
        {
            notificationContainer = notificationLoader.LoadXMLFromResourcesFolder(Constants.ResourcesPathToNotificationsXML, "No Notifications.XML found, will create  one after saving first notification");
            if (notificationContainer.notifications == null)
                notificationContainer.notifications = new List<Notification>();
            notificationsLoaded = true;
        }
        InitializeDefaultValues();
    }

    private void InitializeDefaultValues()
    {
        attachedWindows = new List<int>();
        windows = new List<BaseNode>();
        dialogueNodes = new List<DialogueNode>();
        responseNodes = new List<ResponseNode>();
        currentNodeIndex = 0;
        conversationNode = null;
        notificationNode = null;
        selectedNode = null;
        EditorConstants.windowsToAttach = new List<int>();
        EditorConstants.dialogueIdIndex = 1;
    }
    #endregion

    void OnGUI()
    {
        Event e = Event.current;
        mousePos = e.mousePosition;

        ShowNodeContextMenuOnRightClickOverNode(e, mousePos);

        AddWindowsToBeAttached();

        DrawCurvesBetweenAttachedNodes();

        ShowContextMenuOnRightClick();

        DrawWindowsInNodeEditor();
    }
    #region Draw Methods

    private void DrawWindowsInNodeEditor()
    {
        BeginWindows();

        for (int i = 0; i < windows.Count; i++)
        {
            windows[i].window = GUI.Window(i, windows[i].window, DrawNodeWindow, windows[i].windowTitle);
        }

        EndWindows();
    }

    private void DrawNodeWindow(int id)
    {
        windows[id].DrawWindow();
        GUI.DragWindow();
    }


    private void DrawNodeCurve(Rect start, Rect end)
    {
        Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
        Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
        Vector3 startTan = startPos + Vector3.right * 50;
        Vector3 endTan = endPos + Vector3.left * 50;
        Color shadowCol = new Color(0, 0, 0, 0.06f);

        for (int i = 0; i < 3; i++)
        {// Draw a shadow
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
        }

        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
    }

    private void AddWindowsToBeAttached()
    {
        if (EditorConstants.windowsToAttach != null && EditorConstants.windowsToAttach.Count == 2)
        {
            attachedWindows.Add(EditorConstants.windowsToAttach[0]);
            attachedWindows.Add(EditorConstants.windowsToAttach[1]);
            EditorConstants.windowsToAttach.Clear();
        }
    }

    private void DrawCurvesBetweenAttachedNodes()
    {
        if (attachedWindows.Count >= 2)
        {
            for (int i = 0; i < attachedWindows.Count; i += 2)
            {
                BaseNode firstNode = windows.Find(x => x.nodeId == attachedWindows[(i)]);
                BaseNode secondNode = windows.Find(x => x.nodeId == attachedWindows[(i + 1)]);
                DrawNodeCurve(firstNode.window, secondNode.window);
            }
        }
    }

    #endregion

    #region Helper Methods

    #region Context Menu Building Methods
    private void ShowContextMenuOnRightClick()
    {
        if (Event.current.type == EventType.ContextClick)
        {
            GenericMenu menu = new GenericMenu();
            if (conversationNode == null && notificationNode == null)
            {
                menu.AddItem(new GUIContent("Create Conversation Node"), false, CreateConversationNode);
                menu.AddItem(new GUIContent("Create Notification Node"), false, CreateNotificationNode);
            }
            else if (conversationNode != null)
                menu.AddItem(new GUIContent("Save Conversation"), false, SaveConversationToXml);
            else if (notificationNode != null)
                menu.AddItem(new GUIContent("Save Notification"), false, SaveNotificationToXml);

            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Clear Editor"), false, InitializeBlankEditor);
            menu.ShowAsContext();
            Event.current.Use();
        }
    }

    private void ShowNodeContextMenuOnRightClickOverNode(Event e, Vector2 mousePos)
    {
        if (e.type == EventType.MouseDown && e.button == 1 && windows.Count != 0)
        {
            GenericMenu menu = new GenericMenu();
            for (int i = 0; i < windows.Count; i++)
            {
                if (windows[i].window.Contains(mousePos) && windows[i].isParentToAttachedNode == false && windows[i].nodeId != 0)
                {
                    selectedNode = windows[i];
                    menu.AddItem(new GUIContent("Delete Node"), false, DeleteSelectedNode);
                    menu.ShowAsContext();
                    Event.current.Use();
                    break;
                }
            }
        }
    }
    #endregion

    //TODO: Go Into COnversationNodeEditor Partial
    private void CreateConversationNode()
    {
        conversationNode = CreateInstance<ConversationNode>();
        conversationNode.window = new Rect(10, 10, 200, 200);
        conversationNode.addDialogueEvent += CreateDialogueNode;
        conversationNode.conversation.id = (conversationContainer.Conversations.Count+1);
        conversationNode.nodeId = currentNodeIndex;
        currentNodeIndex++;
        windows.Add(conversationNode);
    }

    //TODO: GO into DialogueNode Editor Partial
    private void CreateDialogueNode(BaseNode parentNode)
    {
        DialogueNode dialogueNode = CreateInstance<DialogueNode>();
        dialogueNode.parentNode = parentNode;
        conversationNode.AddDialogue(dialogueNode.dialogue);
        dialogueNode.SetID(EditorConstants.dialogueIdIndex);
        EditorConstants.dialogueIdIndex++;
        dialogueNode.window = new Rect(parentNode.window.x + 175, parentNode.window.y, 175, 150);
        dialogueNode.addDialogueEvent += CreateDialogueNode;
        dialogueNode.addResponseEvent += CreateResponseNode;
        dialogueNode.nodeId = currentNodeIndex;
        currentNodeIndex++;
        dialogueNodes.Add(dialogueNode);
        windows.Add(dialogueNode);

        EditorConstants.windowsToAttach.Add(dialogueNode.nodeId);
    }

    //TODO: Go into a Response Node Editor Partial
    private void CreateResponseNode(int responseId, int dialogueId, bool isContinueResponse)
    {
        ResponseNode responseNode = CreateInstance<ResponseNode>();
        responseNode.continueResponse = isContinueResponse;
        responseNode.parentDialogueId = dialogueId;
        DialogueNode dialogeNode = dialogueNodes.Find(x => x.dialogue.id == dialogueId);
        dialogeNode.AddResponse(responseNode.response);
        responseNode.parentNode = dialogeNode;
        responseNode.SetId(responseId);
        responseNode.window = new Rect(dialogeNode.window.x+215, dialogeNode.window.y, 215, 150);
        responseNode.addDialogueEvent += CreateDialogueNode;
        responseNode.addExistingDialogueEvent += AddExisitingDialogueEventToResponse;
        responseNode.nodeId = currentNodeIndex;
        currentNodeIndex++;
        responseNodes.Add(responseNode);
        windows.Add(responseNode);

        EditorConstants.windowsToAttach.Add(responseNode.nodeId);
    }

    private void AddExisitingDialogueEventToResponse(int id)
    {
        DialogueNode dialogueNode = dialogueNodes.Find(x => x.dialogue.id == id);

        EditorConstants.windowsToAttach.Add(dialogueNode.nodeId);
    }

    private void SaveConversationToXml()
    {
        GenericXmlLoader<ConversationsContainer> loader = new GenericXmlLoader<ConversationsContainer>();
        Debug.Log("Saving out Conversation");
        foreach(var node in dialogueNodes)
        {
            node.AddEndingResponseIfDialogueHasNoResponses();
        }

        conversationContainer.Conversations.Add(conversationNode.conversation);

        loader.SaveXMLFile(conversationContainer, "Assets/DialogueSystemManager/Resources/XML/Conversations.xml");

        AssetDatabase.Refresh();

        InitializeBlankEditor();
    }

    #region Deleting Nodes and Information they have attached
    private void DeleteSelectedNode()
    {
        for (int i = 0; i < windows.Count; i++)
        {
            if (windows[i] == selectedNode)
            {
                RemoveAttachedLinesFromSelectedWindow();
                windows.Remove(selectedNode);
                DialogueNode dialogueNode = dialogueNodes.Find(x => x.nodeId == selectedNode.nodeId);
                if (dialogueNode != null)
                    DeleteDialogueNodeFromItsParentScope(dialogueNode);
                ResponseNode responseNode = responseNodes.Find(x => x.nodeId == selectedNode.nodeId);
                if (responseNode != null)
                    DeleteResponseNodeFromItsParentScope(responseNode);
                break;
            }
        }
    }

    private void RemoveAttachedLinesFromSelectedWindow()
    {
        //Removes Parent Node Attachment line
        int index = attachedWindows.FindIndex(x => x.Equals(selectedNode.nodeId));
        attachedWindows.RemoveAt(index);
        attachedWindows.RemoveAt(index - 1);

        //Need to do this because we can delete rerouted items. If selected node is a rerouted item we want to delete both line attachments
        index = attachedWindows.FindIndex(x => x.Equals(selectedNode.nodeId));
        if (index != -1)
        {
            attachedWindows.RemoveAt(index + 1);
            attachedWindows.RemoveAt(index);
        }
    }

    private void DeleteDialogueNodeFromItsParentScope(DialogueNode dialogueNode)
    {
        conversationNode.conversation.dialogues.Remove(dialogueNode.dialogue);
        if(conversationNode.conversation.dialogues.Count == 0)
            conversationNode.Reset();
        dialogueNodes.Remove(dialogueNode);
        EditorConstants.dialogueIdIndex--;
        //If the deleted nodes parent is a dialogue node then we need to reset things in the parent dialogue node
        DialogueNode partentDialogueNode = dialogueNodes.Find(x => x.nodeId == dialogueNode.parentNode.nodeId);
        if(partentDialogueNode != null)
        {
            partentDialogueNode.isParentToAttachedNode = false;
            partentDialogueNode.attachedDialogue = false;
        }
        //If the deleted nodes parent is a response dialogue node then we need to reset things in the parent response node
        ResponseNode parentResponseNode = responseNodes.Find(x => x.nodeId == dialogueNode.parentNode.nodeId);
        if(parentResponseNode != null)
        {
            parentResponseNode.isParentToAttachedNode = false;
            parentResponseNode.dialogueHookedUp = false;
            parentResponseNode.response.nextDialogueId = 0;
        }

    }

    private void DeleteResponseNodeFromItsParentScope(ResponseNode responseNode)
    {
        responseNodes.Remove(responseNode);
        DialogueNode dialogueNode = dialogueNodes.Find(x => x.nodeId == responseNode.parentNode.nodeId);
        if(dialogueNode != null)
        {
            dialogueNode.dialogue.responses.Remove(responseNode.response);
            dialogueNode.ResponseIDs.Add(responseNode.response.Id);
            dialogueNode.attachedResponses--;
            if (dialogueNode.attachedResponses == 0)
            {
                dialogueNode.isParentToAttachedNode = false;
                dialogueNode.continueResponseAttached = false;
            }

        }
    }
    #endregion

    #endregion
}
