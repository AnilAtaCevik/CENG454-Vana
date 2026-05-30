using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Static brain for the checkpoint system. Survives scene reload
/// because all fields are static. CheckpointZone writes to it,
/// CheckpointRestorer reads from it after respawn.
/// </summary>
public static class CheckpointManager
{
    public static bool HasCheckpoint = false;
    public static HashSet<int> ActivatedCheckpoints = new HashSet<int>();

    // Saved state
    public static Vector3 SavedPosition;
    public static float SavedHealth;
    public static float SavedMaxHealth;
    public static float SavedFuel;
    public static float SavedMaxFuel;
    public static bool SavedCarryingPassengers;

    public static void SaveCheckpoint(int index, Vector3 position,
        float curHealth, float maxHealth,
        float curFuel, float maxFuel,
        bool carryingPassengers)
    {
        HasCheckpoint = true;
        ActivatedCheckpoints.Add(index);
        SavedPosition = position;
        SavedHealth = curHealth;
        SavedMaxHealth = maxHealth;
        SavedFuel = curFuel;
        SavedMaxFuel = maxFuel;
        SavedCarryingPassengers = carryingPassengers;

        Debug.Log($"[CheckpointManager] Saved checkpoint {index} at {position}");
    }

    /// <summary>Call when starting a new level to reset all checkpoint data.</summary>
    public static void ClearAll()
    {
        HasCheckpoint = false;
        ActivatedCheckpoints.Clear();
        SavedCarryingPassengers = false;
        Debug.Log("[CheckpointManager] All checkpoints cleared.");
    }
}