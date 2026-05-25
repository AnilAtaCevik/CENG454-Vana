using UnityEngine;

public class MainMenuScreen : MonoBehaviour, IScreen
{
    public string ScreenName => "MainMenuScreen";
    [SerializeField] private GameObject canvas;

    public void Show()
    {
        canvas.SetActive(true);
    }

    public void Hide()
    {
        canvas.SetActive(false);
    }
}