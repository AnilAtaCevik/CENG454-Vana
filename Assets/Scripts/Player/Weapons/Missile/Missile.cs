using UnityEngine;

public class Missile : MonoBehaviour, IPoolable
{
    [Header("Movement")]
    [SerializeField] private float speed = 40f;

    [Header("Lifetime")]
    [SerializeField] private float lifeTime = 3f;

    [Header("Explosion")]
    [SerializeField] private GameObject explosionVfx;

    [Header("Damage")]
    [SerializeField] private float explosionRadius = 10f;

    [Header("Engine VFX")]
    [SerializeField] private ParticleSystem launchBurstVfx;
    [SerializeField] private ParticleSystem afterburnerVfx;

    [Header("Audio")]
    [SerializeField] private AudioSource flightAudio;
    [SerializeField] private AudioClip launchClip;

    private Rigidbody rb;
    private ObjectPool ownerPool;
    private float lifeTimer;
    private bool isReturning;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Initialize(ObjectPool pool)
    {
        ownerPool = pool;
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

        if (launchClip != null)
        {
            AudioSource.PlayClipAtPoint(launchClip, transform.position, 1f);
        }

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
        if (flightAudio != null)
        {
            flightAudio.Stop();
        }

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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}