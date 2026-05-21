using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioClip menuMusic;
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
        PlayMenuMusic();
    }

    public UnityEngine.Audio.AudioMixerGroup GetSFXGroup()
    {
        return audioMixer.FindMatchingGroups("SFX")[0];
    }

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

    private float SliderToDecibel(float value)
    {
        value = Mathf.Clamp(value, 0.0001f, 1f);
        return Mathf.Log10(value) * 20f;
    }

    private void LoadSavedVolumes()
    {
        SetSFXVolume(PlayerPrefs.GetFloat(SFX_PARAM, 1f));
        SetMusicVolume(PlayerPrefs.GetFloat(MUSIC_PARAM, 1f));
        SetDialogueVolume(PlayerPrefs.GetFloat(DIALOGUE_PARAM, 1f));
    }

    public void PlayMenuMusic()
    {
        if(menuMusic == null) return;
        if(_musicSource == null)
        {
            _musicSource = gameObject.AddComponent<AudioSource>();
            _musicSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Music")[0];
            _musicSource.loop = true ;
            _musicSource.playOnAwake = false;
        }

        bool musicEnabled = SaveSystem.LoadMusicEnabled();

        if (musicEnabled && !_musicSource.isPlaying)
        {
            _musicSource.clip = menuMusic;
            _musicSource.Play();
        }
        else if (!musicEnabled)
        {
            _musicSource.Stop();
        }
    }

    public void StopMenuMusic()
    {
        if (_musicSource != null && _musicSource.isPlaying)
            _musicSource.Stop();
    }
}