using UnityEngine;

public class Fuel : MonoBehaviour
{
    [SerializeField] float maxFuel = 100f;
    [SerializeField] float fuelConsumptionPerSecond = 1f;

    private float currentFuel;

    void Start()
    {
        currentFuel = maxFuel;
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
            Debug.Log("Current Fuel:" + currentFuel);
            currentFuel = Mathf.Max(currentFuel, 0f);
        }
    }
}
