using UnityEngine;

// Shown when all levels are completed
public class VictoryScreen : MonoBehaviour
{
    [SerializeField] private GameObject canvas;

    public void Show()
    {
        Time.timeScale = 0f;
        canvas.SetActive(true);
        var gameOver = GetComponent<GameOverScreen>();
        if (gameOver != null)
            gameOver.HideCanvas();
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayVictoryMusic();
    }

    public void OnRestartClicked()
    {
        Time.timeScale = 1f;
        canvas.SetActive(false);
        if (AudioManager.Instance != null)
            AudioManager.Instance.StopMenuMusic();
        LevelSequence.Instance.StartFromBeginning();
    }

    public void OnMenuClicked()
    {
        Time.timeScale = 1f;
        canvas.SetActive(false);
        SceneLoadContext.ReturningToMenu = true;
        LoadingScreen.Instance.LoadScene("MainMenuScene", "Returning to menu...");
    }
    public void HideCanvas()
    {
        canvas.SetActive(false);
    }
}