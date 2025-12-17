using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

[CustomEditor(typeof(TextColorBinderComponent))]
public class TextColorBinderEditor : Editor
{
    private SerializedProperty m_bindersProperty;
    private const float m_buttonWidth = 20f;

    private void OnEnable()
    {
        m_bindersProperty = serializedObject.FindProperty("m_colorBinders");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DisplayDataBinders();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Add Color Binder", GUILayout.Width(150)))
        {
            m_bindersProperty.arraySize++;
        }
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }

    private void DisplayDataBinders()
    {
        for (int i = 0; i < m_bindersProperty.arraySize; i++)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);

            if (m_bindersProperty.GetArrayElementAtIndex(i).isExpanded)
                EditorGUILayout.BeginVertical();

            m_bindersProperty.GetArrayElementAtIndex(i).isExpanded = EditorGUILayout.Foldout(m_bindersProperty.GetArrayElementAtIndex(i).isExpanded, "Color Binder " + (i + 1));

            if (m_bindersProperty.GetArrayElementAtIndex(i).isExpanded)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(10);
                DisplayFoldout(i);
                GUILayout.Space(10);

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }

            GUILayout.Space(10);

            if (GUILayout.Button("X", GUILayout.Width(m_buttonWidth)))
            {
                if (m_bindersProperty.arraySize > 0)
                    m_bindersProperty.RemoveFromObjectArrayAt(i);
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
    }

    private void DisplayFoldout(int index)
    {
        SerializedProperty keyProperty = m_bindersProperty.GetArrayElementAtIndex(index).FindPropertyRelative("m_key");
        SerializedProperty targetsProperty = m_bindersProperty.GetArrayElementAtIndex(index).FindPropertyRelative("m_targets");
        Color defaultColor = GUI.color;

        EditorGUILayout.BeginVertical();

        //key field
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(5);
        EditorGUILayout.LabelField("Key", GUILayout.Width(50));
        keyProperty.stringValue = EditorGUILayout.TextField(keyProperty.stringValue);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Targets", GUILayout.Width(50));

        GUILayout.FlexibleSpace();

        GUI.color = Color.green;
        if (GUILayout.Button("+", GUILayout.Width(30)))
        {
            targetsProperty.arraySize++;
        }

        GUI.color = Color.red;
        if (GUILayout.Button("-", GUILayout.Width(30)))
        {
            if (targetsProperty.arraySize > 0)
                targetsProperty.arraySize--;
        }

        GUI.color = defaultColor;
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(5);

        for (int i = 0; i < targetsProperty.arraySize; i++)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(5);
            EditorGUILayout.LabelField("Target " + (i + 1) + ":", GUILayout.Width(75));
            targetsProperty.GetArrayElementAtIndex(i).objectReferenceValue = (TextMeshProUGUI)EditorGUILayout.ObjectField(targetsProperty.GetArrayElementAtIndex(i).objectReferenceValue, typeof(TextMeshProUGUI), true);
            EditorGUILayout.EndHorizontal();
        }
        GUILayout.Space(5);
        EditorGUILayout.EndVertical();

        GUILayout.Space(5);
        EditorGUILayout.EndVertical();
    }
}
