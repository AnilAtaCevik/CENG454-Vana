using UnityEngine;

public class AABullet : MonoBehaviour
{
    [SerializeField] private float speed = 60f;
    [SerializeField] private float damage = 2f;
    [SerializeField] private float lifeTime = 4f;

    private Vector3 moveDirection = Vector3.zero;
    private bool hasDirection = false;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        if (hasDirection)
        {
            transform.position += moveDirection * speed * Time.deltaTime;
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

    public void Seek(Transform targetTransform)
    {
        if (targetTransform != null)
        {
            moveDirection = (targetTransform.position - transform.position).normalized;
            
            if (moveDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveDirection);
            }
            
            hasDirection = true;
        }
        else
        {
            moveDirection = transform.forward;
            hasDirection = true;
        }
    }
}