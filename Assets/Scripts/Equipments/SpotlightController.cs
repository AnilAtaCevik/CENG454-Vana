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

    private bool isEnabled = false;

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
        }
    }

    void HandleRotation()
    {
        if (spotlightPivot == null || Mouse.current == null)
            return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        float screenHeight = Screen.height;

        float normalizedMouseY = 1f - (mousePosition.y / screenHeight);

        float targetAngle =
            Mathf.Lerp(
                maxDownAngle,
                maxUpAngle,
                normalizedMouseY
            );

        Quaternion targetRotation =
            Quaternion.Euler(
                targetAngle,
                0f,
                0f
            );

        spotlightPivot.localRotation =
            Quaternion.Slerp(
                spotlightPivot.localRotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
    }
}