using UnityEngine;
using UnityEngine.UIElements;

public class BlendSelector : MonoBehaviour
{
    [SerializeField]
    private BlendableImage m_blendableImage;

    private int m_blendIndex;
    private const int MAX_BLEND_INDEX = 18;

    private void OnDisable()
    {
        m_blendIndex = 0;
        m_blendableImage.material.SetFloat("_BlendMode", 0);
    }

    public void CycleBlend(int increment)
    {
        m_blendIndex += increment;

        if (m_blendIndex < 0)
            m_blendIndex = MAX_BLEND_INDEX;
        else if (m_blendIndex > MAX_BLEND_INDEX)
            m_blendIndex = 0;

        m_blendableImage.material.SetFloat("_BlendMode", m_blendIndex);
    } 
}
