using System;

public static class GameEventSystem
{
    //because of the GamEventSystem is static clas you dont have to added to the scene you can use directly
    // Health & Fuel
    public static event Action<float> OnHealthChanged;
    public static event Action<float> OnFuelChanged;

    // Mission
    public static event Action<string> OnMissionStarted;
    public static event Action<string> OnMissionCompleted;

    // UI Screen
    public static event Action<string> OnScreenChanged;

    // Feedback
    public static event Action<string> OnFeedbackRequested;

    public static void HealthChanged(float value) => OnHealthChanged?.Invoke(value);
    public static void FuelChanged(float value) => OnFuelChanged?.Invoke(value);
    public static void MissionStarted(string missionName) => OnMissionStarted?.Invoke(missionName);
    public static void MissionCompleted(string missionName) => OnMissionCompleted?.Invoke(missionName);
    public static void ScreenChanged(string screenName) => OnScreenChanged?.Invoke(screenName);
    public static void FeedbackRequested(string message) => OnFeedbackRequested?.Invoke(message);
}