using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] InputAction ascentDescent;
    [SerializeField] InputAction rightLeft;
    [SerializeField] InputAction pitch;
    [SerializeField] float ascentDescentStrength = 1000;
    [SerializeField] float rightLeftStrength = 10;
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
}
