using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;
using SimpleJSON;

[System.Serializable]
public class CombinedLabelBinder : GenericBinder<TextMeshProUGUI>
{
    [SerializeField]
    private string m_format;

    [SerializeField]
    private string m_lineBreakIdentifier;

    public override bool TryBindData(Dictionary<string, JSONNode> data)
    {
        if (base.TryBindData(data))
        {
            if (m_format.Contains("[]"))
            {
                string formattedText = FormatText(data);
                foreach (TextMeshProUGUI target in m_targets)
                {
                    target.text = formattedText;
                    if (m_lineBreakIdentifier.HasValue())
                        target.text = target.text.Replace(m_lineBreakIdentifier, "\n");
                }                   
            }
            else
            {
                Debug.LogError($"It is impossible to format the text. Format string must contain equivalent number of '[]' as there are Keys.");
                return false;
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

    private string FormatText(Dictionary<string, JSONNode> data)
    {
        string result = m_format;
        Regex rgx = new Regex("\\[]");

        foreach(string key in m_keys)
        {
            result = rgx.Replace(result, data[key], 1);
        }

        return result;
    }
}
