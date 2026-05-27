using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class HelicopterCrashDetector : MonoBehaviour
{
    [Header("Crash Settings")]
    [SerializeField] private float crashForceThreshold = 10f;
    [SerializeField] private float resetDelay = 1.5f;

    [Header("Effects")]
    [SerializeField] private GameObject explosionPrefab;

    private bool hasCrashed = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (hasCrashed) return;

        float impactForce = collision.relativeVelocity.magnitude;
        Debug.Log("Carpma Siddeti: " + impactForce);

        if (impactForce >= crashForceThreshold)
        {
            StartCoroutine(CrashRoutine());
        }
    }

    private IEnumerator CrashRoutine()
    {
        hasCrashed = true;

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

        yield return new WaitForSeconds(resetDelay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}