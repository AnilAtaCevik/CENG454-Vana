using UnityEngine;

/// <summary>
/// Landing platform that restores resources (health, fuel, ammo, flares)
/// after the helicopter lands. Reusable - player can re-enter after exiting.
///
/// Communicates purely through GameEvents: fires OnResupplyRequested and
/// every system that needs topping up (HeliHealth, Fuel, weapon/equipment
/// scripts) listens for it and restores itself, then broadcasts its own
/// state change so the HUD updates automatically.
/// </summary>
public class ResupplyStation : LandingZone
{
    void Awake()
    {
        interactionLabel = "RESUPPLYING...";
        oneTimeUse = false;
    }

    protected override void OnLandingComplete()
    {
        Debug.Log("[ResupplyStation] Resupply complete!");

        // One event -> health, fuel, weapons and equipment all restore themselves.
        GameEvents.RaiseResupplyRequested();

        GameEvents.RaiseFeedback("RESUPPLIED", FeedbackSeverity.Info);
    }
}