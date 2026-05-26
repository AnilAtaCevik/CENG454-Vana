using UnityEngine;

public class Bullet : MonoBehaviour, IPoolable
{
    [Header("Movement")]
    [SerializeField] private float speed = 200f;
    [SerializeField] private float lifeTime = 3f;

    [Header("Damage")]
    [SerializeField] private float damage = 10f;

    [Header("Impact Audio")]
    [SerializeField] private AudioClip impactClip;
    [SerializeField] private AudioClip enemyImpactClip;
    [SerializeField] private float impactVolume = 1f;
    [SerializeField] private float enemyImpactVolume = 1f;

    private Rigidbody rb;
    private Vector3 direction;

    private ObjectPool ownerPool;

    private ObjectPool impactVfxPool;
    private ObjectPool enemyImpactVfxPool;
    private ObjectPool impactAudioPool;
    private ObjectPool enemyImpactAudioPool;

    private float lifeTimer;
    private bool isReturning;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Initialize(
        Vector3 dir,
        ObjectPool pool,
        ObjectPool normalVfxPool,
        ObjectPool enemyVfxPool,
        ObjectPool normalAudioPool,
        ObjectPool enemyAudioPool
    )
    {
        direction = dir.normalized;
        ownerPool = pool;

        impactVfxPool = normalVfxPool;
        enemyImpactVfxPool = enemyVfxPool;
        impactAudioPool = normalAudioPool;
        enemyImpactAudioPool = enemyAudioPool;

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

        IDamageable damageable = FindDamageable(collision);

        bool hitDamageable = damageable != null;

        if (hitDamageable)
        {
            damageable.TakeDamage(damage);

            SpawnVfx(enemyImpactVfxPool, hitPoint);

            PlayImpactAudio(
                enemyImpactAudioPool,
                enemyImpactClip,
                enemyImpactVolume,
                hitPoint
            );
        }
        else
        {
            SpawnVfx(impactVfxPool, hitPoint);

            PlayImpactAudio(
                impactAudioPool,
                impactClip,
                impactVolume,
                hitPoint
            );
        }

        ReturnToPool();
    }

    IDamageable FindDamageable(Collision collision)
    {
        IDamageable damageable =
            collision.collider.GetComponentInParent<IDamageable>();

        if (damageable != null)
            return damageable;

        damageable =
            collision.collider.GetComponentInChildren<IDamageable>();

        if (damageable != null)
            return damageable;

        damageable =
            collision.gameObject.GetComponent<IDamageable>();

        return damageable;
    }

    void SpawnVfx(ObjectPool pool, Vector3 position)
    {
        if (pool == null)
            return;

        GameObject vfx = pool.Get();

        if (vfx == null)
            return;

        vfx.transform.position = position;
        vfx.transform.rotation = Quaternion.identity;

        PooledAutoReturn pooledAutoReturn =
            vfx.GetComponent<PooledAutoReturn>();

        if (pooledAutoReturn != null)
        {
            pooledAutoReturn.Initialize(pool);
        }
    }

    void PlayImpactAudio(
        ObjectPool pool,
        AudioClip clip,
        float volume,
        Vector3 position
    )
    {
        if (pool == null || clip == null)
            return;

        GameObject audioObj = pool.Get();

        if (audioObj == null)
            return;

        audioObj.transform.position = position;

        PooledAudio pooledAudio =
            audioObj.GetComponent<PooledAudio>();

        if (pooledAudio != null)
        {
            pooledAudio.Initialize(pool, clip, volume);
        }
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