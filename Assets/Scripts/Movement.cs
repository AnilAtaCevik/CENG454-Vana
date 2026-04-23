using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] InputAction ascentDescent;
    [SerializeField] InputAction rightLeft;
    [SerializeField] InputAction pitch;
    [SerializeField] float ascentDescentStrength = 1000;
    [SerializeField] float rightLeftStrength = 1000;
    [SerializeField] float pitchStrength = 10;

    bool isMovingLeft = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        ascentDescent.Enable();
        rightLeft.Enable();
        pitch.Enable();
    }

    void FixedUpdate()
    {
        ProcessAscentDescent();
        ProcessRightLeft();
    }

    //ASCENT - DESCENT
    private void ProcessAscentDescent()
    {
        float verticalInput = ascentDescent.ReadValue<float>();

        if (verticalInput > 0)
        {
            Ascent();
        }
        else if (verticalInput < 0)
        {
            Descent();
        }
    }

    private void Ascent()
    {
        ApplyAscentDescent(ascentDescentStrength);
    }

    private void Descent()
    {
        ApplyAscentDescent(-ascentDescentStrength);
    }

    private void ApplyAscentDescent(float verticalThisFrame)
    {
        rb.AddRelativeForce(Vector3.up * Time.fixedDeltaTime * verticalThisFrame);
    }

    //RIGHT - LEFT
    private void ProcessRightLeft()
    {
        float rightInput = rightLeft.ReadValue<float>();

        if (rightInput > 0)
        {
            if (isMovingLeft == true)
            {
                isMovingLeft = false;
                rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, 180f, 0f));
            }
            Right();
        }

        else if (rightInput < 0)
        {
            if (isMovingLeft == false)
            {
                isMovingLeft = true;
                rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, 180f, 0f));
            }
            Left();
        }
    }

    private void Right()
    {
        ApplyRightLeft(rightLeftStrength);
    }

    private void Left()
    {
        ApplyRightLeft(rightLeftStrength);
    }

    private void ApplyRightLeft(float rightThisFrame)
    {
        rb.AddRelativeForce(Vector3.right * Time.fixedDeltaTime * rightThisFrame);
    }
}
