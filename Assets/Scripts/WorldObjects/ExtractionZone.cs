using UnityEngine;

/// <summary>
/// Landing zone for picking up passengers. Helicopter lands for 5 seconds,
/// passengers board, objective completes. Sets PassengerState so DropoffZone
/// becomes available.
/// </summary>
public class ExtractionZone : LandingZone, IInteractable
{
    [Header("Pickup Settings")]
    [SerializeField] private string pickupObjective = "Pick up passengers";

    void Awake()
    {
        interactionLabel = "PICKING UP...";
        oneTimeUse = true;
        PassengerState.IsCarryingPassengers = false;
    }

    protected override void OnLandingComplete()
    {
        Debug.Log("[ExtractionZone] Passengers picked up!");
        PassengerState.IsCarryingPassengers = true;
        GameEvents.RaiseObjectiveCompleted(pickupObjective);
        GameEvents.RaiseFeedback("PASSENGERS ABOARD", FeedbackSeverity.Info);
    }
}