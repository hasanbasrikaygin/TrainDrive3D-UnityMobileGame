using DG.Tweening;
using System.Collections;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class RewindState : TrainState
{

    
    public RewindState(Train train) : base(train) { }

    public override void EnterState()
    {
        train.isShieldActive = true;
        train.gameOverCanvas.SetActive(false);
        

        // Kanatlar� aktif et
        train.wings.SetActive(true);
        train.wingsAnimator.SetBool("wingsUp", true);
        train.wingsAnimator.speed = 1.5f;
        // Trenin kafas�n� son kuyru�a g�re hizala, d�nd�r ve ta��may� ba�lat
        AlignAndMoveHeadToLastTail();
    }

    public override void ExitState()
    {
        train.isShieldActive = false;
        train.wingsAnimator.SetBool("wingsUp", false);
        train.wingsAnimator.speed = 1.0f;
        Debug.Log("Exit Rewind State");
        train.wings.SetActive(false);

        // Rigidbody'i eski haline d�nd�r
        //train.rb.isKinematic = wasKinematic;
    }

    public override void UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            train.TransitionToState(train.GrowState);
        }
    }

    private void AlignAndMoveHeadToLastTail()
    {
        // E�er kuyruk yoksa i�lemi durdur
        if (train.bodyParts.Count == 0) return;

        // Son kuyru�u al
        GameObject lastTail = train.bodyParts[train.bodyParts.Count - 1];

        // Rotasyonu ve pozisyonu hizala
        Quaternion targetRotation = lastTail.transform.rotation * Quaternion.Euler(0, 180, 0);
        Vector3 targetPosition = lastTail.transform.position - lastTail.transform.forward * train.bodyDistance;

        // DOTween ile yumu�ak ge�i� yap
        Sequence alignSequence = DOTween.Sequence();
        float distance = Vector3.Distance(train.transform.position, targetPosition);
        float moveDuration = distance * .05f;
        // 1. Ad�m: Y�kselme
        float ascentHeight = 5f; // Y�kselme y�ksekli�i
        float ascentDuration = .7f; // Y�kselme s�resi
        alignSequence.Append(train.transform.DOMoveY(train.transform.position.y + ascentHeight, ascentDuration).SetEase(Ease.InOutQuad));

        // 2. Ad�m: Rotasyonu hizala ve hedef pozisyona do�ru hareket
        
        alignSequence.Append(train.transform.DORotateQuaternion(targetRotation, moveDuration).SetEase(Ease.InOutQuad));
        alignSequence.Join(train.transform.DOMoveX(targetPosition.x, moveDuration).SetEase(Ease.InOutQuad));
        alignSequence.Join(train.transform.DOMoveZ(targetPosition.z, moveDuration).SetEase(Ease.InOutQuad));

        // 3. Ad�m: Al�alma
        float descentDuration = .5f; // Al�alma s�resi
        alignSequence.Append(train.transform.DOMoveY(targetPosition.y, descentDuration).SetEase(Ease.InOutQuad));

        // Kuyruklar� tersine �evir
        alignSequence.OnComplete(() =>
        {
            ReverseTails();
            Debug.Log("Rewind Ba�lad�!");
        });

        alignSequence.Play();
    }
    public override void FixedUpdateState()
    {

    }
    private void ReverseTails()
    {
        // Kuyruk listesini tersine �evir
        if (train.mod == 0)
        {
            train.bodyParts.Reverse();
        }
            
        train.positions.Reverse();
        train.animationManager.StartCountdown();
    }

}
