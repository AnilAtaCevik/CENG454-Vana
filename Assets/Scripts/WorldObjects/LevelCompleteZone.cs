using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Instant trigger — when helicopter flies over this zone, level is complete.
/// No landing required. Clears checkpoint data so next level starts fresh.
/// </summary>
[RequireComponent(typeof(Collider))]
public class LevelCompleteZone : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string completionMessage = "LEVEL COMPLETE!";
    [SerializeField] private string nextSceneName = "";

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

        // Clear checkpoints so next level starts fresh
        CheckpointManager.ClearAll();

        // Notify HUD and mission system
        GameEvents.RaiseFeedback(completionMessage, FeedbackSeverity.Info);
        GameEvents.RaiseMissionCompleted();

        // Load next scene if specified
        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);

        Debug.Log($"[LevelCompleteZone] Level complete! Loading: {nextSceneName}");
    }
}