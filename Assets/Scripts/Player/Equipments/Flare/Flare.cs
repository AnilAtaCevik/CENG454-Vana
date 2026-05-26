using UnityEngine;

public class Flare : MonoBehaviour, IPoolable
{
    [Header("Lifetime")]
    [SerializeField] private float lifeTime = 4f;

    [Header("Movement")]
    [SerializeField] private float forwardSpeed = 8f;
    [SerializeField] private float fallSpeed = 1.5f;

    [Header("Guided Missile Counter")]
    [SerializeField] private string guidedMissileTag = "GuidedMissile";

    [Header("Destroy Feedback")]
    [SerializeField] private GameObject guidedMissileDestroyVfx;
    [SerializeField] private AudioClip guidedMissileDestroyClip;
    [SerializeField] private float destroySoundVolume = 1f;

    private ObjectPool ownerPool;

    private Vector3 moveDirection;

    private float lifeTimer;
    private bool isReturning;

    public void Initialize(
        Vector3 helicopterForward,
        ObjectPool pool
    )
    {
        moveDirection = helicopterForward.normalized;

        ownerPool = pool;

        lifeTimer = lifeTime;
        isReturning = false;
    }

    public void OnGetFromPool()
    {
        lifeTimer = lifeTime;
        isReturning = false;
    }

    public void OnReturnToPool()
    {
        isReturning = true;
    }

    void Update()
    {
        if (isReturning)
            return;

        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0f)
        {
            ReturnToPool();
            return;
        }

        Vector3 movement =
            moveDirection * forwardSpeed +
            Vector3.down * fallSpeed;

        transform.position += movement * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(guidedMissileTag))
        {
            Vector3 hitPoint = other.transform.position;

            if (guidedMissileDestroyVfx != null)
            {
                Instantiate(
                    guidedMissileDestroyVfx,
                    hitPoint,
                    Quaternion.identity
                );
            }

            if (guidedMissileDestroyClip != null)
            {
                AudioSource.PlayClipAtPoint(
                    guidedMissileDestroyClip,
                    hitPoint,
                    destroySoundVolume
                );
            }

            Destroy(other.gameObject);

            ReturnToPool();
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