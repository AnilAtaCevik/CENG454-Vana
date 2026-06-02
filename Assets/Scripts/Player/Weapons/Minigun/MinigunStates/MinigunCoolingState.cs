public class MinigunCoolingState : IMinigunState
{
    public void Enter(Minigun minigun)
    {
        minigun.StartCoolingState();
    }

    public void Update(Minigun minigun)
    {
        minigun.TickStateTimer();

        if (minigun.IsStateTimerFinished())
        {
            WeaponEvents.RaiseMinigunCooldownFinished();
            minigun.ChangeState(new MinigunIdleState());
        }
    }

    public void Exit(Minigun minigun)
    {
    }
}