using UnityEngine;
using UnityEngine.SceneManagement;

// Shown when the helicopter is destroyed
public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private GameObject canvas;

    private void OnEnable()
    {
        GameEvents.OnHelicopterDestroyed += Show;
    }

    private void OnDisable()
    {
        GameEvents.OnHelicopterDestroyed -= Show;
    }

    private void Show()
    {
        Time.timeScale = 0f;
        canvas.SetActive(true);
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayGameOverMusic();
    }

    public void OnRestartClicked()
    {
        Time.timeScale = 1f;
        canvas.SetActive(false);
        string currentScene = SceneManager.GetActiveScene().name;
        LoadingScreen.Instance.LoadScene(currentScene, "Restarting...");
    }
//
    public void OnMenuClicked()
    {
        Time.timeScale = 1f;
        canvas.SetActive(false);
        SceneLoadContext.ReturningToMenu = true;
        LoadingScreen.Instance.LoadScene("MainMenuScene", "Returning to menu...");
        // LevelSequence.Instance.StartFromBeginning();
    }
    public void HideCanvas()
    {
        canvas.SetActive(false);
    }
    private void Update()
    {
        // Test: V tuşuna basınca Victory ekranını aç
        if (UnityEngine.InputSystem.Keyboard.current.vKey.wasPressedThisFrame)
        {
            Show();
        }
    }
}