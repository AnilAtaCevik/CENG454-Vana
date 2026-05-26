/// <summary>
/// Contract for any object that participates in object pooling.
/// Examples: bullets, missiles, hit VFX, future projectiles.
/// </summary>
public interface IPoolable
{
    /// <summary>Called when the pool hands this object out for use.</summary>
    void OnGetFromPool();

    /// <summary>Called right before the object goes back into the pool.</summary>
    void OnReturnToPool();
}