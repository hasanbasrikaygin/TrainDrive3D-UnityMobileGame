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
        

        // Kanatlarý aktif et
        train.wings.SetActive(true);
        train.wingsAnimator.SetBool("wingsUp", true);
        train.wingsAnimator.speed = 1.5f;
        // Trenin kafasýný son kuyruða göre hizala, döndür ve taþýmayý baþlat
        AlignAndMoveHeadToLastTail();
    }

    public override void ExitState()
    {
        train.isShieldActive = false;
        train.wingsAnimator.SetBool("wingsUp", false);
        train.wingsAnimator.speed = 1.0f;
        Debug.Log("Exit Rewind State");
        train.wings.SetActive(false);

        // Rigidbody'i eski haline döndür
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
        // Eðer kuyruk yoksa iþlemi durdur
        if (train.bodyParts.Count == 0) return;

        // Son kuyruðu al
        GameObject lastTail = train.bodyParts[train.bodyParts.Count - 1];

        // Rotasyonu ve pozisyonu hizala
        Quaternion targetRotation = lastTail.transform.rotation * Quaternion.Euler(0, 180, 0);
        Vector3 targetPosition = lastTail.transform.position - lastTail.transform.forward * train.bodyDistance;

        // DOTween ile yumuþak geçiþ yap
        Sequence alignSequence = DOTween.Sequence();
        float distance = Vector3.Distance(train.transform.position, targetPosition);
        float moveDuration = distance * .05f;
        // 1. Adým: Yükselme
        float ascentHeight = 5f; // Yükselme yüksekliði
        float ascentDuration = .7f; // Yükselme süresi
        alignSequence.Append(train.transform.DOMoveY(train.transform.position.y + ascentHeight, ascentDuration).SetEase(Ease.InOutQuad));

        // 2. Adým: Rotasyonu hizala ve hedef pozisyona doðru hareket
        
        alignSequence.Append(train.transform.DORotateQuaternion(targetRotation, moveDuration).SetEase(Ease.InOutQuad));
        alignSequence.Join(train.transform.DOMoveX(targetPosition.x, moveDuration).SetEase(Ease.InOutQuad));
        alignSequence.Join(train.transform.DOMoveZ(targetPosition.z, moveDuration).SetEase(Ease.InOutQuad));

        // 3. Adým: Alçalma
        float descentDuration = .5f; // Alçalma süresi
        alignSequence.Append(train.transform.DOMoveY(targetPosition.y, descentDuration).SetEase(Ease.InOutQuad));

        // Kuyruklarý tersine çevir
        alignSequence.OnComplete(() =>
        {
            ReverseTails();
            Debug.Log("Rewind Baþladý!");
        });

        alignSequence.Play();
    }
    public override void FixedUpdateState()
    {

    }
    private void ReverseTails()
    {
        // Kuyruk listesini tersine çevir
        if (train.mod == 0)
        {
            train.bodyParts.Reverse();
        }
            
        train.positions.Reverse();
        train.animationManager.StartCountdown();
    }

}
