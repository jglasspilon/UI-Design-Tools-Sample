using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MaskableGraphic))]
public class ColorSelector : ContrastCalculateInvoker
{
    private Image m_targetImage;

    public void SetColorFromSwab(Image source)
    {
        if(m_targetImage == null)
            m_targetImage = GetComponent<Image>();

        m_targetImage.color = source.color;
        RecaculateContrastRatios();
    }
}
