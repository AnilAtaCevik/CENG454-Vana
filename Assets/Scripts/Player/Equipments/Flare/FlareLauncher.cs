using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class FlareLauncher : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform flareSpawnPoint;
    [SerializeField] private Movement helicopterMovement;
    [SerializeField] private ObjectPool flarePool;
    [SerializeField] private ObjectPool flareDeployAudioPool;

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

        EquipmentEvents.RaiseFlareAmmoChanged(currentCharges, maxCharges);
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
        { 
            GameEvents.RaiseFeedback(
                "Flare system cooling down!",
                FeedbackSeverity.Warning
            );

            return;
        }

        if (currentCharges <= 0)
        {
            GameEvents.RaiseFeedback(
                "No flares remaining!",
                FeedbackSeverity.Warning
            );

            return;
        }

        currentCharges--;
        nextDeployTime = Time.time + deployCooldown;

        EquipmentEvents.RaiseFlareAmmoChanged(currentCharges, maxCharges);
        EquipmentEvents.RaiseFlareUsed();
        EquipmentEvents.RaiseFlareCooldownStarted(deployCooldown);

        StartCoroutine(DeployFlareBurst());
    }

    IEnumerator DeployFlareBurst()
    {
        isDeploying = true;

        for (int i = 0; i < flareCountPerUse; i++)
        {
            SpawnSingleFlare();

            PlayDeployAudio();

            yield return new WaitForSeconds(delayBetweenFlares);
        }

        isDeploying = false;
    }

    void SpawnSingleFlare()
    {
        if (flarePool == null || flareSpawnPoint == null)
            return;

        Vector3 randomOffset = new Vector3(
            Random.Range(-spreadRadius, spreadRadius),
            Random.Range(-spreadRadius * 0.25f, spreadRadius * 0.25f),
            Random.Range(-spreadRadius, spreadRadius)
        );

        GameObject flare = flarePool.Get();

        if (flare == null)
            return;

        flare.transform.position =
            flareSpawnPoint.position + randomOffset;

        flare.transform.rotation =
            flareSpawnPoint.rotation;

        Flare flareScript =
            flare.GetComponent<Flare>();

        if (flareScript != null)
        {
            flareScript.Initialize(
                GetHelicopterMoveDirection(),
                flarePool
            );
        }
    }

    Vector3 GetHelicopterMoveDirection()
    {
        if (helicopterMovement != null &&
            helicopterMovement.rb != null &&
            helicopterMovement.rb.linearVelocity.sqrMagnitude > 0.1f)
        {
            Vector3 velocityDirection = helicopterMovement.rb.linearVelocity.normalized;
            velocityDirection.y = 0f;

            if (velocityDirection.sqrMagnitude > 0.01f)
            {
                return velocityDirection.normalized;
            }
        }

        Vector3 fallbackDirection = transform.forward;
        fallbackDirection.y = 0f;

        if (fallbackDirection.sqrMagnitude > 0.01f)
        {
            return fallbackDirection.normalized;
        }

        return Vector3.forward;
    }

    void PlayDeployAudio()
    {
        if (flareDeployAudioPool == null || deployClip == null || flareSpawnPoint == null)
            return;

        GameObject audioObj = flareDeployAudioPool.Get();

        if (audioObj == null)
            return;

        audioObj.transform.position = flareSpawnPoint.position;

        PooledAudio pooledAudio = audioObj.GetComponent<PooledAudio>();

        if (pooledAudio != null)
        {
            pooledAudio.Initialize(
                flareDeployAudioPool,
                deployClip,
                deployVolume
            );
        }
    }
}