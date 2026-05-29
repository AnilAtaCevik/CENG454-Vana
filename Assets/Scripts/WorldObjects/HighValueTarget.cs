using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Stationary or mobile target that must be destroyed by helicopter weapons.
/// Implements IDamageable so teammate's bullets/missiles can deal damage.
/// </summary>
public class HighValueTarget : MonoBehaviour, IDamageable
{
    [Header("HVT Settings")]
    [SerializeField] private float maxHealth = 200f;

    [Tooltip("Objective text shown in HUD for this target")]
    [SerializeField] private string objectiveText = "Destroy High Value Target";

    [Header("Destruction Effects")]
    [Tooltip("Optional: VFX prefab spawned at HVT position when destroyed")]
    [SerializeField] private GameObject destructionVFX;

    [Tooltip("Optional: Sound played at HVT position when destroyed")]
    [SerializeField] private AudioClip destructionSFX;

    [Header("On Destroyed")]
    [Tooltip("Optional: wire additional Inspector actions on destruction")]
    [SerializeField] private UnityEvent onDestroyed;

    private float currentHealth;
    private bool destroyed = false;

    void Start()
    {
        currentHealth = maxHealth;
        GameEvents.RaiseObjectiveAdded(objectiveText);
    }

    /// Called by teammate's bullets/missiles via IDamageable interface.
    public void TakeDamage(float amount)
    {
        if (destroyed) return;

        currentHealth = Mathf.Max(0f, currentHealth - amount);
        Debug.Log($"[HVT] Took {amount} damage. Health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0f)
            Eliminate();
    }

    private void Eliminate()
    {
        destroyed = true;
        Debug.Log("[HVT] Eliminated!");

        GameEvents.RaiseFeedback("TARGET ELIMINATED", FeedbackSeverity.Info);
        GameEvents.RaiseObjectiveCompleted(objectiveText);

        if (destructionVFX != null)
            Instantiate(destructionVFX, transform.position, Quaternion.identity);

        if (destructionSFX != null)
            AudioSource.PlayClipAtPoint(destructionSFX, transform.position);

        onDestroyed?.Invoke();
        gameObject.SetActive(false);
    }
}