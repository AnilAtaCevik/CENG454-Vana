/// <summary>
/// Simple static tracker for whether the helicopter is carrying passengers.
/// Set by ExtractionZone (pickup), cleared by DropoffZone (delivery).
/// </summary>
public static class PassengerState
{
    public static bool IsCarryingPassengers = false;
}