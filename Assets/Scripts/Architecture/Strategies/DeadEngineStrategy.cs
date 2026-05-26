using UnityEngine;

public class DeadEngineStrategy : IFlightStrategy
{
    public void ExecuteMovement(Movement ctx, float powerMultiplier)
    {
        ctx.targetZ = 0f; 
        Vector3 verticalDrag = new Vector3(0, ctx.rb.linearVelocity.y * 0.05f, 0);
        ctx.rb.AddForce(-verticalDrag, ForceMode.Acceleration);
    }
}
