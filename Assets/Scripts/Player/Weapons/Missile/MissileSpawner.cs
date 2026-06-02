using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class MissileSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform[] firePoints;

    [Header("Aiming")]
    [SerializeField] private LineRenderer aimLine;
    [SerializeField] private float maxAimTime = 3f;
    [SerializeField] private float aimDistance = 100f;

    [Header("Cooldown")]
    [SerializeField] private float fireCooldown = 5f;

    [Header("Ammo")]
    [SerializeField] private int maxAmmo = 6;

    [Header("Launch Sequence")]
    [SerializeField] private float delayBetweenMissiles = 0.15f;

    [Header("Pools")]
    [SerializeField] private ObjectPool missilePool;
    [SerializeField] private ObjectPool missileExplosionVfxPool;
    [SerializeField] private ObjectPool missileLaunchAudioPool;

    private int currentAmmo;
    private bool isAiming = false;
    private float aimTimer = 0f;
    private float nextFireTime = 0f;

    void OnEnable()
    {
        GameEvents.OnResupplyRequested += HandleResupplyRequested;
    }

    void OnDisable()
    {
        GameEvents.OnResupplyRequested -= HandleResupplyRequested;
    }

    void Start()
    {
        currentAmmo = maxAmmo;
        WeaponEvents.RaiseMissileAmmoChanged(currentAmmo, maxAmmo);
    }

    void Update()
    {
        HandleInput();

        if (isAiming)
        {
            UpdateAimLine();
        }
    }

    void HandleInput()
    {
        if (Mouse.current == null)
            return;

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            if (CanStartAiming())
            {
                StartAiming();
            }
        }

        if (isAiming)
        {
            aimTimer += Time.deltaTime;

            if (aimTimer >= maxAimTime)
            {
                FireMissiles();
            }
        }

        if (Mouse.current.rightButton.wasReleasedThisFrame)
        {
            if (isAiming)
            {
                FireMissiles();
            }
        }
    }

    bool CanStartAiming()
    {
        if (Time.time < nextFireTime)
        {
            GameEvents.RaiseFeedback(
                "Missile system cooling down!",
                FeedbackSeverity.Warning
            );

            return false;
        }

        if (currentAmmo < 2)
        {
            GameEvents.RaiseFeedback(
                "No missiles remaining!",
                FeedbackSeverity.Warning
            );

            return false;
        }

        return true;
    }

    void StartAiming()
    {
        isAiming = true;
        aimTimer = 0f;

        if (aimLine != null)
        {
            aimLine.enabled = true;
        }
    }

    void UpdateAimLine()
    {
        if (aimLine == null || firePoints == null || firePoints.Length == 0)
            return;

        Vector3 startPos = firePoints[0].position;
        Vector3 endPos = firePoints[0].position + firePoints[0].forward * aimDistance;

        aimLine.SetPosition(0, startPos);
        aimLine.SetPosition(1, endPos);
    }

    void FireMissiles()
    {
        isAiming = false;
        currentAmmo -= 2;

        WeaponEvents.RaiseMissileAmmoChanged(currentAmmo, maxAmmo);
        WeaponEvents.RaiseMissileFired();
        WeaponEvents.RaiseMissileCooldownStarted(fireCooldown);

        nextFireTime = Time.time + fireCooldown;

        if (aimLine != null)
        {
            aimLine.enabled = false;
        }

        StartCoroutine(FireSequence());
        StartCoroutine(MissileCooldownRoutine());
    }

    IEnumerator FireSequence()
    {
        if (firePoints == null || firePoints.Length == 0)
            yield break;

        SpawnMissile(firePoints[0]);

        yield return new WaitForSeconds(delayBetweenMissiles);

        if (firePoints.Length > 1)
        {
            SpawnMissile(firePoints[1]);
        }
    }

    IEnumerator MissileCooldownRoutine()
    {
        yield return new WaitForSeconds(fireCooldown);

        WeaponEvents.RaiseMissileCooldownFinished();
    }

    void SpawnMissile(Transform firePoint)
    {
        if (missilePool == null || firePoint == null)
            return;

        GameObject missile = missilePool.Get();

        if (missile == null)
            return;

        missile.transform.position = firePoint.position;
        missile.transform.rotation = firePoint.rotation;

        Missile missileScript = missile.GetComponent<Missile>();

        if (missileScript != null)
        {
            missileScript.Initialize(
                missilePool,
                missileExplosionVfxPool,
                missileLaunchAudioPool
            );
        }
    }

    void HandleResupplyRequested()
    {
        currentAmmo = maxAmmo;
        nextFireTime = 0f;

        if (aimLine != null)
        {
            aimLine.enabled = false;
        }

        isAiming = false;

        WeaponEvents.RaiseMissileAmmoChanged(currentAmmo, maxAmmo);
        WeaponEvents.RaiseMissileCooldownFinished();

        GameEvents.RaiseFeedback(
            "Missiles reloaded!",
            FeedbackSeverity.Info
        );
    }
}
