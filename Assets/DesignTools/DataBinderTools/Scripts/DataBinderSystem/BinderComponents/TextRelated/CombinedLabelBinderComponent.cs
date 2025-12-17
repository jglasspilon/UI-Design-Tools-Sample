using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinedLabelBinderComponent : BinderComponent
{
    [SerializeField]
    private CombinedLabelBinder[] m_combinedLabelBinders = new CombinedLabelBinder[1];

    public override IDataBindable[] GetAllBinders()
    {
        return m_combinedLabelBinders;
    }
}
