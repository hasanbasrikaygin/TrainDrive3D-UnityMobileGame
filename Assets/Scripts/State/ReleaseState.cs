using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleaseState : TrainState
{
    public ReleaseState(Train train) : base(train) { }
    public override void EnterState()
    {
        Debug.Log("Entered release state");
        //for (int i = train.bodyParts.Count; i > 0; i--)
        //{
        //    //train.bodyParts[i].SetActive(false); // Vagonu görünmez yap
        //}
        //foreach (var item in train.bodyParts)
        //{
        //    item.transform.SetParent(train.craneHook.transform);
        //}
        train.bodyParts[0].GetComponent<BoxCollider>().enabled = true;
        train.bodyParts.Clear();
        train.StopRecording();
        train.ReturnAllWagonsToPool();
        train.positions.Clear();
        train.positions.Add(train.transform.position);
        train.StartRecording();
        //
        train.StartCoroutine(MoveTrain());
        
        
        //train.positionQueue.Clear();
        //train.rotationQueue.Clear();

    }

    public override void ExitState()
    {
       
    }

    public override void FixedUpdateState()
    {
       
    }

    public override void UpdateState()
    {
        
    }
    private IEnumerator MoveTrain()
    {
        AudioManager.Instance.PlayWagonDropoff();
        AudioManager.Instance.PlayPassengerPickup();
        yield return new WaitForSeconds(.1f);
        train.TransitionToState(train.MoveState); // MoveState'e geçiþ


    }

    void Start()
    {
        
    }

   
    void Update()
    {
        
    }
}
