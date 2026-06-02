using UnityEngine;
using UnityEngine.InputSystem;

public class Minigun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform[] firePoints;
    [SerializeField] private ObjectPool bulletPool;

    [Header("Shooting")]
    [SerializeField] private float fireRate = 0.05f;
    [SerializeField] private float spinUpDelay = 0.95f;
    [SerializeField] private ParticleSystem[] muzzleFlashes;

    [Header("Timing")]
    [SerializeField] private float firingDuration = 5f;
    [SerializeField] private float cooldownTime = 5f;

    [Header("Impact Pools")]
    [SerializeField] private ObjectPool impactVfxPool;
    [SerializeField] private ObjectPool enemyImpactVfxPool;
    [SerializeField] private ObjectPool impactAudioPool;
    [SerializeField] private ObjectPool enemyImpactAudioPool;

    [Header("Audio")]
    [SerializeField] private AudioSource firingAudio;
    [SerializeField] private AudioSource overheatAudio;

    private IMinigunState currentState;

    private float stateTimer;
    private float fireStartTime;
    private float nextFireTime;

    void Start()
    {
        ChangeState(new MinigunIdleState());
    }

    void Update()
    {
        currentState?.Update(this);
    }

    public void ChangeState(IMinigunState newState)
    {
        currentState?.Exit(this);
        currentState = newState;
        currentState.Enter(this);
    }

    public bool IsFirePressed()
    {
        return Mouse.current != null && Mouse.current.leftButton.isPressed;
    }

    public void TickStateTimer()
    {
        stateTimer -= Time.deltaTime;
    }

    public bool IsStateTimerFinished()
    {
        return stateTimer <= 0f;
    }

    public void ResetStateTimer()
    {
        stateTimer = 0f;
    }

    public void StartFiringState()
    {
        StopAllAudio();

        firingAudio?.Play();

        stateTimer = firingDuration;
        fireStartTime = Time.time + spinUpDelay;
        nextFireTime = fireStartTime;
    }

    public void StartOverheatedState()
    {
        StopAllAudio();

        overheatAudio?.Play();

        stateTimer =
            overheatAudio != null
            ? overheatAudio.clip.length
            : 1.5f;

        WeaponEvents.RaiseMinigunOverheated();
    }

    public void StartCoolingState()
    {
        StopAllAudio();
        stateTimer = cooldownTime;
    }

    public void HandleFire()
    {
        if (!IsFirePressed())
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
            GameObject bullet = bulletPool.Get();

            if (bullet == null)
                return;

            bullet.transform.position = firePoints[i].position;
            bullet.transform.rotation = Quaternion.identity;

            Bullet bulletScript = bullet.GetComponent<Bullet>();

            if (bulletScript != null)
            {
                bulletScript.Initialize(
                    firePoints[i].forward,
                    bulletPool,
                    impactVfxPool,
                    enemyImpactVfxPool,
                    impactAudioPool,
                    enemyImpactAudioPool
                );
            }

            if (muzzleFlashes != null && i < muzzleFlashes.Length)
            {
                muzzleFlashes[i].Play();
            }
        }
    }

    public void StopAllAudio()
    {
        firingAudio?.Stop();
        overheatAudio?.Stop();
    }
}