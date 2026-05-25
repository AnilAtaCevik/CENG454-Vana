using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class TitleScreen : MonoBehaviour, IScreen
{
    public string ScreenName => "TitleScreen";
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 1.5f;
    [SerializeField] private CanvasGroup pressAnyKeyGroup;
    private bool _canProceed = false;
    private Coroutine _blinkCoroutine;

    private void Start()
    {
        //using instant transition on startup
        canvasGroup.alpha = 1f;
        UIManager.Instance.SetTransitionStrategy(new InstantTransition());
        UIManager.Instance.ShowScreen(this);
        UIManager.Instance.SetTransitionStrategy(new FadeTransition(0.5f));

    }
    private void Update() {
        if (!_canProceed) return;

        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            _canProceed = false;
            if (_blinkCoroutine != null) StopCoroutine(_blinkCoroutine);
            StartCoroutine(FadeOutAndProceed());
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
        canvasGroup.alpha = 1f;
        _canProceed = true;
        _blinkCoroutine = StartCoroutine(BlinkText());
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        _canProceed = false;
        if (_blinkCoroutine != null) StopCoroutine(_blinkCoroutine);
    }

    //fades out title screen than transitions to main menu
    private IEnumerator FadeOutAndProceed()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(1f - (elapsed / fadeDuration));
            yield return null;
        }

        canvasGroup.alpha = 0f;
        UIManager.Instance.SetTransitionStrategy(new InstantTransition());
        UIManager.Instance.ShowScreen(
            Object.FindAnyObjectByType<MainMenuScreen>()
        );
        UIManager.Instance.SetTransitionStrategy(new FadeTransition(0.5f));
    }

    //blinking text for attention
    private IEnumerator BlinkText()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            pressAnyKeyGroup.alpha = pressAnyKeyGroup.alpha > 0.5f ? 0.2f : 1f;
        }
    }
}