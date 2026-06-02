using UnityEngine;
using System.Collections;

public class HeliHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private GameObject patlamaEfektiPrefab;
    [SerializeField] private float sahneBeklemeSuresi = 2f;

    private float currentHealth;
    private bool isDead = false;

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
        GameEvents.RaiseHealthChanged(currentHealth, maxHealth);
    }

    public bool TakeDamageWithResult(float damageAmount)
    {
        if (isDead) return false;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(currentHealth, 0f);

        Debug.Log("Helikopter hasar aldi! Kalan Can: " + currentHealth);

        GameEvents.RaiseDamageTaken(damageAmount);
        GameEvents.RaiseHealthChanged(currentHealth, maxHealth);

        if (currentHealth <= 0f && !isDead)
        {
            isDead = true;
            StartCoroutine(DieWithDelay());
            return true;
        }

        return false;
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;
        TakeDamageWithResult(damageAmount);
    }

    public void Heal(float amount)
    {
        if (isDead) return;
        currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHealth);
        GameEvents.RaiseHealthChanged(currentHealth, maxHealth);
    }

    /// <summary>Restores health to maximum. Called by the resupply event.</summary>
    public void HealToFull()
    {
        if (isDead) return;
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
}