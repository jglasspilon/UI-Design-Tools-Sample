using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ReplaceWithPrefabWindow : EditorWindow
{
    #region GUI Fields
    private string m_GUI_fieldsHeader = "Replacement Fields";
    private string m_GUI_previewHeader = "Preview Objects";
    
    private GUIStyle m_GUI_myFoldoutStyle;
    private Color m_GUI_defaultBgColor;
    private Color m_GUI_defaultTextColor;
    private Vector2 m_GUI_scrollPos;
    private bool m_GUI_isInitialized;
    #endregion

    #region Functionality Fields
    private string m_targetTag = "";
    private GameObject m_replacementPrefab;
    private GameObject[] m_old = new GameObject[0];
    #endregion

    private const float MAX_WIDTH = 385f;
    private const float MAX_HEIGHT = 610f;
    private const float BUTTON_WIDTH = 120f;

    [MenuItem("Window/Astucemedia/Replace With Prefab")]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow(typeof(ReplaceWithPrefabWindow), false, "Replace With Prefab");
        window.maxSize = new Vector2(MAX_WIDTH, MAX_HEIGHT);
        window.minSize = window.maxSize;
    }

    void OnGUI()
    {
        if(!m_GUI_isInitialized)
            Initialize();

        GUI.backgroundColor = Color.green;
        EditorGUILayout.BeginHorizontal(GUI.skin.box, GUILayout.MaxWidth(155));
        {
            GUILayout.Space(5);
            EditorGUILayout.LabelField(m_GUI_fieldsHeader, EditorStyles.boldLabel, GUILayout.Width(130));
            EditorGUILayout.EndHorizontal();
        }
        GUI.backgroundColor = m_GUI_defaultBgColor;

        ShowFieldsGUI();
        GUILayout.Space(8);

        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Space(380 - BUTTON_WIDTH);
            if (m_targetTag.Length > 0 && m_replacementPrefab != null)
            {
                if (GUILayout.Button("Find & Preview", GUILayout.Width(BUTTON_WIDTH)))
                {
                    FindObjectsToReplace();
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        GUILayout.Space(8);

        if (m_old.Length > 0)
        {
            ShowPreviewGUI();

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical(GUILayout.Width(370 - BUTTON_WIDTH));
                {
                    GUI.color = Color.red;
                    EditorGUILayout.LabelField("!!WARNING!!", GUILayout.Width(370 - BUTTON_WIDTH));
                    GUI.color = m_GUI_defaultTextColor;

                    EditorGUILayout.LabelField("Make sure to save your scene", GUILayout.Width(370 - BUTTON_WIDTH));
                    EditorGUILayout.LabelField("before replacing objects as this cannot ", GUILayout.Width(370 - BUTTON_WIDTH));
                    EditorGUILayout.LabelField("easily be undone.", GUILayout.Width(370 - BUTTON_WIDTH));
                    EditorGUILayout.EndVertical();
                }
                
                if (m_targetTag.Length > 0 && m_replacementPrefab != null)
                {
                    if (GUILayout.Button("Replace", GUILayout.Width(BUTTON_WIDTH), GUILayout.Height(70)))
                    {
                        Replace();
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
        }       
    }

    private void Initialize()
    {
        m_GUI_myFoldoutStyle = new GUIStyle(EditorStyles.foldout);
        m_GUI_myFoldoutStyle.fontStyle = FontStyle.Bold;
        m_GUI_defaultBgColor = GUI.backgroundColor;
        m_GUI_defaultTextColor = GUI.color;
        m_GUI_isInitialized = true;
    }

    private void ShowFieldsGUI()
    {
        EditorGUILayout.BeginHorizontal(GUI.skin.box, GUILayout.MaxWidth(370));
        {
            GUILayout.Space(5);
            EditorGUILayout.BeginVertical();
            {             
                EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(300));
                {
                    EditorGUILayout.BeginVertical();
                    {
                        EditorGUILayout.LabelField("Target tag:", GUILayout.Width(150));
                        m_targetTag = EditorGUILayout.TextField(m_targetTag, GUILayout.Width(150));
                        EditorGUILayout.EndVertical();
                    }

                    GUILayout.Space(15);

                    EditorGUILayout.BeginVertical();
                    {
                        GUILayout.Space(18);
                        EditorGUILayout.LabelField("->", GUILayout.Width(15));
                        EditorGUILayout.EndVertical();
                    }

                    GUILayout.Space(15);

                    EditorGUILayout.BeginVertical();
                    {
                        EditorGUILayout.LabelField("Replacement Prefab:", GUILayout.Width(150));
                        m_replacementPrefab = EditorGUILayout.ObjectField(m_replacementPrefab, typeof(GameObject), true, GUILayout.Width(160)) as GameObject;
                        EditorGUILayout.EndVertical();
                    }
                    EditorGUILayout.EndHorizontal();
                }
                GUILayout.Space(5);
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    private void ShowPreviewGUI()
    {
        GUI.backgroundColor = Color.yellow;
        EditorGUILayout.BeginHorizontal(GUI.skin.box, GUILayout.MaxWidth(155));
        {
            GUILayout.Space(5);
            EditorGUILayout.LabelField(m_GUI_previewHeader, EditorStyles.boldLabel, GUILayout.Width(130));
            EditorGUILayout.EndHorizontal();
        }
        GUI.backgroundColor = m_GUI_defaultBgColor;

        EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.MaxWidth(370));
        {
            m_GUI_scrollPos = EditorGUILayout.BeginScrollView(m_GUI_scrollPos, GUILayout.Width(370), GUILayout.MaxHeight(375));
            { 
                foreach (GameObject obj in m_old)
                {
                    EditorGUILayout.LabelField(obj.name);
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }
    }

    private void FindObjectsToReplace()
    {
        m_old = GameObject.FindGameObjectsWithTag(m_targetTag);
    }

    private void Replace()
    {
        List<GameObject> _new = new List<GameObject>();
        List<int> indexs = new List<int>();

        //generate the object from the prefab (set parent, set, anchored position, set name)
        foreach (GameObject obj in m_old)
        {
            indexs.Add(obj.transform.GetSiblingIndex());
            GameObject newObj = PrefabUtility.InstantiatePrefab(m_replacementPrefab) as GameObject;
            newObj.transform.parent = obj.transform.parent;
            newObj.transform.position = obj.transform.position;
            newObj.transform.rotation = obj.transform.rotation;
            newObj.transform.localScale = obj.transform.localScale;
            newObj.name = obj.name;
            newObj.transform.SetAsLastSibling();
            _new.Add(newObj);
        }

        //destroy the old gameobjects
        for (int i = 0; i < m_old.Length; i++)
        {
            DestroyImmediate(m_old[i]);
        }

        m_old = new GameObject[0];

        //reorganize approriately
        for (int i = 0; i < _new.Count; i++)
        {
            _new[i].transform.SetSiblingIndex(indexs[i]);
        }
    }
}
