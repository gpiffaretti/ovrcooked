using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SimpleTimer))]
[CanEditMultipleObjects]
public class SimpleTimerEditor : Editor
{

    void OnEnable()
    {


    }

    public override void OnInspectorGUI()
    {

        EditorGUILayout.LabelField(new GUIContent($"Running:"), new GUIContent(serializedObject.FindProperty("running").boolValue.ToString()));
        EditorGUILayout.LabelField(new GUIContent($"Total time:"), new GUIContent(serializedObject.FindProperty("totalTime").floatValue.ToString()));
        EditorGUILayout.LabelField(new GUIContent($"Elapsed:"), new GUIContent(serializedObject.FindProperty("elapsedTime").floatValue.ToString()));

        //base.OnInspectorGUI();
    }
}