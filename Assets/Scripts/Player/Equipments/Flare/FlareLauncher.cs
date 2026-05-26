using UnityEngine;
using UnityEngine.InputSystem;

public class FlareLauncher : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject flarePrefab;
    [SerializeField] private Transform flareSpawnPoint;

    [Header("Ammo")]
    [SerializeField] private int maxCharges = 3;

    [Header("Cooldown")]
    [SerializeField] private float deployCooldown = 5f;

    private int currentCharges;
    private float nextDeployTime = 0f;

    void Start()
    {
        currentCharges = maxCharges;
    }

    void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            TryDeployFlare();
        }
    }

    void TryDeployFlare()
    {
        if (Time.time < nextDeployTime)
            return;

        if (currentCharges <= 0)
            return;

        DeployFlare();

        currentCharges--;
        nextDeployTime = Time.time + deployCooldown;
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