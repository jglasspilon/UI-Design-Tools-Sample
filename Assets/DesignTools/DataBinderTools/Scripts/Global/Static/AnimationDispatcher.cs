using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationDispatcher
{
    public static IEnumerator TriggerAnimation(Animator target, string param, float proportionOfWait = 0)
    {
        if (target == null)
            yield break;
        switch (param)
        {
            case "Show":
                if (target.GetCurrentAnimatorStateInfo(0).IsName("Hidden"))
                {
                    target.SetTrigger(param);
                    yield return null;
                    yield return new WaitForSeconds(target.GetCurrentAnimatorStateInfo(0).length * proportionOfWait);
                }
                break;

            case "Hide":
                if (target.GetCurrentAnimatorStateInfo(0).IsName("Visible") || target.GetCurrentAnimatorStateInfo(0).IsName("Update"))
                {
                    target.SetTrigger(param);
                    yield return null;
                    yield return new WaitForSeconds(target.GetCurrentAnimatorStateInfo(0).length * proportionOfWait);
                }
                break;

            case "Update":
                if (target.GetCurrentAnimatorStateInfo(0).IsName("Visible"))
                {
                    target.SetTrigger(param);
                    yield return null;
                    yield return new WaitForSeconds(target.GetCurrentAnimatorStateInfo(0).length * proportionOfWait);
                }
                break;

            default:
                target.SetTrigger(param);
                yield return null;
                yield return new WaitForSeconds(target.GetCurrentAnimatorStateInfo(0).length * proportionOfWait);
                break;
        }
    }
}
