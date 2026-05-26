public class DamagedEngineDecorator : EnginePowerDecorator
{
    private float _powerPenalty;

    public DamagedEngineDecorator(IEnginePower wrappedEngine, float penalty) : base(wrappedEngine)
    {
        _powerPenalty = penalty;
    }

    public override float GetPowerMultiplier()
    {
        return base.GetPowerMultiplier() * (1f - _powerPenalty);
    }
}