using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[CreateAssetMenu]
public class StyleSheet : ScriptableObject
{
    [SerializeField]
    private Color m_clientColor;

    [SerializeField]
    private SerializedKeyValuePair<ClientColorValue, Color>[] m_clientColorModifiers;

    [SerializeField]
    private SerializedKeyValuePair<string, Color>[] m_contentColors;

    [SerializeField]
    private SerializedKeyValuePair<InterfaceColorValue, Color>[] m_interfaceColors;

    [SerializeField]
    private FontSetting[] m_fontSettings;

    public static readonly string DEFAULT_STYLESHEET_PATH = "StyleSheet";

    public Color ClientColor { get { return m_clientColor; } }
    public Dictionary<ClientColorValue, Color> ColorModifiers { get { return m_clientColorModifiers.ToDictionary(x => x.Key, x => x.Value); } }
    public Dictionary<string, Color> ContentColors { get { return m_contentColors.ToDictionary(x =>  x.Key, x => x.Value); } }
    public Dictionary<string, Color> InterfaceColors { get { return m_interfaceColors.ToDictionary(x => x.Key.ToString(), x => x.Value); } }
    public Dictionary<string, FontSetting> FontSettings { get { return m_fontSettings.ToDictionary(x => x.Name); } }

    public void UpdateClientColor(Color clientColor)
    {
        m_clientColor = clientColor;
    }

    public void UpdateContentColor(string colorKey, Color newColor)
    {
        if (m_contentColors.Where(x => x.Key == colorKey).Count() > 0)
            m_contentColors.First(x => x.Key == colorKey).Value = newColor;
        else 
        {
            SerializedKeyValuePair<string, Color>[] temp = new SerializedKeyValuePair<string, Color>[m_contentColors.Count() + 1];

            for(int i = 0; i < m_contentColors.Length; i++)
            {
                temp[i] = m_contentColors[i];
            }

            temp[temp.Length - 1] = new SerializedKeyValuePair<string, Color> { Key = colorKey, Value = newColor };
            m_contentColors = temp;
        }
    }
}

[Serializable]
public class FontSetting
{
    public string Name;
    public TMP_FontAsset FontAsset;
    public int FontSize;
    public int LineHeight;
}

public enum ClientColorValue
{
    Client,
    Client95,
    Client90,
    Client75,
    Client60,
    Client40,
    Client20,
    Client10
}

public enum InterfaceColorValue
{
    Interface100,
    Interface95,
    Interface90,
    Interface85,
    Interface80,
    Interface75,
    Interface70,
    Interface65,
    Interface60,
    Interface55,
    Interface50, 
    Interface45,
    Interface40,
    Interface35,
    Interface30,
    Interface25,
    Interface20, 
    Interface10,
    Interface0
}
