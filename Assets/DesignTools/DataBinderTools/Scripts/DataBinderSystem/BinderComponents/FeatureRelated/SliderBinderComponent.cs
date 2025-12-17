using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderBinderComponent : BinderComponent
{
    [SerializeField]
    private SliderBinder[] m_sliderBinders = new SliderBinder[1];

    public override IDataBindable[] GetAllBinders()
    {
        return m_sliderBinders;
    }
}
