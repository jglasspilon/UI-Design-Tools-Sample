using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Coroutine with the ability to return data once completed
/// </summary>
public class CoroutineWithData
{
    public Coroutine coroutine { get; private set; }
    public object result { get; private set; }
    private IEnumerator target;
    /// <summary>
    /// Allows to track and return data after coroutine completion. The particular coroutine must 'yield return' the data desired upon completion
    /// </summary>
    /// <param name="owner">Class calling this coroutine (generally should be 'this')</param>
    /// <param name="target">The Coroutine to run and return data from</param>
    public CoroutineWithData(MonoBehaviour owner, IEnumerator target)
    {
        this.target = target;
        this.coroutine = owner.StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        while (target.MoveNext())
        {
            result = target.Current;
            yield return result;
        }
    }
}
