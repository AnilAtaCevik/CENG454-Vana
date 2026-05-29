using UnityEngine;
using UnityEngine.InputSystem;

public class SpotlightController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Light spotlight;
    [SerializeField] private Transform spotlightPivot;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float maxUpAngle = 90f;
    [SerializeField] private float maxDownAngle = -90f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip turnOnClip;
    [SerializeField] private AudioClip turnOffClip;
    [SerializeField] private float audioVolume = 1f;

    private bool isEnabled = false;

    void Start()
    {
        if (spotlight != null)
        {
            spotlight.enabled = isEnabled;
        }

        EquipmentEvents.RaiseSpotlightToggled(isEnabled);
    }

    void Update()
    {
        HandleToggle();
        HandleRotation();
    }

    void HandleToggle()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            isEnabled = !isEnabled;

            if (spotlight != null)
            {
                spotlight.enabled = isEnabled;
            }

            PlayToggleSound();

            EquipmentEvents.RaiseSpotlightToggled(isEnabled);
        }
    }

    void HandleRotation()
    {
        if (spotlightPivot == null || Mouse.current == null)
            return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        float normalizedMouseY = 1f - (mousePosition.y / Screen.height);

        float targetAngle = Mathf.Lerp(
            maxDownAngle,
            maxUpAngle,
            normalizedMouseY
        );

        Quaternion targetRotation = Quaternion.Euler(
            targetAngle,
            0f,
            0f
        );

        spotlightPivot.localRotation = Quaternion.Slerp(
            spotlightPivot.localRotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    void PlayToggleSound()
    {
        if (audioSource == null)
            return;

        AudioClip selectedClip = isEnabled ? turnOnClip : turnOffClip;

        if (selectedClip != null)
        {
            audioSource.PlayOneShot(selectedClip, audioVolume);
        }
    }
}