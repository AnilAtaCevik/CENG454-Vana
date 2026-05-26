using UnityEngine;

public class GuidedMissile : MonoBehaviour
{
    [SerializeField] private float speed = 50f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float damage = 25f;
    [SerializeField] private float lifeTime = 7f;
    [SerializeField] private float proximityRadius = 3f;

    private Transform target;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        if (target == null)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget <= proximityRadius)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            transform.LookAt(target.position);
        }
        else if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        else
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }
}