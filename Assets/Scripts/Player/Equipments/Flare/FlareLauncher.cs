using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class FlareLauncher : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject flarePrefab;
    [SerializeField] private Transform flareSpawnPoint;

    [Header("Deploy Pattern")]
    [SerializeField] private int flareCountPerUse = 4;
    [SerializeField] private float spreadRadius = 1.5f;
    [SerializeField] private float delayBetweenFlares = 0.12f;

    [Header("Ammo")]
    [SerializeField] private int maxCharges = 3;

    [Header("Cooldown")]
    [SerializeField] private float deployCooldown = 5f;

    [Header("Audio")]
    [SerializeField] private AudioClip deployClip;
    [SerializeField] private float deployVolume = 1f;

    private int currentCharges;
    private float nextDeployTime = 0f;
    private bool isDeploying = false;

    void Start()
    {
        currentCharges = maxCharges;
    }

    void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            TryDeployFlare();
        }
    }

    void TryDeployFlare()
    {
        if (isDeploying)
            return;

        if (Time.time < nextDeployTime)
            return;

        if (currentCharges <= 0)
            return;

        currentCharges--;
        nextDeployTime = Time.time + deployCooldown;

        StartCoroutine(DeployFlareBurst());
    }

    IEnumerator DeployFlareBurst()
    {
        isDeploying = true;

        for (int i = 0; i < flareCountPerUse; i++)
        {
            SpawnSingleFlare();

            if (deployClip != null && flareSpawnPoint != null)
            {
                AudioSource.PlayClipAtPoint(
                    deployClip,
                    flareSpawnPoint.position,
                    deployVolume
                );
            }

            yield return new WaitForSeconds(delayBetweenFlares);
        }

        isDeploying = false;
    }

    void SpawnSingleFlare()
    {
        if (flarePrefab == null || flareSpawnPoint == null)
            return;

        Vector3 randomOffset = new Vector3(
            Random.Range(-spreadRadius, spreadRadius),
            Random.Range(-spreadRadius * 0.25f, spreadRadius * 0.25f),
            Random.Range(-spreadRadius, spreadRadius)
        );

        Instantiate(
            flarePrefab,
            flareSpawnPoint.position + randomOffset,
            flareSpawnPoint.rotation
        );
    }
}