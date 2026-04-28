using UnityEngine;

public class AASystem : MonoBehaviour
{
    [Header("AA Settings")]
    [SerializeField] private float engagementRange = 50f;
    [SerializeField] private float fireRate = 0.2f; 

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform turretHead;
    [SerializeField] private Transform firePoint;         // ARTIK ZORUNLU: Merminin çıkacağı nokta 
    [SerializeField] private GameObject projectilePrefab; // ARTIK ZORUNLU: Atılacak mermi objesi 

    private float nextFireTime = 0f;

    void Update()
    {
        if (player == null || turretHead == null || firePoint == null || projectilePrefab == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= engagementRange)
        {
            TrackPlayer();
            
            if (Time.time >= nextFireTime)
            {
                Fire();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void TrackPlayer()
    {
        Vector3 direction = (player.position - turretHead.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        turretHead.rotation = Quaternion.Slerp(turretHead.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void Fire()
    {
        // Namlunun ucunda (firePoint.position) ve namlunun baktığı açıda (firePoint.rotation) mermiyi yarat
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, engagementRange);
    }
}