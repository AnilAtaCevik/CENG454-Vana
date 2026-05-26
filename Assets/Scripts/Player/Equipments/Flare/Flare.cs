using UnityEngine;

public class Flare : MonoBehaviour
{
    [Header("Lifetime")]
    [SerializeField] private float lifeTime = 4f;

    [Header("Guided Missile Counter")]
    [SerializeField] private string guidedMissileTag = "GuidedMissile";

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(guidedMissileTag))
        {
            Destroy(other.gameObject);
        }
    }
}