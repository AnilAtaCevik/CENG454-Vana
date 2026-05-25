using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ExitDialog : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private RectTransform dialogPanel;
    private bool _isOpen = false;

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (_isOpen)
                Hide();
            else
                Show();
        }
    }

    public void Show()
    {
        canvas.SetActive(true);
        _isOpen = true;
        StartCoroutine(ScaleIn());
    }

    public void Hide()
    {
       canvas.SetActive(false);
       _isOpen = false;
    }

    public void OnYesClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnNoClicked()
    {
        Hide();
        UIManager.Instance.ShowScreen(
        Object.FindAnyObjectByType<MainMenuScreen>()
        );
    }
    private IEnumerator ScaleIn()
    {
        float duration = 0.2f;
        float elapsed = 0f;

        dialogPanel.localScale = Vector3.zero;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float scale = Mathf.SmoothStep(0f, 1f, t);
            dialogPanel.localScale = new Vector3(scale, scale, 1f);
            yield return null;
        }

        dialogPanel.localScale = Vector3.one;
    }
}