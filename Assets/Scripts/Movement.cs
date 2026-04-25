using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] InputAction ascentDescent;
    [SerializeField] InputAction rightLeft;
    [SerializeField] InputAction pitch;
    [SerializeField] float ascentDescentStrength = 1000f;
    [SerializeField] float rightLeftStrength = 1000f;
    [SerializeField] float pitchStrength = 10f;
    [SerializeField] float tiltSpeed = 5f; 
    [SerializeField] float maxTiltAngle = 15f;

    float targetZ = 0f;
    float targetY = 0f;

    bool isMovingLeft = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
        targetZ = 0f;

        ProcessAscentDescent();
        ProcessRightLeft();
        ProcessPitch();

        StabilizeRotation();
    }

//  ========================
//  ASCENT - DESCENT
//  ========================

    private void ProcessAscentDescent()
    {
        float verticalInput = ascentDescent.ReadValue<float>();

        if (verticalInput > 0) Ascent();
        else if (verticalInput < 0) Descent();        
    }

    private void Ascent() {ApplyAscentDescent(ascentDescentStrength);}
    private void Descent() {ApplyAscentDescent(-ascentDescentStrength);}

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
                targetY -= 180f;
            }
            
            RightLeftMove();
        }

        else if (rightInput < 0)
        {
            if (isMovingLeft == false)
            {
                isMovingLeft = true;
                targetY += 180f;
            }

            RightLeftMove();
        }
    }

    private void RightLeftMove()
    {
        ApplyRightLeft(rightLeftStrength);
        targetZ += -maxTiltAngle;
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
}
