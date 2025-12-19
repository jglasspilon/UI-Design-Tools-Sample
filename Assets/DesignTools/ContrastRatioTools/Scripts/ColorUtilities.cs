using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ColorUtilities
{
    /// <summary>
    /// Takes a hexidecimal string and returns the color value
    /// </summary>
    /// <param name="str">Hex string to parse.</param>
    /// <returns>Returns the color value of a hex if string is relevant, otherwise returns white.</returns>
    public static Color ParseColor(this string str)
    {
        if (str != null)
        {
            str = str.Replace("\"", "");

            string hexString = str;
            if (!hexString.Contains("#"))
                hexString = "#" + str;

            Color newColor;

            ColorUtility.TryParseHtmlString(hexString, out newColor);
            return newColor;
        }
        else
            return Color.white;
    }

    /// <summary>
    /// Returns the color based on the string arrow value. Supports -1, 0, 1.
    /// </summary>
    /// <param name="color"></param>
    /// <param name="arrowValue"></param>
    /// <param name="isDark"></param>
    /// <returns></returns>
    public static Color SetColorByArrow(this Color color, string arrowValue, bool isDark = false)
    {
        Color32 red = new Color32(244, 36, 35, 255);
        Color32 white = new Color32(255, 255, 255, 255);
        Color32 green = new Color32(5, 178, 115, 255);

        if (arrowValue == "1")
        {
            color = green;
        }
        if (arrowValue == "0")
        {
            if (isDark)
                color = Color.black;
            else
                color = white;
        }
        if (arrowValue == "-1")
        {
            color = red;
        }

        return color;
    }

    public static float GetContrastRatio(Color color1, Color color2, out Vector2 luminance)
    {
        float lum1 = GetRelativeLuminance(color1);
        float lum2 = GetRelativeLuminance(color2);

        float lighter = Mathf.Max(lum1, lum2);
        float darker = Mathf.Min(lum1, lum2);
        luminance = new Vector2(lum1, lum2);

        return (lighter + 0.05f) / (darker + 0.05f);
    }

    private static float Linearize(float channel)
    {
        if(channel > 1)
            channel /= 255.0f; // Normalize to [0,1]

        return (channel <= 0.03928f) ? channel / 12.92f : Mathf.Pow((channel + 0.055f) / 1.055f, 2.4f);
    }

    private  static float GetRelativeLuminance(Color color)
    {
        float r = Linearize(color.r);
        float g = Linearize(color.g);
        float b = Linearize(color.b);

        return 0.2126f * r + 0.7152f * g + 0.0722f * b;
    }

    public static Color Add(this Color color, Color addedColor)
    {
        return new Color(
                (color.r * (1f - addedColor.a) + addedColor.r * addedColor.a),
                (color.g * (1f - addedColor.a) + addedColor.g * addedColor.a),
                (color.b * (1f - addedColor.a) + addedColor.b * addedColor.a),
                Mathf.Clamp01(color.a + addedColor.a)
                );
    }

    public static Color LerpColor(this Color color, Color colorAtZero, float correctionFactor)
    {
        return new Color(Mathf.Lerp(colorAtZero.r, color.r, correctionFactor), Mathf.Lerp(colorAtZero.g, color.g, correctionFactor), Mathf.Lerp(colorAtZero.b, color.b, correctionFactor), color.a);
    }

    public static void SetOpacity(this MaskableGraphic[] graphics, float targetOpacity)
    {
        foreach (MaskableGraphic graphic in graphics)
        {
            Color opacity = graphic.color;
            opacity.a = targetOpacity;
            graphic.color = opacity;
        }
    }

    public static IEnumerator AnimateOpacity(this MaskableGraphic[] graphics, float targetOpacity, float duration = 0.5f)
    {
        if (graphics.Length == 0 || graphics[0].color.a == targetOpacity)
            yield break;

        float time = 0;
        float currentOpacity = graphics[0].color.a;
        bool up = currentOpacity < targetOpacity;
        while (time < 1)
        {
            graphics.SetOpacity(Mathf.Lerp(currentOpacity, targetOpacity, time));
            time += Time.deltaTime / duration;
            yield return null;
        }

        graphics.SetOpacity(targetOpacity);
    }
}
