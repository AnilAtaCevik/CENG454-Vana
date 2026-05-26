using UnityEngine;

public class WeaponEventDebugListener : MonoBehaviour
{
    void OnEnable()
    {
        WeaponEvents.OnMissileAmmoChanged += HandleMissileAmmoChanged;
        WeaponEvents.OnMissileFired += HandleMissileFired;
        WeaponEvents.OnMissileCooldownStarted += HandleMissileCooldownStarted;
        WeaponEvents.OnMinigunOverheated += HandleMinigunOverheated;
        WeaponEvents.OnMinigunCooldownFinished += HandleMinigunCooldownFinished;
    }

    void OnDisable()
    {
        WeaponEvents.OnMissileAmmoChanged -= HandleMissileAmmoChanged;
        WeaponEvents.OnMissileFired -= HandleMissileFired;
        WeaponEvents.OnMissileCooldownStarted -= HandleMissileCooldownStarted;
        WeaponEvents.OnMinigunOverheated -= HandleMinigunOverheated;
        WeaponEvents.OnMinigunCooldownFinished -= HandleMinigunCooldownFinished;
    }

    void HandleMissileAmmoChanged(int currentAmmo, int maxAmmo)
    {
        Debug.Log("Missile Ammo: " + currentAmmo + "/" + maxAmmo);
    }

    void HandleMissileFired()
    {
        Debug.Log("Missile fired event received.");
    }

    void HandleMissileCooldownStarted(float cooldownTime)
    {
        Debug.Log("Missile cooldown started: " + cooldownTime);
    }

    void HandleMinigunOverheated()
    {
        Debug.Log("Minigun overheated event received.");
    }

    void HandleMinigunCooldownFinished()
    {
        Debug.Log("Minigun cooldown finished event received.");
    }
}