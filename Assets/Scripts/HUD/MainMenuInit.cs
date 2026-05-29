using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(-1000)]
public class MainMenuInit : MonoBehaviour
{
    [SerializeField] private GameObject settingsScreenObject;
    [SerializeField] private GameObject backToGameButtonRoot;
    [SerializeField] private string mainMenuSceneName = "MainMenuScene";

    void Awake()
    {
        if (!SceneLoadContext.OpenSettings) return;

        // Disable duplicate EventSystem from MainMenuScene
        var eventSystems = FindObjectsByType<UnityEngine.EventSystems.EventSystem>
            (FindObjectsSortMode.None);
        if (eventSystems.Length > 1)
        {
            foreach (var es in eventSystems)
            {
                if (es.gameObject.scene.name == mainMenuSceneName)
                {
                    es.gameObject.SetActive(false);
                    break;
                }
            }
        }

        // Disable MainMenuScene camera so game underneath stays visible
        foreach (var cam in FindObjectsByType<Camera>(FindObjectsSortMode.None))
        {
            if (cam.gameObject.scene.name == mainMenuSceneName)
                cam.gameObject.SetActive(false);
        }

        // Disable other IScreen GameObjects so they can't override settings
        foreach (var mb in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
        {
            if (mb is IScreen && mb.gameObject.scene.name == mainMenuSceneName
                && mb.gameObject != settingsScreenObject)
            {
                mb.gameObject.SetActive(false);
            }
        }

        StopMenuAudioCompletely();
    }

    void Start()
    {
        if (SceneLoadContext.OpenSettings)
        {
            SceneLoadContext.OpenSettings = false;
            StartCoroutine(ShowSettingsDelayed());
        }
        else if (SceneLoadContext.ReturningToMenu)
        {
            SceneLoadContext.ReturningToMenu = false;
            StartCoroutine(RestartMenuMusicDelayed());
        }
    }

    private IEnumerator RestartMenuMusicDelayed()
    {
        yield return null;

        // Ensure timeScale=1 because PlayHelicopterThenMusic uses WaitForSeconds
        // (not Realtime), which would hang at timeScale=0
        Time.timeScale = 1f;
        AudioListener.volume = 1f;
        AudioListener.pause = false;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopAllCoroutines();
            AudioManager.Instance.StopMenuMusic();
            AudioManager.Instance.PlayMenuMusic();
            Debug.Log("[MainMenuInit] Restarted menu music on main menu return");
        }
    }

    private IEnumerator ShowSettingsDelayed()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        if (settingsScreenObject == null)
        {
            Debug.LogError("[MainMenuInit] settingsScreenObject is NULL!");
            yield break;
        }

        // Activate SettingsScreen and all its parents
        Transform t = settingsScreenObject.transform;
        while (t != null)
        {
            t.gameObject.SetActive(true);
            t = t.parent;
        }

        // Activate all children recursively
        foreach (var child in settingsScreenObject.GetComponentsInChildren<Transform>(true))
            child.gameObject.SetActive(true);

        // Force Canvas to overlay
        Canvas canvas = settingsScreenObject.GetComponentInChildren<Canvas>(true);
        if (canvas != null)
        {
            canvas.enabled = true;
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;
        }

        // Force all CanvasGroups visible
        foreach (var cg in settingsScreenObject.GetComponentsInChildren<CanvasGroup>(true))
        {
            cg.alpha = 1;
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }

        // Call IScreen.Show() so SettingsScreen wires slider listeners
        IScreen settingsScreen = settingsScreenObject.GetComponent<IScreen>();
        if (settingsScreen != null)
        {
            settingsScreen.Show();
            Debug.Log("[MainMenuInit] Called IScreen.Show() on settings");
        }

        // Disable friend's back button(s) so user only uses BACK TO GAME
        foreach (var btn in settingsScreenObject.GetComponentsInChildren<Button>(true))
        {
            string btnName = btn.gameObject.name.ToLower();
            if (btnName.Contains("back") || btnName.Contains("return"))
            {
                if (backToGameButtonRoot != null &&
                    (btn.gameObject == backToGameButtonRoot ||
                     btn.transform.IsChildOf(backToGameButtonRoot.transform)))
                    continue;

                btn.gameObject.SetActive(false);
                Debug.Log("[MainMenuInit] Disabled friend's back button: " + btn.gameObject.name);
            }
        }

        // Show BACK TO GAME button
        if (backToGameButtonRoot != null && SceneLoadContext.ReturnToGameAfterSettings)
        {
            backToGameButtonRoot.SetActive(true);
            Canvas backCanvas = backToGameButtonRoot.GetComponentInParent<Canvas>();
            if (backCanvas != null) backCanvas.sortingOrder = 101;
        }

        Time.timeScale = 0f;
        StopMenuAudioCompletely();
    }

    public void ReturnToGame()
    {
        SceneLoadContext.ReturnToGameAfterSettings = false;

        // Flush PlayerPrefs to disk so settings persist
        PlayerPrefs.Save();
        Debug.Log("[MainMenuInit] PlayerPrefs saved");

        StopMenuAudioCompletely();

        var pauseMenu = FindFirstObjectByType<PauseMenuController>();
        if (pauseMenu != null)
            pauseMenu.ContinueGame();
        else
        {
            Time.timeScale = 1f;
            AudioListener.volume = 1f;
        }

        var unloadOp = SceneManager.UnloadSceneAsync(mainMenuSceneName);
        if (unloadOp != null)
            unloadOp.completed += (_) => StopMenuAudioCompletely();
    }

    private void StopMenuAudioCompletely()
    {
        if (AudioManager.Instance == null) return;
        AudioManager.Instance.StopAllCoroutines();
        AudioManager.Instance.StopMenuMusic();
    }
}