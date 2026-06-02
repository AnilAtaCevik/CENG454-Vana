public class MinigunIdleState : IMinigunState
{
    public void Enter(Minigun minigun)
    {
        minigun.StopAllAudio();
        minigun.ResetStateTimer();
    }

    public void Update(Minigun minigun)
    {
        if (minigun.IsFirePressed())
        {
            minigun.ChangeState(new MinigunFiringState());
        }
    }

    public void Exit(Minigun minigun)
    {
    }
}