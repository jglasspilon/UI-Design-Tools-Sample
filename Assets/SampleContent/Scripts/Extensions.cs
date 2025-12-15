using UnityEngine;
using System.Collections;

public static class Extensions
{
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

    public static float GetCurveDuration(this AnimationCurve curve)
    {
        if (curve == null || curve.keys.Length == 0)
            return 0f;

        return curve.keys[curve.keys.Length - 1].time;
    }
}
