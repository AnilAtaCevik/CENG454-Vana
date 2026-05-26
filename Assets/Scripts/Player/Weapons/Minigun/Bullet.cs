using UnityEngine;

public class Bullet : MonoBehaviour, IPoolable
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
    private ObjectPool ownerPool;
    private float lifeTimer;
    private bool isReturning;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Initialize(Vector3 dir, ObjectPool pool)
    {
        direction = dir.normalized;
        ownerPool = pool;
        lifeTimer = lifeTime;
        isReturning = false;
    }

    public void OnGetFromPool()
    {
        lifeTimer = lifeTime;
        isReturning = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void OnReturnToPool()
    {
        direction = Vector3.zero;
        isReturning = true;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        if (isReturning || rb == null)
            return;

        rb.linearVelocity = direction * speed;
    }

    void Update()
    {
        if (isReturning)
            return;

        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0f)
        {
            ReturnToPool();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isReturning)
            return;

        Vector3 hitPoint = collision.contacts[0].point;
        bool isEnemy = collision.gameObject.CompareTag("Enemy");

        if (isEnemy)
        {
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

        ReturnToPool();
    }

    void ReturnToPool()
    {
        if (isReturning)
            return;

        if (ownerPool != null)
        {
            ownerPool.Return(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}