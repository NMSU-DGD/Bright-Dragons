using UnityEngine;
using System.Collections;
using DialogueSystem;

public static class Constants
{
    public const string FAILED_TO_LOAD_XML_FROM_RESOURCES = "Make sure you have a Conversations/Notifications .xml/.txt under the Resources/XML folder structure. "+
        "\nIf you are running the demo scene make sure to rename the Conversations-Demo to just Conversations and the same for Notifications-Demo";

    public const string FAILED_TO_LOAD_CONVERSATIONS_FROM_EXTERNAL = "First Time Loading from outside debug mode, we will pull the conversations from the resources folder." +
        "\nAnd after the first conversation has been completed we will save it the conversation to the MyDocuments/DialogueSystemManager folder and load from there from now on";

    public const string FAILED_TO_LOAD_NOTIFICATIONS = "Make sure you have a Notifications .xml/.txt under the Resources/XML folder structure. " +
        "\nIf you are running the demo scene make sure to rename the Notifications-Demo to just Notifications";

    public enum DialogueType
    {
        responsesWithPortrait = 1,
        responsesWithNoPortrait = 2
    }

    public enum DialogueSelectType
    {
        UpAndDown = 1,
        MouseAndKeyboard = 2
    }

    public enum HighlightableType
    {
        text = 1,
        button = 2
    }

    public enum ImageType
    {
        simple = 0,
        sliced = 1
    }

    public enum DialogueDisplaySpeed
    {
        slow = 0,
        medium = 1,
        fast = 2,
        instant = 3
    }

    public static string ResourcesPathToConversationsXML = "XML/Conversations";
    public static string ResourcesPathToNotificationsXML = "XML/Notifications";
    public static string ResourcesFolderForCharacterPortraits = "CharacterPortraits";

    public static float SetDialogueDisplaySpeed()
    {
        switch (DialogueSystemManager.Instance.displaySpeedForDialogue)
        {
            case DialogueDisplaySpeed.slow:
                return .1f;
            case DialogueDisplaySpeed.medium:
                return .05f;
            case DialogueDisplaySpeed.fast:
                return 0.01f;
            case DialogueDisplaySpeed.instant:
                return 0;
            default:
                return 0;
        }
    }
}
