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
    private ContrastCalculateInvoker[] m_invokers;

    [SerializeField]
    [Range(1f, 20f)]
    private float m_acceptableThreshold = 4.5f;

    [SerializeField]
    private bool m_inverse, m_debug;  

    private MaskableGraphic m_objectToSetColor;
    protected float m_lightContrastRatio, m_darkContrastRatio;
    protected Vector2 m_lightLuminanceValues, m_darkLuminanceValues;

    private void Awake()
    {
        m_objectToSetColor = GetComponent<MaskableGraphic>();   
    }

    protected virtual void OnEnable()
    {
        if(m_invokers != null)
        {
            foreach(ContrastCalculateInvoker invoker in m_invokers)
            {
                invoker.OnCalculate += Calculate;
            }
        }
        Calculate();
    }

    private void OnDisable()
    {
        if (m_invokers != null)
        {
            foreach (ContrastCalculateInvoker invoker in m_invokers)
            {
                invoker.OnCalculate -= Calculate;
            }
        }
    }

    public virtual void Calculate()
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
        m_lightContrastRatio = ColorUtilities.GetContrastRatio(test, m_lightColor, out m_lightLuminanceValues);
        m_darkContrastRatio = ColorUtilities.GetContrastRatio(test, m_darkColor, out m_darkLuminanceValues);

        if(m_inverse)
            SetColor(m_lightContrastRatio < m_acceptableThreshold ? m_lightContrastRatio > m_darkContrastRatio ? m_darkColor : m_lightColor : m_darkColor);
        else
            SetColor(m_lightContrastRatio < m_acceptableThreshold ? m_lightContrastRatio > m_darkContrastRatio ? m_lightColor : m_darkColor : m_lightColor);

        if (m_debug)
            Debug.Log($"Dark = {m_darkContrastRatio} - Light = {m_lightContrastRatio}");

        if (m_lightContrastRatio < m_acceptableThreshold && m_darkContrastRatio < m_acceptableThreshold)
            Debug.Log($"Both color options for {name} fail the accesibility test. Light color scored {m_lightContrastRatio}. Dark color scored {m_darkContrastRatio}. Consider changing the colors.");
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
