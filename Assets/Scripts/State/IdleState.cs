using UnityEngine;
using UnityEngine.SceneManagement;

public class IdleState : TrainState
{
    public IdleState(Train train) : base(train) { }

    public override void EnterState()
    {

        if (train.speedLinesEffectIn.isPlaying)
        {
            train.speedLinesEffectIn.Stop();
        }
        if (train.speedLinesEffectOut.isPlaying)
        {
            train.speedLinesEffectOut.Stop();
        }
    }


    public override void UpdateState()
    {
        // Bekleme durumundayken yapýlacak iþlemler
    }
    public override void FixedUpdateState()
    {

    }

    public override void ExitState()
    {
        //train.scoreManager.StartScore();
    }
}
