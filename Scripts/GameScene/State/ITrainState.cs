public interface ITrainState
{
    void EnterState();
    void UpdateState();
    void FixedUpdateState(); 
    void ExitState();
}
