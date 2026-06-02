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
/// interactions, resupply, and HUD feedback.
/// </summary>
public static class GameEvents
{
    // ========================
    // HEALTH
    // ========================

    public static event Action<float, float> OnHealthChanged;
    public static event Action<float> OnDamageTaken;
    public static event Action OnHelicopterDestroyed;

    public static void RaiseHealthChanged(float current, float max)
    {
        OnHealthChanged?.Invoke(current, max);
    }

    public static void RaiseDamageTaken(float amount)
    {
        OnDamageTaken?.Invoke(amount);
    }

    public static void RaiseHelicopterDestroyed()
    {
        OnHelicopterDestroyed?.Invoke();
    }

    // ========================
    // FUEL
    // ========================

    public static event Action<float, float> OnFuelChanged;
    public static event Action OnFuelEmpty;

    public static void RaiseFuelChanged(float current, float max)
    {
        OnFuelChanged?.Invoke(current, max);
    }

    public static void RaiseFuelEmpty()
    {
        OnFuelEmpty?.Invoke();
    }

    // ========================
    // RESUPPLY
    // ========================

    public static event Action OnResupplyRequested;

    public static void RaiseResupplyRequested()
    {
        OnResupplyRequested?.Invoke();
    }

    // ========================
    // GAME STATE
    // ========================

    public static event Action OnGamePaused;
    public static event Action OnGameResumed;

    public static void RaiseGamePaused()
    {
        OnGamePaused?.Invoke();
    }

    public static void RaiseGameResumed()
    {
        OnGameResumed?.Invoke();
    }

    // ========================
    // MISSION
    // ========================

    public static event Action<string> OnMissionStarted;
    public static event Action<string> OnObjectiveAdded;
    public static event Action<string> OnObjectiveCompleted;
    public static event Action OnMissionCompleted;
    public static event Action OnMissionFailed;

    public static void RaiseMissionStarted(string missionName)
    {
        OnMissionStarted?.Invoke(missionName);
    }

    public static void RaiseObjectiveAdded(string objective)
    {
        OnObjectiveAdded?.Invoke(objective);
    }

    public static void RaiseObjectiveCompleted(string objective)
    {
        OnObjectiveCompleted?.Invoke(objective);
    }

    public static void RaiseMissionCompleted()
    {
        OnMissionCompleted?.Invoke();
    }

    public static void RaiseMissionFailed()
    {
        OnMissionFailed?.Invoke();
    }

    // ========================
    // INTERACTION
    // ========================

    public static event Action<string, Vector3> OnInteractionStarted;
    public static event Action<float> OnInteractionProgress;
    public static event Action OnInteractionCompleted;
    public static event Action OnInteractionCancelled;

    public static void RaiseInteractionStarted(string label, Vector3 worldPos)
    {
        OnInteractionStarted?.Invoke(label, worldPos);
    }

    public static void RaiseInteractionProgress(float progress)
    {
        OnInteractionProgress?.Invoke(progress);
    }

    public static void RaiseInteractionCompleted()
    {
        OnInteractionCompleted?.Invoke();
    }

    public static void RaiseInteractionCancelled()
    {
        OnInteractionCancelled?.Invoke();
    }

    // ========================
    // FEEDBACK
    // ========================

    public static event Action<string, FeedbackSeverity> OnFeedbackRaised;

    public static void RaiseFeedback(string message, FeedbackSeverity severity = FeedbackSeverity.Info)
    {
        OnFeedbackRaised?.Invoke(message, severity);
    }
}
