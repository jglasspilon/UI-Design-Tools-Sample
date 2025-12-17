using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DataBinderList))]
public class DataBinderListEditor : Editor
{
    SerializedProperty m_nodeStructureProperty;
    SerializedProperty m_prefabProperty;
    SerializedProperty m_holderProperty;
    SerializedProperty m_isAnimatedListProperty;
    SerializedProperty m_delayProperty;
    SerializedProperty m_waitProperty;
    SerializedProperty m_animationTriggerOutProperty;
    SerializedProperty m_animationTriggerInProperty;
    SerializedProperty m_animationTriggerUpdateProperty;
    SerializedProperty m_isCappedProperty;
    SerializedProperty m_maxCountProperty;
    SerializedProperty m_isManuallySortedProperty;
    SerializedProperty m_sortFieldProperty;
    private const float m_buttonWidth = 30f;

    private void OnEnable()
    {
        m_nodeStructureProperty = serializedObject.FindProperty("m_nodeStructure");
        m_prefabProperty = serializedObject.FindProperty("m_databinderPrefabToGenerate");
        m_holderProperty = serializedObject.FindProperty("m_holder");
        m_isAnimatedListProperty = serializedObject.FindProperty("m_isAnimatedList");
        m_delayProperty = serializedObject.FindProperty("m_delay");
        m_waitProperty = serializedObject.FindProperty("m_wait");
        m_animationTriggerOutProperty = serializedObject.FindProperty("m_animationTriggerOut");
        m_animationTriggerInProperty = serializedObject.FindProperty("m_animationTriggerIn");
        m_animationTriggerUpdateProperty = serializedObject.FindProperty("m_animationTriggerUpdate");
        m_isCappedProperty = serializedObject.FindProperty("m_isCapped");
        m_maxCountProperty = serializedObject.FindProperty("m_maxCount");
        m_isManuallySortedProperty = serializedObject.FindProperty("m_isManuallySorted");
        m_sortFieldProperty = serializedObject.FindProperty("m_sortField");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DisplayNodeStructure(m_nodeStructureProperty);
        GUILayout.Space(10);

        EditorGUILayout.PropertyField(m_prefabProperty);
        EditorGUILayout.PropertyField(m_holderProperty);
        EditorGUILayout.PropertyField(m_isAnimatedListProperty);

        if (m_isAnimatedListProperty.boolValue)
        {
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(m_delayProperty);
            EditorGUILayout.PropertyField(m_waitProperty);
            EditorGUILayout.PropertyField(m_animationTriggerOutProperty);
            EditorGUILayout.PropertyField(m_animationTriggerInProperty);
            EditorGUILayout.PropertyField(m_animationTriggerUpdateProperty);
        }

        EditorGUILayout.PropertyField(m_isCappedProperty);

        if(m_isCappedProperty.boolValue)
            EditorGUILayout.PropertyField(m_maxCountProperty);

        EditorGUILayout.PropertyField(m_isManuallySortedProperty);

        if(m_isManuallySortedProperty.boolValue)
            EditorGUILayout.PropertyField(m_sortFieldProperty);

        serializedObject.ApplyModifiedProperties();
    }

    private void DisplayNodeStructure(SerializedProperty nodeStructureProperty)
    {
        EditorGUI.indentLevel = 0;
        Color defaultColor = GUI.color;

        EditorGUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Space(5);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Node Structure:", GUILayout.Width(95));
        GUILayout.FlexibleSpace();

        GUI.color = Color.green;
        if (GUILayout.Button("+", GUILayout.Width(m_buttonWidth)))
        {
            nodeStructureProperty.arraySize++;
        }

        GUI.color = Color.red;
        if (GUILayout.Button("-", GUILayout.Width(m_buttonWidth)))
        {
            if (nodeStructureProperty.arraySize > 0)
                nodeStructureProperty.arraySize--;
        }

        GUI.color = defaultColor;
        GUILayout.Space(5);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(10);
        for (int i = 0; i < nodeStructureProperty.arraySize; i++)
        {
            SerializedProperty node = nodeStructureProperty.GetArrayElementAtIndex(i);
            EditorGUILayout.LabelField("[", GUILayout.Width(5));
            EditorGUILayout.PropertyField(node, GUIContent.none, GUILayout.MinWidth(1), GUILayout.MaxWidth(75));
            EditorGUILayout.LabelField("]", GUILayout.Width(5));
        }

        EditorGUILayout.LabelField("[Array to generate from]", GUILayout.Width(150));
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);
        EditorGUILayout.EndVertical();
    }
}
