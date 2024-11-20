using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class GrowState : TrainState
{
    public GrowState(Train train) : base(train) { }
    float moveDistance = 3f; // Oklar�n yukar� a�a�� hareket mesafesi
    float moveDuration = 1f; // Hareket s�resi
    float rotateAmount = 5f; // Oklar�n d�n�� miktar�
    float rotateDuration = 1f; // D�n�� s�resi
    public override void EnterState()
    {

        Grow(); // Segment ekleme i�lemi

        train.ReturnToPreviousState();
        //train.TransitionToState(train.MoveState);
    }

    public override void UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            train.TransitionToState(train.RewindState);
        }
    }
    private void Grow()
    {
        Vector3 newPartPosition = train.bodyParts.Count == 0 ?
            train.transform.position - train.transform.forward * train.bodyDistance :
            train.bodyParts[train.bodyParts.Count - 1].transform.position -
            train.bodyParts[train.bodyParts.Count - 1].transform.forward * train.bodyDistance;

        GameObject newBodyPart = train.GetWagonFromPool();
        if (train.bodyParts.Count == 0)
        {
            // �lk vagonda �arp��may� devre d��� b�rak.
            newBodyPart.GetComponent<Collider>().enabled = false;
        }
        newBodyPart.transform.position = newPartPosition;
        newBodyPart.transform.rotation = Quaternion.identity;

        train.bodyParts.Add(newBodyPart);
        
    }
    public override void FixedUpdateState()
    {

    }
    public override void ExitState()
    {
        if(train.bodyParts.Count > 9 && train.mod == 0)
        {

            train.rightDoorAnimator.SetBool("isOpen", true);
            train.leftDoorAnimator.SetBool("isOpen", true);
            train.itemManager.isWagonCountFull=true;
            train.itemManager.HideFoodWagons();
            AnimateArrow(train.arrow1);
            AnimateArrow(train.arrow2);
            AnimateArrow(train.arrow3);
        }
        train.UpdateUI();
    }
    void AnimateArrow(GameObject arrow)
    {
        arrow.SetActive(true);
        // Yukar� a�a�� hareket
        LeanTween.moveLocalY(arrow, arrow.transform.localPosition.y + moveDistance, moveDuration).setEase(LeanTweenType.easeInOutSine).setLoopPingPong();

        // Hafif d�nme
        LeanTween.rotateZ(arrow, rotateAmount, rotateDuration).setEase(LeanTweenType.easeInOutSine).setLoopPingPong();
    }
}
