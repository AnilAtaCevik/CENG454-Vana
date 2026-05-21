using System.Collections;
using UnityEngine;

public class FadeTransition : ITransitionStrategy
{
    private float _duration;

    public FadeTransition(float duration = 0.5f)
    {
        _duration = duration;
    }

    public IEnumerator Transition(CanvasGroup from, CanvasGroup to)
    {
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