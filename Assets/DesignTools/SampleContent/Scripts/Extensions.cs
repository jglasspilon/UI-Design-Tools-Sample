using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class Extensions
{
    /// <summary>
    /// Animates a transfrom from one local position to another. 
    /// </summary>
    /// <param name="content">transform to move</param>
    /// <param name="targetPositiion">target position in local space</param>
    /// <param name="animCurve">animation curve to handle the interpolating curve</param>
    /// <param name="is2D"> can be set to 2D to ignore any change in the z axis</param>
    /// <returns></returns>
    public static IEnumerator MoveToLocal(this Transform content, Vector3 targetPositiion, AnimationCurve animCurve, bool is2D = true)
    {
        float timer = 0;
        float duration = animCurve.GetCurveDuration();
        Vector3 startPosition = content.localPosition;

        if(is2D)
        {
            targetPositiion = new Vector3(targetPositiion.x, targetPositiion.y, startPosition.z);
        }

        while (timer < duration)
        {
            timer += Time.deltaTime;
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPositiion, animCurve.Evaluate(timer));
            content.localPosition = newPosition;
            yield return null;
        }

        content.localPosition = targetPositiion;
    }

    /// <summary>
    /// Animates a transfrom from one world position to another. 
    /// </summary>
    /// <param name="content">transform to move</param>
    /// <param name="targetPositiion">target position in world space</param>
    /// <param name="animCurve">animation curve to handle the interpolating curve</param>
    /// <param name="is2D"> can be set to 2D to ignore any change in the z axis</param>
    /// <returns></returns>
    public static IEnumerator MoveTo(this Transform content, Vector3 targetPositiion, AnimationCurve animCurve, bool is2D = true)
    {
        float timer = 0;
        float duration = animCurve.GetCurveDuration();
        Vector3 startPosition = content.position;

        if (is2D)
        {
            targetPositiion = new Vector3(targetPositiion.x, targetPositiion.y, startPosition.z);
        }

        while (timer < duration)
        {
            timer += Time.deltaTime;
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPositiion, animCurve.Evaluate(timer));
            content.position = newPosition;
            yield return null;
        }

        content.position = targetPositiion;
    }

    /// <summary>
    /// Forces a full rebuild of all nested ContentSizeFitters and LayoutGroups,
    /// starting from the deepest children and bubbling upward.
    /// </summary>
    public static void ForceRebuildNested(this RectTransform root)
    {
        if (root == null)
            return;

        Canvas.ForceUpdateCanvases();
        RebuildRecursive(root);
        LayoutRebuilder.ForceRebuildLayoutImmediate(root);
        Canvas.ForceUpdateCanvases();

        void RebuildRecursive(RectTransform rt)
        {
            for (int i = 0; i < rt.childCount; i++)
            {
                var child = rt.GetChild(i) as RectTransform;
                if (child != null)
                    RebuildRecursive(child);
            }

            if (HasLayoutComponent(rt))
                LayoutRebuilder.ForceRebuildLayoutImmediate(rt);
        }

        bool HasLayoutComponent(RectTransform rt)
        {
            return rt.GetComponent<ContentSizeFitter>() ||
                   rt.GetComponent<HorizontalLayoutGroup>() ||
                   rt.GetComponent<VerticalLayoutGroup>() ||
                   rt.GetComponent<GridLayoutGroup>() ||
                   rt.GetComponent<LayoutElement>();
        }
    }  

    /// <summary>
    /// Return the length in seconds of the animation curve
    /// </summary>
    /// <param name="curve"></param>
    /// <returns></returns>
    public static float GetCurveDuration(this AnimationCurve curve)
    {
        if (curve == null || curve.keys.Length == 0)
            return 0f;

        return curve.keys[curve.keys.Length - 1].time;
    }

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
    /// Determines whether a string has value or not.
    /// </summary>
    /// <param name="str">String to test.</param>
    /// <returns>Returns true if there is value in the tested string.</returns>
    public static bool HasValue(this string str)
    {
        if (!string.IsNullOrEmpty(str) && !str.Contains("error") && str != "NaN")
            return true;
        else
            return false;
    }

    /// <summary>
    /// Determines if a string is null/empty.
    /// </summary>
    /// <param name="str">String to test.</param>
    /// <returns>Returns true if string to test is null or empty.</returns>
    public static bool IsNulOrEmpty(this string str)
    {
        return string.IsNullOrEmpty(str);
    }

    /// <summary>
    /// Determines if a string is null/empty/has an error.
    /// </summary>
    /// <param name="str">String to test.</param>
    /// <returns>Returns true if the string to test is null, empty or contains an error.</returns>
    public static bool IsNulEmptyOrError(this string str)
    {
        if (string.IsNullOrEmpty(str) || str.Contains("error") || str == "NaN")
            return true;
        else
            return false;
    }

    /// <summary>
    /// Determines if the string is a variation of true.
    /// </summary>
    /// <param name="str">String to test.</param>
    /// <returns>Returns true if the string to test is any variation of the word 'true'.</returns>
    public static bool IsTrue(this string str)
    {
        if (str.ToLower() == "true")
            return true;
        else
            return false;
    }

    /// <summary>
    /// Determines if the string is a variation of false.
    /// </summary>
    /// <param name="str">String to test.</param>
    /// <returns>Returns true of the string to test is any variation of the word 'false'.</returns>
    public static bool IsFalse(this string str)
    {
        if ((str.ToLower() == "false"))
            return true;
        else
            return false;
    }

    /// <summary>
    /// Determines if the gameobject exists and has the desired component.
    /// </summary>
    /// <typeparam name="T">Component to look for.</typeparam>
    /// <param name="obj">Object to test.</param>
    /// <returns>Returns true of the object to test has the component we are looking for.</returns>
    public static bool ExistsAndHasComponent<T>(this GameObject obj)
    {
        if (obj != null && obj.GetComponent(typeof(T)) != null)
            return true;
        else
            return false;
    }
}
