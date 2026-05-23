using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private IScreen _currentScreen;
    private IScreen _previousScreen;
    private ITransitionStrategy _transitionStrategy;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        _transitionStrategy = new FadeTransition(0.5f);
    }

    // public void ShowScreen(IScreen screen)
    // {
    //     if (_currentScreen != null)
    //         _currentScreen.Hide();

    //     _currentScreen = screen;
    //     _currentScreen.Show();
    // }

    public void SetTransitionStrategy(ITransitionStrategy strategy)
    {
        _transitionStrategy = strategy;
    }

    public void ShowScreen(IScreen screen)
    {
        StartCoroutine(DoTransition(screen));
    }

    public void HideAll()
    {
        if (_currentScreen != null)
            _currentScreen.Hide();
        _currentScreen = null;
    }

    private IEnumerator DoTransition(IScreen nextScreen)
    {
        CanvasGroup fromGroup = null;
        CanvasGroup toGroup = null;

        if (_currentScreen != null)
        {
            fromGroup = (_currentScreen as MonoBehaviour)
                ?.GetComponentInChildren<CanvasGroup>(true);
        }

        if (nextScreen != null)
        {
            toGroup = (nextScreen as MonoBehaviour)
                ?.GetComponentInChildren<CanvasGroup>(true);
        }

        yield return StartCoroutine(
            _transitionStrategy.Transition(fromGroup, toGroup)
        );

        if (_currentScreen != null)
        _currentScreen.Hide();

        _previousScreen = _currentScreen;
        _currentScreen = nextScreen;

        if (_currentScreen != null)
        _currentScreen.Show();
    }
    public void GoBack()
    {
    Debug.Log("GoBack called. previousScreen: " + (_previousScreen != null ? _previousScreen.ScreenName : "NULL"));
    if (_previousScreen != null)
        ShowScreen(_previousScreen);
    }
}