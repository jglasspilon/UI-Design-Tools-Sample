using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StringBinder : GenericBinder<string>
{
    public override bool TryBindData(Dictionary<string, JSONNode> data)
    {
        if (base.TryBindData(data))
        {
            for(int i = 0; i < m_targets.Length; i++)
            {
                m_targets[i] = data[Key];
            }
            return true;
        }
        else
            return false;
    }

    public override void ClearData()
    {
        //do nothing
    }
}
