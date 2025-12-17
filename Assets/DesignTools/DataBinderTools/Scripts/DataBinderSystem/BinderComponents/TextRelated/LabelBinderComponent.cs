using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LabelBinderComponent : BinderComponent
{
    [SerializeField]
    private LabelBinder[] m_labelBinders = new LabelBinder[1];

    public override IDataBindable[] GetAllBinders()
    {
        return m_labelBinders;
    }
}
