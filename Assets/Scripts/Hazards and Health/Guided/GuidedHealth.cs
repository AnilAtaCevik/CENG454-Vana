using UnityEngine;

public class GuidedHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 50f;
    private float currentHealth;

    [Header("Effects")]
    [SerializeField] private GameObject explosionPrefab;

    [Header("Loot Settings")]
    [SerializeField] private GameObject fuelCanPrefab;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Guided Launcher Health: " + currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, transform.rotation);
        }

        if (fuelCanPrefab != null)
        {
            Instantiate(fuelCanPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}