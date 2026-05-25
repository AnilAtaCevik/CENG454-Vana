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

    [Header("Engine VFX")]
    [SerializeField] private ParticleSystem launchBurstVfx;
    [SerializeField] private ParticleSystem afterburnerVfx;

    [Header("Audio")]
    [SerializeField] private AudioSource flightAudio;
    [SerializeField] private AudioClip launchClip;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.linearVelocity = transform.forward * speed;

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
            AudioSource.PlayClipAtPoint(
                launchClip,
                transform.position,
                1f
            );
        }

        if (flightAudio != null)
        {
            flightAudio.Play();
        }

        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
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

            AudioSource audioSource =
                explosion.GetComponent<AudioSource>();

            if (audioSource != null)
            {
                audioSource.Play();
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