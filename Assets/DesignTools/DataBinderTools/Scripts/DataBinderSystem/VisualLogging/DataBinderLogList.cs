using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DataBinderLogList : PoolableObject
{
    [SerializeField]
    private RectTransform m_listHolder;

    [SerializeField]
    private Vector3 m_offset;

    private List<DataBinderLog> m_logs = new List<DataBinderLog>();

    private void OnEnable()
    {
        MoveToMouse();
    }

    public void Update()
    {
        MoveToMouse();
    }

    public void BuildLogList(BinderComponent[] componentsToLog)
    {
        foreach (BinderComponent component in componentsToLog)
        {
            foreach(IDataBindable binder in component.GetAllBinders())
            {
                foreach(KeyValuePair<string, JSONNode> data in binder.BoundData)
                {
                    DataBinderLog newLog = AssetPoolManager.Instance.PullFrom<DataBinderLog>(m_listHolder);
                    newLog.LogBinderData(data.Key, data.Value);
                    m_logs.Add(newLog);
                }
            }
        }

        StartCoroutine(ForceRebuildLayout());
    }

    public void ClearLogList()
    {
        if (m_logs.Count == 0)
            return;

        AssetPoolManager.Instance.ReturnToPool(m_logs);
        m_logs.Clear();
    }

    public override void ResetForPool()
    {
        ClearLogList();
        m_listHolder.position = Vector3.zero;
    }

    private void MoveToMouse()
    {
        Vector3 mousePositionInScene = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePositionInScene.z = 0;
        m_listHolder.position = mousePositionInScene + m_offset;
    }
    
    private IEnumerator ForceRebuildLayout()
    {
        yield return null;
        (transform as RectTransform).ForceRebuildNested();
    }
}
