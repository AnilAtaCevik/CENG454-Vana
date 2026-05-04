using UnityEngine;
using UnityEngine.InputSystem;

public class MissileSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform[] firePoints;

    [Header("Shooting")]
    [SerializeField] private float fireCooldown = 1.5f;

    private float nextFireTime = 0f;


    void Update()
    {
        HandleFire();
    }


    void HandleFire()
    {
        if (Mouse.current == null)
            return;

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            if (Time.time >= nextFireTime)
            {
                Fire();
                nextFireTime = Time.time + fireCooldown;
            }
        }
    }


    void Fire()
    {
        foreach (Transform fp in firePoints)
        {
            Instantiate(missilePrefab, fp.position, fp.rotation);
        }
    }
}