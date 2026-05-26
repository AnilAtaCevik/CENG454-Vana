using UnityEngine;
using UnityEngine.InputSystem;

public class FlareLauncher : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject flarePrefab;
    [SerializeField] private Transform flareSpawnPoint;

    void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            DeployFlare();
        }
    }

    void DeployFlare()
    {
        if (flarePrefab == null || flareSpawnPoint == null)
            return;

        Instantiate(
            flarePrefab,
            flareSpawnPoint.position,
            flareSpawnPoint.rotation
        );
    }
}