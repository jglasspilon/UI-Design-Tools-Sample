using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SliderBinder : GenericBinder<Slider>
{
    [SerializeField]
    private E.SliderBinderType m_BindToValues;

    public E.SliderBinderType BindToValues { get { return m_BindToValues; } }

    // [0] = Value
    // [1] = MaxValue

    public override bool TryBindData(Dictionary<string, JSONNode> data)
    {
        if (base.TryBindData(data))
        {
            BindToSlider(data);
            return true;
        }
        else
            return false;
    }

    public override void ClearData()
    {
        //do nothing
    }

    private void BindToSlider(Dictionary<string, JSONNode> data)
    {
        foreach(Slider target in m_targets)
        {
            if(m_BindToValues == E.SliderBinderType.JustValue)
                target.value = data[Key];

            if(m_BindToValues == E.SliderBinderType.JustMax)
                target.maxValue = data[Key];

            if(m_BindToValues == E.SliderBinderType.ValueAndMax)
            {
                target.value = data[Keys[0]];
                target.maxValue = data[Keys[1]];
            }
        }
    }
}
