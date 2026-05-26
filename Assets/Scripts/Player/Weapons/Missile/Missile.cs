using UnityEngine;

public class Missile : MonoBehaviour, IPoolable
{
    [Header("Movement")]
    [SerializeField] private float speed = 40f;

    [Header("Lifetime")]
    [SerializeField] private float lifeTime = 3f;

    [Header("Damage")]
    [SerializeField] private float explosionRadius = 10f;
    [SerializeField] private float damage = 50f;

    [Header("Engine VFX")]
    [SerializeField] private ParticleSystem launchBurstVfx;
    [SerializeField] private ParticleSystem afterburnerVfx;

    [Header("Audio")]
    [SerializeField] private AudioSource flightAudio;
    [SerializeField] private AudioClip launchClip;

    private Rigidbody rb;

    private ObjectPool ownerPool;
    private ObjectPool explosionVfxPool;
    private ObjectPool launchAudioPool;

    private float lifeTimer;
    private bool isReturning;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Initialize(
        ObjectPool pool,
        ObjectPool explosionPool,
        ObjectPool missileLaunchAudioPool
    )
    {
        ownerPool = pool;
        explosionVfxPool = explosionPool;
        launchAudioPool = missileLaunchAudioPool;

        lifeTimer = lifeTime;
        isReturning = false;

        if (rb != null)
        {
            rb.linearVelocity = transform.forward * speed;
            rb.angularVelocity = Vector3.zero;
        }

        if (launchBurstVfx != null)
        {
            launchBurstVfx.Play();
        }

        if (afterburnerVfx != null)
        {
            afterburnerVfx.Play();
        }

        PlayLaunchAudio();

        if (flightAudio != null)
        {
            flightAudio.Play();
        }
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
        isReturning = true;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (flightAudio != null)
        {
            flightAudio.Stop();
        }

        if (launchBurstVfx != null)
        {
            launchBurstVfx.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        if (afterburnerVfx != null)
        {
            afterburnerVfx.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    void FixedUpdate()
    {
        if (isReturning || rb == null)
            return;

        rb.linearVelocity = transform.forward * speed;
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

        Explode();
    }

    void Explode()
    {
        StopFlightAudio();

        SpawnExplosionVfx();

        ApplyAreaDamage();

        ReturnToPool();
    }

    void ApplyAreaDamage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(
            transform.position,
            explosionRadius
        );

        foreach (Collider hit in hitColliders)
        {
            IDamageable damageable = FindDamageable(hit);

            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
        }
    }

    IDamageable FindDamageable(Collider hit)
    {
        IDamageable damageable =
            hit.GetComponentInParent<IDamageable>();

        if (damageable != null)
            return damageable;

        damageable =
            hit.GetComponentInChildren<IDamageable>();

        if (damageable != null)
            return damageable;

        damageable =
            hit.GetComponent<IDamageable>();

        return damageable;
    }

    void SpawnExplosionVfx()
    {
        if (explosionVfxPool == null)
            return;

        GameObject explosion = explosionVfxPool.Get();

        if (explosion == null)
            return;

        explosion.transform.position = transform.position;
        explosion.transform.rotation = Quaternion.identity;

        PooledAutoReturn pooledAutoReturn =
            explosion.GetComponent<PooledAutoReturn>();

        if (pooledAutoReturn != null)
        {
            pooledAutoReturn.Initialize(explosionVfxPool);
        }

        AudioSource audioSource =
            explosion.GetComponent<AudioSource>();

        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    void PlayLaunchAudio()
    {
        if (launchAudioPool == null || launchClip == null)
            return;

        GameObject audioObj = launchAudioPool.Get();

        if (audioObj == null)
            return;

        audioObj.transform.position = transform.position;

        PooledAudio pooledAudio =
            audioObj.GetComponent<PooledAudio>();

        if (pooledAudio != null)
        {
            pooledAudio.Initialize(
                launchAudioPool,
                launchClip,
                1f
            );
        }
    }

    void StopFlightAudio()
    {
        if (flightAudio != null)
        {
            flightAudio.Stop();
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            explosionRadius
        );
    }
}