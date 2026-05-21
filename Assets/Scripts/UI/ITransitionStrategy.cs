using System.Collections;

public interface ITransitionStrategy
{
    IEnumerator Transition(UnityEngine.CanvasGroup from, UnityEngine.CanvasGroup to);
}