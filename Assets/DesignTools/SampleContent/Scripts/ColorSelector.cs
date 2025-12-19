using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

[RequireComponent(typeof(MaskableGraphic))]
public class ColorSelector : ContrastCalculateInvoker
{
    [SerializeField]
    [Tooltip("Specify the path to the desired style sheet, relative to the resources folder.")]
    private string m_pathToStyleSheet = "StyleSheet";

    private MaskableGraphic m_targetImage;
    private StyleSheet m_styleSheet;
    private int m_colorIndex = 0;
    private Color[] m_contentColors = new Color[0];

    private void Awake()
    {
        m_styleSheet = Resources.Load<StyleSheet>(m_pathToStyleSheet);

        if (m_styleSheet == null)
        {
            Debug.LogError($"Failed to load style sheet from Resoures/{m_pathToStyleSheet}");
            return;
        }

        m_contentColors = m_styleSheet.ContentColors.Select(x => x.Value).ToArray();
    }

    public void CycleColor(int increment)
    {
        int max = m_contentColors.Length - 1;
        m_colorIndex += increment;

        if (m_colorIndex < 0)
            m_colorIndex = max;
        else if (m_colorIndex > max)
            m_colorIndex = 0;

        UpdateColorFromIndex(m_colorIndex);
    }

    private void UpdateColorFromIndex(int colorIndex)
    {
        if (m_targetImage == null)
            m_targetImage = GetComponent<MaskableGraphic>();

        m_targetImage.color = m_contentColors[colorIndex];
        RecaculateContrastRatios();
    }
}
