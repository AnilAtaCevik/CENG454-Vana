using UnityEngine;

public class Flare : MonoBehaviour
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

    private Vector3 moveDirection;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void Initialize(Vector3 helicopterForward)
    {
        moveDirection = helicopterForward.normalized;
    }

    void Update()
    {
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
        }
    }
}