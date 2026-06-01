using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playButtonText;

    private void Start()
    {
        UpdatePlayButtonLabel();
    }

    //shows "New Game" if no save exits, otherwise "Continue"
    private void UpdatePlayButtonLabel()
    {
        int lastLevel = SaveSystem.LoadLastLevel();
        playButtonText.text = lastLevel == 0 ? "New Game" : "Continue";
    }

    public void OnPlayClicked()
    {
        LevelSequence.Instance.StartFromBeginning();
    }

    public void OnMissionsClicked()
    {
        UIManager.Instance.ShowScreen(
            Object.FindAnyObjectByType<MissionsScreen>()
        );
    }

    public void OnCreditsClicked()
    {
        UIManager.Instance.ShowScreen(
            Object.FindAnyObjectByType<CreditsScreen>()
        );
    }

    public void OnSettingsClicked()
    {
        UIManager.Instance.ShowScreen(
            Object.FindAnyObjectByType<SettingsScreen>()
        );
    }

    public void OnExitClicked()
    {
         Object.FindAnyObjectByType<ExitDialog>()?.Show();
    }
}