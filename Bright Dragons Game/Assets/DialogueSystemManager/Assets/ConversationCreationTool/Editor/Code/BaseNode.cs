using UnityEngine;
using System.Collections;
using UnityEditor;

public abstract class BaseNode : ScriptableObject
{
    public Rect window { get; set; }

    public int nodeId { get; set; }

    public bool isParentToAttachedNode { get; set; }

    public BaseNode parentNode { get; set; }

    public string windowTitle { get; set; }

    public virtual void DrawWindow()
    {

    }

    public virtual string TextField(string label, string text)
    {
        var textDimensions = GUI.skin.label.CalcSize(new GUIContent(label));
        EditorGUIUtility.labelWidth = textDimensions.x;
        return EditorGUILayout.TextField(label, text);
    }

    public virtual bool ToggleField(string label, bool toggle)
    {
        var textDimensions = GUI.skin.label.CalcSize(new GUIContent(label));
        EditorGUIUtility.labelWidth = textDimensions.x;
        return EditorGUILayout.Toggle(label, toggle);
    }

    public virtual int IntField(string label, int num)
    {
        var textDimensions = GUI.skin.label.CalcSize(new GUIContent(label));
        EditorGUIUtility.labelWidth = textDimensions.x;
        return EditorGUILayout.IntField(label, num);
    }
}
