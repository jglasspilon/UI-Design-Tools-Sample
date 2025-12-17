using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using TMPro;

[CustomEditor(typeof(CombinedLabelBinderComponent))]
public class CombinedLabelBinderEditor : Editor
{
    private SerializedProperty m_bindersProperty;
    private const float m_buttonWidth = 30f;

    private void OnEnable()
    {
        m_bindersProperty = serializedObject.FindProperty("m_combinedLabelBinders");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DisplayDataBinders();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Add Label Binder", GUILayout.Width(150)))
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

            m_bindersProperty.GetArrayElementAtIndex(i).isExpanded = EditorGUILayout.Foldout(m_bindersProperty.GetArrayElementAtIndex(i).isExpanded, "Label Binder " + (i + 1));

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
        SerializedProperty targetsProperty = m_bindersProperty.GetArrayElementAtIndex(index).FindPropertyRelative("m_targets");
        SerializedProperty formatProperty = m_bindersProperty.GetArrayElementAtIndex(index).FindPropertyRelative("m_format");
        SerializedProperty keysProperty = m_bindersProperty.GetArrayElementAtIndex(index).FindPropertyRelative("m_keys");
        SerializedProperty lineBreakIdProperty = m_bindersProperty.GetArrayElementAtIndex(index).FindPropertyRelative("m_lineBreakIdentifier");
        Color defaultColor = GUI.color;
        bool formatError = false;
        bool formatWarning = false;

        GUIStyle formatMessageStyle = new GUIStyle();
        formatMessageStyle.fontSize = 10;

        //target object field
        EditorGUILayout.BeginVertical();

        //format string field
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(5);
        EditorGUILayout.LabelField("Format:", GUILayout.Width(50));

        if (!formatProperty.stringValue.Contains("[]"))
        {
            GUI.color = Color.red;
            formatMessageStyle.normal.textColor = Color.red;
            formatError = true;
        }

        else
        {
            Regex rgx = new Regex("\\[]");
            if(rgx.Matches(formatProperty.stringValue).Count != keysProperty.arraySize)
            {
                GUI.color = Color.yellow;
                formatMessageStyle.normal.textColor = Color.yellow;
                formatWarning = true;
            }

            else
            {
                GUI.color = defaultColor;
            }
        }

        formatProperty.stringValue = EditorGUILayout.TextField(formatProperty.stringValue);       
        EditorGUILayout.EndHorizontal();

        if(formatError)
            EditorGUILayout.LabelField("The Format must contain '[]' in order to replace with the data.", formatMessageStyle);
        if(formatWarning)
            EditorGUILayout.LabelField("The Format should contain the same amount of '[]' as keys. Result may not be as expected.", formatMessageStyle);

        GUI.color = defaultColor;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Line Break Id:", GUILayout.Width(100));
        lineBreakIdProperty.stringValue = EditorGUILayout.TextField(lineBreakIdProperty.stringValue);
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
        DisplayKeys(index);
    }

    private void DisplayKeys(int index)
    {
        Color defaultColor = GUI.color;
        SerializedProperty keysProperty = m_bindersProperty.GetArrayElementAtIndex(index).FindPropertyRelative("m_keys");

        EditorGUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Space(5);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(5);
        EditorGUILayout.LabelField("Keys:", GUILayout.Width(50));

        GUILayout.FlexibleSpace();
        GUI.color = Color.green;
        if (GUILayout.Button("+", GUILayout.Width(m_buttonWidth)))
        {
            keysProperty.arraySize++;
        }

        GUI.color = Color.red;
        if (GUILayout.Button("-", GUILayout.Width(m_buttonWidth)))
        {
            if (keysProperty.arraySize > 0)
                keysProperty.arraySize--;
        }
        GUI.color = defaultColor;
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(5);
        for (int i = 0; i < keysProperty.arraySize; i++)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(5);
            keysProperty.GetArrayElementAtIndex(i).stringValue = EditorGUILayout.TextField(keysProperty.GetArrayElementAtIndex(i).stringValue);
            EditorGUILayout.EndHorizontal();
        }
        GUILayout.Space(5);
        EditorGUILayout.EndVertical();

        GUILayout.Space(5);
        EditorGUILayout.EndVertical();
    }
}
