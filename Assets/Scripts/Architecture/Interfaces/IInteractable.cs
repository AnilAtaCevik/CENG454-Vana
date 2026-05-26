using UnityEngine;

/// <summary>
/// Contract for world objects the helicopter can interact with
/// by entering and staying in a trigger zone for a set duration.
/// Examples: Extraction Zone, Resupply Station, future capture points.
/// </summary>
public interface IInteractable
{
    /// <summary>
    /// How many seconds the interactor must stay before completion.
    /// Use 0 for instant pickups.
    /// </summary>
    float InteractionDuration { get; }

    /// <summary>
    /// Whether this object can currently be interacted with.
    /// Returns false during cooldown, after one-shot use, etc.
    /// </summary>
    bool IsAvailable { get; }

    /// <summary>Called the moment the interactor enters the trigger.</summary>
    void OnInteractionStart(GameObject interactor);

    /// <summary>Called when the interactor has stayed for InteractionDuration.</summary>
    void OnInteractionComplete(GameObject interactor);

    /// <summary>Called if the interactor leaves before completion.</summary>
    void OnInteractionCancelled(GameObject interactor);
}