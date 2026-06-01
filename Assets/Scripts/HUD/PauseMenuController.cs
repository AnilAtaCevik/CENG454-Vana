using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class PauseMenuController : MonoBehaviour
{
    [Header("Pause Button")]
    [SerializeField] private Button pauseButton;

    [Header("Pause Panel")]
    [SerializeField] private GameObject pausePanel;

    [Header("Buttons")]
    [SerializeField] private Button continueButton;
    [SerializeField] private Button replayButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;

    [Header("Scene Names")]
    [SerializeField] private string mainMenuSceneName = "MainMenuScene";

    private bool isPaused = false;

    void Start()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (pauseButton != null) pauseButton.onClick.AddListener(PauseGame);
        if (continueButton != null) continueButton.onClick.AddListener(ContinueGame);
        if (replayButton != null) replayButton.onClick.AddListener(ReplayGame);
        if (settingsButton != null) settingsButton.onClick.AddListener(OpenSettings);
        if (exitButton != null) exitButton.onClick.AddListener(ExitToMainMenu);

        // Stop any menu music carried over from MainMenuScene so it doesn't continue in gameplay
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopAllCoroutines();
            AudioManager.Instance.StopMenuMusic();
        }
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            // If settings is open, ESC = back to game
            if (SceneLoadContext.ReturnToGameAfterSettings)
            {
                var init = FindFirstObjectByType<MainMenuInit>();
                if (init != null) init.ReturnToGame();
                return;
            }

            if (isPaused) ContinueGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        AudioListener.volume = 0f;   // global mute - silences all audio regardless of source config

        if (pausePanel != null) pausePanel.SetActive(true);
        GameEvents.RaiseGamePaused();
    }

    public void ContinueGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        AudioListener.volume = 1f;   // restore audio

        if (pausePanel != null) pausePanel.SetActive(false);
        GameEvents.RaiseGameResumed();
    }

    public void ReplayGame()
    {
        Time.timeScale = 1f;
        AudioListener.volume = 1f;
        CheckpointManager.ClearAll();   // fresh restart - wipe any saved checkpoint
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenSettings()
    {
        SceneLoadContext.OpenSettings = true;
        SceneLoadContext.ReturnToGameAfterSettings = true;

        Time.timeScale = 0f;
        // Keep AudioListener.volume = 0 from PauseGame (silent settings)

        if (pausePanel != null) pausePanel.SetActive(false);
        SceneManager.LoadScene(mainMenuSceneName, LoadSceneMode.Additive);
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        // Don't unmute here - keep transition silent so game SFX (helicopter, weapons)
        // don't briefly play before SampleScene unloads.
        // RestartMenuMusicDelayed in MainMenuInit will restore volume.
        SceneLoadContext.ReturningToMenu = true;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}