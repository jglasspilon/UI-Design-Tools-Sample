using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceDataBinderComponent : BinderComponent
{
    [SerializeField]
    private StringBinder[] m_stringBinders = new StringBinder[1];

    public StringBinder[] StringBinders { get { return m_stringBinders; } set { m_stringBinders = value; } }

    public override IDataBindable[] GetAllBinders()
    {
        return m_stringBinders;
    }
}
