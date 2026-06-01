// using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;
// using TMPro;

// // Represents a single mission card in the missions screen
// public class MissionCard : MonoBehaviour
// {
//     [SerializeField] private TextMeshProUGUI missionNameText;
//     [SerializeField] private TextMeshProUGUI descriptionText;
//     [SerializeField] private Button button;

//     private MissionData _data;

//     // Sets up the card with mission data
//     public void Setup(MissionData data)
//     {
//         _data = data;
//         missionNameText.text = data.missionName;
//         descriptionText.text = data.description;
//         button.onClick.RemoveAllListeners();
//         button.onClick.AddListener(OnCardClicked);
//     }

//     // Loads the mission scene when card is clicked
//     private void OnCardClicked()
//     {
//         if (!string.IsNullOrEmpty(_data.sceneName))
//             SceneManager.LoadScene(_data.sceneName);
//     }

//     // Resets the card before returning to pool
//     public void ResetCard()
//     {
//         _data = null;
//         button.onClick.RemoveAllListeners();
//     }
// }


// 2
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;

// public class MissionCard : MonoBehaviour
// {
//     [SerializeField] private MissionData missionData;
//     [SerializeField] private Button button;

//     private void Start()
//     {
//         if (button != null)
//             button.onClick.AddListener(OnClicked);
//     }

//     private void OnClicked()
//     {
//         if (missionData != null && !string.IsNullOrEmpty(missionData.sceneName))
//             SceneManager.LoadScene(missionData.sceneName);
//     }
// }

//3
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

// Represents a single mission button on the mission map
public class MissionCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI missionNameText;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI levelNumberText;

    private MissionData _data;

    // Sets up the card with mission data and map position
    public void Setup(MissionData data)
    {
        _data = data;
        if (missionNameText != null)
            missionNameText.text = data.missionName;

        RectTransform rt = GetComponent<RectTransform>();
        if (rt != null)
            rt.anchoredPosition = data.mapPosition;
        if (levelNumberText != null)
            levelNumberText.text = data.missionNumber.ToString("D2");

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnCardClicked);
        gameObject.SetActive(true);
    }

    private void OnCardClicked()
    {
        // if (_data != null && !string.IsNullOrEmpty(_data.sceneName))
        //     LoadingScreen.Instance.LoadScene(_data.sceneName, _data.missionName);
        if (_data != null)
        LevelSequence.Instance.StartFromMission(_data);
    }

    // Resets card before returning to pool
    public void ResetCard()
    {
        _data = null;
        button.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
    }
}