using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class StyledGraphics : MonoBehaviour
{
    [Dropdown("m_colorSectionOptions")]
    public string m_colorSection;
    [Dropdown("GetStyleSheetColors"), OnValueChanged("ApplyColorInEditor")]
    public string m_color;

    [SerializeField, OnValueChanged("ApplyColorInEditor")]
    private MaskableGraphic[] m_additionalComponentsToColor;

    [Foldout("Preview"), ReadOnly, Label("Color")]
    public Color m_colorPreview;
    [Foldout("Preview"), ReadOnly, Label("Value")]
    public string m_colorValue;

    [ShowIf("m_reloadStylesheet")]

    private StyleSheet m_styleSheet;
    private string[] m_colorSectionOptions = new string[] {"Content Colors", "Client Colors", "Interface Colors"};
    private string m_pickAColor = "-- pick a color --";
    
    private bool LoadStylesheet()
    {
        string stylePath = StyleSheet.DEFAULT_STYLESHEET_PATH;
        Object styleObject = Resources.Load(stylePath);

        if (styleObject == null)
        {
            Debug.LogError($"Failed to find Style Sheet {stylePath}");
            return false;
        }

        if (styleObject is not StyleSheet)
        {
            Debug.LogError($"This object is not a StyleSheet {stylePath}");
            return false;
        }

        m_styleSheet = styleObject as StyleSheet;
        return true;
    }

    private string[] GetStyleSheetColors()
    {
        
        if (m_styleSheet == null)
            if (!LoadStylesheet()) return new string[] { "" };

        if(m_colorSection == null)
            return new string[] {""};

        List<string> colorList = new List<string>();
        colorList.Add(m_pickAColor);

        switch (m_colorSection)
        { 
            case "Content Colors":
                colorList.AddRange(m_styleSheet.ContentColors.Keys.ToArray());
                break;
            case "Client Colors":
                colorList.Add("Client");
                colorList.AddRange(m_styleSheet.ColorModifiers.Keys.Select(x => x.ToString()));
                break;
            case "Interface Colors":
                colorList.AddRange(m_styleSheet.InterfaceColors.Keys.ToArray());
                break;
            default:
                Debug.LogError($"Unknown color section: {m_colorSection}");
                break;
        }

        return colorList.ToArray();
    }

    public void ApplyColorInEditor()
    {
        Color color = Color.white;
        if (m_colorSection == null) return;
        if (m_color == null || m_color == "" || m_color == m_pickAColor) return;

        if (m_styleSheet == null)
            LoadStylesheet();

        switch (m_colorSection)
        {
            case "Content Colors":
                if (!m_styleSheet.ContentColors.TryGetValue(m_color, out color))
                {
                    Debug.LogError($"Color not found in the Colors section: {m_color}");
                    return;
                }
                break;

            case "Client Colors":
                if (!System.Enum.TryParse(m_color, out ClientColorValue clientColor))
                {
                    Debug.LogError($"Color not found in the Client Colors section: {m_color}");
                    return;
                }
                if (clientColor == ClientColorValue.Client)
                    color = m_styleSheet.ClientColor;
                else if (m_styleSheet.ColorModifiers.TryGetValue(clientColor, out Color colorModifier))
                    color = m_styleSheet.ClientColor.Add(colorModifier);
                else
                {
                    Debug.LogError($"No color modifier found for Client Color Value {m_color}");
                    return;
                }
                break;

            case "Interface Colors":
                if (!m_styleSheet.InterfaceColors.TryGetValue(m_color, out color))
                {
                    Debug.LogError($"Color not found in the Interface Colors section: {m_color}");
                    return;
                }
                break;
            default:
                return;
        }

        m_colorPreview = color;
        m_colorValue = $"#{color.ToHexString().Substring(0,6)} {color.ToHexString().Substring(6, 2)}";

        MaskableGraphic graphic = GetComponent<MaskableGraphic>();
        if (graphic != null)
        {
            color.a = graphic.color.a;
            graphic.color = color;
        }

        if (m_additionalComponentsToColor != null && m_additionalComponentsToColor.Length > 0)
        {
            foreach (MaskableGraphic graphicFromList in m_additionalComponentsToColor)
            {
                if (graphicFromList != null)
                {
                    color.a = graphicFromList.color.a;
                    graphicFromList.color = color;
                }
            }
        }
    }

    void OnEnable()
    {
        LoadStylesheet();
        ApplyColorInEditor();
    }
}
