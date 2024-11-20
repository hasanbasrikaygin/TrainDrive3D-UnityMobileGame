public abstract class TrainState : ITrainState
{
    protected Train train;

    public TrainState(Train train)
    {
        this.train = train;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void FixedUpdateState();
    public abstract void ExitState();
}
