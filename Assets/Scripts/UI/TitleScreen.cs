using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
//right noe title Screen and Main Manu is in the same Scene (MainMenuScene)
//Title Screen pens and when you press anythin on your keybooard it become hidden
//in the next step you are going to make a connection w MainMenu screen

public class TitleScreen : MonoBehaviour, IScreen
{
    public string ScreenName => "TitleScreen";
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 1.5f;
    private bool _canProceed = false;

    private void Start()
    {
        UIManager.Instance.ShowScreen(this);
    }
    private void Update() {
        if (!_canProceed) return;

        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            // SceneManager.LoadScene("MainMenuScene");
            _canProceed = false;
            StartCoroutine(FadeOutAndProceed());
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
        // _canProceed = true;
        StartCoroutine(FadeIn());
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        _canProceed = false;
    }
    private IEnumerator FadeIn()
    {
        canvasGroup.alpha = 0f;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
        _canProceed = true;
    }

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
        UIManager.Instance.HideAll();
    }
}