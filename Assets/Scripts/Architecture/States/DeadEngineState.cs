using UnityEngine;

public class DeadEngineState : IFlightState
{
    private Movement ctx;

    public DeadEngineState(Movement context)
    {
        this.ctx = context;
    }

    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public void Tick()
    {
    }
}
