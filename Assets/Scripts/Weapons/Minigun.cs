using UnityEngine;
using UnityEngine.InputSystem;

public class Minigun : MonoBehaviour
{
    public enum MinigunState
    {
        Idle,
        Firing,
        Overheated,
        Cooling
    }

    [Header("References")]
    [SerializeField] private Transform[] firePoints;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Shooting")]
    [SerializeField] private float fireRate = 0.05f;
    [SerializeField] private float spinUpDelay = 0.918f;
    [SerializeField] private ParticleSystem[] muzzleFlashes;

    [Header("Timing")]
    [SerializeField] private float firingDuration = 5f; 
    [SerializeField] private float cooldownTime = 5f;

    [Header("Audio")]
    [SerializeField] private AudioSource firingAudio; 
    [SerializeField] private AudioSource overheatAudio;

    private MinigunState currentState = MinigunState.Idle;
    private float stateTimer;
    private float fireStartTime;
    private float nextFireTime;


    void Update()
    {
        HandleInput();
        HandleState();
    }


    void HandleInput()
    {
        if (Mouse.current == null) return;
        bool isPressed = Mouse.current.leftButton.isPressed;

        if (isPressed)
        {
            if (currentState == MinigunState.Idle)
                EnterState(MinigunState.Firing);
        }
        
        else
        {
            if (currentState == MinigunState.Firing)
                EnterState(MinigunState.Idle);
        }
    }


    void HandleState()
    {
        stateTimer -= Time.deltaTime;

        switch (currentState)
        {
            case MinigunState.Firing:
                HandleFire();

                if (stateTimer <= 0f)
                    EnterState(MinigunState.Overheated);
                break;

            case MinigunState.Overheated:
                if (stateTimer <= 0f)
                    EnterState(MinigunState.Cooling);
                break;

            case MinigunState.Cooling:
                if (stateTimer <= 0f)
                    EnterState(MinigunState.Idle);
                break;
        }
    }


    void EnterState(MinigunState newState)
    {
        currentState = newState;

        StopAllAudio();

        switch (newState)
        {
            case MinigunState.Idle:
                stateTimer = 0f;
                break;

            case MinigunState.Firing:
                firingAudio?.Play();

                stateTimer = firingDuration;

                fireStartTime = Time.time + spinUpDelay;
                nextFireTime = fireStartTime;
                break;

            case MinigunState.Overheated:
                overheatAudio?.Play();
                stateTimer = overheatAudio != null ? overheatAudio.clip.length : 1.5f;
                break;

            case MinigunState.Cooling:
                stateTimer = cooldownTime;
                break;
        }
    }


    void HandleFire()
    {
        if (Mouse.current == null || !Mouse.current.leftButton.isPressed)
            return;

        if (Time.time < fireStartTime)
            return;

        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }


    void Shoot()
    {
        for (int i = 0; i < firePoints.Length; i++)
        {
            GameObject bullet = Instantiate(
                bulletPrefab,
                firePoints[i].position,
                Quaternion.identity
            );

            Bullet bulletScript = bullet.GetComponent<Bullet>();

            if (bulletScript != null)
            {
                bulletScript.SetDirection(firePoints[i].forward);
            }

            if (muzzleFlashes != null && i < muzzleFlashes.Length)
            {
                muzzleFlashes[i].Play();
            }
        }
    }


    void StopAllAudio()
    {
        firingAudio?.Stop();
        overheatAudio?.Stop();
    }
}