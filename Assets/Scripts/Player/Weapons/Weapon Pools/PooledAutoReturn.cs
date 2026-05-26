using UnityEngine;

public class PooledAutoReturn : MonoBehaviour, IPoolable
{
    [SerializeField] private float returnDelay = 1f;

    private ObjectPool ownerPool;
    private float timer;
    private bool isActive;

    public void Initialize(ObjectPool pool)
    {
        ownerPool = pool;
        timer = returnDelay;
        isActive = true;
    }

    public void OnGetFromPool()
    {
        timer = returnDelay;
        isActive = true;

        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem particle in particles)
        {
            particle.Clear();
            particle.Play();
        }
    }

    public void OnReturnToPool()
    {
        isActive = false;

        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();

        foreach (ParticleSystem particle in particles)
        {
            particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    void Update()
    {
        if (!isActive)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            if (ownerPool != null)
            {
                ownerPool.Return(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}