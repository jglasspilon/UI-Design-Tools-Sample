using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class AutoScroller : MonoBehaviour
{
    [SerializeField]
    private float m_speed;

    private ScrollRect m_scroll;

    private void Awake()
    {
        m_scroll = GetComponent<ScrollRect>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = m_scroll.content.anchoredPosition;
        pos.x += m_speed * Time.deltaTime; 

        m_scroll.content.anchoredPosition = pos;
    }
}
