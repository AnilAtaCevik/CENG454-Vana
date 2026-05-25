using UnityEngine;

public class BackButton : MonoBehaviour
{
    // Returns to the main menu screen from any screen
    public void OnBackClicked()
    {
        UIManager.Instance.ShowScreen(
            Object.FindAnyObjectByType<MainMenuScreen>()
        );
    }
}