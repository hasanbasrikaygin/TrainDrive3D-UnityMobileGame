public class DeadState : TrainState
{
    public DeadState(Train train) : base(train) { }

    public override void EnterState()
    {
        // �lme durumuna giri�te yap�lacak i�lemler
        //train.Die();
    }

    public override void UpdateState()
    {
        // �lme durumundayken yap�lacak i�lemler
    }

    public override void ExitState()
    {
        // �lme durumundan ��k��ta yap�lacak i�lemler
    }

    public override void FixedUpdateState()
    {
   
    }
}
