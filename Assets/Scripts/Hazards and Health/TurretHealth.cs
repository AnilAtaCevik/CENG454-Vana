using UnityEngine;

public class TurretHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 50f;
    private float currentHealth;

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
            Destroy(gameObject); 
        }
    }
}