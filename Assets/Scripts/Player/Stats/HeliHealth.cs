using UnityEngine;
using System.Collections;

public class HeliHealth : MonoBehaviour, IDamageable
{
    public enum HealthState
    {
        Alive,
        Dead
    }

    [Header("Health State")]
    [SerializeField] private HealthState currentHealthState = HealthState.Alive;

    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private GameObject patlamaEfektiPrefab;
    [SerializeField] private float sahneBeklemeSuresi = 2f;

    private float currentHealth;

    private void OnEnable()
    {
        GameEvents.OnResupplyRequested += HealToFull;
    }

    private void OnDisable()
    {
        GameEvents.OnResupplyRequested -= HealToFull;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        currentHealthState = HealthState.Alive;
        GameEvents.RaiseHealthChanged(currentHealth, maxHealth);
    }

    public bool TakeDamageWithResult(float damageAmount)
    {
        if (currentHealthState == HealthState.Dead) return false;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(currentHealth, 0f);

        Debug.Log("Helikopter hasar aldi! Kalan Can: " + currentHealth);

        GameEvents.RaiseDamageTaken(damageAmount);
        GameEvents.RaiseHealthChanged(currentHealth, maxHealth);

        if (currentHealth <= 0f && currentHealthState == HealthState.Alive)
        {
            currentHealthState = HealthState.Dead;
            StartCoroutine(DieWithDelay());
            return true;
        }

        return false;
    }

    public void TakeDamage(float damageAmount)
    {
        if (currentHealthState == HealthState.Dead) return;
        TakeDamageWithResult(damageAmount);
    }

    public void Heal(float amount)
    {
        if (currentHealthState == HealthState.Dead) return;
        currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHealth);
        GameEvents.RaiseHealthChanged(currentHealth, maxHealth);
    }

    public void HealToFull()
    {
        if (currentHealthState == HealthState.Dead) return;
        currentHealth = maxHealth;
        GameEvents.RaiseHealthChanged(currentHealth, maxHealth);
    }

    private IEnumerator DieWithDelay()
    {
        if (patlamaEfektiPrefab != null)
        {
            GameObject instantiatedExplosion = Instantiate(patlamaEfektiPrefab, transform.position, transform.rotation);
            Destroy(instantiatedExplosion, 5f);
        }

        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (var rend in renderers)
        {
            rend.enabled = false;
        }

        if (TryGetComponent<Collider>(out Collider col))
        {
            col.enabled = false;
        }

        if (TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
        }

        yield return new WaitForSeconds(sahneBeklemeSuresi);
        GameEvents.RaiseHelicopterDestroyed();
    }

    public bool IsDead()
    {
        return currentHealthState == HealthState.Dead;
    }
}