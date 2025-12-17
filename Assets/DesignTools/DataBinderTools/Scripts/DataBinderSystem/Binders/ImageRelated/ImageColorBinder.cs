using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ImageColorBinder : GenericBinder<Image>
{
    public override bool TryBindData(Dictionary<string, JSONNode> data)
    {
        if (base.TryBindData(data))
        {
            string hexString = data[Key];

            if (!hexString.Contains("#"))
                hexString = "#" + data[Key];

            Color newColor;

            if (!ColorUtility.TryParseHtmlString(hexString, out newColor))
                Debug.LogError($"Color: {data[Key]} could not be parsed");

            foreach (Image target in m_targets)
            {
                target.color = new Color(newColor.r, newColor.g, newColor.b, target.color.a);
            }
            return true;
        }
        else
            return false;
    }

    public override void ClearData()
    {
        foreach (Image target in m_targets)
        {
            target.color = new Color(80, 80, 80, target.color.a);
        }
    }
}
