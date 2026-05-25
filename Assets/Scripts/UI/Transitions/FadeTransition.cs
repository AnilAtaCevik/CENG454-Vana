using System.Collections;
using UnityEngine;

// Strategy pattern: transition between screens using a fade out and fade in animation
public class FadeTransition : ITransitionStrategy
{
    private float _duration;

    public FadeTransition(float duration = 0.5f)
    {
        _duration = duration;
    }

    public IEnumerator Transition(CanvasGroup from, CanvasGroup to)
    {
        //fade out the current screen
        if (from != null)
        {
            float elapsed = 0f;
            while (elapsed < _duration)
            {
                elapsed += Time.deltaTime;
                from.alpha = Mathf.Clamp01(1f - (elapsed / _duration));
                yield return null;
            }
            from.alpha = 0f;
            from.gameObject.SetActive(false);
        }

        //fade in the next screen
        if (to != null)
        {
            to.gameObject.SetActive(true);
            to.alpha = 0f;
            float elapsed = 0f;
            while (elapsed < _duration)
            {
                elapsed += Time.deltaTime;
                to.alpha = Mathf.Clamp01(elapsed / _duration);
                yield return null;
            }
            to.alpha = 1f;
        }
    }
}