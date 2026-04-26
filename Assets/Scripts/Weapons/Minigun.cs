using UnityEngine;
using UnityEngine.InputSystem;

public class Minigun : MonoBehaviour
{
    public enum MinigunState
    {
        Idle,
        SpinningUp,
        Firing,
        Overheated,
        Cooling
    }

    [Header("References")]
    [SerializeField] private Transform[] firePoints;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Shooting")]
    [SerializeField] private float fireRate = 0.1f;
    private float nextFireTime;

    [Header("Timing")]
    [SerializeField] private float spinUpTime = 2.25f;
    [SerializeField] private float firingDuration = 5.3f;
    [SerializeField] private float cooldownTime = 3f;

    [Header("Debug")]
    [SerializeField] private MinigunState currentState = MinigunState.Idle;

    private float stateTimer = 0f;


    void Update()
    {
        HandleState();
    }


    void HandleState()
    {
        switch (currentState)
        {
            case MinigunState.Idle:
                HandleIdle();
                break;

            case MinigunState.SpinningUp:
                HandleSpinningUp();
                break;

            case MinigunState.Firing:
                HandleFiring();
                break;

            case MinigunState.Overheated:
                HandleOverheated();
                break;

            case MinigunState.Cooling:
                HandleCooling();
                break;
        }
    }


    void HandleIdle()
    {
        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            currentState = MinigunState.SpinningUp;
            stateTimer = spinUpTime;
        }
    }


    void HandleSpinningUp()
    {
        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0f)
        {
            currentState = MinigunState.Firing;
            stateTimer = firingDuration;
        }
    }


    void HandleFiring()
    {
        stateTimer -= Time.deltaTime;

        HandleFire();

        if (stateTimer <= 0f)
        {
            currentState = MinigunState.Overheated;
        }
    }


    void HandleOverheated()
    {
        currentState = MinigunState.Cooling;
        stateTimer = cooldownTime;
    }


    void HandleCooling()
    {
        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0f)
        {
            currentState = MinigunState.Idle;
        }
    }


    void HandleFire()
    {
        if (Mouse.current == null || !Mouse.current.leftButton.isPressed)
            return;

        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }


    void Shoot()
    {
        foreach (Transform fp in firePoints)
        {
            Instantiate(bulletPrefab, fp.position, fp.rotation);
        }
    }
}