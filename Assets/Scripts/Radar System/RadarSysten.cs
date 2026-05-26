using UnityEngine;

public class RadarSystem : MonoBehaviour
{
    [Header("Radar Settings")]
    [SerializeField] private float radarRange = 400f;
    [SerializeField] private float highAltitudeThreshold = 50f;
    [SerializeField] private string targetTag = "Player";

    private Transform detectedTarget;
    private bool isTargetFlagged = false;

    private void Start()
    {
        InvokeRepeating(nameof(ScanForTargets), 0f, 0.2f);
    }

    private void ScanForTargets()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag(targetTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestPlayer = null;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestPlayer = player;
            }
        }

        if (nearestPlayer != null && shortestDistance <= radarRange)
        {
            detectedTarget = nearestPlayer.transform;
            isTargetFlagged = detectedTarget.position.y >= highAltitudeThreshold;
        }
        else
        {
            detectedTarget = null;
            isTargetFlagged = false;
        }
    }

    public bool IsTargetFlagged()
    {
        return isTargetFlagged;
    }

    public Transform GetDetectedTarget()
    {
        return detectedTarget;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radarRange);
    }
}