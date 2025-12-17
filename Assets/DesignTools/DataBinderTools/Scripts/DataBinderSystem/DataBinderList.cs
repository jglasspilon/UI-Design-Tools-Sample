using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System;

/// <summary>
/// Controls and generates a list of dataBinders
/// </summary>
public class DataBinderList : MonoBehaviour
{
    [SerializeField]
    private string[] m_nodeStructure = new string[1];

    [SerializeField]
    private DataBinder m_databinderPrefabToGenerate;

    [SerializeField]
    private Transform m_holder;

    [SerializeField]
    private bool m_isAnimatedList;

    [SerializeField]
    private string m_animationTriggerOut;

    [SerializeField]
    private string m_animationTriggerIn;

    [SerializeField]
    private string m_animationTriggerUpdate;

    [SerializeField]
    private float m_delay = 0.0f;

    [SerializeField]
    private float m_wait = 0.0f;

    [SerializeField]
    private bool m_isCapped;

    [SerializeField]
    private int m_maxCount;

    [SerializeField]
    private bool m_isManuallySorted;

    [SerializeField]
    private string m_sortField;

    public float WaitTime { get { return m_wait; } set { m_wait = value; } }
    public List<DataBinder> Binders { get; private set; } = new List<DataBinder>();
    public bool IsGenerating { get; private set; }
    public int MaxCount { get { return m_maxCount;} set { m_maxCount = value; } }

    public bool HasContent
    {
        get { return Binders.Count > 0; }
    }

    private void Awake()
    {
        if(transform.childCount > 0)
        {
            foreach(DataBinder binder in GetComponentsInChildren<DataBinder>())
            {
                Binders.Add(binder);
            }
        }
    }

    /// <summary>
    /// Performs the bind operation on all databinders in the list
    /// </summary>
    public void BindList()
    {
        foreach (DataBinder binder in Binders)
        {
            binder.BindData();
        }
    }

    /// <summary>
    /// Generates a list of databinders based on the supplied json file. Clears current list if already exists
    /// </summary>
    /// <param name="json"></param>
    public void GenerateList(JSONNode json, DataBinder prefabOverride = null)
    {
        StartCoroutine(GenerateListRoutine(json, prefabOverride));
    }

    /// <summary>
    /// Instantly generates a list of databinders based on the supplied json file. Clears current list if already exists
    /// </summary>
    /// <param name="json"></param>
    public void GenerateListInstant(JSONNode json, DataBinder prefabOverride = null)
    {
        if (HasContent)
            ClearListInstant();

        JSONNode jsonStructure;
        JsonNodeBuilder.GetCompleteNodeStructure(json, m_nodeStructure, out jsonStructure, 0);
        int numberToGenerate = 0;

        if (m_isCapped)
            numberToGenerate = m_maxCount > jsonStructure.Count ? jsonStructure.Count : m_maxCount;

        if (prefabOverride != null)
            m_databinderPrefabToGenerate = prefabOverride;

        Binders = Generator.GenerateListFromData(jsonStructure, m_databinderPrefabToGenerate, m_holder, numberToGenerate);
    }

    /// <summary>
    /// Clears the current list of Databinders
    /// </summary>
    public void ClearList()
    {
        if (isActiveAndEnabled)
            StartCoroutine(ClearListRoutine());
        else
            ClearListInstant();
    }

    /// <summary>
    /// Clears all children of the DataBinderList. For emergency or instant clearing only
    /// </summary>
    public void ForceClearInstant()
    {
        Binders.Clear();
        for(int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// Registers new data to the list of databinders without clearing the whole list
    /// </summary>
    /// <param name="json"></param>
    public void UpdateList(JSONNode json, bool createNodeStructure = true)
    {
        JSONNode jsonStructure;

        if (createNodeStructure)
            JsonNodeBuilder.GetCompleteNodeStructure(json, m_nodeStructure, out jsonStructure, 0);
        else
            jsonStructure = json;

        if (jsonStructure.Count > 0)
        {
            if (m_isAnimatedList && m_databinderPrefabToGenerate.GetComponent<Animator>() == null)
                Debug.LogError($"The prefab {m_databinderPrefabToGenerate.name} is missing an animator and you are trying to animate it");

            if (m_isAnimatedList && m_animationTriggerUpdate.HasValue() && m_databinderPrefabToGenerate.GetComponent<Animator>() != null)
            {
                StartCoroutine(UpdateWithAnimation(jsonStructure));
                return;
            }            
            else
                UpdateListInstant(json);
        }
        else
        {
            if (m_isAnimatedList && m_animationTriggerOut.HasValue() && m_databinderPrefabToGenerate.GetComponent<Animator>() != null)
            {
                StartCoroutine(ClearListRoutine());
                return;
            }
            else
            {
                ClearListInstant();
                Debug.LogWarning("Trying to update from empty list in Json. Update was skipped");
            }
        }   
    }

    /// <summary>
    /// Registers new data to the list of databinders. Does not clear list unless the response is not the same size as the current List.
    /// </summary>
    /// <param name="json"></param>
    public void UpdateListInstant(JSONNode json, DataBinder prefabOverride = null)
    {
        JSONNode jsonStructure;
        JsonNodeBuilder.GetCompleteNodeStructure(json, m_nodeStructure, out jsonStructure, 0);
        int numberToGenerate = jsonStructure.Count;

        if (numberToGenerate > 0)
        {
            //find what is lowest, the size fo the requested list or the current list
            if (m_isCapped)
                numberToGenerate = m_maxCount > jsonStructure.Count ? jsonStructure.Count : m_maxCount;

            if (numberToGenerate != Binders.Count)
            {
                ClearListInstant();
                GenerateListInstant(json, prefabOverride);
            }
            else
            {
                //register the new data to all available binders
                for (int i = 0; i < numberToGenerate; i++)
                {
                    //if manually sorted, ignore the order from the data and get the data from the element with the corresponding ID
                    if (m_isManuallySorted && m_sortField.HasValue() && Binders[i].SortID.HasValue())
                    {
                        foreach(JSONNode jsonElement in jsonStructure)
                        {
                            if(jsonElement[m_sortField] == Binders[i].SortID)
                            {
                                Binders[i].RegisterData(jsonElement);
                                Binders[i].BindData();
                                Debug.Log("sort");
                            }
                        }
                    }

                    //otherwise, get the data from the same element in the array
                    else
                    {
                        Binders[i].RegisterData(jsonStructure[i]);
                        Binders[i].BindData();
                    }
                }
            }
        }
    }

    //Coroutine controlling the list generating (animates if it is an animated list)
    private IEnumerator GenerateListRoutine(JSONNode json, DataBinder prefabOverride)
    {
        IsGenerating = true;

        if (HasContent)
        {
            yield return StartCoroutine(ClearListRoutine());
        }

        if (m_isAnimatedList)
        {
            while (Binders.Count > 0)
                yield return null;
        }

        JSONNode jsonStructure;
        JsonNodeBuilder.GetCompleteNodeStructure(json, m_nodeStructure, out jsonStructure, 0);
        int numberToGenerate = 0;

        if (m_isCapped)
            numberToGenerate = m_maxCount > jsonStructure.Count ? jsonStructure.Count : m_maxCount;

        if (prefabOverride != null)
            m_databinderPrefabToGenerate = prefabOverride;

        Binders = Generator.GenerateListFromData(jsonStructure, m_databinderPrefabToGenerate, m_holder, numberToGenerate);
        IsGenerating = false;

        if (m_isAnimatedList)
        {
            if (m_databinderPrefabToGenerate.GetComponent<Animator>() == null)
            {
                Debug.LogError($"The prefab {m_databinderPrefabToGenerate.name} is missing an animator and you are trying to animate it");
                yield break;
            }

            if (m_animationTriggerIn.HasValue())
            {
                foreach (DataBinder binder in Binders)
                {
                    binder.GetComponent<Animator>().SetTrigger(m_animationTriggerIn);

                    if (m_delay != 0)
                        yield return new WaitForSeconds(m_delay);
                }
            }
        }
    }

    //Coroutine controlling the clearing of the list (animates if it is an animated list)
    private IEnumerator ClearListRoutine()
    {
        //if is animated, play the out animation with appropriate delay and wait time
        if (m_isAnimatedList)
        {
            if (m_databinderPrefabToGenerate.GetComponent<Animator>() == null)
            {
                Debug.LogError($"The prefab {m_databinderPrefabToGenerate.name} is missing an animator and you are trying to animate it");
                yield break;
            }

            if (m_animationTriggerOut.HasValue())
            {
                foreach (DataBinder binder in Binders)
                {
                    Animator anim = binder.GetComponent<Animator>();
                    anim.SetTrigger(m_animationTriggerOut);

                    if (m_delay != 0)
                        yield return new WaitForSeconds(m_delay);
                }
            }

            yield return new WaitForSeconds(m_wait);
        }

        //clear the list
        for (int i = 0; i < Binders.Count; i++)
        {
            Destroy(Binders[i].gameObject);
        }

        Binders.Clear();

        if (m_isAnimatedList)
            yield return null;
    }

    //Instantly clears the list regardless of if it is an animated list or not
    private void ClearListInstant()
    {
        //clear the list
        for (int i = 0; i < Binders.Count; i++)
        {
            DestroyImmediate(Binders[i].gameObject);
        }

        Binders.Clear();
    }

    //Updates the data of each databinder and plays the update animation if it is an animated list
    private IEnumerator UpdateWithAnimation(JSONNode json)
    {
        //find what is lowest, the size fo the requested list or the current list
        int lowestCount = json.Count < Binders.Count ? json.Count : Binders.Count;

        float animationTimer = 0;
        foreach (DataBinder binder in Binders)
        {
            binder.GetComponent<Animator>().SetTrigger(m_animationTriggerUpdate);
            animationTimer = binder.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        }

        yield return new WaitForSeconds(animationTimer/2);

        for (int i = 0; i < lowestCount; i++)
        {
            Binders[i].RegisterData(json[i]);
            Binders[i].BindData();
        }

        //if there are more binders then needed, destroy the extra
        if (json.Count < Binders.Count)
        {
            for (int i = json.Count - 1; i < Binders.Count; i++)
            {
                DestroyImmediate(Binders[i].gameObject);
                Binders.RemoveAt(i);
            }
        }

        //if there are too few binders, generate new ones as needed
        else if (json.Count > Binders.Count)
        {
            int numberToGenerate = json.Count;

            if (m_isCapped)
                if (numberToGenerate > m_maxCount)
                    numberToGenerate = m_maxCount;

            for (int i = Binders.Count - 1; i < numberToGenerate; i++)
            {
                DataBinder newInstance = Generator.GenerateSingleDataBinder(json[i], m_databinderPrefabToGenerate, m_holder);
                Binders.Add(newInstance);

                if (m_isAnimatedList)
                {
                    if (m_databinderPrefabToGenerate.GetComponent<Animator>() == null)
                    {
                        Debug.LogError($"The prefab {m_databinderPrefabToGenerate.name} is missing an animator and you are trying to animate it");
                    }
                    else
                        newInstance.GetComponent<Animator>().SetTrigger(m_animationTriggerIn);
                }
            }
        }
    }
}
