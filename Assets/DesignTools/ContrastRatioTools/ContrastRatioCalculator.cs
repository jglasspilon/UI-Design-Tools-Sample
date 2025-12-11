using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(MaskableGraphic))]
public class ContrastRatioCalculator : MonoBehaviour
{
    [SerializeField]
    private MaskableGraphic m_backgroundObject;

    [SerializeField]
    private Color m_lightColor = new Color(0.9215f, 0.9294f, 0.9411f);

    [SerializeField]
    private Color m_darkColor = new Color(0.1137f, 0.1176f, 0.1254f);

    [SerializeField]
    [Range(1f, 20f)]
    private float m_acceptableThreshold = 4.5f;

    [SerializeField]
    private bool m_inverse, m_debug;

    [Header("Events Which Trigger Calculate")][SerializeField]
    private UnityEvent m_CalculateOn;

    private MaskableGraphic m_objectToSetColor;

    private void Awake()
    {
        m_objectToSetColor = GetComponent<MaskableGraphic>();   
    }

    private void OnEnable()
    {
        if (m_lightColor != null)
            m_CalculateOn.AddListener(Calculate);

        Calculate();
    }

    private void OnDisable()
    {
        if (m_lightColor != null)
            m_CalculateOn.RemoveListener(Calculate);
    }

    public void Calculate()
    {
        if(m_backgroundObject == null)
        {
            Debug.LogError($"Failed to calculate contrast ratio for {name}. No background object set.");
            return;
        }

        CalculateAndSetFromColor(m_backgroundObject.color);
    }

    public void CheckContrastRatioAndSetColor(Color color)
    {
        if(color == null)
        {
            Debug.LogError($"Failed to select color from contrast ratio because the color to test with was null.");
            return;
        }

        CalculateAndSetFromColor(color);
    }

    private void CalculateAndSetFromColor(Color test)
    {
        float lightContrastRatio = ColorUtilities.GetContrastRatio(test, m_lightColor);
        float darkContrastRatio = ColorUtilities.GetContrastRatio(test, m_darkColor);

        if(m_inverse)
            SetColor(lightContrastRatio < m_acceptableThreshold ? lightContrastRatio > darkContrastRatio ? m_darkColor : m_lightColor : m_darkColor);
        else
            SetColor(lightContrastRatio < m_acceptableThreshold ? lightContrastRatio > darkContrastRatio ? m_lightColor : m_darkColor : m_lightColor);

        if (m_debug)
            Debug.Log($"Dark = {darkContrastRatio} - Light = {lightContrastRatio}");

        if (lightContrastRatio < m_acceptableThreshold && darkContrastRatio < m_acceptableThreshold)
            Debug.Log($"Both color options for {name} fail the accesibility test. Light color scored {lightContrastRatio}. Dark color scored {darkContrastRatio}. Consider changing the colors.");
    }

    private void SetColor(Color color)
    {
        if (m_objectToSetColor == null)
        {
            m_objectToSetColor = GetComponent<MaskableGraphic>();

            if (m_objectToSetColor == null)
            {
                Debug.LogError($"Failed to set color for {name}. No object to set color on.");
                return;
            }
        }

        color.a = m_objectToSetColor.color.a;
        m_objectToSetColor.color = color;
    }
}
