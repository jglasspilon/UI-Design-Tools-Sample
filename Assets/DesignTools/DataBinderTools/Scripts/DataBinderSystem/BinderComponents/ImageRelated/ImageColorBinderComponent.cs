using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageColorBinderComponent : BinderComponent
{
    [SerializeField]
    private ImageColorBinder[] m_colorBinders;
    public override IDataBindable[] GetAllBinders()
    {
        return m_colorBinders;
    }
}
