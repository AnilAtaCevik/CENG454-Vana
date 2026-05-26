using UnityEngine;

public class FuelCan : MonoBehaviour
{
    [SerializeField] float refillAmount = 25f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IFuelReceiver fuelReceiver = other.GetComponent<IFuelReceiver>();
            
            if (fuelReceiver != null)
            {
                fuelReceiver.RefillFuel(refillAmount);
                gameObject.SetActive(false); 
            }
        }
    }
}