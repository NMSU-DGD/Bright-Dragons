using UnityEngine;
using System.Collections;
using UnityEditor;
using DialogueSystem;

[CustomEditor(typeof(DialogueSystemManager))]
public class DialogueSystemManagerEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DialogueSystemManager myTarget = (DialogueSystemManager)target;
        if (myTarget.dialogueImageType == Constants.ImageType.sliced || myTarget.responseButtonImageType == Constants.ImageType.sliced)
            EditorGUILayout.HelpBox("If choosing ImageType Sliced make sure the image you are using has the borders set or it will default back to simple", MessageType.Info, true);

        if (myTarget.turnOffResponseButtonBackGroundImage == true && myTarget.highlightedResponseArea == Constants.HighlightableType.button)
            EditorGUILayout.HelpBox("You should highlight the text if you are turning the response buttons off!", MessageType.Warning, true);
    }
}
