using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class E 
{
    #region DataPlatform Enums
    public enum FinanceChartType
    {
        SingleChart = 0,
        CandlestickChart = 1,
        PerformanceChart = 2,
    }

    public enum SliderBinderType
    {
        JustValue = 0,
        ValueAndMax = 1,
        JustMax = 2,
    }

    public enum CandidateImageType
    {
        Headshots = 0,
        Cutouts = 1,
    }

    public enum ImageSource
    {
        Url = 0,
        Resources = 1,
        StreamingAssets = 2,
    }
    #endregion
}
