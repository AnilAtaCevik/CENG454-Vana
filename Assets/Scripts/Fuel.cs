using UnityEngine;

public class Fuel : MonoBehaviour
{
    [SerializeField] AudioSource fuelWarningAudio;
    [SerializeField] float maxFuel = 100f;
    [SerializeField] float fuelConsumptionPerSecond = 1f;

    private float currentFuel;

    private bool hasPlayedWarning = false;

    void Start()
    {
        currentFuel = maxFuel;
    }

    void Update()
    {
        ConsumeFuel();
        CheckFuelWarning();
    }

    private void ConsumeFuel()
    {
        if (currentFuel > 0)
        {
            currentFuel -= fuelConsumptionPerSecond * Time.deltaTime;
            currentFuel = Mathf.Max(currentFuel, 0f);
            
            Debug.Log("Current Fuel:" + currentFuel);
        }
    }

    private void CheckFuelWarning()
    {
        if (currentFuel < 25f && !hasPlayedWarning)
        {
            fuelWarningAudio.Play();
            hasPlayedWarning = true;
        }
    }

    public bool HasFuel()
    {
        return currentFuel > 0;
    }

    public void RefillFuel(float amount)
    {
        currentFuel += amount;
        currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);

        if (currentFuel >= 25f) hasPlayedWarning = false;
    }
}
