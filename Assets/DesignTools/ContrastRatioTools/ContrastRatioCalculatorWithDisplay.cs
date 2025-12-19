using System.Collections;
using TMPro;
using UnityEngine;

public class ContrastRatioCalculatorWithDisplay : ContrastRatioCalculator
{
    [SerializeField]
    [Header("Display Components:")]
    private TextMeshProUGUI m_lightValue1;
    
    [SerializeField] 
    private TextMeshProUGUI m_lightValue2, m_lightRresult, m_darkValue1, m_darkValue2, m_darkRresult;

    [SerializeField]
    private GameObject m_lightSelectedObject, m_darkSelectedObject;

    [SerializeField]
    private RectTransform m_lightLayout, m_darkLayout;

    protected override void OnEnable()
    {
        base.OnEnable();
        DisplayValues();
    }

    public override void Calculate()
    {
        base.Calculate();
        DisplayValues();
    }

    private void DisplayValues()
    {
        m_lightValue1.text = $"L<sub>1</sub> = {m_lightLuminanceValues.x:0.00}";
        m_lightValue2.text = $"L<sub>2</sub> = {m_lightLuminanceValues.y:0.00}";
        m_lightRresult.text = $"r = {m_lightContrastRatio:0.0}";
        m_lightSelectedObject.SetActive(m_lightContrastRatio > m_darkContrastRatio);

        m_darkValue1.text = $"L<sub>1</sub> = {m_darkLuminanceValues.x:0.00}";
        m_darkValue2.text = $"L<sub>2</sub> = {m_darkLuminanceValues.y:0.00}";
        m_darkRresult.text = $"r = {m_darkContrastRatio:0.0}";
        m_darkSelectedObject.SetActive(m_darkContrastRatio > m_lightContrastRatio);

        StartCoroutine(ForceRebuildLayouts());
    }

    private IEnumerator ForceRebuildLayouts()
    {
        yield return null;
        m_lightLayout.ForceRebuildNested();
        m_darkLayout.ForceRebuildNested();
    }
}
