using UnityEngine;

public class PooledAudio : MonoBehaviour, IPoolable
{
    [SerializeField] private AudioSource audioSource;

    private ObjectPool ownerPool;
    private bool isActive;

    public void Initialize(ObjectPool pool, AudioClip clip, float volume)
    {
        ownerPool = pool;

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource == null || clip == null)
            return;

        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();

        isActive = true;
    }

    public void OnGetFromPool()
    {
        isActive = false;

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void OnReturnToPool()
    {
        isActive = false;

        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.clip = null;
        }
    }

    void Update()
    {
        if (!isActive || audioSource == null)
            return;

        if (!audioSource.isPlaying)
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