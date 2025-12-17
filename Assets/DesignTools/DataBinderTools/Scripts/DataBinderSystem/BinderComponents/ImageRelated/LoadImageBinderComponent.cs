using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadImageBinderComponent : BinderComponent
{
    [SerializeField]
    private LoadImageBinder[] m_imageBinders;
    public override IDataBindable[] GetAllBinders()
    {
        return m_imageBinders;
    }
}
