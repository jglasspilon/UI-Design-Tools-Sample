using System;
using UnityEngine;

public class ContrastCalculateInvoker: MonoBehaviour
{
    public event Action OnCalculate;

    public virtual void RecaculateContrastRatios()
    {
        OnCalculate?.Invoke();
    }
}
