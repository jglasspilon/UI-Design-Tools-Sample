using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectTransformBinderComponent : BinderComponent
{
    [SerializeField]
    private RectTransformBinder[] m_rectTransformBinders = new RectTransformBinder[1];

    public override IDataBindable[] GetAllBinders()
    {
        return m_rectTransformBinders;
    }
}
