using UnityEngine;

/// <summary>
/// Landing zone for delivering passengers. Only available when helicopter
/// is carrying passengers (PassengerState.IsCarryingPassengers == true).
/// Helicopter lands for 5 seconds, passengers delivered, objective completes.
/// </summary>
public class DropoffZone : LandingZone, IInteractable
{
    [Header("Dropoff Settings")]
    [SerializeField] private string dropoffObjective = "Deliver passengers to safety";

    void Awake()
    {
        interactionLabel = "DROPPING OFF...";
        oneTimeUse = true;
    }

    /// <summary>
    /// Only available when carrying passengers AND not already completed.
    /// </summary>
    public override bool IsAvailable =>
        base.IsAvailable && PassengerState.IsCarryingPassengers;

    protected override void OnLandingComplete()
    {
        Debug.Log("[DropoffZone] Passengers delivered!");
        PassengerState.IsCarryingPassengers = false;
        GameEvents.RaiseObjectiveCompleted(dropoffObjective);
        GameEvents.RaiseFeedback("PASSENGERS DELIVERED", FeedbackSeverity.Info);
    }
}