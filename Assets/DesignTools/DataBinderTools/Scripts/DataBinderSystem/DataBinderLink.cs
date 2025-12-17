using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Points each connected component to the selected node structure within the json file
/// </summary>
public class DataBinderLink: MonoBehaviour
{
    [SerializeField]
    private string[] m_nodeStructureSteps = new string[1];

    [SerializeField]
    private BinderComponent[] m_connectedBinderComponents = new BinderComponent[1];

    public string[] NodeStructureSteps { get { return m_nodeStructureSteps; } }
    public BinderComponent[] ConnectedBinderComponents { get { return m_connectedBinderComponents; } set { m_connectedBinderComponents = value; } }   
}
