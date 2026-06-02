using UnityEngine;

[CreateAssetMenu(fileName = "AltitudePenaltyDecorator", menuName = "Flight/Decorators/Altitude Penalty")]
public class AltitudePenaltyDecoratorSO : BaseEnginePowerSO
{
    [Header("Wrapped Component")]
    [SerializeField] private BaseEnginePowerSO _wrappedEngine;

    [Header("Penalty Settings")]
    [SerializeField] private float serviceCeiling = 50f;
    [SerializeField] private float absoluteCeiling = 100f;
    [SerializeField] private float altitudeSoftness = 3f;

    public override float GetPowerMultiplier(float currentHeight)
    {
        float basePower = _wrappedEngine != null ? _wrappedEngine.GetPowerMultiplier(currentHeight) : 1.0f;

        if (currentHeight <= serviceCeiling) return basePower;
        if (currentHeight >= absoluteCeiling) return 0f;

        float penalty = Mathf.Pow((currentHeight - serviceCeiling) / (absoluteCeiling - serviceCeiling), altitudeSoftness);
        return basePower * (1f - penalty);
    }
}