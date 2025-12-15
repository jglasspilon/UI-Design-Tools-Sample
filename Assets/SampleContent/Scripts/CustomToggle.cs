using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

[RequireComponent(typeof(Button))]
public class CustomToggle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    [Header("Button Transition Based Elements")]
    private GameObject m_activeGraphic;

    [SerializeField]
    [Header("Animation Transition Based Elements")]
    private Animator m_transitionAnim;

    [SerializeField]
    private string m_normalTrigger = "Normal", m_highlightTrigger = "Highlighted", m_pressedTrigger = "Pressed", m_selectedTrigger = "Selected", m_disabledTrigger = "Disabled";

    [SerializeField]
    [Header("Toggle Elements")]
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (m_transitionAnim == null || m_isOn)
            return;

        m_transitionAnim.SetTrigger(m_highlightTrigger);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (m_transitionAnim == null || m_isOn)
            return;

        m_transitionAnim.SetTrigger(m_normalTrigger);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (m_transitionAnim == null || (m_isOn && !m_toggleGroup.CanTurnOffToggles))
            return;

        if (m_toggleGroup != null)
            m_toggleGroup.DispatchToggleEventToGroup(true, this);

        m_transitionAnim.SetTrigger(m_pressedTrigger);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (m_transitionAnim == null || (m_isOn && !m_toggleGroup.CanTurnOffToggles))
            return;

        Toggle();
        string stateTrigger = m_isOn ? m_selectedTrigger : m_normalTrigger;
        m_transitionAnim.SetTrigger(stateTrigger);
    }

    public void Toggle()
    {
        if (m_isOn && m_toggleGroup != null && !m_toggleGroup.CanTurnOffToggles)
            return;

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

        if(m_activeGraphic != null)
            m_activeGraphic.SetActive(isOn);

        if (m_transitionAnim == null)
            return;

        string stateTrigger = isOn ? m_selectedTrigger : m_normalTrigger;
        m_transitionAnim.SetTrigger(stateTrigger);
    }
}
