using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DataBinderHoverDetector : MonoBehaviour
{
    [SerializeField]
    private BinderComponent[] m_bindersToLog;

    [SerializeField]
    private MaskableGraphic m_displayElement;

    private RectTransform m_rectTransform;
    private bool m_isInside;
    private Transform m_logHolder;
    private Color m_displayElementColor;
    private DataBinderLogList m_logList;

    private void Awake()
    {
        if (m_displayElement == null)
            return;

        m_rectTransform = transform as RectTransform;
        m_displayElementColor = m_displayElement.color;
        m_displayElementColor.a = 0;
        m_displayElement.color = m_displayElementColor;

        GameObject logHolderObject = GameObject.FindGameObjectWithTag("LogHolder");

        if(logHolderObject == null)
        {
            Debug.Log("Log holder not found. Make sure a game object in the scene has the tag \"LogHolder\".");
            return;
        }

        m_logHolder = logHolderObject.transform;
    }

    private void Update()
    {
        bool nowInside = RectTransformUtility.RectangleContainsScreenPoint(m_rectTransform, Input.mousePosition, Camera.main);

        if (nowInside && !m_isInside)
        {
            m_isInside = true;
            LoadAndDisplayLogs();
        }
        else if (!nowInside && m_isInside)
        {
            m_isInside = false;
            ClearLogs();
        }
    }

    private void OnDisable()
    {
        ClearLogs();
    }

    public void LoadAndDisplayLogs()
    {
        if (m_logList == null)
            m_logList = AssetPoolManager.Instance.PullFrom<DataBinderLogList>(m_logHolder);

        m_logList.BuildLogList(m_bindersToLog);

        if (m_displayElement == null)
            return;

        m_displayElementColor.a = 1f;
        m_displayElement.color = m_displayElementColor;
    }

    public void ClearLogs()
    {
        if(m_logList != null)
        {
            AssetPoolManager.Instance.ReturnToPool(m_logList);
            m_logList = null;
        }

        if (m_displayElement == null)
            return;

        m_displayElementColor.a = 0f;
        m_displayElement.color = m_displayElementColor;
    }
}
