using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen Instance { get; private set; }

    [SerializeField] private GameObject canvas;
    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private TextMeshProUGUI sceneNameText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Call this instead of SceneManager.LoadScene
    public void LoadScene(string sceneName, string displayName = "")
    {
        StartCoroutine(LoadAsync(sceneName, displayName));
    }

    private IEnumerator LoadAsync(string sceneName, string displayName)
    {
        canvas.SetActive(true);
        UIManager.Instance.HideAll();

        var victory = Object.FindAnyObjectByType<VictoryScreen>();
        if (victory != null) victory.HideCanvas();

        var gameOver = Object.FindAnyObjectByType<GameOverScreen>();
        if (gameOver != null) gameOver.HideCanvas();

        if (sceneNameText != null)
        {
            string name = string.IsNullOrEmpty(displayName) ? sceneName : displayName;
            sceneNameText.text = name + " scene is loading, please be patient...";
        }

        if (progressBar != null) progressBar.value = 0f;
        if (progressText != null) progressText.text = "0%";

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        float displayedProgress = 0f;

        while (operation.progress < 0.9f || displayedProgress < 1f)
        {
            float targetProgress = Mathf.Clamp01(operation.progress / 0.9f);
            displayedProgress = Mathf.MoveTowards(displayedProgress, targetProgress, Time.deltaTime * 0.8f);

            if (progressBar != null) progressBar.value = displayedProgress;
            if (progressText != null) progressText.text = Mathf.RoundToInt(displayedProgress * 100) + "%";

            if (operation.progress >= 0.9f && displayedProgress >= 1f) break;

            yield return null;
        }

        if (progressBar != null) progressBar.value = 1f;
        if (progressText != null) progressText.text = "100%";

        yield return new WaitForSecondsRealtime(0.5f);

        operation.allowSceneActivation = true;
        canvas.SetActive(false);
    }
}