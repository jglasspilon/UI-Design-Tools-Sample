using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// A custom ContentSizeFitter with min/max width and height clamping.
/// </summary>
[RequireComponent(typeof(RectTransform))]
[ExecuteAlways]
public class ClampedContentSizeFitter : UIBehaviour, ILayoutSelfController
{
    public enum FitMode { Unconstrained, MinSize, PreferredSize }

    [SerializeField] 
    private FitMode m_horizontalFit = FitMode.Unconstrained;
    
    [SerializeField] 
    private FitMode m_verticalFit = FitMode.Unconstrained;

    [Header("Width Clamp")]
    [SerializeField]
    private float m_minWidth = 0f;

    [SerializeField]
    private float m_maxWidth = float.PositiveInfinity;

    [Header("Height Clamp")]
    [SerializeField]
    private float m_minHeight = 0f;

    [SerializeField]
    private float m_maxHeight = float.PositiveInfinity;

    private RectTransform m_rect;

    protected override void Awake()
    {
        m_rect = GetComponent<RectTransform>();
    }

    protected override void OnEnable()
    {
        SetDirty();
    }

    protected override void OnDisable()
    {
        LayoutRebuilder.MarkLayoutForRebuild(m_rect);
    }

    protected override void OnRectTransformDimensionsChange()
    {
        SetDirty();
    }

    public void SetLayoutHorizontal()
    {
        HandleSelfFitting(true);
    }

    public void SetLayoutVertical()
    {
        HandleSelfFitting(false);
    }

    private void HandleSelfFitting(bool isHorizontal)
    {
        FitMode fit = isHorizontal ? m_horizontalFit : m_verticalFit;
        if (fit == FitMode.Unconstrained) return;

        float size = (fit == FitMode.MinSize)
            ? LayoutUtility.GetMinSize(m_rect, isHorizontal ? 0 : 1)
            : LayoutUtility.GetPreferredSize(m_rect, isHorizontal ? 0 : 1);

        if (isHorizontal)
            size = Mathf.Clamp(size, m_minWidth, m_maxWidth);
        else
            size = Mathf.Clamp(size, m_minHeight, m_maxHeight);

        m_rect.SetSizeWithCurrentAnchors(isHorizontal ? RectTransform.Axis.Horizontal : RectTransform.Axis.Vertical, size);
    }

    protected void SetDirty()
    {
        if (!IsActive()) 
            return;

        LayoutRebuilder.MarkLayoutForRebuild(m_rect);
    }
}