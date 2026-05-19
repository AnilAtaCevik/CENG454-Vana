using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private IScreen _currentScreen;

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

    public void ShowScreen(IScreen screen)
    {
        if (_currentScreen != null)
            _currentScreen.Hide();

        _currentScreen = screen;
        _currentScreen.Show();
    }

    public void HideAll()
    {
        if (_currentScreen != null)
            _currentScreen.Hide();

        _currentScreen = null;
    }
}