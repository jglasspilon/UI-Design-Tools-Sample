using System.Collections;
using UnityEngine;

public class ContentCycler : MonoBehaviour
{
    [SerializeField]
    private Transform m_content;

    [SerializeField]
    private Vector2 m_cycleDistance;

    [SerializeField]
    private AnimationCurve m_animCurve;

    private int m_count;
    private Coroutine m_cycleRoutine;

    public void Cycle(int cycleBy)
    {
        m_count += cycleBy;

        if (m_cycleRoutine != null)
            StopCoroutine(m_cycleRoutine);

        Vector2 targetPosition = m_cycleDistance * m_count;
        m_cycleRoutine = StartCoroutine(m_content.MoveToLocal(targetPosition, m_animCurve));
    }
}
