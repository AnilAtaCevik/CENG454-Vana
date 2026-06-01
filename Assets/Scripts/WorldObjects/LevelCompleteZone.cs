using UnityEngine;

/// <summary>
/// Instant trigger — when the helicopter flies over this zone, the level is
/// complete. No landing required.
///
/// This script intentionally does NOT know what scene comes next or how the
/// level sequence is structured. It only does the three things it is
/// uniquely responsible for:
///   1. Clears checkpoint state so the next level starts fresh.
///   2. Raises the LEVEL COMPLETE feedback message for the HUD.
///   3. Raises the MissionCompleted event.
///
/// Whoever cares — LevelSequence (loads next scene via LoadingScreen),
/// mission UI, audio cues, anything else — subscribes to MissionCompleted
/// and reacts independently. No direct dependencies between this trigger
/// and any other system.
/// </summary>
[RequireComponent(typeof(Collider))]
public class LevelCompleteZone : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string completionMessage = "LEVEL COMPLETE!";

    private bool _triggered = false;

    void Reset()
    {
        var col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (_triggered) return;
        if (!other.CompareTag("Player")) return;

        _triggered = true;

        // 1. Clear checkpoints so the next level starts fresh.
        CheckpointManager.ClearAll();

        // 2. Tell the world: this level is done. Anyone who cares reacts.
        GameEvents.RaiseFeedback(completionMessage, FeedbackSeverity.Info);
        GameEvents.RaiseMissionCompleted();

        Debug.Log("[LevelCompleteZone] Level complete — MissionCompleted raised.");
    }
}