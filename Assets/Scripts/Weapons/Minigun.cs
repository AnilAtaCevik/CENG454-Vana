using UnityEngine;

public class Minigun : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;
    public GameObject bulletPrefab;

    [Header("Stats")]
    public float fireRate = 0.1f;
    private float nextFireTime;


    void Update()
    {
        HandleFire();
    }


    void HandleFire()
    {
        if (Input.GetMouseButton(0))
        {
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }


    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}