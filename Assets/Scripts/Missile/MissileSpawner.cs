using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class MissileSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private Transform[] firePoints;

    [Header("Aiming")]
    [SerializeField] private LineRenderer aimLine;
    [SerializeField] private float maxAimTime = 3f;
    [SerializeField] private float aimDistance = 100f;

    [Header("Cooldown")]
    [SerializeField] private float fireCooldown = 5f;

    [Header("Ammo")]
    [SerializeField] private int maxAmmo = 6;


    private int currentAmmo;

    private bool isAiming = false;

    private float aimTimer = 0f;
    private float nextFireTime = 0f;



    void Start()
    {
        currentAmmo = maxAmmo;
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

        // START AIM
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            if (Time.time >= nextFireTime && currentAmmo >= 2)
            {
                StartAiming();
            }
        }

        // HOLD AIM
        if (isAiming)
        {
            aimTimer += Time.deltaTime;

            // AUTO FIRE AFTER 3 SEC
            if (aimTimer >= maxAimTime)
            {
                FireMissiles();
            }
        }

        // RELEASE FIRE
        if (Mouse.current.rightButton.wasReleasedThisFrame)
        {
            if (isAiming)
            {
                FireMissiles();
            }
        }
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
        if (aimLine == null || firePoints.Length == 0)
            return;

        Vector3 startPos = firePoints[0].position;
        Vector3 endPos =
            firePoints[0].position +
            firePoints[0].forward * aimDistance;

        aimLine.SetPosition(0, startPos);
        aimLine.SetPosition(1, endPos);
    }


    void FireMissiles()
    {
        isAiming = false;
        currentAmmo -= 2;

        nextFireTime = Time.time + fireCooldown;

        if (aimLine != null)
        {
            aimLine.enabled = false;
        }

        StartCoroutine(FireSequence());

        Debug.Log("Current Ammo: " + currentAmmo);
    }


    IEnumerator FireSequence()
    {
        // RIGHT
        Instantiate(
            missilePrefab,
            firePoints[0].position,
            firePoints[0].rotation
        );

        yield return new WaitForSeconds(0.15f);

        // LEFT
        if (firePoints.Length > 1)
        {
            Instantiate(
                missilePrefab,
                firePoints[1].position,
                firePoints[1].rotation
            );
        }
    }
}