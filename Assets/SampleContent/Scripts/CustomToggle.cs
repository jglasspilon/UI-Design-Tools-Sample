using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CustomToggle : MonoBehaviour
{
    [SerializeField]
    private GameObject m_activeGraphic;

    [SerializeField]
    private CustomToggleGroup m_toggleGroup;

    [SerializeField]
    private bool m_isOn;

    private void OnEnable()
    {
        if (m_toggleGroup != null)
            m_toggleGroup.SubscribeToggle(this);

        SetGraphic(m_isOn);
    }

    private void OnDisable()
    {
        if (m_toggleGroup != null)
            m_toggleGroup.UnsubscribeToggle(this);
    }

    public void Toggle()
    {
        if (m_isOn)
            TurnOff();
        else
            TurnOn();
    }

    public void TurnOn()
    {
        if (m_toggleGroup != null)
            m_toggleGroup.DispatchToggleEventToGroup(true, this);

        SetGraphic(true);       
    }

    public void TurnOff()
    {
        if (m_toggleGroup != null && !m_toggleGroup.CanTurnOffToggles)
            return;

        if (m_toggleGroup != null)
            m_toggleGroup.DispatchToggleEventToGroup(false, this);

        SetGraphic(false);       
    }

    public void SetGraphic(bool isOn)
    {
        m_isOn = isOn;
        m_activeGraphic.SetActive(isOn);
    }
}
