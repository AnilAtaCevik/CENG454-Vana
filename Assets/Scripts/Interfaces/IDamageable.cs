using UnityEngine;

/// <summary>
/// Contract for anything that can receive damage:
/// helicopter, high value targets, enemies, destructibles.
/// </summary>
public interface IDamageable
{
    float CurrentHealth { get; }
    float MaxHealth { get; }
    bool IsAlive { get; }

    void TakeDamage(float amount, GameObject source = null);
}