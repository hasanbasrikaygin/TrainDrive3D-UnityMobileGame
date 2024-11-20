using System.Collections.Generic;
using UnityEngine;
public class NumberControlState : TrainState
{
    public NumberControlState(Train train) : base(train) { }

    private bool isComplete = false;
    private int controlCounter = 0;

    public override void EnterState()
    {
        Debug.Log(" ----------------- " + controlCounter + " ----------------- ");
        WagonNumberControl();

        if (isComplete)
        {
            train.TransitionToState(train.MoveState);
        }
        else
        {
            // Eðer hala birleþtirme yapýlabilecek vagonlar varsa tekrar kontrol et
            EnterState();
        }
    }

    public override void ExitState()
    {
        train.UpdateUI();
    }

    public override void FixedUpdateState()
    {

    }

    public override void UpdateState()
    {

    }

    private void WagonNumberControl()
    {
        isComplete = true; // Assume control is complete unless a merge occurs

        for (int i = train.wagonNumbers.Count - 1; i > 0; i--)
        {
            
            if (train.wagonNumbers[i] == train.wagonNumbers[i - 1])
            {
                Debug.Log("Eþleþme var: " + "1. i : " + train.wagonNumbers[i] + " 2. i : " + train.wagonNumbers[i - 1]);

                // Calculate the index for UpgradeWagonNumber
                //int number = (int)Mathf.Log(train.wagonNumbers[i], 2);
                int number = train.wagonNumbers[i];

                // Upgrade the wagon number
                
                if(number < 2048)
                {
                    //train.bodyParts[i - 1].gameObject.GetComponent<MergeWagon>().ApplyWagonNumber(number*2);
                    train.wagonNumbers[i - 1] *= 2;
                    train.bodyParts[i - 1].gameObject.GetComponent<MergeWagon>().ApplyWagonNumber(number * 2);
                }
                else
                {
                    train.wagonNumbers[i-1] = (train.wagonNumbers[i - 1] / 1000) * 2000;
                    train.bodyParts[i - 1].gameObject.GetComponent<MergeWagon>().ApplyWagonNumber((number / 1000) * 1000 * 2);
                }
                

                // Remove the merged wagon from the list and deactivate it
                train.wagonNumbers.RemoveAt(i);
                train.bodyParts[i].gameObject.SetActive(false);
                train.bodyParts.RemoveAt(i);

                isComplete = false; // A merge occurred, so the process is not complete
            }
        }

        controlCounter++;
    }

}
