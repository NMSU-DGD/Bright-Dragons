using UnityEngine;
using System.Collections;
using DialogueSystem.Models;
using DialogueSystem.Services;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DialogueSystem.Managers;

namespace DialogueSystem
{
    public class DialogueSystemManager : Singleton<DialogueSystemManager>
    {
        public Conversation currentConversation { get; set; }
        public int currentDialogueIndex { get; set; }

        #region Exposed Fields
        [Header("Turn off if you want to test saving and loading conversations")]
        //If this is on then conversations will always load from resource folder
        public bool debugMode;
        //If you want notifications in your game please check this box
        [Header("Turn off if you do not want the system to load notifications for your game")]
        public bool loadNotifications = true;

        //These toggle which location we want to save our conversations to when we play our game outside debug mode
        [Header("Choose One of the two locations where to save")]
        public bool saveToMyDocuments = true;
        public bool saveToPersistentDataFile;

        //This will automatically progress through conversation dialogues is you have no responses
        public bool haveConversationsAutoContinueIfNoResponses;
        //This goes with the above, it is how much time we will wait before progressing the conversation
        public float timetillProgress;

        //Type of dialogue user will display during game
        public Constants.DialogueType dialogueType;
        [Header("Only Allowed With Portraits")]
        //Displays the character names under his portrait
        public bool showCharacterName;
        //This will instead of displaying the name of the conversation starter under the portrait it will instead display the conversation name
        public bool displayConversationNameAsPortraitName;

        [Header("Dialogue And Notification Options")]
        //This is the speed at which the dialogue text displays during conversations
        public Constants.DialogueDisplaySpeed displaySpeedForDialogue;
        //Font for the dialogue text
        public Font dialogueFont;
        //Whatever image the user wants to set as the background image of the Dialogue Panel
        public Sprite dialogueBoxBackGroundImage;
        //Whatever image the user wants to set as the background image of the notification Panel
        public Sprite notificationBoxBackGroundImage;
        //This is the image type we wish to use for the Dialogue Back Ground Image. Sliced can only be used if the border is set.
        public Constants.ImageType notificationImageType;
        //This is the image type we wish to use for the Dialogue Back Ground Image. Sliced can only be used if the border is set.
        public Constants.ImageType dialogueImageType;
        //Adjusts the opacity to whatever we set it to be
        [Range(0.0f, 1.0f)]
        public float alphaOfDialoguePanel;
        //Dialogue Text Color
        public Color dialogueTextColor;
        //Selectable type is how you will go about selecting the response you want or interact with the response on the screen
        public Constants.DialogueSelectType dialogueSelectType;

        [Header("Continue Button Configurations")]
        //If set will use for the continue buttons sprite. If nothing then the default is used
        public Sprite continueDialogueButtonSprite; 
        //If text is set that is what will be displayed in continue button. If not no text is displayed
        public string continueButtonText;
        //Will set the continue button to this width and will make adjustments to always put it in the same location based on that width
        public float widthOfContinueButton = 64;
        //Font for continue button if continue button has font
        public Font continueButtonFont;

        [Header("Response Button Configurations")]
        //If the user doesn't want to show the response button background image make sure to check this option
        public bool turnOffResponseButtonBackGroundImage;
        //Can choose if the text or the button highlights for the dialogue when hovering over it
        public Constants.HighlightableType highlightedResponseArea;
        //Whatever the user sets this too the highlighted color will appear that way.
        public Color highlightedResponseColor;
        //Whatever image the user wants to set as the background image of the response button
        public Sprite responseButtonBackGroundImage;
        //This is the image type we wish to use for the response type. Sliced can only be used if the border is set.
        public Constants.ImageType responseButtonImageType;
        //Font for the response buttons 
        public Font responseButtonFont;
        //Color For Response Button Text
        public Color responseButtonTextColor;
        //Color For Response Button
        public Color responseButtonColor;
        #endregion


        private void Awake()
        {
            if(FindObjectOfType<EventSystem>() == null)
            {
                GameObject eventSystemGO = new GameObject("EventSystem",typeof(EventSystem));
                DontDestroyOnLoad(eventSystemGO);
                eventSystemGO.AddComponent<StandaloneInputModule>();
            }

            DialogueService.Instance.Load(); //Load all conversations when the system comes alive
            currentDialogueIndex = 0;
            GameObject dialogueGui = Resources.Load<GameObject>("Prefabs/DialogueSystemGUI");
            GameObject dialogueGuiGO = Instantiate(dialogueGui);
            dialogueGuiGO.name = "DialogueSystemGUI";
            DontDestroyOnLoad(this);
            DontDestroyOnLoad(dialogueGuiGO);
        }

        public void StartDialogueForNpc(string nameOfNpc)
        {
            currentConversation = DialogueService.Instance.LoadConversationForNpc(nameOfNpc);

            if(currentConversation != null)
            {
                if(dialogueType == Constants.DialogueType.responsesWithPortrait)
                {
                    Sprite portrait = DialogueService.Instance.GetPortraitByName(nameOfNpc);
                    string displayName = "";
                    if (portrait != null)
                        displayName = portrait.name;
                    if (displayConversationNameAsPortraitName)
                        displayName = currentConversation.name;
                    DialogueService.Instance.LoadNextDialogueIntoViewWithCharacterPortrait(currentConversation.dialogues[currentDialogueIndex], portrait,displayName);
                }
                else if(dialogueType == Constants.DialogueType.responsesWithNoPortrait)
                    DialogueService.Instance.LoadNextDialogueIntoView(currentConversation.dialogues[currentDialogueIndex]);
                currentDialogueIndex++;
            }
        }

        public bool DialogueSystemIsActive()
        {
            return DialogueViewManager.Instance.dialogueView.gameObject.activeInHierarchy;
        }

        public void UnlockConversationsByPhase(int phaseNum)
        {
            DialogueService.Instance.UnlockConversationsByPhase(phaseNum);
        }

        public void UnlockConversationsForNPCByPhase(string nameOfNpc, int phaseNum)
        {
            if(nameOfNpc != null)
            {
                DialogueService.Instance.UnlockConversationsByPhaseAndNpcName(nameOfNpc, phaseNum);
            }
        }

        public void OpenNotification(string nameOfStarter)
        {
            DialogueService.Instance.LoadNotification(nameOfStarter);
        }

        public void CloseNotification()
        {
            NotificationViewManager.Instance.CloseNotificiation();
        }
    }
}