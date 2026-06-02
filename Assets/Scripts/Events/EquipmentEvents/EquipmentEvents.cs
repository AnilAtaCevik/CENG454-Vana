using System;

public static class EquipmentEvents
{
    public static event Action<bool> OnSpotlightToggled;

    public static event Action<int, int> OnFlareAmmoChanged;
    public static event Action OnFlareUsed;
    public static event Action<float> OnFlareCooldownStarted;
    public static event Action OnFlareCooldownFinished;

    public static void RaiseSpotlightToggled(bool isEnabled)
    {
        OnSpotlightToggled?.Invoke(isEnabled);
    }

    public static void RaiseFlareAmmoChanged(int currentCharges, int maxCharges)
    {
        OnFlareAmmoChanged?.Invoke(currentCharges, maxCharges);
    }

    public static void RaiseFlareUsed()
    {
        OnFlareUsed?.Invoke();
    }

    public static void RaiseFlareCooldownStarted(float cooldownTime)
    {
        OnFlareCooldownStarted?.Invoke(cooldownTime);
    }

    public static void RaiseFlareCooldownFinished()
    {
        OnFlareCooldownFinished?.Invoke();
    }
}
