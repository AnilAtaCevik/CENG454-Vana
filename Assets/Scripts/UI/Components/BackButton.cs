using UnityEngine;

public class BackButton : MonoBehaviour
{
    public void OnBackClicked()
    {
        UIManager.Instance.ShowScreen(
            Object.FindAnyObjectByType<MainMenuScreen>()
        );
    }
}