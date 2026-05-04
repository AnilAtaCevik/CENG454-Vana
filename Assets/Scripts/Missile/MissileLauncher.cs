using UnityEngine;
using UnityEngine.InputSystem;

public class MissileLauncher : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private Transform firePoint;

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
        Instantiate(missilePrefab, firePoint.position, firePoint.rotation);
    }
}