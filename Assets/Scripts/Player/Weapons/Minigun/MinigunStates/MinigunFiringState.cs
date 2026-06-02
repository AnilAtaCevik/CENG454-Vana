public class MinigunFiringState : IMinigunState
{
    public void Enter(Minigun minigun)
    {
        minigun.StartFiringState();
    }

    public void Update(Minigun minigun)
    {
        if (!minigun.IsFirePressed())
        {
            minigun.ChangeState(new MinigunIdleState());
            return;
        }

        minigun.TickStateTimer();

        minigun.HandleFire();

        if (minigun.IsStateTimerFinished())
        {
            minigun.ChangeState(new MinigunOverheatedState());
        }
    }

    public void Exit(Minigun minigun)
    {
    }
}