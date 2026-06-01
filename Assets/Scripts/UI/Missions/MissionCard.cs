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