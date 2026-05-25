using System.Collections;
// Strategy pattern: defines the contract for all screen transition types
public interface ITransitionStrategy
{
    IEnumerator Transition(UnityEngine.CanvasGroup from, UnityEngine.CanvasGroup to);
}