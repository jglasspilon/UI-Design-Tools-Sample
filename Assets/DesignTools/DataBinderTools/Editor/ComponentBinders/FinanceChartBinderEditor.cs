using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FinanceChartBinderComponent))]
public class FinanceChartBinderEditor : Editor
{
    private FinanceChartBinderComponent m_chartBinder;
    private SerializedProperty m_bindersProperty;
    private const float m_buttonWidth = 20f;
    int labelWidth = 185;

    private void OnEnable()
    {
        m_chartBinder = (FinanceChartBinderComponent)target;
        m_bindersProperty = serializedObject.FindProperty("m_chartBinders");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DisplayDataBinders();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Add Chart Binder", GUILayout.Width(150)))
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

            m_bindersProperty.GetArrayElementAtIndex(i).isExpanded = EditorGUILayout.Foldout(m_bindersProperty.GetArrayElementAtIndex(i).isExpanded, "Chart Binder " + (i + 1));

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
        SerializedProperty chartTypeProperty = m_bindersProperty.GetArrayElementAtIndex(index).FindPropertyRelative("m_chartType");
        Color defaultColor = GUI.color;

        //target object field
        EditorGUILayout.BeginVertical();
        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(chartTypeProperty);
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
            targetsProperty.GetArrayElementAtIndex(i).objectReferenceValue = (ChartDraw)EditorGUILayout.ObjectField(targetsProperty.GetArrayElementAtIndex(i).objectReferenceValue, typeof(ChartDraw), true);
            EditorGUILayout.EndHorizontal();
        }
        GUILayout.Space(5);
        EditorGUILayout.EndVertical();

        GUILayout.Space(10);
        serializedObject.ApplyModifiedProperties();

        if ((m_chartBinder.GetAllBinders()[index] as FinanceChartBinder).ChartType == E.FinanceChartType.SingleChart)
            DisplaySingleChartKeys(index);

        if ((m_chartBinder.GetAllBinders()[index] as FinanceChartBinder).ChartType == E.FinanceChartType.CandlestickChart)
            DisplayCandlestickChartKeys(index);

        if ((m_chartBinder.GetAllBinders()[index] as FinanceChartBinder).ChartType == E.FinanceChartType.PerformanceChart)
            DisplayPerformanceChartKeys(index);
    }

    private void DisplaySingleChartKeys(int index)
    {       
        SerializedProperty keysProperty = m_bindersProperty.GetArrayElementAtIndex(index).FindPropertyRelative("m_keys");
        if (keysProperty.arraySize != 5)
            keysProperty.arraySize = 5;

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(20);
        EditorGUILayout.LabelField("Normalized Values Key:", GUILayout.Width(labelWidth));
        keysProperty.GetArrayElementAtIndex(0).stringValue = EditorGUILayout.TextField(keysProperty.GetArrayElementAtIndex(0).stringValue);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(20);
        EditorGUILayout.LabelField("Y Axis Labels Key:", GUILayout.Width(labelWidth));
        keysProperty.GetArrayElementAtIndex(1).stringValue = EditorGUILayout.TextField(keysProperty.GetArrayElementAtIndex(1).stringValue);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(20);
        EditorGUILayout.LabelField("X Axis Labels Key:", GUILayout.Width(labelWidth));
        keysProperty.GetArrayElementAtIndex(2).stringValue = EditorGUILayout.TextField(keysProperty.GetArrayElementAtIndex(2).stringValue);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(20);
        EditorGUILayout.LabelField("X Axis Labels Position Key:", GUILayout.Width(labelWidth));
        keysProperty.GetArrayElementAtIndex(3).stringValue = EditorGUILayout.TextField(keysProperty.GetArrayElementAtIndex(3).stringValue);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(20);
        EditorGUILayout.LabelField("Previous Day Close Key:", GUILayout.Width(labelWidth));
        keysProperty.GetArrayElementAtIndex(4).stringValue = EditorGUILayout.TextField(keysProperty.GetArrayElementAtIndex(4).stringValue);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(5);
        EditorGUILayout.EndVertical();
    }

    private void DisplayCandlestickChartKeys(int index)
    {
        SerializedProperty keysProperty = m_bindersProperty.GetArrayElementAtIndex(index).FindPropertyRelative("m_keys");
        if (keysProperty.arraySize != 7)
            keysProperty.arraySize = 7;

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(20);
        EditorGUILayout.LabelField("Normalized Closed Values Key:", GUILayout.Width(labelWidth));
        keysProperty.GetArrayElementAtIndex(0).stringValue = EditorGUILayout.TextField(keysProperty.GetArrayElementAtIndex(0).stringValue);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(20);
        EditorGUILayout.LabelField("Y Axis Labels Key:", GUILayout.Width(labelWidth));
        keysProperty.GetArrayElementAtIndex(1).stringValue = EditorGUILayout.TextField(keysProperty.GetArrayElementAtIndex(1).stringValue);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(20);
        EditorGUILayout.LabelField("X Axis Labels Key:", GUILayout.Width(labelWidth));
        keysProperty.GetArrayElementAtIndex(2).stringValue = EditorGUILayout.TextField(keysProperty.GetArrayElementAtIndex(2).stringValue);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(20);
        EditorGUILayout.LabelField("X Axis Labels Position Key:", GUILayout.Width(labelWidth));
        keysProperty.GetArrayElementAtIndex(3).stringValue = EditorGUILayout.TextField(keysProperty.GetArrayElementAtIndex(3).stringValue);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(20);
        EditorGUILayout.LabelField("Normalized Open Values Key:", GUILayout.Width(labelWidth));
        keysProperty.GetArrayElementAtIndex(4).stringValue = EditorGUILayout.TextField(keysProperty.GetArrayElementAtIndex(4).stringValue);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(20);
        EditorGUILayout.LabelField("Normalized Low Values Key:", GUILayout.Width(labelWidth));
        keysProperty.GetArrayElementAtIndex(5).stringValue = EditorGUILayout.TextField(keysProperty.GetArrayElementAtIndex(5).stringValue);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(20);
        EditorGUILayout.LabelField("Normalized High Values Key:", GUILayout.Width(labelWidth));
        keysProperty.GetArrayElementAtIndex(6).stringValue = EditorGUILayout.TextField(keysProperty.GetArrayElementAtIndex(6).stringValue);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(5);
        EditorGUILayout.EndVertical();
    }

    private void DisplayPerformanceChartKeys(int index)
    {
        SerializedProperty keysProperty = m_bindersProperty.GetArrayElementAtIndex(index).FindPropertyRelative("m_keys");
        if (keysProperty.arraySize != 5)
            keysProperty.arraySize = 5;

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(20);
        EditorGUILayout.LabelField("Normalized Net Change 1 Key:", GUILayout.Width(labelWidth));
        keysProperty.GetArrayElementAtIndex(0).stringValue = EditorGUILayout.TextField(keysProperty.GetArrayElementAtIndex(0).stringValue);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(20);
        EditorGUILayout.LabelField("Normalized Net Change 2 Key:", GUILayout.Width(labelWidth));
        keysProperty.GetArrayElementAtIndex(1).stringValue = EditorGUILayout.TextField(keysProperty.GetArrayElementAtIndex(1).stringValue);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(20);
        EditorGUILayout.LabelField("Y Axis Labels Key:", GUILayout.Width(labelWidth));
        keysProperty.GetArrayElementAtIndex(2).stringValue = EditorGUILayout.TextField(keysProperty.GetArrayElementAtIndex(2).stringValue);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(20);
        EditorGUILayout.LabelField("X Axis Labels Key:", GUILayout.Width(labelWidth));
        keysProperty.GetArrayElementAtIndex(3).stringValue = EditorGUILayout.TextField(keysProperty.GetArrayElementAtIndex(3).stringValue);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(20);
        EditorGUILayout.LabelField("X Axis Labels Position Key:", GUILayout.Width(labelWidth));
        keysProperty.GetArrayElementAtIndex(4).stringValue = EditorGUILayout.TextField(keysProperty.GetArrayElementAtIndex(4).stringValue);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(5);
        EditorGUILayout.EndVertical();
    }
}
