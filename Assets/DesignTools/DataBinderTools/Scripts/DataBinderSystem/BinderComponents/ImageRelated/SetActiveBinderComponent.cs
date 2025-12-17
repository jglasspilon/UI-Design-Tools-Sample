using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveBinderComponent : BinderComponent
{
    [SerializeField]
    private SetActiveBinder[] m_gameObjectEnablerBinders = new SetActiveBinder[1];

    public override IDataBindable[] GetAllBinders()
    {
        return m_gameObjectEnablerBinders;
    }
}
