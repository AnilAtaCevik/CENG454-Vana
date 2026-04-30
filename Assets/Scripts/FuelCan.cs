using UnityEngine;

public class FuelCan : MonoBehaviour
{
    [SerializeField] float refillAmount = 25f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Fuel helicopterFuel = other.GetComponent<Fuel>();
            
            if (helicopterFuel != null)
            {
                helicopterFuel.RefillFuel(refillAmount);

                Destroy(gameObject);
            }
        }
    }
}
