using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScreen : MonoBehaviour, IScreen
{
    public string ScreenName => "SettingsScreen";

    [SerializeField] private GameObject canvas;

    [Header("Sliders")]
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider dialogueSlider;
    [SerializeField] private Slider sensitivitySlider;

    [Header("Value Texts")]
    [SerializeField] private TextMeshProUGUI sfxValueText;
    [SerializeField] private TextMeshProUGUI musicValueText;
    [SerializeField] private TextMeshProUGUI dialogueValueText;
    [SerializeField] private TextMeshProUGUI sensitivityValueText;

    [Header("Toggle")]
    [SerializeField] private Toggle bgmToggle;

    public void Show()
    {
        canvas.SetActive(true);
        LoadSettings();
    }

    public void Hide()
    {
        canvas.SetActive(false);
    }

    private void LoadSettings()
    {
        sfxSlider.SetValueWithoutNotify(AudioManager.Instance.GetSFXVolume());
        musicSlider.SetValueWithoutNotify(AudioManager.Instance.GetMusicVolume());
        dialogueSlider.SetValueWithoutNotify(AudioManager.Instance.GetDialogueVolume());
        sensitivitySlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("aimSens", 1f));
        bgmToggle.SetIsOnWithoutNotify(SaveSystem.LoadMusicEnabled());

        sfxSlider.onValueChanged.RemoveAllListeners();
        musicSlider.onValueChanged.RemoveAllListeners();
        dialogueSlider.onValueChanged.RemoveAllListeners();
        sensitivitySlider.onValueChanged.RemoveAllListeners();
        bgmToggle.onValueChanged.RemoveAllListeners();

        sfxSlider.onValueChanged.AddListener(OnSFXChanged);
        musicSlider.onValueChanged.AddListener(OnMusicChanged);
        dialogueSlider.onValueChanged.AddListener(OnDialogueChanged);
        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
        bgmToggle.onValueChanged.AddListener(OnBGMToggleChanged);

        UpdateAllValueTexts();
    }

    private void OnSFXChanged(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);
        sfxValueText.text = Mathf.RoundToInt(value * 100) + "%";
        // GameEventSystem.ScreenChanged("SFXVolume:" + value);
    }

    private void OnMusicChanged(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
        musicValueText.text = Mathf.RoundToInt(value * 100) + "%";
        // GameEventSystem.ScreenChanged("MusicVolume:" + value);
    }

    private void OnDialogueChanged(float value)
    {
        AudioManager.Instance.SetDialogueVolume(value);
        dialogueValueText.text = Mathf.RoundToInt(value * 100) + "%";
        // GameEventSystem.ScreenChanged("DialogueVolume:" + value);
    }
    private void OnSensitivityChanged(float value)
    {
        PlayerPrefs.SetFloat("aimSens", value);
        sensitivityValueText.text = value.ToString("F1") + "x";
    }

    private void OnBGMToggleChanged(bool isOn)
    {
        SaveSystem.SaveMusicEnabled(isOn);
        if (isOn)
            AudioManager.Instance.PlayMenuMusic();
        else
            AudioManager.Instance.StopMenuMusic();
    }
    public void OnResetClicked()
    {
        sfxSlider.SetValueWithoutNotify(1f);
        musicSlider.SetValueWithoutNotify(1f);
        dialogueSlider.SetValueWithoutNotify(1f);
        sensitivitySlider.SetValueWithoutNotify(1f);
        bgmToggle.SetIsOnWithoutNotify(true);

        AudioManager.Instance.SetSFXVolume(1f);
        AudioManager.Instance.SetMusicVolume(1f);
        AudioManager.Instance.SetDialogueVolume(1f);
        PlayerPrefs.SetFloat("aimSens", 1f);
        SaveSystem.SaveMusicEnabled(true);
        AudioManager.Instance.PlayMenuMusic();

        UpdateAllValueTexts();
    }
    private void UpdateAllValueTexts()
    {
    sfxValueText.text = Mathf.RoundToInt(sfxSlider.value * 100) + "%";
    musicValueText.text = Mathf.RoundToInt(musicSlider.value * 100) + "%";
    dialogueValueText.text = Mathf.RoundToInt(dialogueSlider.value * 100) + "%";
    sensitivityValueText.text = sensitivitySlider.value.ToString("F1") + "x";
    }
}