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
    bool TryBindData(Dictionary<string, JSONNode> data);
    void ClearData();
}
