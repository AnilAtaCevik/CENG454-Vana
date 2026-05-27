using UnityEngine;

public class BackAndForthHazard : MonoBehaviour
{
    [Header("Back & Forth Settings")]
    [SerializeField] private Vector3 moveDirection = new Vector3(1f, 0f, 0f);
    [SerializeField] private float speed = 3f;
    [SerializeField] private float maxDistance = 5f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
        moveDirection = moveDirection.normalized;
    }

    private void Update()
    {
        float pingPongValue = Mathf.PingPong(Time.time * speed, maxDistance);
        transform.position = startPosition + (moveDirection * pingPongValue);
    }
}