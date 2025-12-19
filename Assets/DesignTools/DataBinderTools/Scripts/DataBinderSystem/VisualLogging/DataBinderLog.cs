using SimpleJSON;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataBinderLog : PoolableObject
{
    [SerializeField]
    private TextMeshProUGUI m_fieldText, m_valueText;

    public void LogBinderData(string field, JSONNode value)
    {
        m_fieldText.text = field;
        m_valueText.text = value;
    }

    public override void ResetForPool()
    {
        m_fieldText.text = "";
        m_valueText.text = "";
    }
}
