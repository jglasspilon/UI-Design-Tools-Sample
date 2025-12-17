using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RectTransformBinderComponent))]
public class RectTransformBinderEditor : Editor
{
    private SerializedProperty m_bindersProperty;
    private const float m_buttonWidth = 20f;

    private void OnEnable()
    {
        m_bindersProperty = serializedObject.FindProperty("m_rectTransformBinders");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DisplayDataBinders();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Add Transform Binder", GUILayout.Width(150)))
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

            m_bindersProperty.GetArrayElementAtIndex(i).isExpanded = EditorGUILayout.Foldout(m_bindersProperty.GetArrayElementAtIndex(i).isExpanded, "Transform Binder " + (i + 1));

            if (m_bindersProperty.GetArrayElementAtIndex(i).isExpanded)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(10);
                DisplayFoldout(i);
                GUILayout.Space(10);

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }

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
        SerializedProperty scaleProperty = m_bindersProperty.GetArrayElementAtIndex(index).FindPropertyRelative("m_scale");
        Color defaultColor = GUI.color;

        //target object field
        EditorGUILayout.BeginVertical();
        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Scale:", GUILayout.Width(100));
        scaleProperty.floatValue = EditorGUILayout.FloatField(scaleProperty.floatValue);
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
            targetsProperty.GetArrayElementAtIndex(i).objectReferenceValue = (RectTransform)EditorGUILayout.ObjectField(targetsProperty.GetArrayElementAtIndex(i).objectReferenceValue, typeof(RectTransform), true);
            EditorGUILayout.EndHorizontal();
        }
        GUILayout.Space(5);
        EditorGUILayout.EndVertical();

        GUILayout.Space(10);
        DisplayKeys(index);
    }

    private void DisplayKeys(int index)
    {
        SerializedProperty keysProperty = m_bindersProperty.GetArrayElementAtIndex(index).FindPropertyRelative("m_keys");
        if (keysProperty.arraySize != 4)
            keysProperty.arraySize = 4;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("X Position Key:", GUILayout.Width(100));
        keysProperty.GetArrayElementAtIndex(0).stringValue = EditorGUILayout.TextField(keysProperty.GetArrayElementAtIndex(0).stringValue);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Y Position Key:", GUILayout.Width(100));
        keysProperty.GetArrayElementAtIndex(1).stringValue = EditorGUILayout.TextField(keysProperty.GetArrayElementAtIndex(1).stringValue);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Width Key:", GUILayout.Width(100));
        keysProperty.GetArrayElementAtIndex(2).stringValue = EditorGUILayout.TextField(keysProperty.GetArrayElementAtIndex(2).stringValue);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Height Key:", GUILayout.Width(100));
        keysProperty.GetArrayElementAtIndex(3).stringValue = EditorGUILayout.TextField(keysProperty.GetArrayElementAtIndex(3).stringValue);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(5);
        EditorGUILayout.EndVertical();
    }
}
