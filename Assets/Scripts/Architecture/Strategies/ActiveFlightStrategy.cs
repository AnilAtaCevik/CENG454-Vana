using UnityEngine;

public class ActiveFlightStrategy : IFlightStrategy
{
    public void ExecuteMovement(Movement ctx, float powerMultiplier)
    {
        ProcessAscentDescent(ctx, powerMultiplier);
        ProcessRightLeft(ctx, powerMultiplier);
        ProcessPitch(ctx, powerMultiplier);
        VelocityLimiter(ctx);
    }

    private void ProcessAscentDescent(Movement ctx, float powerMultiplier)
    {
        float verticalInput = ctx.ascentDescent.ReadValue<float>();
        float currentHeight = ctx.transform.position.y;

        float finalStrength = ctx.ascentDescentStrength * powerMultiplier; 

        if (verticalInput > 0)
        {
            ctx.CheckAltitudeAudio(currentHeight);

            if (currentHeight <= ctx.absoluteCeiling)
            {
                ctx.rb.AddRelativeForce(Vector3.up * Time.fixedDeltaTime * finalStrength);
            }
            else
            {
                Vector3 vel = ctx.rb.linearVelocity;
                if (vel.y > 0) ctx.rb.linearVelocity = new Vector3(vel.x, vel.y * 0.9f, vel.z);
            }
        }
        else if (verticalInput < 0)
        {
            ctx.rb.AddRelativeForce(Vector3.up * Time.fixedDeltaTime * -ctx.ascentDescentStrength);
        }
    }

    private void ProcessRightLeft(Movement ctx, float powerMultiplier)
    {
        float rightInput = ctx.rightLeft.ReadValue<float>();
        float finalStrength = ctx.rightLeftStrength * powerMultiplier;

        if (rightInput > 0)
        {
            if (ctx.isMovingLeft) { ctx.isMovingLeft = false; ctx.rb.linearVelocity *= 0.5f; ctx.targetY -= 180f; }
            ctx.rb.AddRelativeForce(Vector3.right * Time.fixedDeltaTime * finalStrength);
            ctx.targetZ += -10;
        }
        else if (rightInput < 0)
        {
            if (!ctx.isMovingLeft) { ctx.isMovingLeft = true; ctx.rb.linearVelocity *= 0.5f; ctx.targetY += 180f; }
            ctx.rb.AddRelativeForce(Vector3.right * Time.fixedDeltaTime * finalStrength);
            ctx.targetZ += -10;
        }
    }

    private void ProcessPitch(Movement ctx, float powerMultiplier)
    {
        float pitchInput = ctx.pitch.ReadValue<float>();
        float finalStrength = ctx.pitchStrength * powerMultiplier;

        if (pitchInput != 0)
        {
            float direction = pitchInput > 0 ? 1 : -1;
            ctx.targetZ += ctx.isMovingLeft ? -ctx.maxTiltAngle * direction : ctx.maxTiltAngle * direction;
            ctx.rb.AddRelativeForce(Vector3.forward * Time.fixedDeltaTime * finalStrength * direction);
        }
    }

    private void VelocityLimiter(Movement ctx)
    {
        if (ctx.rb.linearVelocity.magnitude > ctx.maxSpeed)
            ctx.rb.linearVelocity = ctx.rb.linearVelocity.normalized * ctx.maxSpeed;

        if (ctx.rightLeft.ReadValue<float>() == 0 && ctx.pitch.ReadValue<float>() == 0)
        {
            Vector3 horizontalVelocity = new Vector3(ctx.rb.linearVelocity.x, 0, ctx.rb.linearVelocity.z);
            ctx.rb.AddForce(-horizontalVelocity * ctx.linearDrag, ForceMode.Acceleration);
        }
    }
}