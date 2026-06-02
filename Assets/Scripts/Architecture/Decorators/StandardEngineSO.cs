using UnityEngine;

[CreateAssetMenu(fileName = "StandardEngine", menuName = "Flight/Engines/Standard Engine")]
public class StandardEngineSO : BaseEnginePowerSO
{
    public override float GetPowerMultiplier(float currentHeight)
    {
        return 1.0f;
    }
}
