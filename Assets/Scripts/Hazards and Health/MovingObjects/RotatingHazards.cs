using UnityEngine;

public class RotatingHazard : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private Vector3 angularVelocity = new Vector3(0f, 50f, 0f);

    private void Update()
    {
        transform.Rotate(angularVelocity * Time.deltaTime);
    }
}