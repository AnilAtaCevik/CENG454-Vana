using System;
using UnityEngine;

public static class WeaponEvents
{
    public static event Action<int, int> OnMissileAmmoChanged;
    public static event Action OnMissileFired;
    public static event Action<float> OnMissileCooldownStarted;
    public static event Action OnMissileCooldownFinished;

    public static event Action OnMinigunOverheated;
    public static event Action OnMinigunCooldownFinished;

    public static void RaiseMissileAmmoChanged(int currentAmmo, int maxAmmo)
    {
        OnMissileAmmoChanged?.Invoke(currentAmmo, maxAmmo);
    }

    public static void RaiseMissileFired()
    {
        OnMissileFired?.Invoke();
    }

    public static void RaiseMissileCooldownStarted(float cooldownTime)
    {
        OnMissileCooldownStarted?.Invoke(cooldownTime);
    }

    public static void RaiseMissileCooldownFinished()
    {
        OnMissileCooldownFinished?.Invoke();
    }

    public static void RaiseMinigunOverheated()
    {
        OnMinigunOverheated?.Invoke();
    }

    public static void RaiseMinigunCooldownFinished()
    {
        OnMinigunCooldownFinished?.Invoke();
    }
}
