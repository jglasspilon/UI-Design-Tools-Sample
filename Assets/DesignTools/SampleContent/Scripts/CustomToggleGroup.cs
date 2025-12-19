using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class CustomToggleGroup : MonoBehaviour
{
    [SerializeField]
    private bool m_canTurnOffToggles;

    private List<CustomToggle> m_toggles = new List<CustomToggle>();

    public bool CanTurnOffToggles { get { return  m_canTurnOffToggles; } } 

    public void SubscribeToggle(CustomToggle toggle)
    {
        if (m_toggles.Contains(toggle))
            return;

        m_toggles.Add(toggle);
    }

    public void UnsubscribeToggle(CustomToggle toggle)
    {
        if (!m_toggles.Contains(toggle))
            return;

        m_toggles.Remove(toggle);
    }

    public void DispatchToggleEventToGroup(bool toggleState, CustomToggle triggerer)
    {
        if (!toggleState || m_toggles.Count < 2)
            return;

        foreach(CustomToggle toggle in m_toggles.Where(x => x != triggerer))
        {
            toggle.SetGraphic(false);
        }
    }
}
