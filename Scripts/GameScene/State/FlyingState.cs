using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingState : TrainState
{
    public FlyingState(Train train) : base(train) { }
    public override void EnterState()
    {
        //train.rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        train.animator.SetBool("flying", true);
        //train.rb.useGravity = false;
        Debug.Log("Entered FlyingState");
        train.wings.SetActive(true);
        train.wingsAnimator.SetBool("wingsUp", true);
        train.StartCoroutine(RunMoveState());
        //train.runMoveCoroutine = train.StartCoroutine(RunMoveState());
    }



    public override void FixedUpdateState()
    {
        
        
    }

    private void Move()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            train.TransitionToState(train.MoveState);
        }
        // Sürekli hareket
        Vector3 move = train.transform.forward * Time.deltaTime * train.playerSpeed;
        train.characterController.Move(move);

        // Dönüþ iþlemleri
        if (train.leftTurnAction.ReadValue<float>() > 0)
        {
            train.transform.Rotate(Vector3.up, -train.turnSpeed * Time.deltaTime);
        }
        if (train.rightTurnAction.ReadValue<float>() > 0)
        {
            train.transform.Rotate(Vector3.up, train.turnSpeed * Time.deltaTime);
        }


        float targetHeight = 8f; // Belirli bir yükseklik
        Vector3 position = train.transform.position;
        position.y = Mathf.Lerp(position.y, targetHeight, Time.deltaTime * 5f); // Hafifçe yukarý çýkýþ
        train.transform.position = position;


        // Pozisyonlarý kaydet
        float distance = (train.transform.position - train.positions[0]).magnitude;
        if (distance > train.positionRecordInterval)
        {
            train.positions.Insert(0, train.transform.position);

            if (train.positions.Count > (train.bodyParts.Count + 1) * Mathf.RoundToInt(train.bodyDistance / train.positionRecordInterval))
            {
                train.positions.RemoveAt(train.positions.Count - 1);
            }
        }

        // Kuyruk parçalarýný takip ettir
        FollowBodyParts();
    }
    private void FollowBodyParts()
    {
        for (int i = 0; i < train.bodyParts.Count; i++)
        {
            int index = Mathf.Min((i + 1) * Mathf.RoundToInt(train.bodyDistance / train.positionRecordInterval), train.positions.Count - 1);
            Vector3 targetPosition = train.positions[index];
            GameObject bodyPart = train.bodyParts[i];
            bodyPart.transform.position = Vector3.Lerp(bodyPart.transform.position, targetPosition, train.bodyFollowSpeed * Time.deltaTime);
            bodyPart.transform.LookAt(targetPosition);
        }
    }
    public override void ExitState()
    {
        
        //train.rb.useGravity = true;
        train.wings.SetActive(false);
        train.animator.SetBool("flying", false);
        //train.rb.constraints = RigidbodyConstraints.None;
    }
    public override void UpdateState()
    {
        Move();
    }
    IEnumerator RunMoveState()
    {
        yield return new WaitForSeconds(7);
        train.TransitionToState(train.MoveState);
    }

}
