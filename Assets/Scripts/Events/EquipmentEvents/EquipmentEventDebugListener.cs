using UnityEngine;

public class EquipmentEventDebugListener : MonoBehaviour
{
    void OnEnable()
    {
        EquipmentEvents.OnSpotlightToggled += HandleSpotlightToggled;
        EquipmentEvents.OnFlareAmmoChanged += HandleFlareAmmoChanged;
        EquipmentEvents.OnFlareUsed += HandleFlareUsed;
        EquipmentEvents.OnFlareCooldownStarted += HandleFlareCooldownStarted;
    }

    void OnDisable()
    {
        EquipmentEvents.OnSpotlightToggled -= HandleSpotlightToggled;
        EquipmentEvents.OnFlareAmmoChanged -= HandleFlareAmmoChanged;
        EquipmentEvents.OnFlareUsed -= HandleFlareUsed;
        EquipmentEvents.OnFlareCooldownStarted -= HandleFlareCooldownStarted;
    }

    void HandleSpotlightToggled(bool isEnabled)
    {
        Debug.Log("Spotlight toggled: " + isEnabled);
    }

    void HandleFlareAmmoChanged(int currentCharges, int maxCharges)
    {
        Debug.Log("Flare Ammo: " + currentCharges + "/" + maxCharges);
    }

    void HandleFlareUsed()
    {
        Debug.Log("Flare used event received.");
    }

    void HandleFlareCooldownStarted(float cooldownTime)
    {
        Debug.Log("Flare cooldown started: " + cooldownTime);
    }
}