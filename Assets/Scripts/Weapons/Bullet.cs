using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 200f;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private GameObject impactVfx;
    [SerializeField] private AudioClip impactClip;

    private Rigidbody rb;

    private Vector3 direction;

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = direction * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (impactVfx != null)
        {
            Instantiate(impactVfx, collision.contacts[0].point, Quaternion.identity);
        }

        if (impactClip != null)
        {
            AudioSource.PlayClipAtPoint
            (
                impactClip,
                collision.contacts[0].point,
                1f
            );
        }

        Destroy(gameObject);
    }
}