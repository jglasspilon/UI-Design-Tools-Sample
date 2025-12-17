using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(SliderBinderComponent))]
public class SliderBinderEditor : Editor
{

    private SliderBinderComponent m_chartBinder;
    private SerializedProperty m_bindersProperty;
    private const float m_buttonWidth = 20f;
    int labelWidth = 185;

    private void OnEnable()
    {
        m_chartBinder = (SliderBinderComponent)target;
        m_bindersProperty = serializedObject.FindProperty("m_sliderBinders");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DisplayDataBinders();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Add Slider Binder", GUILayout.Width(150)))
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

            m_bindersProperty.GetArrayElementAtIndex(i).isExpanded = EditorGUILayout.Foldout(m_bindersProperty.GetArrayElementAtIndex(i).isExpanded, "Slider Binder " + (i + 1));

            if (m_bindersProperty.GetArrayElementAtIndex(i).isExpanded)
            {
                DisplayFoldout(i);
                GUILayout.Space(10);
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
        SerializedProperty bindToValuesProperty = m_bindersProperty.GetArrayElementAtIndex(index).FindPropertyRelative("m_BindToValues");
        SerializedProperty keysProperty = m_bindersProperty.GetArrayElementAtIndex(index).FindPropertyRelative("m_keys");
        SerializedProperty keyProperty = m_bindersProperty.GetArrayElementAtIndex(index).FindPropertyRelative("m_key");
        Color defaultColor = GUI.color;

        //target object field
        EditorGUILayout.BeginVertical();
        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(bindToValuesProperty);
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
            targetsProperty.GetArrayElementAtIndex(i).objectReferenceValue = (Slider)EditorGUILayout.ObjectField(targetsProperty.GetArrayElementAtIndex(i).objectReferenceValue, typeof(Slider), true);
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.BeginVertical();
        Debug.Log((m_chartBinder.GetAllBinders()[index] as SliderBinder).BindToValues);
        if ((m_chartBinder.GetAllBinders()[index] as SliderBinder).BindToValues == E.SliderBinderType.JustValue)
        {
            keysProperty.arraySize = 0;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Value Key:", GUILayout.Width(100));
            keyProperty.stringValue = EditorGUILayout.TextField(keyProperty.stringValue);
            EditorGUILayout.EndHorizontal();
        }

        if ((m_chartBinder.GetAllBinders()[index] as SliderBinder).BindToValues == E.SliderBinderType.JustMax)
        {
            keysProperty.arraySize = 0;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Max Key:", GUILayout.Width(100));
            keyProperty.stringValue = EditorGUILayout.TextField(keyProperty.stringValue);
            EditorGUILayout.EndHorizontal();
        }

        if ((m_chartBinder.GetAllBinders()[index] as SliderBinder).BindToValues == E.SliderBinderType.ValueAndMax)
        {
            if (keysProperty.arraySize != 2)
                keysProperty.arraySize = 2;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Value Key:", GUILayout.Width(100));
            keysProperty.GetArrayElementAtIndex(0).stringValue = EditorGUILayout.TextField(keysProperty.GetArrayElementAtIndex(0).stringValue);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Max Key:", GUILayout.Width(100));
            keysProperty.GetArrayElementAtIndex(1).stringValue = EditorGUILayout.TextField(keysProperty.GetArrayElementAtIndex(1).stringValue);
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();
    }
}
