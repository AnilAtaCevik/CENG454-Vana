using UnityEngine;

public class Missile : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 40f;

    [Header("Lifetime")]
    [SerializeField] private float lifeTime = 3f;

    [Header("Explosion")]
    [SerializeField] private GameObject explosionVfx;

    [Header("Damage")]
    [SerializeField] private float explosionRadius = 10f;
    [SerializeField] private float damage = 50f;


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
            GameObject explosion = Instantiate(
                explosionVfx,
                transform.position,
                Quaternion.identity
            );

            AudioSource audioSource = explosion.GetComponent<AudioSource>();

            if (audioSource != null)
            {
                audioSource.Play();
            }
        }

        Collider[] hitColliders =
            Physics.OverlapSphere(
                transform.position,
                explosionRadius
            );

        foreach (Collider hit in hitColliders)
        {
            EnemyHealth enemy =
                hit.GetComponent<EnemyHealth>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            explosionRadius
        );
    }
}
