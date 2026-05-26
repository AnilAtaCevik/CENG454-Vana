using System;
using UnityEngine;

/// <summary>
/// Severity levels for HUD feedback messages.
/// </summary>
public enum FeedbackSeverity
{
    Info,
    Warning,
    Critical
}

/// <summary>
/// Central event hub for game-wide events: health, fuel, mission state,
/// interactions, and HUD feedback. Mirrors WeaponEvents.cs in style.
///
/// Subscribers (HUD elements, audio managers) listen via += / -= in
/// OnEnable/OnDisable. Publishers (HeliHealth, Fuel, mission systems)
/// fire via the Raise...() methods.
/// </summary>
public static class GameEvents
{
    // ========================
    // HEALTH
    // ========================

    /// <summary>(currentHealth, maxHealth) — fired whenever helicopter health changes.</summary>
    public static event Action<float, float> OnHealthChanged;

    /// <summary>(damageAmount) — fired the instant damage is applied (for flash/screen shake).</summary>
    public static event Action<float> OnDamageTaken;

    public static event Action OnHelicopterDestroyed;

    public static void RaiseHealthChanged(float current, float max) => OnHealthChanged?.Invoke(current, max);
    public static void RaiseDamageTaken(float amount) => OnDamageTaken?.Invoke(amount);
    public static void RaiseHelicopterDestroyed() => OnHelicopterDestroyed?.Invoke();

    // ========================
    // FUEL
    // ========================

    /// <summary>(currentFuel, maxFuel) — fired whenever fuel level changes.</summary>
    public static event Action<float, float> OnFuelChanged;

    public static event Action OnFuelEmpty;

    public static void RaiseFuelChanged(float current, float max) => OnFuelChanged?.Invoke(current, max);
    public static void RaiseFuelEmpty() => OnFuelEmpty?.Invoke();

    // ========================
    // GAME STATE
    // ========================

    public static event Action OnGamePaused;
    public static event Action OnGameResumed;

    public static void RaiseGamePaused() => OnGamePaused?.Invoke();
    public static void RaiseGameResumed() => OnGameResumed?.Invoke();

    // ========================
    // MISSION
    // ========================

    public static event Action<string> OnMissionStarted;
    public static event Action<string> OnObjectiveAdded;
    public static event Action<string> OnObjectiveCompleted;
    public static event Action OnMissionCompleted;
    public static event Action OnMissionFailed;

    public static void RaiseMissionStarted(string missionName) => OnMissionStarted?.Invoke(missionName);
    public static void RaiseObjectiveAdded(string objective) => OnObjectiveAdded?.Invoke(objective);
    public static void RaiseObjectiveCompleted(string objective) => OnObjectiveCompleted?.Invoke(objective);
    public static void RaiseMissionCompleted() => OnMissionCompleted?.Invoke();
    public static void RaiseMissionFailed() => OnMissionFailed?.Invoke();

    // ========================
    // INTERACTION (extraction zones, resupply stations)
    // ========================

    /// <summary>(label, worldPos) — label is "EXTRACTING..." or "RESUPPLYING...", worldPos is helicopter position</summary>
    public static event Action<string, Vector3> OnInteractionStarted;

    /// <summary>(progress 0..1) — for showing the 5-second timer fill.</summary>
    public static event Action<float> OnInteractionProgress;

    public static event Action OnInteractionCompleted;
    public static event Action OnInteractionCancelled;

    public static void RaiseInteractionStarted(string label, Vector3 worldPos)
        => OnInteractionStarted?.Invoke(label, worldPos);
    public static void RaiseInteractionProgress(float progress) => OnInteractionProgress?.Invoke(progress);
    public static void RaiseInteractionCompleted() => OnInteractionCompleted?.Invoke();
    public static void RaiseInteractionCancelled() => OnInteractionCancelled?.Invoke();

    // ========================
    // FEEDBACK (HUD message label)
    // ========================

    /// <summary>(message, severity) — broadcast a centered HUD message like "Low fuel!"</summary>
    public static event Action<string, FeedbackSeverity> OnFeedbackRaised;

    public static void RaiseFeedback(string message, FeedbackSeverity severity = FeedbackSeverity.Info)
    {
        OnFeedbackRaised?.Invoke(message, severity);
    }
}