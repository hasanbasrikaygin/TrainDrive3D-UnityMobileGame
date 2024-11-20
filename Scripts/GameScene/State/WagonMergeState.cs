using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WagonMergeState : TrainState
{
    int currentGoldCount;

    public WagonMergeState(Train train) : base(train)
    {
        //this.number = number;  , int number = 2, Color? color = null
        //this.color = color ?? Color.white; // Varsayýlan renk beyaz
    }

    public override void EnterState()
    {
        for (int i = train.wagonNumbers.Count - 1; i > 0; i--)
        {
            if (train.wagonNumbers[i] > train.wagonNumbers[i - 1])
            {
                train.gameManager.GameOver();
                train.TransitionToState(train.IdleState);
                if (train.wagonNumbers.Count > 0)
                {
                    train.wagonNumbers.RemoveAt(train.wagonNumbers.Count - 1);
                }
                return;
            }
        }
        Debug.Log(train.number);
        Grow();
        train.TransitionToState(train.NumberControlState);
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
        //train.rb.useGravity = true;
    }
    private void Grow()
    {
        Vector3 newPartPosition = train.bodyParts.Count == 0 ?
            train.transform.position - train.transform.forward * train.bodyDistance :
            train.bodyParts[train.bodyParts.Count - 1].transform.position -
            train.bodyParts[train.bodyParts.Count - 1].transform.forward * train.bodyDistance;

        GameObject newBodyPart = train.mergeWagonPool.GetWagon(train.number,train.color);
        newBodyPart.transform.position = newPartPosition;
        newBodyPart.transform.rotation = Quaternion.identity;

        train.bodyParts.Add(newBodyPart);

    }

}
