using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class GrowState : TrainState
{
    public GrowState(Train train) : base(train) { }
    float moveDistance = 3f; // Oklarýn yukarý aþaðý hareket mesafesi
    float moveDuration = 1f; // Hareket süresi
    float rotateAmount = 5f; // Oklarýn dönüþ miktarý
    float rotateDuration = 1f; // Dönüþ süresi
    public override void EnterState()
    {

        Grow(); // Segment ekleme iþlemi

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
            // Ýlk vagonda çarpýþmayý devre dýþý býrak.
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
        // Yukarý aþaðý hareket
        LeanTween.moveLocalY(arrow, arrow.transform.localPosition.y + moveDistance, moveDuration).setEase(LeanTweenType.easeInOutSine).setLoopPingPong();

        // Hafif dönme
        LeanTween.rotateZ(arrow, rotateAmount, rotateDuration).setEase(LeanTweenType.easeInOutSine).setLoopPingPong();
    }
}
