public abstract class EnginePowerDecorator : IEnginePower
{
    protected IEnginePower _wrappedEngine;
    
    public EnginePowerDecorator(IEnginePower wrappedEngine)
    {
        _wrappedEngine = wrappedEngine;
    }
    
    public virtual float GetPowerMultiplier() => _wrappedEngine.GetPowerMultiplier();
}