using UnityEngine;

public class AltitudePenaltyDecorator : EnginePowerDecorator
{
    private float _serviceCeiling;
    private float _absoluteCeiling;
    private float _altitudeSoftness;

    public AltitudePenaltyDecorator(IEnginePower wrappedEngine, float serviceCeiling, float absoluteCeiling, float softness) 
        : base(wrappedEngine)
    {
        _serviceCeiling = serviceCeiling;
        _absoluteCeiling = absoluteCeiling;
        _altitudeSoftness = softness;
    }

    public override float GetPowerMultiplier(float currentAltitude)
    {
        float basePower = base.GetPowerMultiplier(currentAltitude);
        float multiplier = 1f;

        if (currentAltitude < _serviceCeiling)
        {
            multiplier = Mathf.Clamp01((_serviceCeiling - currentAltitude) / _altitudeSoftness);
        }
        else if (currentAltitude >= _serviceCeiling && currentAltitude <= _absoluteCeiling)
        {
            multiplier = Mathf.Clamp01((_absoluteCeiling - currentAltitude) / _altitudeSoftness * 20);
        }
        else
        {
            multiplier = 0f; 
        }

        return basePower * multiplier;
    }
}
