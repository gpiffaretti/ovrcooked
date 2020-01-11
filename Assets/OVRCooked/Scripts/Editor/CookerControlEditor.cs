using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CookerControl))]
[CanEditMultipleObjects]
public class CookerControlEditor : Editor
{
    
    void OnEnable()
    {
        
        
    }

    public override void OnInspectorGUI()
    {

        EditorGUILayout.LabelField(new GUIContent($"Intensity:"), new GUIContent(serializedObject.FindProperty("intensity").floatValue.ToString()));
        EditorGUILayout.LabelField(new GUIContent($"Is on:"), new GUIContent(serializedObject.FindProperty("isOn").boolValue.ToString()));
        
        base.OnInspectorGUI();
    }
}