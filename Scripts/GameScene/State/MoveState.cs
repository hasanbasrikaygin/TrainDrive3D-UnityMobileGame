using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.InputSystem.XR;

public class MoveState : TrainState
{
    private int updateFrameInterval = 5;
    private int currentFrame = 0;

public MoveState(Train train) : base(train) { }

    public override void EnterState()
    {
        Quaternion currentRotation = train.transform.rotation;
        train.transform.rotation = Quaternion.Euler(0, currentRotation.eulerAngles.y, 0);
        Debug.Log("Entered Move State");
        // Character Controller kullanýldýðý için Rigidbody kýsýtlamalarýna gerek yok
    }

    public override void FixedUpdateState()
    {

    }
    private void FollowBodyParts()
    {
        for (int i = 0; i < train.bodyParts.Count; i++)
        {
            int index = Mathf.Min((i+1 ) * Mathf.RoundToInt(train.bodyDistance / train.positionRecordInterval), train.positions.Count - 1);
            Vector3 targetPosition = train.positions[index];
            GameObject bodyPart = train.bodyParts[i];

            if (Vector3.Distance(bodyPart.transform.position, targetPosition) > 0.1f) // Mesafe kontrolü ekledim
            {
                bodyPart.transform.position = Vector3.Lerp(bodyPart.transform.position, targetPosition, train.bodyFollowSpeed * Time.deltaTime);
                bodyPart.transform.LookAt(targetPosition);
            }
            else
            {
                bodyPart.transform.position = targetPosition; // Doðrudan pozisyon atama
            }
        }

    }

    public override void UpdateState()
    {
        train.groundedPlayer = train.characterController.isGrounded;
        if (train.groundedPlayer && train.playerVelocity.y < 0)
        {
            train.playerVelocity.y = 0f;
        }
        FollowBodyParts();
        currentFrame++;
        if (currentFrame % updateFrameInterval == 0)
        {
            
        }
        // Sürekli hareket
        Vector3 move = train.transform.forward * Time.deltaTime * train.playerSpeed;
        train.characterController.Move(move);

        // Tekerlek izlerini kontrol etmek için dönüþ iþlemleri
        bool isTurning = false;
        // Dönüþ iþlemleri
        if (train.leftTurnAction.ReadValue<float>() > 0)
        {
            train.transform.Rotate(Vector3.up, -train.turnSpeed * Time.deltaTime);
            isTurning = true; // Tren dönüyor
        }
        if (train.rightTurnAction.ReadValue<float>() > 0)
        {
            train.transform.Rotate(Vector3.up, train.turnSpeed * Time.deltaTime);
            isTurning = true; // Tren dönüyor
        }
        if (train.groundedPlayer)
        {
            train.RLWTireSkid.emitting = isTurning;
            train.RRWTireSkid.emitting = isTurning;
        }
        else
        {
            train.RLWTireSkid.emitting = false;
            train.RRWTireSkid.emitting = false;
        }
        // Yerçekimi uygulamasý
        train.playerVelocity.y += train.gravityValue * Time.deltaTime;
        train.characterController.Move(train.playerVelocity * Time.deltaTime);
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
      
    }

    public override void ExitState()
    {
        // Character Controller kullanýldýðý için Rigidbody kýsýtlamalarýný kaldýrmaya gerek yok
    }
}
