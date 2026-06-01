using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Decorator pattern: adds sound feedback to any Button and it does not modifies it
[RequireComponent(typeof(Button))]
public class ButtonSoundDecorator : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip hoverSound;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource.ignoreListenerPause = true;
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.outputAudioMixerGroup = AudioManager.Instance != null
            ? AudioManager.Instance.GetSFXGroup()
            : null;
        _audioSource.playOnAwake = false;
    }

    //plays click sound when pressed
    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickSound != null && _audioSource != null )
            _audioSource.PlayOneShot(clickSound);
    }

    //plays hover sound when hovered
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null && _audioSource != null )
            _audioSource.PlayOneShot(hoverSound);
    }
}