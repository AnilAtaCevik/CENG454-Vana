using UnityEngine;
using UnityEngine.SceneManagement;

public class HeliHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        GameEvents.RaiseHealthChanged(currentHealth, maxHealth);
    }

    public bool TakeDamageWithResult(float damageAmount)
    {
        if (currentHealth <= 0f) return false;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(currentHealth, 0f);

        Debug.Log("Helikopter hasar aldi! Kalan Can: " + currentHealth);

        GameEvents.RaiseDamageTaken(damageAmount);
        GameEvents.RaiseHealthChanged(currentHealth, maxHealth);

        if (currentHealth <= 0f)
        {
            Die();
            return true;
        }

        return false;
    }

    public void TakeDamage(float damageAmount)
    {
        TakeDamageWithResult(damageAmount);
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHealth);
        GameEvents.RaiseHealthChanged(currentHealth, maxHealth);
    }

    private void Die()
    {
        GameEvents.RaiseHelicopterDestroyed();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}