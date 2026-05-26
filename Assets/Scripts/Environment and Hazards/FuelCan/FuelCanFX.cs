using UnityEngine;

public class FuelCanFX : MonoBehaviour
{
    Vector3 startPosition;
    Vector3 endPosition;

    [SerializeField] Vector3 movementVector;
    [SerializeField] float oscilationSpeed;

    float movementFactor;

    void Start()
    {
        startPosition = transform.position;
        endPosition = startPosition + movementVector;
    }

    void Update()
    {
        movementFactor = Mathf.PingPong(Time.time * oscilationSpeed, 1f);
        transform.position = Vector3.Lerp(startPosition, endPosition, movementFactor);
    }
}