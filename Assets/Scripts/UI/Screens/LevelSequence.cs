using UnityEngine;

// Manages the order of levels and persists across scenes
public class LevelSequence : MonoBehaviour
{
    public static LevelSequence Instance { get; private set; }

    [SerializeField] private MissionData[] levels;

    private int _currentIndex = 0;

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

    // Starts from the first level
    public void StartFromBeginning()
    {
        _currentIndex = 0;
        LoadCurrent();
    }

    // Starts from a specific mission
    public void StartFromMission(MissionData mission)
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i] == mission)
            {
                _currentIndex = i;
                break;
            }
        }
        LoadCurrent();
    }

    // Loads the next level in sequence
    public void LoadNextLevel()
    {
        _currentIndex++;
        if (_currentIndex < levels.Length)
            LoadCurrent();
        else
        {
            Object.FindAnyObjectByType<VictoryScreen>()?.Show();
        }
    }

    public bool HasNextLevel() => _currentIndex + 1 < levels.Length;

    public MissionData GetCurrentMission() => levels[_currentIndex];

    private void LoadCurrent()
    {
        MissionData current = levels[_currentIndex];
        SaveSystem.SaveLastLevel(_currentIndex);
        LoadingScreen.Instance.LoadScene(current.sceneName, current.missionName);
    }
}