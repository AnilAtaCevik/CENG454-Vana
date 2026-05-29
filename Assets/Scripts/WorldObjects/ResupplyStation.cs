using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Landing platform that restores resources (health, fuel, ammo, flares)
/// after the helicopter lands. Reusable - player can re-enter after exiting.
///
/// Resupply amounts are wired through the Inspector via onResupplyComplete
/// UnityEvent (drag  HeliHealth.Heal, Fuel.Refill, etc.)
/// </summary>
public class ResupplyStation : LandingZone
{
    [Header("Resupply Actions")]
    [Tooltip("Wire teammate's HeliHealth.Heal, Fuel.Refill, weapon ammo restore here in Inspector")]
    [SerializeField] private UnityEvent onResupplyComplete;

    void Awake()
    {
        interactionLabel = "RESUPPLYING...";
        oneTimeUse = false;
    }

    protected override void OnLandingComplete()
    {
        Debug.Log("[ResupplyStation] Resupply complete!");
        GameEvents.RaiseFeedback("RESUPPLIED", FeedbackSeverity.Info);
        onResupplyComplete?.Invoke();
    }
}