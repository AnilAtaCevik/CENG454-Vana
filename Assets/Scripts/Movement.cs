using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    Fuel fuel;

    [SerializeField] InputAction ascentDescent;
    [SerializeField] InputAction rightLeft;
    [SerializeField] InputAction pitch;
    [SerializeField] AudioSource altitudeWarningAudio;
    [SerializeField] float ascentDescentStrength = 1000f;
    [SerializeField] float rightLeftStrength = 1000f;
    [SerializeField] float pitchStrength = 10f;
    [SerializeField] float tiltSpeed = 5f; 
    [SerializeField] float maxTiltAngle = 15f;
    [SerializeField] float maxSpeed = 30f;
    [SerializeField] float linearDrag = 2f;
    [SerializeField] float serviceCeiling = 50f;
    [SerializeField] float absoluteCeiling = 100f;
    [SerializeField] float altitudeSoftness = 3f;

    float targetZ = 0f;
    float targetY = 0f;

    bool isMovingLeft = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        fuel = GetComponent<Fuel>();
        targetY = rb.rotation.eulerAngles.y;
    }

    void OnEnable()
    {
        ascentDescent.Enable();
        rightLeft.Enable();
        pitch.Enable();
    }

    void FixedUpdate()
    {
        if (fuel != null && !fuel.HasFuel()) return; 

        targetZ = 0f;

        ProcessAscentDescent();
        ProcessRightLeft();
        ProcessPitch();

        VelocityLimiter();
        StabilizeRotation();
    }

//  ========================
//  ASCENT - DESCENT
//  ========================

    private void ProcessAscentDescent()
    {
        float verticalInput = ascentDescent.ReadValue<float>();
        float currentHeight = transform.position.y;

        if (verticalInput > 0)
        {
            if (currentHeight < serviceCeiling)
            {
                float proximityMultiplier = Mathf.Clamp01((serviceCeiling - currentHeight) / altitudeSoftness);
                ApplyAscentDescent(ascentDescentStrength * proximityMultiplier);

                if (altitudeWarningAudio.isPlaying) altitudeWarningAudio.Stop();
            }

            else if (currentHeight >= serviceCeiling && currentHeight <= absoluteCeiling)
            {
                float proximityMultiplier = Mathf.Clamp01((absoluteCeiling - currentHeight) / altitudeSoftness * 20);
                ApplyAscentDescent(ascentDescentStrength * proximityMultiplier);

                if (!altitudeWarningAudio.isPlaying) altitudeWarningAudio.Play();
            }

            else
            {
                Vector3 vel = rb.linearVelocity;

                if (vel.y > 0)
                {
                    rb.linearVelocity = new Vector3(vel.x, vel.y * 0.9f, vel.z);
                }
            }
        }
        
        else if (verticalInput < 0)
        {
            ApplyAscentDescent(-ascentDescentStrength);
        }
    }

    private void ApplyAscentDescent(float verticalThisFrame)
    {
        rb.AddRelativeForce(Vector3.up * Time.fixedDeltaTime * verticalThisFrame);
    }

//  ========================
//  RIGHT - LEFT
//  ========================

    private void ProcessRightLeft()
    {
        float rightInput = rightLeft.ReadValue<float>();

        if (rightInput > 0)
        {
            if (isMovingLeft == true)
            {
                isMovingLeft = false;
                rb.linearVelocity = rb.linearVelocity * 0.5f;
                targetY -= 180f;
            }

            RightLeftMove();
        }

        else if (rightInput < 0)
        {
            if (isMovingLeft == false)
            {
                isMovingLeft = true;
                rb.linearVelocity = rb.linearVelocity * 0.5f;
                targetY += 180f;
            }

            RightLeftMove();
        }
    }

    private void RightLeftMove()
    {
        ApplyRightLeft(rightLeftStrength);
        targetZ += -10;
    }

    private void ApplyRightLeft(float rightThisFrame)
    {
        rb.AddRelativeForce(Vector3.right * Time.fixedDeltaTime * rightThisFrame);
    }

//  ========================
//  PITCH
//  ========================

    private void ProcessPitch()
    {
        float pitchInput = pitch.ReadValue<float>();

        if (pitchInput > 0)
        {
            if (!isMovingLeft)
            {
                targetZ += maxTiltAngle;
                rb.AddRelativeForce(Vector3.forward * Time.fixedDeltaTime * pitchStrength);
            }
            else
            {
                targetZ += -maxTiltAngle;
                rb.AddRelativeForce(Vector3.forward * Time.fixedDeltaTime * pitchStrength);
            }
        }
        else if (pitchInput < 0)
        {
            if (!isMovingLeft)
            {
                targetZ += -maxTiltAngle;
                rb.AddRelativeForce(Vector3.forward * Time.fixedDeltaTime * -pitchStrength);
            }
            else
            {
                targetZ += maxTiltAngle;
                rb.AddRelativeForce(Vector3.forward * Time.fixedDeltaTime * -pitchStrength);
            }
        }
    }

//  ========================
//  ROTATION HANDLING
//  ========================

    private void StabilizeRotation()
    {
        targetZ = Mathf.Clamp(targetZ, -maxTiltAngle, maxTiltAngle);

        Quaternion targetRotation = Quaternion.Euler(0f, targetY, targetZ);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * tiltSpeed));
    }

//  ========================
//  VELOCITY LIMITER
//  ========================

    private void VelocityLimiter()
    {
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }

        float horizontalInput = rightLeft.ReadValue<float>();
        float pitchInput = pitch.ReadValue<float>();

        if (horizontalInput == 0 && pitchInput == 0)
        {
            Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(-horizontalVelocity * linearDrag, ForceMode.Acceleration);
        }
    }
}