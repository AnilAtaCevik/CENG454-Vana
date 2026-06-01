using UnityEngine;

public class AASystem : MonoBehaviour
{
    [Header("Targeting Settings")]
    [SerializeField] private float baseRange = 250f;
    [SerializeField] private float radarBonusRange = 150f;
    [SerializeField] private string targetTag = "Player";
    [SerializeField] private Transform partToRotate;
    [SerializeField] private Transform barrelToRotate;
    [SerializeField] private float turnSpeed = 10f;

    [Header("Shooting Settings")]
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private Transform[] firePoints;
    [SerializeField] private float fireRate = 4f;
    [SerializeField] private float requiredLockTime = 3f;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip fireSound;

    private Transform target;
    private float fireCountdown = 0f;
    private int currentFirePointIndex = 0;
    private float lockOnTimer = 0f;
    private float currentRange;

    private void Start()
    {
        currentRange = baseRange;
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.2f);
        
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        CheckRadarSynergy();

        if (target == null)
        {
            lockOnTimer = 0f;
            return;
        }

        LockOnTarget();

        lockOnTimer += Time.deltaTime;

        if (lockOnTimer >= requiredLockTime)
        {
            if (fireCountdown <= 0f)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, target.position);
                if (HasClearLineOfSight(distanceToPlayer))
                {
                    Fire();
                    fireCountdown = fireRate;
                }
            }
        }

        if (fireCountdown > 0f)
        {
            fireCountdown -= Time.deltaTime;
        }
    }

    private void UpdateTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag(targetTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestPlayer = null;

        foreach (GameObject player in players)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer < shortestDistance)
            {
                shortestDistance = distanceToPlayer;
                nearestPlayer = player;
            }
        }

        if (nearestPlayer != null && shortestDistance <= currentRange)
        {
            target = nearestPlayer.transform;
        }
        else
        {
            target = null;
        }
    }

    private void LockOnTarget()
    {
        if (target == null) return;

        if (partToRotate != null)
        {
            Vector3 targetDirY = target.position - partToRotate.position;
            targetDirY.y = 0f; 
            if (targetDirY != Vector3.zero)
            {
                Quaternion lookRotY = Quaternion.LookRotation(targetDirY);
                partToRotate.rotation = Quaternion.Slerp(partToRotate.rotation, lookRotY, Time.deltaTime * turnSpeed);
            }
        }

        if (barrelToRotate != null)
        {
            Vector3 targetDirX = target.position - barrelToRotate.position;
            float distanceToTarget = new Vector2(targetDirX.x, targetDirX.z).magnitude;
            float angleX = Mathf.Atan2(targetDirX.y, distanceToTarget) * Mathf.Rad2Deg;

            Quaternion targetLocalRotation = Quaternion.Euler(-angleX, 0f, 0f);
            barrelToRotate.localRotation = Quaternion.Slerp(barrelToRotate.localRotation, targetLocalRotation, Time.deltaTime * turnSpeed);
        }
    }

    private bool HasClearLineOfSight(float distanceToPlayer)
    {
        if (firePoints.Length == 0) return false;

        Transform currentFP = firePoints[currentFirePointIndex];
        Vector3 directionToPlayer = (target.position - currentFP.position).normalized;
        RaycastHit hit;

        if (Physics.Raycast(currentFP.position, directionToPlayer, out hit, distanceToPlayer))
        {
            if (hit.collider.CompareTag(targetTag))
            {
                return true;
            }
        }
        return false;
    }

    private void Fire()
    {
        if (firePoints.Length == 0 || missilePrefab == null || barrelToRotate == null) return;

        Transform currentFP = firePoints[currentFirePointIndex];
        GameObject bulletGO = Instantiate(missilePrefab, currentFP.position, barrelToRotate.rotation);

        AABullet bullet = bulletGO.GetComponent<AABullet>();
        if (bullet != null)
        {
            bullet.Seek(target);
        }

        if (audioSource != null && fireSound != null)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            audioSource.clip = fireSound;
            audioSource.Play();
        }

        currentFirePointIndex = (currentFirePointIndex + 1) % firePoints.Length;
    }

    private void CheckRadarSynergy()
    {
        try
        {
            GameObject radarObj = GameObject.FindGameObjectWithTag("Radar");
            currentRange = (radarObj != null) ? (baseRange + radarBonusRange) : baseRange;
        }
        catch
        {
            currentRange = baseRange;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, currentRange);
    }
}