using UnityEngine;

public class Missile : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 40f;

    [Header("Lifetime")]
    [SerializeField] private float lifeTime = 5f;

    [Header("Explosion")]
    [SerializeField] private GameObject explosionVfx;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;

        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    void Explode()
    {
        if (explosionVfx != null)
        {
            Instantiate(explosionVfx, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}