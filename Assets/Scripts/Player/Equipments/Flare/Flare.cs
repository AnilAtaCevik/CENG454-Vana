using UnityEngine;

public class Flare : MonoBehaviour
{
    [Header("Lifetime")]
    [SerializeField] private float lifeTime = 4f;

    [Header("Guided Missile Counter")]
    [SerializeField] private string guidedMissileTag = "GuidedMissile";

    [Header("Destroy Feedback")]
    [SerializeField] private GameObject guidedMissileDestroyVfx;
    [SerializeField] private AudioClip guidedMissileDestroyClip;
    [SerializeField] private float destroySoundVolume = 1f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
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