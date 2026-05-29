using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GuidedMissile : MonoBehaviour
{
    
    
    [SerializeField] private GameObject explosionVfxPrefab;
    [SerializeField] private float speed = 15f;
    [SerializeField] private float damage = 25f;
    [SerializeField] private AudioClip normalHitSound;
    [SerializeField] private AudioClip fatalDeathSound;
    [Range(0f, 1f)] [SerializeField] private float normalHitVolume = 0.5f;
    [Range(0f, 1f)] [SerializeField] private float fatalDeathVolume = 1.0f;
    
    private Transform target;
    private Rigidbody rb;
    private bool isExploded;
    
    private Renderer[] meshRenderers;
    private Collider[] colliders;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderers = GetComponentsInChildren<Renderer>();
        colliders = GetComponentsInChildren<Collider>();
    }

    private void OnEnable()
    {
        isExploded = false;
        
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        ToggleVisibility(true);
    }

    private void FixedUpdate()
    {
        if (target == null || isExploded) return;

        Vector3 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;
        
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isExploded) return;

        if (other.CompareTag("Player"))
        {
            isExploded = true;
            bool killedThePlayer = false;

            HeliHealth healthScript = other.GetComponent<HeliHealth>();
            if (healthScript != null)
            {
                killedThePlayer = healthScript.TakeDamageWithResult(damage);
            }
            else
            {
                IDamageable damageable = other.GetComponent<IDamageable>();
                damageable?.TakeDamage(damage);
            }

            ToggleVisibility(false);
            
            if (killedThePlayer)
            {
                StartCoroutine(FatalExplosionCoroutine());
            }
            else
            {
                Explode(false);
                gameObject.SetActive(false);
            }
        }
    }

    private void Explode(bool fatalBlow)
    {
        if (explosionVfxPrefab != null)
        {
            Instantiate(
                explosionVfxPrefab,
                transform.position,
                Quaternion.identity
            );
        }

        AudioClip soundToPlay = fatalBlow ? fatalDeathSound : normalHitSound;
        float volumeToApply = fatalBlow ? fatalDeathVolume : normalHitVolume;

        if (soundToPlay != null)
        {
            AudioSource.PlayClipAtPoint(
                soundToPlay,
                transform.position,
                volumeToApply
            );
        }
    }
    

    private IEnumerator FatalExplosionCoroutine()
    {
        Explode(true);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        gameObject.SetActive(false);
    }

    private void ToggleVisibility(bool visible)
    {
        if (meshRenderers != null)
        {
            foreach (var r in meshRenderers) r.enabled = visible;
        }
        
        if (colliders != null)
        {
            foreach (var c in colliders) c.enabled = visible;
        }
    }
}