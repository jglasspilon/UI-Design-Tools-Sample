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
        m_cycleRoutine = StartCoroutine(MoveTo(targetPosition));
    }

    private IEnumerator MoveTo(Vector2 targetPositiion)
    {
        float timer = 0;
        float duration = GetCurveDuration(m_animCurve);
        Vector2 startPosition = m_content.localPosition;

        while(timer < duration)
        {
            timer += Time.deltaTime;
            Vector2 newPosition = Vector2.Lerp(startPosition, targetPositiion, m_animCurve.Evaluate(timer));
            m_content.localPosition = newPosition;
            yield return null;
        }

        m_content.localPosition = targetPositiion;
    }

    public float GetCurveDuration(AnimationCurve curve)
    {
        if (curve == null || curve.keys.Length == 0)
            return 0f;

        return curve.keys[curve.keys.Length - 1].time;
    }
}
