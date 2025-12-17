using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RectTransformBinder : GenericBinder<RectTransform>
{
    [SerializeField]
    private float m_scale = 1.0f;

    //entries in keys are hardset by the inspector and represent the following
    //  [0] = x position
    //  [1] = y position
    //  [2] = width
    //  [3] = height

    public override bool TryBindData(Dictionary<string, JSONNode> data)
    {
        if (base.TryBindData(data))
        {
            if (TrySetRectTransformData(data))
                return true;
            else
                return false;
        }
        else
            return false;
    }

    public override void ClearData()
    {
        foreach (RectTransform target in m_targets)
        {
            target.anchoredPosition = new Vector2(99999, 99999);
            target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);
            target.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
        }
    }

    private bool TrySetRectTransformData(Dictionary<string, JSONNode> data)
    {
        float xPos = 0;
        float yPos = 0;
        float width = 0;
        float height = 0;

        try
        {
            xPos = data[Keys[0]];
            yPos = data[Keys[1]];
            width = data[Keys[2]];
            height = data[Keys[3]];

            foreach (RectTransform target in m_targets)
            {
                target.anchoredPosition = new Vector2(xPos * m_scale, target.anchoredPosition.y);
                target.anchoredPosition = new Vector2(target.anchoredPosition.x, yPos * -m_scale);
                target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width * m_scale);
                target.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height * m_scale);
            }
            return true;
        }
        catch(System.Exception ex)
        {
            Debug.LogError(ex);
            return false;
        }      
    }
}
