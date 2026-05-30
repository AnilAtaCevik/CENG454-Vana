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
                ctx.rb.AddForce(Vector3.up * Time.fixedDeltaTime * finalStrength);
            }
            else
            {
                Vector3 vel = ctx.rb.linearVelocity;
                if (vel.y > 0) ctx.rb.linearVelocity = new Vector3(vel.x, vel.y * 0.9f, vel.z);
            }
        }
        else if (verticalInput < 0)
        {
            ctx.rb.AddForce(Vector3.down * Time.fixedDeltaTime * ctx.ascentDescentStrength);
        }
    }

    private void ProcessRightLeft(Movement ctx, float powerMultiplier)
    {
        float rightInput = ctx.rightLeft.ReadValue<float>();
        float finalStrength = ctx.rightLeftStrength * powerMultiplier;

        if (rightInput > 0)
        {
            if (ctx.isMovingLeft) { ctx.isMovingLeft = false; ctx.rb.linearVelocity *= 0.5f; ctx.targetY -= 180f; }
            ctx.targetZ += -10;
        }
        else if (rightInput < 0)
        {
            if (!ctx.isMovingLeft) { ctx.isMovingLeft = true; ctx.rb.linearVelocity *= 0.5f; ctx.targetY += 180f; }
            ctx.targetZ += -10;
        }

        if (rightInput != 0)
        {
            ctx.rb.AddForce(Vector3.right * rightInput * Time.fixedDeltaTime * finalStrength);
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
            
            ctx.rb.AddForce(Vector3.forward * direction * Time.fixedDeltaTime * finalStrength);
        }
    }

    private void VelocityLimiter(Movement ctx)
    {
        Vector3 horizontalVelocity = new Vector3(ctx.rb.linearVelocity.x, 0, ctx.rb.linearVelocity.z);
        float verticalVelocityY = ctx.rb.linearVelocity.y;

        if (horizontalVelocity.magnitude > ctx.maxSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * ctx.maxSpeed;
        }

        verticalVelocityY = Mathf.Clamp(verticalVelocityY, -ctx.maxClimbSpeed, ctx.maxClimbSpeed);

        ctx.rb.linearVelocity = new Vector3(horizontalVelocity.x, verticalVelocityY, horizontalVelocity.z);

        if (ctx.rightLeft.ReadValue<float>() == 0 && ctx.pitch.ReadValue<float>() == 0)
        {
            ctx.rb.AddForce(-horizontalVelocity * ctx.linearDrag, ForceMode.Acceleration);
        }

        if (ctx.ascentDescent.ReadValue<float>() == 0)
        {
            ctx.rb.AddForce(Vector3.down * verticalVelocityY * ctx.verticalDrag, ForceMode.Acceleration);
        }
    }
}