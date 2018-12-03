using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem.Models;
using DialogueSystem.Data;
using DialogueSystem.Managers;
using System.Linq;
using System.IO;
using System;

namespace DialogueSystem.Services
{
    public class DialogueService : Singleton<DialogueService>
    {

        private static List<Conversation> conversations = new List<Conversation>();
        private static List<Notification> notifications = new List<Notification>();
        string dialogueSystemFolder = "";
        public List<Sprite> characterPortraits = new List<Sprite>();

        void Awake()
        {
            if (DialogueSystemManager.Instance.saveToMyDocuments)
                dialogueSystemFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/DialogueSystemManager";
            else if (DialogueSystemManager.Instance.saveToPersistentDataFile)
                dialogueSystemFolder = Application.persistentDataPath + "/DialogueSystemManager";
            else
                dialogueSystemFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/DialogueSystemManager";
        }

        public void Load()
        {
            LoadFilesForSystem();
            characterPortraits = Resources.LoadAll<Sprite>(Constants.ResourcesFolderForCharacterPortraits).ToList();
        }

        public Sprite GetPortraitByName(string name)
        {
            return characterPortraits.Find(x => x.name.ToLower() == name.ToLower());
        }

        public Conversation LoadConversationForNpc(string nameOfStarter)
        {
            List<Conversation> conversationsWithNPC = null;
            if (conversations != null)
                conversationsWithNPC = conversations.FindAll(convo => convo.starter.ToLower() == nameOfStarter.ToLower()).OrderBy(x => x.id).ToList();

            if (conversationsWithNPC == null)
                return null;

            foreach (var convo in conversationsWithNPC)
            {
                if (convo.isCompleted == false && convo.isLocked == false)
                    return convo;
            }

            List<Conversation> repeatableConvoersations = conversationsWithNPC.FindAll(convo => convo.isRepeatable == true && convo.isLocked == false);

            if (repeatableConvoersations.Count > 0)
                return repeatableConvoersations[UnityEngine.Random.Range(0, repeatableConvoersations.Count)];
            else
                return null;
        }

        public void DetermineActionBasedOnResponse(Response response)
        {
            if (response.actionTaken != "" && response.actionTaken != null)
                ActionsTakenManager.Instance.Invoke(response.actionTaken, 0);
            if (response.nextDialogueId == 0)
            {
                CompleteTheCurrentConversation();
                SaveFilesWhenNotInDebugMode();
            }
            else
            {
                Dialogue nextDialogue = DialogueSystemManager.Instance.currentConversation.dialogues.Find(dialogue => dialogue.id == response.nextDialogueId);
                DialogueSystemManager.Instance.currentDialogueIndex = nextDialogue.id;
                LoadNextDialogueIntoView(nextDialogue);
            }
        }

        public void UnlockConversationsByPhase(int phaseNum)
        {
            if (conversations != null)
                UnlockConversationsWithPhaseNum(conversations, phaseNum);
        }

        public void UnlockConversationsByPhaseAndNpcName(string nameOfNpc, int phaseNum)
        {
            if (conversations != null)
            {
                List<Conversation> conversationsWithNPC = conversations.FindAll(convo => convo.starter.ToLower() == nameOfNpc.ToLower()).OrderBy(x => x.id).ToList();
                UnlockConversationsWithPhaseNum(conversationsWithNPC, phaseNum);
            }
        }

        public void LoadNextDialogue()
        {
            if (DialogueSystemManager.Instance.currentDialogueIndex < DialogueSystemManager.Instance.currentConversation.dialogues.Count)
            {
                LoadNextDialogueIntoView(DialogueSystemManager.Instance.currentConversation.dialogues[DialogueSystemManager.Instance.currentDialogueIndex]);
                DialogueSystemManager.Instance.currentDialogueIndex++;
            }
            else
                CompleteTheCurrentConversation();
        }

        public void CompleteTheCurrentConversation()
        {
            DialogueSystemManager.Instance.currentDialogueIndex = 0;
            DialogueSystemManager.Instance.currentConversation.isCompleted = true;
            DialogueViewManager.Instance.CloseDialogueView();
        }

        public void LoadNextDialogueIntoView(Dialogue dialogue)
        {
            //This will spin up the dialogue box view and shoot the dialogue down into it, as well as the responses.
            DialogueViewManager.Instance.LoadDialogue(dialogue);
        }

        public void LoadNextDialogueIntoViewWithCharacterPortrait(Dialogue dialogue, Sprite portrait,string displayName)
        {
            DialogueViewManager.Instance.LoadDialogue(dialogue, portrait,displayName);
        }

        #region Notification Methods
        public void LoadNotification(string nameOfStarter)
        {
            var loadedNotification = notifications.Find(x => x.starter == nameOfStarter);
            if (loadedNotification != null)
                LoadNotificationView(loadedNotification);
            else
                Debug.Log("Can't find notification with that starter name, make sure you entered it correctly. \nIf on Demo scene make sure to remove the '-Demo' from the notifications XML");
        }

        private void LoadNotificationView(Notification notification)
        {
            NotificationViewManager.Instance.LoadNotificationToView(notification);
        }
        #endregion

        #region Private Helper Methods
        private void UnlockConversationsWithPhaseNum(List<Conversation> listOfConversations, int phaseNum)
        {
            foreach (var convo in listOfConversations)
            {
                if (convo.phase == phaseNum)
                    convo.isLocked = false;
            }
        }

        private void LoadFilesForSystem()
        {
            GenericXmlLoader<ConversationsContainer> conversationLoader = new GenericXmlLoader<ConversationsContainer>();
            GenericXmlLoader<NotificationsContainer> notificationLoader = new GenericXmlLoader<NotificationsContainer>();

            if (DialogueSystemManager.Instance.debugMode)
            {
                LoadConversationsFromResourceFolder(conversationLoader);
                if(DialogueSystemManager.Instance.loadNotifications)
                    LoadNotificationsFromResourceFolder(notificationLoader);
            }
            else
            {
                LoadConversationsFromDialogueSystemFolder(conversationLoader);
                if (DialogueSystemManager.Instance.loadNotifications)
                    LoadNotificationsFromResourceFolder(notificationLoader);
            }
        }

        //Put Saving and Loading in there own class called DataService
        #region Methods for Loading and Saving Conversations
        private void LoadConversationsFromResourceFolder(GenericXmlLoader<ConversationsContainer> loader)
        {
            ConversationsContainer conversationsContainer = new ConversationsContainer();
            conversationsContainer = loader.LoadXMLFromResourcesFolder(Constants.ResourcesPathToConversationsXML, Constants.FAILED_TO_LOAD_XML_FROM_RESOURCES);
            if (conversationsContainer.Conversations != null)
                conversations = conversationsContainer.Conversations;
            else
                Debug.Log("Error loading XML File");
        }

        private void LoadConversationsFromDialogueSystemFolder(GenericXmlLoader<ConversationsContainer> loader)
        {
            ConversationsContainer conversationsContainer = new ConversationsContainer();
            conversationsContainer = loader.LoadXMLFromExternalFolder((Path.Combine(dialogueSystemFolder, "Conversations.xml")), Constants.FAILED_TO_LOAD_CONVERSATIONS_FROM_EXTERNAL);
            if (conversationsContainer.Conversations != null)
                conversations = conversationsContainer.Conversations;
            else
                LoadConversationsFromResourceFolder(loader);
        }

        private void SaveFilesWhenNotInDebugMode()
        {
            GenericXmlLoader<ConversationsContainer> loader = new GenericXmlLoader<ConversationsContainer>();
            if (!DialogueSystemManager.Instance.debugMode)
            {
                ConversationsContainer conversationsContainer = new ConversationsContainer();
                conversationsContainer.Conversations = conversations;
                loader.SaveXMLFile(conversationsContainer, (Path.Combine(dialogueSystemFolder, "Conversations.xml")));
            }
        }
        #endregion

        #region Methods for Loading and Saving Notifications
        private void LoadNotificationsFromResourceFolder(GenericXmlLoader<NotificationsContainer> loader)
        {
            NotificationsContainer notificationContainer = new NotificationsContainer();
            notificationContainer = loader.LoadXMLFromResourcesFolder(Constants.ResourcesPathToNotificationsXML, Constants.FAILED_TO_LOAD_XML_FROM_RESOURCES);
            if (notificationContainer.notifications != null)
                notifications = notificationContainer.notifications;
            else
                Debug.Log("Error loading XML File");
        }
        #endregion

        #endregion
    }
}