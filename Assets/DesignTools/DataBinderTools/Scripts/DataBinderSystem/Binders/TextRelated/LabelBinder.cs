using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SimpleJSON;

[System.Serializable]
public class LabelBinder : GenericBinder<TextMeshProUGUI>
{
    [SerializeField]
    private string m_format;

    [SerializeField]
    private string m_breakLineIdentifier;

    public override bool TryBindData(Dictionary<string, JSONNode> data)
    {
        if (base.TryBindData(data))
        {
            foreach (TextMeshProUGUI target in m_targets)
            {
                if (m_format.Contains("[]"))
                    target.text = m_format.Replace("[]", data[Key]);
                else
                    target.text = data[Key];

                if (m_breakLineIdentifier.HasValue())
                    target.text = target.text.Replace(m_breakLineIdentifier, "\n");
            }
            return true;
        }
        else
            return false;
    }

    public override void ClearData()
    {
        foreach (TextMeshProUGUI target in m_targets)
        {
            target.text = "";
        }
    }
}
