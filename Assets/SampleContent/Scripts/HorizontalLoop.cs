using System;
using System.Collections;
using UnityEngine;

public class HorizontalLoop : MonoBehaviour
{
    [SerializeField]
    private RectTransform m_content, m_viewport;

    [SerializeField]
    private float m_itemWidth, m_spacing, m_viewportWidth;

    [SerializeField]
    private bool m_runInUpdate;

    private int m_previousHash;
    private int m_itemCount;
    private float m_totalSpan, m_viewportSpanWithBuffer;
    private bool m_waitingForLayoutRebuild;

    private void Update()
    {
        if (m_runInUpdate)
            UpdateLoop();
    }

    public void UpdateLoop()
    {
        if (m_waitingForLayoutRebuild)
            return;

        if (IsDirty())
        {
            StartCoroutine(RecalculateAfterRebuild());
            return;
        }

        for (int i = 0; i < m_itemCount; i++)
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

    private IEnumerator RecalculateAfterRebuild()
    {
        m_waitingForLayoutRebuild = true;
        yield return null;
        yield return new WaitForEndOfFrame();

        RecalculateSpan();
        m_previousHash = ComputeChildrenHash(m_content);
        m_waitingForLayoutRebuild = false;
    }

    private int ComputeChildrenHash(Transform parent)
    {
        int hash = 0;
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            hash = HashCode.Combine(hash, child.GetInstanceID());
        }
        return hash;
    }

    private bool IsDirty()
    {
        int currentHash = ComputeChildrenHash(m_content);
        return currentHash != m_previousHash;
    }

    private void RecalculateSpan()
    {
        m_totalSpan = 0;
        m_itemCount = m_content.childCount;
        float largestWidth = float.MinValue;

        for (int i = 0; i < m_itemCount; i++)
        {
            RectTransform item = m_content.GetChild(i) as RectTransform;
            float itemWidth = item.rect.width;
            m_totalSpan += itemWidth + m_spacing;

            if (largestWidth < itemWidth)
                largestWidth = itemWidth;
        }

        m_viewportSpanWithBuffer = m_viewportWidth + largestWidth + m_spacing;
    }
}
