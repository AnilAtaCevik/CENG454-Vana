using UnityEngine;

/// <summary>
/// Landing platform that completes the mission after the helicopter lands
/// and remains for the decided duration. One-time use.
/// </summary>
public class ExtractionZone : LandingZone
{
    [Header("Extraction")]
    [Tooltip("Objective text logged when extraction completes")]
    [SerializeField] private string extractionObjective = "Reach the extraction zone";

    void Awake()
    {
        interactionLabel = "EXTRACTING...";
        oneTimeUse = true;
    }

    protected override void OnLandingComplete()
    {
        Debug.Log("[ExtractionZone] Extraction complete!");
        GameEvents.RaiseObjectiveCompleted(extractionObjective);
        GameEvents.RaiseFeedback("MISSION COMPLETE", FeedbackSeverity.Info);
        GameEvents.RaiseMissionCompleted();
    }
}