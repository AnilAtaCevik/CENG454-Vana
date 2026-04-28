using UnityEngine;

public class HelicopterHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    void Start()
    {
        // Oyun başında canı fulle
        currentHealth = maxHealth;
    }

    // Dışarıdan mermiler ve füzeler bu fonksiyonu çağıracak
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log($"Helikopter Vuruldu! Alınan Hasar: {damageAmount} | Kalan Can: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("HELİKOPTER DÜŞTÜ! GÖREV BAŞARISIZ.");
        Destroy(gameObject);
    }
}
