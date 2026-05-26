using UnityEngine;

public class Fuel : MonoBehaviour, IFuelReceiver
{
    [SerializeField] float maxFuel = 100f;
    [SerializeField] float fuelConsumptionPerSecond = 1f;

    private float currentFuel;
    private bool hasPlayedWarning = false;
    private bool isOutOfFuel = false;

    void Start()
    {
        currentFuel = maxFuel;
        GameEvents.RaiseFuelChanged(currentFuel, maxFuel);
    }

    void Update()
    {
        ConsumeFuel();
    }

    private void ConsumeFuel()
    {
        if (currentFuel > 0)
        {
            currentFuel -= fuelConsumptionPerSecond * Time.deltaTime;
            currentFuel = Mathf.Max(currentFuel, 0f);

            GameEvents.RaiseFuelChanged(currentFuel, maxFuel);
            CheckFuelWarning();

            if (currentFuel <= 0 && !isOutOfFuel)
            {
                isOutOfFuel = true;
                GameEvents.RaiseFuelEmpty(); 
                GameEvents.RaiseFeedback("CRITICAL: FUEL DEPLETED!", FeedbackSeverity.Critical);
            }
        }
    }

    private void CheckFuelWarning()
    {
        if (currentFuel < 25f && currentFuel > 0 && !hasPlayedWarning)
        {
            GameEvents.RaiseFeedback("Warning: Low Fuel!", FeedbackSeverity.Warning);
            hasPlayedWarning = true;
        }
    }

    public void RefillFuel(float amount)
    {
        currentFuel += amount;
        currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);
        isOutOfFuel = false;

        if (currentFuel >= 25f) hasPlayedWarning = false;

        GameEvents.RaiseFuelChanged(currentFuel, maxFuel);
        GameEvents.RaiseFeedback("Fuel Replenished", FeedbackSeverity.Info);
    }
}