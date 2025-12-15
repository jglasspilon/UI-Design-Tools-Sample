using UnityEngine;

public class ContentManager : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve m_animCurve;

    private Transform m_mainCam;
    private Coroutine m_moveRoutine;

    private void Awake()
    {
        m_mainCam = Camera.main.transform;
    }

    public void MovePageInFrame(Transform page)
    {
        if (m_moveRoutine != null)
        {
            StopCoroutine(m_moveRoutine);
        }

        m_moveRoutine = StartCoroutine(m_mainCam.MoveTo(page.position, m_animCurve));
    }
}
