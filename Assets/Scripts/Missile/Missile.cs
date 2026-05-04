using UnityEngine;

public class Missile : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 40f;

    [Header("Lifetime")]
    [SerializeField] private float lifeTime = 5f;

    private Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.linearVelocity = transform.forward * speed;

        Destroy(gameObject, lifeTime);
    }
}