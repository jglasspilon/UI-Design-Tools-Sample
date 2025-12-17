using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SimpleJSON;

/// <summary>
/// Registers and binds data from a json file received from the Data Platform
/// </summary>
public class DataBinder: MonoBehaviour
{
    [SerializeField] //Optional ID used when manually organizing a list
    private string m_sortId = "";                                                                                   

    [SerializeField] //A list of all the data links attached
    private DataBinderLink[] m_allBinderLinks = new DataBinderLink[0];

    //Dictionary of all registered data
    private Dictionary<string, JSONNode> m_dataDictionary = new Dictionary<string, JSONNode>();                     
  
    public DataBinderLink[] AllBinderLinks { get { return m_allBinderLinks; } set { m_allBinderLinks = value; } }   
    public string SortID { get { return m_sortId; } }                                                               
    public Dictionary<string, JSONNode> DataDictionary { get { return m_dataDictionary; } }

    /// <summary>
    /// Registers all of the data requested by the keys in all of the connected DataBinders at the node structure for each data registry attached, and adds them to the data dictionary.
    /// </summary>
    /// <param name="json">The full unparsed Json</param>
    /// <param name="index">Represents the index representing the dataBinder's position in a list. Only necessary for use when generating from a list.</param>
    public void RegisterData(JSONNode json, int index = 0)
    {
        m_dataDictionary.Clear();

        //create a new jsonNode for each data registrar using its node structure
        foreach (DataBinderLink binderLink in m_allBinderLinks)
        {
            JSONNode data;
            JsonNodeBuilder.GetCompleteNodeStructure(json, binderLink.NodeStructureSteps, out data, index);

            if (data != null)
            {
                //go into each component binder attached to the data registrar
                foreach (BinderComponent binder in binderLink.ConnectedBinderComponents)
                {
                    //go into each IDataBindable attahced to the component binder
                    foreach (IDataBindable Idata in binder.GetAllBinders())
                    {
                        //if the key is not empty then register it and its data to the data dictionary
                        if (Idata.Key != null && Idata.Key.Length > 0)
                        {
                            if (data[Idata.Key] != null)
                            {
                                if (!m_dataDictionary.TryGetValue(Idata.Key, out JSONNode registeredValue))
                                {
                                    m_dataDictionary.Add(Idata.Key, data[Idata.Key]);
                                }
                                else
                                {
                                    registeredValue = data[Idata.Key];
                                }
                            }
                            else
                                Debug.LogError($"PAYLOAD ERROR: The key {Idata.Key} returned a null value from the json file. Likely the field {Idata.Key} does not exist at the node structure in the json payload or it has no value.");
                        }

                        //if the keys array exists and has elements, then go into each key within
                        if(Idata.Keys != null && Idata.Keys.Length > 0)
                        {
                            foreach(string key in Idata.Keys)
                            {
                                //if the key is not empty then register it and its data to the data dictionary
                                if (key != null && key.Length > 0)
                                {
                                    if (data[key].ToString().ToLower() != "null")
                                    {
                                        if (!m_dataDictionary.TryGetValue(key, out JSONNode registeredValue))
                                        {
                                            m_dataDictionary.Add(key, data[key]);
                                        }
                                        else
                                        {
                                            registeredValue = data[key];
                                        }
                                    }
                                    else
                                        Debug.LogError($"PAYLOAD ERROR: The key {key} returned a null value from the json file. Likely the field {key} does not exist at the node structure in the json payload or it has no value.");
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Binds all data to the attached component binders.
    /// </summary>
    public void BindData()
    {
        foreach (DataBinderLink binderLink in m_allBinderLinks)
        {
            foreach (BinderComponent binder in binderLink.ConnectedBinderComponents)
            {
                foreach (IDataBindable Idata in binder.GetAllBinders())
                {
                    Idata.TryBindData(m_dataDictionary);                        
                }
            }
        }
    }

    /// <summary>
    /// Clears the data from the data binder
    /// </summary>
    public void ClearData()
    {
        foreach (DataBinderLink binderLink in m_allBinderLinks)
        {
            foreach (BinderComponent binder in binderLink.ConnectedBinderComponents)
            {
                foreach (IDataBindable Idata in binder.GetAllBinders())
                {
                    Idata.ClearData();
                }
            }
        }
    }

    /// <summary>
    /// Fetch's the data from the data dictionary represented by the key.
    /// </summary>
    /// <param name="key">Desired key for the data in the dictionary.</param>
    /// <returns>Returns the string data represented by the key in the data dictionary.</returns>
    public JSONNode GetData(string key)
    {
        if (HasKey(key))
        {
            return m_dataDictionary[key];
        }

        else
        {
            Debug.LogError($"The Data Binder {gameObject.name}'s dictionary does not contain data for the desired key: {key}");
            return null;
        }
    }

    /// <summary>
    /// Checks to see if a key exists in the data dictionary.
    /// </summary>
    /// <param name="key">Key to check for.</param>
    /// <returns>Returns tru if the key exists.</returns>
    public bool HasKey(string key)
    {
        return m_dataDictionary.ContainsKey(key);
    }

    /// <summary>
    /// Prints out the dictionary content for debugging purposes
    /// </summary>
    public void DebugDictionary()
    {
        string debugString = "Databinder " + gameObject.name + " contains:\n";
        foreach(KeyValuePair<string, JSONNode> entry in m_dataDictionary)
        {
            debugString += $"Key: {entry.Key} --> {entry.Value}\n";
        }

        if (m_dataDictionary.Count == 0)
            debugString += "NOTHING"; 

        Debug.Log(debugString);
    }
}
