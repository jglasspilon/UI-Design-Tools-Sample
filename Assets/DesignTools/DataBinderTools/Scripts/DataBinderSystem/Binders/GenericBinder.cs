using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class GenericBinder<T>: IDataBindable
{
    [SerializeField]
    protected T[] m_targets = new T[1];

    [SerializeField]
    protected string[] m_keys;

    [SerializeField]
    protected string m_key = "";

    public string Key { get { return m_key; } set { m_key = value; } }
    public string[] Keys { get { return m_keys; } }

    /// <summary>
    /// Binds the data accordingly based on extended type
    /// </summary>
    /// <param name="data">The dictionary in which to fetch the data to bind.</param>
    /// <returns>Returns true if key exists in data and binding was successfull.</returns>
    public virtual bool TryBindData(Dictionary<string, JSONNode> data)
    {
        if (Key != "" && !data.ContainsKey(Key))
        {
            ClearData();
            Debug.LogError($"PAYLOAD ERROR: The key '{Key}' does not exist in the dictionary. Likely the requested payload does not contain the field '{Key}' at the node structure specified in the attached data registry or it's value is null.");
            return false;
        }

        foreach(string key in Keys)
        {
            if (!data.ContainsKey(key))
            {
                ClearData();
                Debug.LogError($"PAYLOAD ERROR: The key '{key}' does not exist in the dictionary. Likely the requested payload does not contain the field '{key}' at the node structure specified in the attached data registry or its value is null.");
                return false;
            }
        }

        return true;
    }

    public abstract void ClearData();
}
