using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manual HUD test tool. Lets you fire GameEvents from keyboard
/// to verify HUD subscribers work, before HeliHealth/Fuel are wired up.
/// Attach to any GameObject in the scene. Delete or disable before final build.
/// </summary>
public class DebugEventTrigger : MonoBehaviour
{
    [SerializeField] float testMaxHealth = 100f;
    [SerializeField] float testMaxFuel = 100f;

    private float testHealth = 100f;
    private float testFuel = 100f;

    void Start()
    {
        // Initial broadcast so HUD draws full bars at start
        GameEvents.RaiseHealthChanged(testHealth, testMaxHealth);
        GameEvents.RaiseFuelChanged(testFuel, testMaxFuel);
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        // H = take 10 damage
        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            testHealth = Mathf.Max(0, testHealth - 10f);
            GameEvents.RaiseDamageTaken(10f);
            GameEvents.RaiseHealthChanged(testHealth, testMaxHealth);
            if (testHealth <= 0) GameEvents.RaiseHelicopterDestroyed();
        }

        // J = heal 20
        if (Keyboard.current.jKey.wasPressedThisFrame)
        {
            testHealth = Mathf.Min(testMaxHealth, testHealth + 20f);
            GameEvents.RaiseHealthChanged(testHealth, testMaxHealth);
        }

        // F = burn 10 fuel
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            testFuel = Mathf.Max(0, testFuel - 10f);
            GameEvents.RaiseFuelChanged(testFuel, testMaxFuel);
            if (testFuel <= 0) GameEvents.RaiseFuelEmpty();
        }

        // G = refill 20 fuel
        if (Keyboard.current.gKey.wasPressedThisFrame)
        {
            testFuel = Mathf.Min(testMaxFuel, testFuel + 20f);
            GameEvents.RaiseFuelChanged(testFuel, testMaxFuel);
        }

        // 1 = info feedback
        // 2 = warning feedback
        // 3 = critical feedback
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
            GameEvents.RaiseFeedback("Objective updated", FeedbackSeverity.Info);
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
            GameEvents.RaiseFeedback("Low fuel", FeedbackSeverity.Warning);
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
            GameEvents.RaiseFeedback("Helicopter critical!", FeedbackSeverity.Critical);

        // M = start mission + automatically add test objectives
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            GameEvents.RaiseMissionStarted("Test Mission Alpha");
            GameEvents.RaiseObjectiveAdded("Destroy the target");
            GameEvents.RaiseObjectiveAdded("Extract civilians");
        }

        // N = complete first objective (strikethrough + gray)
        if (Keyboard.current.nKey.wasPressedThisFrame)
            GameEvents.RaiseObjectiveCompleted("Destroy the target");

        // K = mission complete
        if (Keyboard.current.kKey.wasPressedThisFrame)
            GameEvents.RaiseMissionCompleted();

        // I = simulate 3 second extraction interaction
        //     panel appears, fills up, then disappears
        if (Keyboard.current.iKey.wasPressedThisFrame)
            StartCoroutine(SimulateInteraction());
    }

    private System.Collections.IEnumerator SimulateInteraction()
    {
        // Vector3.zero used for debug — real position comes from world objects
        GameEvents.RaiseInteractionStarted("EXTRACTING...", Vector3.zero);
        float duration = 3f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            GameEvents.RaiseInteractionProgress(elapsed / duration);
            yield return null;
        }
        GameEvents.RaiseInteractionCompleted();
    }
}