public class MinigunOverheatedState : IMinigunState
{
    public void Enter(Minigun minigun)
    {
        minigun.StartOverheatedState();
    }

    public void Update(Minigun minigun)
    {
        minigun.TickStateTimer();

        if (minigun.IsStateTimerFinished())
        {
            minigun.ChangeState(new MinigunCoolingState());
        }
    }

    public void Exit(Minigun minigun)
    {
    }
}