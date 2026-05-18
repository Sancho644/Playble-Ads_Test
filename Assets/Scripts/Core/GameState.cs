namespace Core
{
    public enum GameState
    {
        None,
        Bootstrap,
        WaitingInput,
        MovingToTarget,
        Interacting,
        Victory,
        EndCard,
        Paused
    }
}