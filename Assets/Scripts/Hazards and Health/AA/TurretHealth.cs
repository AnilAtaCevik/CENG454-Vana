using UnityEngine;

public class TurretHealth : MonoBehaviour, IDamageable
{
    public enum HealthState
    {
        Alive,
        Dead
    }

    [Header("Health State")]
    [SerializeField] private HealthState currentHealthState = HealthState.Alive;

    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 50f;
    private float currentHealth;

    [Header("Effects")]
    [SerializeField] private GameObject explosionPrefab;

    [Header("Loot Settings")]
    [SerializeField] private GameObject fuelCanPrefab;

    private void Start()
    {
        currentHealth = maxHealth;
        currentHealthState = HealthState.Alive;
    }

    public void TakeDamage(float damageAmount)
    {
        if (currentHealthState == HealthState.Dead) return;

        currentHealth -= damageAmount;
        Debug.Log("Turret Health: " + currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        currentHealthState = HealthState.Dead;

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
    
    public bool IsDead()
    {
        return currentHealthState == HealthState.Dead;
    }
}