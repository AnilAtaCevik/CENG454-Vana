using UnityEngine;

public class HeliBladeRotater : MonoBehaviour
{
    [SerializeField] Transform mainRotor;
    [SerializeField] Transform tailRotor;
    [SerializeField] float mainSpeed = 1800f;
    [SerializeField] float tailSpeed = 2000f;

    void Update()
    {
        mainRotor.Rotate(Vector3.up * mainSpeed * Time.deltaTime);
        tailRotor.Rotate(Vector3.right * tailSpeed * Time.deltaTime);
    }
}