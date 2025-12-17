using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FinanceChartBinder : GenericBinder<ChartDraw>
{
    [SerializeField]
    private E.FinanceChartType m_chartType;

    public E.FinanceChartType ChartType { get { return m_chartType; }}

    //entries in keys are hardset by the inspector based on the selected enum and represent the following

    //  For Single Chart:
    //  [0] = NormalizedValues
    //  [1] = yAxisLabels
    //  [2] = xAxisLabels
    //  [3] = xAxisLabelsPosition
    //  [4] = previousDayClose

    //  For Candlestick Chart:
    //  [0] = NormalizedClosedValues
    //  [1] = yAxisLabels
    //  [2] = xAxisLabels
    //  [3] = xAxisLabelsPosition
    //  [4] = NormalizedOpenValues
    //  [5] = NormalizedLowValues
    //  [6] = NormalizedHighValues

    //  For Performance Chart:
    //  [0] = NormalizedNetChange1
    //  [1] = NormalizedNetChange2
    //  [2] = yAxisLabels
    //  [3] = xAxisLabels
    //  [4] = xAxisLabelsPosition


    public override bool TryBindData(Dictionary<string, JSONNode> data)
    {
        if (base.TryBindData(data))
        {
            DrawChartFromSelection(data);
            return true;
        }
        else
            return false;
    }

    public override void ClearData()
    {
        foreach(ChartDraw target in m_targets)
        {
            target.ClearGraph();
        }
    }

    private void DrawChartFromSelection(Dictionary<string, JSONNode> data)
    {
        foreach(ChartDraw target in m_targets) {
            switch (m_chartType)
            {
                case E.FinanceChartType.SingleChart:
                    target.DrawChart_Single(data[Keys[0]], data[Keys[1]], data[Keys[2]], data[Keys[3]], data[Keys[4]]);
                    break;

                case E.FinanceChartType.CandlestickChart:
                    target.DrawChart_CandleStick(data[Keys[0]], data[Keys[1]], data[Keys[2]], data[Keys[3]], data[Keys[4]], data[Keys[5]], data[Keys[6]]);
                    break;

                case E.FinanceChartType.PerformanceChart:
                    target.DrawChart_Performance(data[Keys[0]], data[Keys[1]], data[Keys[2]], data[Keys[3]], data[Keys[4]]);
                    break;
            }
        }
    }
}
