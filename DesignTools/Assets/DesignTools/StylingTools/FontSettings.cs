using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public class FontSettings : MonoBehaviour
{
    [SerializeField]
    [Dropdown("GetStylesheetFonts"), OnValueChanged("ApplyFontSettingsInEditor")]
    private string m_fontSettingSelection;

    private StyleSheet m_styleSheet;
    private const string DEFAULT_TEXT = "-- pick a font --";

    private void Awake()
    {
        LoadStylesheet();
        ApplyFontSettingsInEditor();
    }

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

    private string[] GetStylesheetFonts()
    {
        if (m_styleSheet == null)
            if (!LoadStylesheet()) return new string[] { "" };

        List<string> fontList = new List<string>();
        fontList.Add(DEFAULT_TEXT);
        fontList.AddRange(m_styleSheet.FontSettings.Keys.ToArray());

        return fontList.ToArray();
    }

    private void ApplyFontSettingsInEditor()
    {
        if (m_fontSettingSelection == DEFAULT_TEXT)
            return;

        if (!m_styleSheet.FontSettings.ContainsKey(m_fontSettingSelection))
        {
            Debug.LogError($"Failed to set the font settings to {m_fontSettingSelection}. No such option found in the style sheet.");
            return;
        }
       
        FontSetting fontSetting = m_styleSheet.FontSettings[m_fontSettingSelection];
        ApplyFontSetting(fontSetting);
    }

    private void ApplyFontSetting(FontSetting fontSetting)
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        text.font = fontSetting.FontAsset;
        text.fontSize = fontSetting.FontSize;
        text.lineSpacing = fontSetting.LineHeight;
    }
}
