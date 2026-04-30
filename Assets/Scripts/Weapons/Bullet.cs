using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 200f;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private GameObject impactVfx;
    [SerializeField] private AudioClip impactClip;
    [SerializeField] private GameObject enemyImpactVfx;
    [SerializeField] private AudioClip enemyImpactClip;

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

        bool isEnemy = collision.gameObject.CompareTag("Enemy");

        if (isEnemy)
        {
            if (enemyImpactVfx != null)
            {
                Instantiate(enemyImpactVfx, hitPoint, Quaternion.identity);
            }

            // ENEMY SOUND
            if (enemyImpactClip != null)
            {
                AudioSource.PlayClipAtPoint(enemyImpactClip, hitPoint, 1f);
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
                AudioSource.PlayClipAtPoint(impactClip, hitPoint, 1f);
            }
        }

        Destroy(gameObject);
    }
}