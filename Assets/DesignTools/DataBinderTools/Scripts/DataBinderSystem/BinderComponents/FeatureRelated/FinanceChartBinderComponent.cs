using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinanceChartBinderComponent : BinderComponent
{
    [SerializeField]
    private FinanceChartBinder[] m_chartBinders = new FinanceChartBinder[1];

    public override IDataBindable[] GetAllBinders()
    {
        return m_chartBinders;
    }
}
