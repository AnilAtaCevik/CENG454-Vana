using UnityEngine;

public enum HazardAxis { X, Y, Z }

public class MovingHazard : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private HazardAxis movementDirection = HazardAxis.X;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float distance = 5f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float movement = Mathf.Sin(Time.time * speed) * distance;
        Vector3 targetPosition = startPosition;

        switch (movementDirection)
        {
            case HazardAxis.X:
                targetPosition.x += movement;
                break;
            case HazardAxis.Y:
                targetPosition.y += movement;
                break;
            case HazardAxis.Z:
                targetPosition.z += movement;
                break;
        }

        transform.position = targetPosition;
    }
}