using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 200f;
    [SerializeField] private float lifeTime = 3f;

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
    private BulletPool ownerPool;
    private float lifeTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        lifeTimer = lifeTime;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = direction * speed;
    }

    void Update()
    {
        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0f)
        {
            ReturnToPool();
        }
    }

    public void Initialize(Vector3 dir, BulletPool pool)
    {
        direction = dir.normalized;
        ownerPool = pool;
    }

    void OnCollisionEnter(Collision collision)
    {
        Vector3 hitPoint = collision.contacts[0].point;
        bool isEnemy = collision.gameObject.CompareTag("Enemy");

        if (isEnemy)
        {
            if (enemyImpactVfx != null)
                Instantiate(enemyImpactVfx, hitPoint, Quaternion.identity);

            if (enemyImpactClip != null)
                AudioSource.PlayClipAtPoint(enemyImpactClip, hitPoint, enemyImpactVolume);
        }
        else
        {
            if (impactVfx != null)
                Instantiate(impactVfx, hitPoint, Quaternion.identity);

            if (impactClip != null)
                AudioSource.PlayClipAtPoint(impactClip, hitPoint, impactVolume);
        }

        ReturnToPool();
    }

    void ReturnToPool()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (ownerPool != null)
        {
            ownerPool.ReturnBullet(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}