using UnityEngine;

public class HorizontalLoop : MonoBehaviour
{
    [SerializeField]
    private RectTransform m_content, m_viewport;

    [SerializeField]
    private float m_itemWidth, m_spacing, m_viewportWidth;

    [SerializeField]
    private bool m_runInUpdate;

    private int m_itemCount;
    private float m_totalSpan, m_viewportSpanWithBuffer;

    private void Start()
    {
        m_itemCount = m_content.childCount;
        m_totalSpan = m_itemCount * (m_itemWidth + m_spacing);
        m_viewportSpanWithBuffer = m_viewportWidth + m_itemWidth + m_spacing;
    }

    private void Update()
    {
        if (m_runInUpdate)
            UpdateLoop();
    }

    public void UpdateLoop()
    {
        for (int i = 0; i < m_content.childCount; i++)
        {
            RectTransform item = m_content.GetChild(i) as RectTransform;
            Vector3 localPos = m_viewport.InverseTransformPoint(item.position);

            if (localPos.x < -m_viewportSpanWithBuffer / 2f)
            {
                item.localPosition += new Vector3(m_totalSpan, 0, 0);
            }
            else if (localPos.x > m_viewportSpanWithBuffer / 2f)
            {
                item.localPosition -= new Vector3(m_totalSpan, 0, 0);
            }
        }
    }
}
