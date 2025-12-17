using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SimpleJSON;
using TMPro;

public class DataBinderDemo : MonoBehaviour
{
    [SerializeField]
    private DataBinder m_dataBinder;                            

    [SerializeField]
    private DataBinderList m_dataList;                          

    [SerializeField]
    private Animator m_chartWipe;                              

    [SerializeField]
    private ScrollRect m_listScroll;

    private bool m_isInitialized;
    private const string PATH_TO_JSON_FILE = "/JSONData/";      
    private readonly string[] JSON_FILE_NAMES = new string[] { "Dow.json", "Nasdaq.json", "NYSE.json" };
    private int m_fileIndex = 0;

    private void Start()
    {
        SetStartupData();
    }

    /// <summary>
    /// Read from the startup data json file and bind the data to the dataBinder and dataBinder list, bypassing the wipe animation
    /// </summary>
    private void SetStartupData()
    {
        TryChangeData(JSON_FILE_NAMES[m_fileIndex]);
        m_isInitialized = true;
    }

    /// <summary>
    /// Cycle through the possible json files for the demo.
    /// </summary>
    /// <param name="direction">Amount to modify the index by this cycle</param>
    public void CycleFile(int increment)
    {
        int max = JSON_FILE_NAMES.Length - 1;
        m_fileIndex += increment;

        if (m_fileIndex < 0)
            m_fileIndex = max;
        else if (m_fileIndex > max)
            m_fileIndex = 0;

        TryChangeData(JSON_FILE_NAMES[m_fileIndex]);
    }

    /// <summary>
    /// Reads from the JSON file requested and if the JSON data exists, change the data
    /// </summary>
    /// <param name="JSONFile"></param>
    private void TryChangeData(string JSONFile)
    {
        JSONNode json = FileReader.ReadJSONFromFile(Application.streamingAssetsPath + PATH_TO_JSON_FILE + JSONFile);
        if (json != null)
            StartCoroutine(ChangeData(json, m_isInitialized));
        else
            Debug.LogError($"JSON file {JSONFile} does not exist. Could not change data.");
    }

    /// <summary>
    /// Function that registers data to the dataBinder and dataBinder list, and the generates the list of prefabs accordingly and binds the data to the data binder 
    /// Both the dataBinder and the dataBinder list use the same json data in this example but target different nodes within 
    /// </summary>
    /// <param name="json">JSON data to pass to the dataBinder elements</param>
    /// <returns></returns>
    private IEnumerator ChangeData(JSONNode json, bool playAnimation)
    {
        //Registers the json data provided to the dataBinder
        m_dataBinder.RegisterData(json);

        //Plays the animation for the chart wipe and waits for half of the animation
        if(playAnimation)
            yield return StartCoroutine(AnimationDispatcher.TriggerAnimation(m_chartWipe, "Play", 0.5f));

        //Reset the list scroll to the beginning
        m_listScroll.normalizedPosition = Vector2.zero;

        //Generates a list of dataBinders based on the json provided
        m_dataList.GenerateList(json);

        //Forces the list group layout to rebuild for proper display
        StartCoroutine(RefreshLayout());

        //Bind the new data to all databinder components tied to the dataBinder
        m_dataBinder.BindData(); 
    }

    private IEnumerator RefreshLayout()
    {
        yield return null;
        (m_listScroll.transform as RectTransform).ForceRebuildNested();
    }
}
