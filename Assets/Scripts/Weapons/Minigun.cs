using UnityEngine;
using UnityEngine.InputSystem;

public class Minigun : MonoBehaviour
{
    [Header("References")]
    public Transform[] firePoints;
    public GameObject bulletPrefab;


    [Header("Stats")]
    public float fireRate = 0.1f;
    private float nextFireTime;


    [Header("Overheat System")]
    public float heatPerSecond = 1f;
    public float maxHeat = 5f;
    public float cooldownRate = 1.5f;

    private float currentHeat = 0f;
    private bool isOverheated = false;


    void Update()
    {
        HandleOverheat();
        HandleFire();
    }


    void HandleFire()
    {
        if (isOverheated)
            return;

        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
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
    foreach (Transform fp in firePoints)
        {
            Instantiate(bulletPrefab, fp.position, fp.rotation);
        }
    }


    void HandleOverheat()
    {
        if (isOverheated)
        {
            currentHeat -= cooldownRate * Time.deltaTime;

            if (currentHeat <= 0f)
            {
                currentHeat = 0f;
                isOverheated = false;
            }

        return;
        }

        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            currentHeat += heatPerSecond * Time.deltaTime;

            if (currentHeat >= maxHeat)
            {
                currentHeat = maxHeat;
                isOverheated = true;
            }
        }
        
        else
        {
            currentHeat -= cooldownRate * Time.deltaTime;
            currentHeat = Mathf.Max(currentHeat, 0f);
        }
    }
}