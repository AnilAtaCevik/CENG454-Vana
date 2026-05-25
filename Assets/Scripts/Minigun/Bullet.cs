using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 200f;
    [SerializeField] private float lifeTime = 3f;

    [Header("Damage")]
    [SerializeField] private float damage = 10f;

    [Header("Impact VFX")]
    [SerializeField] private GameObject impactVfx;
    [SerializeField] private GameObject enemyImpactVfx;

    [Header("Impact Audio")]
    [SerializeField] private AudioClip impactClip;
    [SerializeField] private AudioClip enemyImpactClip;
    [SerializeField] private float impactVolume = 1f;
    [SerializeField] private float enemyImpactVolume = 1f;

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
        Vector3 hitPoint = collision.contacts[0].point;

        EnemyHealth enemy = collision.collider.GetComponentInParent<EnemyHealth>();
        bool hitEnemy = enemy != null || collision.gameObject.CompareTag("Enemy");

        if (hitEnemy)
        {
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            if (enemyImpactVfx != null)
            {
                Instantiate(enemyImpactVfx, hitPoint, Quaternion.identity);
            }

            if (enemyImpactClip != null)
            {
                AudioSource.PlayClipAtPoint(enemyImpactClip, hitPoint, enemyImpactVolume);
            }
        }
        else
        {
            if (impactVfx != null)
            {
                Instantiate(impactVfx, hitPoint, Quaternion.identity);
            }

            if (impactClip != null)
            {
                AudioSource.PlayClipAtPoint(impactClip, hitPoint, impactVolume);
            }
        }

        Destroy(gameObject);
    }
}