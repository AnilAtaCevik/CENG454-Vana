using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class HelicopterCrashDetector : MonoBehaviour
{
    [Header("Lower Part Settings (Zirhli)")]
    [SerializeField] private float lowerSpeedThreshold = 12f;
    [SerializeField] private float lowerFixedDamage = 20f;

    [Header("Upper Part Settings (Hassas)")]
    [SerializeField] private float upperSpeedThreshold = 6f;
    [SerializeField] private float upperFixedDamage = 40f;

    [Header("General Settings")]
    [SerializeField] private float resetDelay = 1.5f;
    [SerializeField] private GameObject explosionPrefab;

    private bool isDead = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;

        if (collision.collider.CompareTag("MovingObstacle"))
        {
            TriggerDeathEffects();
            return;
        }

        float impactMagnitude = collision.relativeVelocity.magnitude;
        Debug.Log("Carpma Siddeti: " + impactMagnitude);

        Vector3 contactPoint = transform.InverseTransformPoint(collision.contacts[0].point);

        if (contactPoint.y > 0)
        {
            if (impactMagnitude >= upperSpeedThreshold)
            {
                ApplyFixedDamage(upperFixedDamage);
            }
        }
        else
        {
            if (impactMagnitude >= lowerSpeedThreshold)
            {
                ApplyFixedDamage(lowerFixedDamage);
            }
        }
    }

    private void ApplyFixedDamage(float damageAmount)
    {
        HeliHealth healthScript = GetComponent<HeliHealth>();
        
        if (healthScript != null)
        {
            bool deathToggled = healthScript.TakeDamageWithResult(damageAmount);

            if (deathToggled)
            {
                TriggerDeathEffects();
            }
        }
    }

    private void TriggerDeathEffects()
    {
        isDead = true;

        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, transform.rotation);
        }

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            r.enabled = false;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        StartCoroutine(ResetSceneRoutine());
    }

    private IEnumerator ResetSceneRoutine()
    {
        yield return new WaitForSeconds(resetDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}