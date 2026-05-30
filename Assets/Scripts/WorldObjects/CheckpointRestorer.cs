using System.Collections;
using UnityEngine;

/// <summary>
/// Attach to the helicopter. After scene reload (death), checks if a
/// checkpoint was saved and teleports the player there.
/// Health and fuel reset to max naturally from their own Start() methods.
/// Enemy groups are handled by CheckpointZone.Start() automatically.
/// </summary>
public class CheckpointRestorer : MonoBehaviour
{
    IEnumerator Start()
    {
        if (!CheckpointManager.HasCheckpoint) yield break;

        // Wait one frame so all other Start() methods finish first
        // (HeliHealth sets health, Fuel sets fuel, physics initializes)
        yield return null;

        // Teleport to saved checkpoint position
        transform.position = CheckpointManager.SavedPosition;

        // Kill any leftover velocity from before death
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Restore passenger state
        PassengerState.IsCarryingPassengers =
            CheckpointManager.SavedCarryingPassengers;

        Debug.Log($"[CheckpointRestorer] Respawned at checkpoint: " +
                  $"{CheckpointManager.SavedPosition}");
    }
}