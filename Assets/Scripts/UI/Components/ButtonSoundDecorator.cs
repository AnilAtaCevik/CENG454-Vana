using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSoundDecorator : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip hoverSound;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.outputAudioMixerGroup = AudioManager.Instance != null
            ? AudioManager.Instance.GetSFXGroup()
            : null;
        _audioSource.playOnAwake = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickSound != null && _audioSource != null && _audioSource.gameObject.activeInHierarchy)
            _audioSource.PlayOneShot(clickSound);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null && _audioSource != null && _audioSource.gameObject.activeInHierarchy)
            _audioSource.PlayOneShot(hoverSound);
    }
}