using System;
using UnityEngine;

public class ContrastCalculateInvoker: MonoBehaviour
{
    public event Action OnCalculate;

    protected virtual void RecaculateContrastRatios()
    {
        OnCalculate?.Invoke();
    }
}
