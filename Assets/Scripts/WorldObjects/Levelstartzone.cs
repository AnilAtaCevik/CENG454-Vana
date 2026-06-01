using UnityEngine;

/// <summary>
/// Marker zone for the START of a level. Place this where the player begins.
/// Two jobs:
///   1) Gives the minimap an  anchor for the start of the level
///      (the LinearMinimapUI finds it).
///   2) Optionally shows a message when the level loads.
/// This is intentionally lightweight — it does NOT clear checkpoint data,
/// because reloads from a checkpoint death are also "level loads" and we
/// don't want to wipe progress. LevelCompleteZone is the one that clears.
/// </summary>
[RequireComponent(typeof(Collider))]
public class LevelStartZone : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Optional message shown when the level begins. Leave empty to skip.")]
    [SerializeField] private string briefingMessage = "";
    [Tooltip("How long (seconds) to wait before showing the message.")]
    [SerializeField] private float messageDelay = 1f;

    void Reset()
    {
        // Make the collider a trigger by default.
        var col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true;
    }

    void Start()
    {
        if (!string.IsNullOrEmpty(briefingMessage))
            Invoke(nameof(ShowBriefing), messageDelay);
    }

    void ShowBriefing()
    {
        GameEvents.RaiseFeedback(briefingMessage, FeedbackSeverity.Info);
    }
}