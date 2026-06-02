using UnityEngine;

public abstract class BaseEnginePowerSO : ScriptableObject
{
    public abstract float GetPowerMultiplier(float currentHeight);
}