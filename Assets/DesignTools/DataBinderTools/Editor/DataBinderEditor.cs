using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DataBinder))]
public class DataBinderEditor : Editor
{
    private DataBinder m_databinder;
    private SerializedProperty m_binderLinksProperty;
    private SerializedProperty m_idProperty;
    private const float m_buttonWidth = 30f;   

    private void OnEnable()
    {
        m_databinder = (DataBinder)target;
        m_binderLinksProperty = serializedObject.FindProperty("m_allBinderLinks");
        m_idProperty = serializedObject.FindProperty("m_sortId");

        for (int i = 0; i < m_databinder.AllBinderLinks.Length; i++)
        {
            if(m_databinder.AllBinderLinks[i] == null)
                m_databinder.AllBinderLinks[i] = new DataBinderLink();
        }
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUILayout.Space(5);
        EditorGUILayout.PropertyField(m_idProperty);
        GUILayout.Space(5);

        DisplayBinderLink();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Add New Data Binder Link", GUILayout.Width(200), GUILayout.Height(30)))
        {
            DataBinderLink newBinderLink = CreateBinderLink();
            m_binderLinksProperty.AddToObjectArray(newBinderLink);
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(5);

        serializedObject.ApplyModifiedProperties();
    }
    private void DisplayBinderLink()
    {
        for (int i = 0; i < m_binderLinksProperty.arraySize; i++)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);

            if (m_binderLinksProperty.GetArrayElementAtIndex(i).isExpanded)
                EditorGUILayout.BeginVertical();

            m_binderLinksProperty.GetArrayElementAtIndex(i).isExpanded = EditorGUILayout.Foldout(m_binderLinksProperty.GetArrayElementAtIndex(i).isExpanded, "Data Binder Link " + (i + 1));

            if (m_binderLinksProperty.GetArrayElementAtIndex(i).isExpanded)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(10);
                DisplayFoldout(i);
                GUILayout.Space(10);

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }

            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                if (m_binderLinksProperty.arraySize > 0)
                    RemoveRegistrarAt(m_binderLinksProperty.GetArrayElementAtIndex(i), i);
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5);
            EditorGUILayout.EndVertical();
        }
    }
    private void DisplayFoldout(int index)
    {
        serializedObject.ApplyModifiedProperties();

        if (m_databinder.AllBinderLinks[index] == null)
            m_databinder.AllBinderLinks[index] = CreateBinderLink();

        SerializedObject currentDataRegistry = new SerializedObject(m_databinder.AllBinderLinks[index]);
        SerializedProperty nodeStructureProperty = currentDataRegistry.FindProperty("m_nodeStructureSteps");
        SerializedProperty BinderComponentsProperty = currentDataRegistry.FindProperty("m_connectedBinderComponents");
        
        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        GUILayout.Space(10);
        DisplayNodeStructure(nodeStructureProperty);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(5);

        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        GUILayout.Space(10);
        DisplayConnectedBinders(BinderComponentsProperty, index);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(5);

        bool referenceBinderExists = false;
        foreach (BinderComponent binder in m_databinder.AllBinderLinks[index].ConnectedBinderComponents)
        {
            if(binder is ReferenceDataBinderComponent)
            {
                referenceBinderExists = true; 
                break;
            }
        }

        if (referenceBinderExists)
        {
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            DisplayReferenceBinder(index);
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5);

            if (GUILayout.Button("Remove Reference Binder", GUILayout.Width(180), GUILayout.Height(30)))
            {
                RemoveReferenceBinder(index);
            }
        }
        else
        {
            if (GUILayout.Button("Add Reference Binder", GUILayout.Width(180), GUILayout.Height(30)))
            {
                AddReferenceBinder(BinderComponentsProperty, index);
            }
        }
                
        GUILayout.Space(5);
        EditorGUILayout.EndVertical();
        
        currentDataRegistry.ApplyModifiedProperties();
    }
    private void DisplayNodeStructure(SerializedProperty nodeStructureProperty)
    {
        EditorGUI.indentLevel = 0;
        Color defaultColor = GUI.color;

        EditorGUILayout.BeginVertical();
        GUILayout.Space(5);
        EditorGUILayout.BeginHorizontal();
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
        for (int i = 0; i < nodeStructureProperty.arraySize; i++)
        {
            SerializedProperty node = nodeStructureProperty.GetArrayElementAtIndex(i);
            EditorGUILayout.LabelField("[", GUILayout.Width(5));
            EditorGUILayout.PropertyField(node, GUIContent.none, GUILayout.MinWidth(1), GUILayout.MaxWidth(75));
            EditorGUILayout.LabelField("]", GUILayout.Width(5));
        }

        EditorGUILayout.LabelField("[Key]", GUILayout.Width(52));
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);
        EditorGUILayout.EndVertical();        
    }
    private void DisplayConnectedBinders(SerializedProperty binderComponentsProperty, int index)
    {
        SerializedObject currentDataRegistry = new SerializedObject(m_databinder.AllBinderLinks[index]);
        Color defaultColor = GUI.color;

        EditorGUILayout.BeginVertical();
        GUILayout.Space(5);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Bindable Components:", GUILayout.Width(200));
        GUILayout.FlexibleSpace();

        GUI.color = Color.green;
        if (GUILayout.Button("+", GUILayout.Width(m_buttonWidth)))
        {
            binderComponentsProperty.arraySize++;
        }

        GUI.color = Color.red;
        if (GUILayout.Button("-", GUILayout.Width(m_buttonWidth)))
        {
            if (binderComponentsProperty.arraySize > 0 && m_databinder.AllBinderLinks[index].ConnectedBinderComponents[0] is ReferenceDataBinderComponent)
            {
                if (binderComponentsProperty.arraySize > 1)
                {
                    binderComponentsProperty.arraySize--;
                }
            }
            else
            {
                if (binderComponentsProperty.arraySize > 0)
                {
                    binderComponentsProperty.arraySize--;
                }
            }
        }
        GUI.color = defaultColor;
        GUILayout.Space(5);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);

        for (int i = 0; i < binderComponentsProperty.arraySize; i++)
        {
            if (m_databinder.AllBinderLinks[index].ConnectedBinderComponents.Length > 0 && m_databinder.AllBinderLinks[index].ConnectedBinderComponents[0] is ReferenceDataBinderComponent && i == 0)
            {
                continue;               
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(binderComponentsProperty.GetArrayElementAtIndex(i), GUIContent.none);
                EditorGUILayout.EndHorizontal();
            }
        }
        GUILayout.Space(10);
        EditorGUILayout.EndVertical();
    }
    private void DisplayReferenceBinder(int index)
    {
        ReferenceDataBinderComponent referenceData = m_databinder.AllBinderLinks[index].ConnectedBinderComponents[0] as ReferenceDataBinderComponent;
        Color defaultColor = GUI.color;

        EditorGUILayout.BeginVertical();
        GUILayout.Space(5);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Non-Bindable Data:");

        GUI.color = Color.green;
        if (GUILayout.Button("+", GUILayout.Width(m_buttonWidth)))
        {
            IncrementReferenceBinderData(index);
        }

        GUI.color = Color.red;
        if (GUILayout.Button("-", GUILayout.Width(m_buttonWidth)))
        {
            if (referenceData.StringBinders.Length > 1)
            {
                DecrementReferenceBinderData(index);
            }
        }
        GUI.color = defaultColor;
        GUILayout.Space(5);
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);

        for (int i = 0; i < referenceData.StringBinders.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Key:", GUILayout.Width(30));
            referenceData.StringBinders[i].Key = EditorGUILayout.TextField(referenceData.StringBinders[i].Key, GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();
        }
        GUILayout.Space(10);
        EditorGUILayout.EndVertical();
    }
    private DataBinderLink CreateBinderLink()
    {
        DataBinderLink newInstance = m_databinder.gameObject.AddComponent<DataBinderLink>();
        return newInstance;
    }
    private void RemoveRegistrarAt(SerializedProperty registrarProperty, int regIndex)
    {
        RemoveAttachedReferenceBinders(regIndex);
        RemoveAttachedDataRegistrars(registrarProperty, regIndex);
        m_binderLinksProperty.RemoveFromObjectArrayAt(regIndex);
        RemoveEmptyEntriesFromRegistrars();

        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }
    private void RemoveReferenceBinder(int regIndex)
    {
        DataBinderLink registrar = m_databinder.AllBinderLinks[regIndex];
        List<BinderComponent> newList = new List<BinderComponent>();

        foreach(BinderComponent binder in m_databinder.AllBinderLinks[regIndex].ConnectedBinderComponents)
        {
            if(!(binder is ReferenceDataBinderComponent))
            {
                newList.Add(binder);
            }
        }

        RemoveAttachedReferenceBinders(regIndex);
        m_databinder.AllBinderLinks[regIndex].ConnectedBinderComponents = newList.ToArray();
    }
    private void RemoveAttachedReferenceBinders(int regIndex)
    {
        List<ReferenceDataBinderComponent> toRemove = new List<ReferenceDataBinderComponent>();

        foreach (BinderComponent refBinder in m_databinder.AllBinderLinks[regIndex].ConnectedBinderComponents)
        {
            if (refBinder != null && refBinder is ReferenceDataBinderComponent)
            {
                foreach (ReferenceDataBinderComponent refComponent in m_databinder.GetComponents<ReferenceDataBinderComponent>())
                {
                    if (refComponent.GetInstanceID() == refBinder.GetInstanceID())
                    {
                        toRemove.Add(refComponent);
                    }
                }
            }
        }

        for (int i = 0; i < toRemove.Count; i++)
        {
            DestroyImmediate(toRemove[i], true);
        }
    }
    private void RemoveAttachedDataRegistrars(SerializedProperty regProperty, int regIndex)
    {
        List<DataBinderLink> toRemove = new List<DataBinderLink>();

        foreach (DataBinderLink binderLinkComponent in m_databinder.GetComponents<DataBinderLink>())
        {
            if (binderLinkComponent.GetInstanceID() == (regProperty.GetPropertyObject() as DataBinderLink).GetInstanceID())
            {
                toRemove.Add(binderLinkComponent);
            }
        }

        for (int i = 0; i < toRemove.Count; i++)
        {
            DestroyImmediate(toRemove[i], true);
        }
    }
    private void RemoveEmptyEntriesFromRegistrars()
    {
        List<DataBinderLink> newList = new List<DataBinderLink>();

        foreach(DataBinderLink binderLink in m_databinder.AllBinderLinks)
        {
            if (binderLink != null)
                newList.Add(binderLink);
        }

        m_databinder.AllBinderLinks = newList.ToArray();
    }
    private void AddReferenceBinder(SerializedProperty binderComponentsProperty, int regIndex)
    {
        ReferenceDataBinderComponent newInstance = m_databinder.gameObject.AddComponent<ReferenceDataBinderComponent>();
        BinderComponent[] newArray = new BinderComponent[binderComponentsProperty.arraySize + 1];
        newArray[0] = newInstance;

        for(int i = 1; i < newArray.Length; i++)
        {
            newArray[i] = binderComponentsProperty.GetArrayElementAtIndex(i - 1).GetPropertyObject() as BinderComponent;
        }

        m_databinder.AllBinderLinks[regIndex].ConnectedBinderComponents = newArray;

        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }
    private void IncrementReferenceBinderData(int regIndex)
    {
        ReferenceDataBinderComponent referenceData = m_databinder.AllBinderLinks[regIndex].ConnectedBinderComponents[0] as ReferenceDataBinderComponent;
        List<StringBinder> newList = new List<StringBinder>();

        foreach(StringBinder binder in referenceData.StringBinders)
        {
            newList.Add(binder);
        }

        newList.Add(new StringBinder());
        referenceData.StringBinders = newList.ToArray();
    }
    private void DecrementReferenceBinderData(int regIndex)
    {
        ReferenceDataBinderComponent referenceData = m_databinder.AllBinderLinks[regIndex].ConnectedBinderComponents[0] as ReferenceDataBinderComponent;
        List<StringBinder> newList = new List<StringBinder>();

        foreach (StringBinder binder in referenceData.StringBinders)
        {
            newList.Add(binder);
        }

        newList.RemoveAt(newList.Count - 1);
        referenceData.StringBinders = newList.ToArray();
    }
}
