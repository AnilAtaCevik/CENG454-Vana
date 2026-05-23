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
    private void UpdatePlayButtonLabel()
    {
        int lastLevel = SaveSystem.LoadLastLevel();
        playButtonText.text = lastLevel == 0 ? "New Game" : "Continue";
    }

    public void OnPlayClicked()
    {
        // int lastLevel = SaveSystem.LoadLastLevel();
        // if (lastLevel == 0)
        //     SceneManager.LoadScene("SampleScene");
        // else
        //     SceneManager.LoadScene(lastLevel);
        SceneManager.LoadScene("SampleScene");
    }

    public void OnMissionsClicked()
    {
        Debug.Log("Missions clicked");
    }

    public void OnCreditsClicked()
    {
        UIManager.Instance.ShowScreen(
            Object.FindAnyObjectByType<CreditsScreen>()
        );
        Debug.Log("Credits clicked");
    }

    public void OnSettingsClicked()
    {
        Debug.Log("Settings clicked");
    }

    public void OnExitClicked()
    {
        Debug.Log("Exit clicked");
    }
}