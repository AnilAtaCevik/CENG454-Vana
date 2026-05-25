using System.Collections;
using UnityEngine;

// Strategy pattern: switching screens with no animation
public class InstantTransition : ITransitionStrategy
{
    public IEnumerator Transition(CanvasGroup from, CanvasGroup to)
    {
        if (from != null)
        {
            from.alpha = 0f;
            from.gameObject.SetActive(false);
        }

        if (to != null)
        {
            to.gameObject.SetActive(true);
            to.alpha = 1f;
        }

        yield return null;
    }
}