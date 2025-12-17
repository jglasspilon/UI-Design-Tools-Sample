using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextColorBinderComponent : BinderComponent
{
    [SerializeField]
    private TextColorBinder[] m_colorBinders;
    public override IDataBindable[] GetAllBinders()
    {
        return m_colorBinders;
    }
}
