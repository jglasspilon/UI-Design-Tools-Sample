using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bindable data object.
/// </summary>
public interface IDataBindable
{
    string Key { get; set; }
    string[] Keys { get; }
    public Dictionary<string, JSONNode> BoundData { get; }

    bool TryBindData(Dictionary<string, JSONNode> data);
    void ClearData();
}
