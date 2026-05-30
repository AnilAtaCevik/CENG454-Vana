using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public Rigidbody rb { get; private set; }

    [Header("Inputs")]
    public InputAction ascentDescent;
    public InputAction rightLeft;
    public InputAction pitch;

    [Header("Audio")]
    [SerializeField] AudioSource altitudeWarningAudio;

    [Header("Flight Settings")]
    public float ascentDescentStrength = 1000f;
    public float rightLeftStrength = 1000f;
    public float pitchStrength = 10f;
    public float tiltSpeed = 5f; 
    public float maxTiltAngle = 15f;
    public float maxSpeed = 30f;
    public float linearDrag = 2f;
    public float serviceCeiling = 50f;
    public float absoluteCeiling = 100f;
    public float altitudeSoftness = 3f;
    public float maxClimbSpeed = 12f;
    public float verticalDrag = 2.5f;   

    public float targetZ;
    public float targetY;
    public bool isMovingLeft;

    private IFlightStrategy currentFlightStrategy;
    private IEnginePower enginePower;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetY = rb.rotation.eulerAngles.y;

        IEnginePower baseEngine = new BaseEnginePower(); 
        enginePower = new AltitudePenaltyDecorator(baseEngine, serviceCeiling, absoluteCeiling, altitudeSoftness);
        
        currentFlightStrategy = new ActiveFlightStrategy(); 
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

    private void HandleFuelEmpty()
    {
        currentFlightStrategy = new DeadEngineStrategy();
        if (altitudeWarningAudio != null && altitudeWarningAudio.isPlaying) altitudeWarningAudio.Stop();
    }

    private void HandleFuelChanged(float current, float max)
    {
        if (current > 0 && currentFlightStrategy is DeadEngineStrategy)
        {
            currentFlightStrategy = new ActiveFlightStrategy();
        }
    }

    void FixedUpdate()
    {
        if (rb.isKinematic)
        {
            Debug.LogWarning("Helikopter şu an Kinematic oldu! Inspector'dan veya başka bir scriptten müdahale ediliyor.", this.gameObject);
        }
        
        targetZ = 0f;
        float currentHeight = transform.position.y;
        
        float currentPower = enginePower.GetPowerMultiplier(currentHeight);
        
        currentFlightStrategy.ExecuteMovement(this, currentPower);
        StabilizeRotation();
    }

    private void StabilizeRotation()
    {
        targetZ = Mathf.Clamp(targetZ, -maxTiltAngle, maxTiltAngle);
        Quaternion targetRotation = Quaternion.Euler(0f, targetY, targetZ);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * tiltSpeed));

        rb.angularVelocity = Vector3.zero;
    }

    public void CheckAltitudeAudio(float currentHeight)
    {
        if (altitudeWarningAudio == null) return;

        if (currentHeight < serviceCeiling && altitudeWarningAudio.isPlaying) 
            altitudeWarningAudio.Stop();
        else if (currentHeight >= serviceCeiling && currentHeight <= absoluteCeiling && !altitudeWarningAudio.isPlaying) 
            altitudeWarningAudio.Play();
    }
}