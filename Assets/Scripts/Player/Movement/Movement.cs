using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public Rigidbody rb { get; private set; }
    
    [Header("Engine System")]
    public BaseEnginePowerSO EnginePower; 

    [Header("Inputs")]
    public InputAction ascentDescent;
    public InputAction rightLeft;
    public InputAction pitch;

    [Header("Audio")]
    public AudioSource altitudeWarningAudio;

    [Header("Flight Settings")]
    public float ascentDescentStrength = 1000f;
    public float rightLeftStrength = 1000f;
    public float pitchStrength = 10f;
    public float tiltSpeed = 5f;
    public float maxTiltAngle = 15f;
    public float maxSpeed = 30f;
    public float linearDrag = 2f;
    public float maxClimbSpeed = 12f;
    public float verticalDrag = 2.5f;

    public float targetZ;
    public float targetY;
    public bool isMovingLeft;

    private IFlightState currentState;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetY = rb.rotation.eulerAngles.y;

        ChangeState(new ActiveFlightState(this));
    }

    void OnEnable()
    {
        ascentDescent.Enable();
        rightLeft.Enable();
        pitch.Enable();

        GameEvents.OnFuelEmpty += HandleFuelEmpty;
        GameEvents.OnFuelChanged += HandleFuelChanged;
    }

    void OnDisable()
    {
        ascentDescent.Disable();
        rightLeft.Disable();
        pitch.Disable();

        GameEvents.OnFuelEmpty -= HandleFuelEmpty;
        GameEvents.OnFuelChanged -= HandleFuelChanged;
    }

    public void ChangeState(IFlightState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    private void HandleFuelEmpty()
    {
        ChangeState(new DeadEngineState(this));
        if (altitudeWarningAudio != null && altitudeWarningAudio.isPlaying) altitudeWarningAudio.Stop();
    }

    private void HandleFuelChanged(float current, float max)
    {
        if (current > 0 && currentState is DeadEngineState)
        {
            ChangeState(new ActiveFlightState(this));
        }
    }

    void FixedUpdate()
    {
        targetZ = 0f;
        currentState?.Tick();
        StabilizeRotation();
    }

    private void StabilizeRotation()
    {
        targetZ = Mathf.Clamp(targetZ, -maxTiltAngle, maxTiltAngle);
        Quaternion targetRotation = Quaternion.Euler(0f, targetY, targetZ);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * tiltSpeed));

        if (!rb.isKinematic)
        {
            rb.angularVelocity = Vector3.zero;
        }
    }
}