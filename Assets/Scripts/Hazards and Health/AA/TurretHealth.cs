using UnityEngine;

public class TurretHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 50f;
    private float currentHealth;

    [Header("Effects")]
    [SerializeField] private GameObject explosionPrefab;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Turret Health: " + currentHealth);

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

        Destroy(gameObject);
    }
}