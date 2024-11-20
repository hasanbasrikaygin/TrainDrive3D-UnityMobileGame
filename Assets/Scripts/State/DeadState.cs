public class DeadState : TrainState
{
    public DeadState(Train train) : base(train) { }

    public override void EnterState()
    {
        // Ölme durumuna giriþte yapýlacak iþlemler
        //train.Die();
    }

    public override void UpdateState()
    {
        // Ölme durumundayken yapýlacak iþlemler
    }

    public override void ExitState()
    {
        // Ölme durumundan çýkýþta yapýlacak iþlemler
    }

    public override void FixedUpdateState()
    {
   
    }
}
