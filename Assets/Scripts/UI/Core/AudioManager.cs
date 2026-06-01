using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

// Singleton: managing all audio playbacks and volume settings across scenes
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip helicopterSound;
    [SerializeField] private AudioClip gameOverMusic;
    [SerializeField] private AudioClip victoryMusic;
    private AudioSource _musicSource;
    private const string SFX_PARAM = "SFXVolume";
    private const string MUSIC_PARAM = "MusicVolume";
    private const string DIALOGUE_PARAM = "DialogueVolume";

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

    private void Start()
    {
        LoadSavedVolumes();
        InitMusicSource();
        PlayMenuMusic();
    }

    // creates the AudioSource for music if it doesn't exist
    public void InitMusicSource()
    {
        if(_musicSource != null) return;
        _musicSource = gameObject.AddComponent<AudioSource>();
        _musicSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Music")[0];
        _musicSource.playOnAwake = false;
        _musicSource.loop = false;
    }

    // Returns the SFX mixer group for audio sources using them
    public UnityEngine.Audio.AudioMixerGroup GetSFXGroup()
    {
        return audioMixer.FindMatchingGroups("SFX")[0];
    }

    // Converts 0-1 slider value to decibel and saves to PlayerPrefs
    public void SetSFXVolume(float sliderValue)
    {
        audioMixer.SetFloat(SFX_PARAM, SliderToDecibel(sliderValue));
        PlayerPrefs.SetFloat(SFX_PARAM, sliderValue);
    }

    public void SetMusicVolume(float sliderValue)
    {
        audioMixer.SetFloat(MUSIC_PARAM, SliderToDecibel(sliderValue));
        PlayerPrefs.SetFloat(MUSIC_PARAM, sliderValue);
    }

    public void SetDialogueVolume(float sliderValue)
    {
        audioMixer.SetFloat(DIALOGUE_PARAM, SliderToDecibel(sliderValue));
        PlayerPrefs.SetFloat(DIALOGUE_PARAM, sliderValue);
    }

    public float GetSFXVolume() => PlayerPrefs.GetFloat(SFX_PARAM, 1f);
    public float GetMusicVolume() => PlayerPrefs.GetFloat(MUSIC_PARAM, 1f);
    public float GetDialogueVolume() => PlayerPrefs.GetFloat(DIALOGUE_PARAM, 1f);

    //converts linear slider value to logarithmic decibel scale
    private float SliderToDecibel(float value)
    {
        value = Mathf.Clamp(value, 0.0001f, 1f);
        return Mathf.Log10(value) * 20f;
    }

    //loads and applies volume settings
    private void LoadSavedVolumes()
    {
        SetSFXVolume(PlayerPrefs.GetFloat(SFX_PARAM, 1f));
        SetMusicVolume(PlayerPrefs.GetFloat(MUSIC_PARAM, 1f));
        SetDialogueVolume(PlayerPrefs.GetFloat(DIALOGUE_PARAM, 1f));
    }

    //plays helicopter sound for 5 second than switches to menu music
    public void PlayMenuMusic()
    {
        if (!SaveSystem.LoadMusicEnabled()) return;
        StartCoroutine(PlayHelicopterThenMusic());
    }
    private IEnumerator PlayHelicopterThenMusic()
    {
        if (helicopterSound != null)
        {
            _musicSource.clip = helicopterSound;
            _musicSource.loop = false;
            _musicSource.Play();
            yield return new WaitForSeconds(5f);
        }

        if (menuMusic != null && SaveSystem.LoadMusicEnabled())
        {
            _musicSource.clip = menuMusic;
            _musicSource.loop = true;
            _musicSource.Play();
        }
    }
    public void StopMenuMusic()
    {
        if (_musicSource != null && _musicSource.isPlaying)
            StopAllCoroutines();
            _musicSource.Stop();
    }
    public void PlayGameOverMusic()
    {
        StopAllCoroutines();
        if (_musicSource == null) InitMusicSource();
        _musicSource.Stop();
        if (gameOverMusic != null)
        {
            _musicSource.clip = gameOverMusic;
            _musicSource.loop = false;
            _musicSource.Play();
        }
    }

    public void PlayVictoryMusic()
    {
        StopAllCoroutines();
        if (_musicSource == null) InitMusicSource();
        _musicSource.Stop();
        if (victoryMusic != null)
        {
            _musicSource.clip = victoryMusic;
            _musicSource.loop = false;
            _musicSource.Play();
        }
    }
}