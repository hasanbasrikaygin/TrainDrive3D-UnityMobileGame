using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CleanStartState : TrainState
{
    public CleanStartState(Train train) : base(train) { }
    private float duration = .3f;  // Her bir noktaya geçiþ süresi
    private PathType pathType = PathType.CatmullRom;  // Yol oluþturma þekli
    private int initialWagonCount = 0;

    public override void EnterState()
    {


        train.isShieldActive = true;
        train.gameOverCanvas.SetActive(false);

        if (train.mod == 0)
        {
            //    Mevcut vagon sayýsýný kaydet
            initialWagonCount = train.bodyParts.Count;

            // Tüm vagonlarý temizle ve havuza geri gönder
            train.ReturnAllWagonsToPool();

            // Treni baþlangýç pozisyonuna ayarla
            train.playerVelocity = Vector3.zero;
            train.bodyParts.Clear();
            train.positions.Clear();
            Vector3 startPosition = train.cleanStartPathPoints[0].position;
            train.transform.position = startPosition;
            train.positions.Insert(0, startPosition);

            // Önceki vagon sayýsý kadar yeniden vagon ekle
            for (int i = 0; i < initialWagonCount; i++)
            {
                GameObject wagon = train.GetWagonFromPool();
                train.bodyParts.Add(wagon);
            }

            // Kuyruklarý trenin arkasýna düzgün bir þekilde yerleþtir
            for (int i = 0; i < train.bodyParts.Count; i++)
            {
                Vector3 offsetPosition = startPosition - train.transform.forward * (i + 1) * train.bodyDistance;
                train.bodyParts[i].transform.position = offsetPosition;
                train.bodyParts[i].transform.rotation = Quaternion.identity;
            }

            // Treni hareket ettirmeye baþla
        }
        else
        {
            Vector3 startPosition = train.cleanStartPathPoints[0].position;
            train.transform.position = startPosition;
        }
        train.transform.LookAt(train.cleanStartLookAtTarget.transform);
        train.StartCoroutine(MoveTrain());
    }

    public override void ExitState() 
    {
        train.isShieldActive = false;
        if (train.speedLinesEffectIn!=null)
        {
            train.speedLinesEffectIn.Play();
        }
    }

    public override void UpdateState()
    {
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
        FollowBodyParts();
    }

    private IEnumerator MoveTrain()
    {
        yield return new WaitForSeconds(.1f);

        // Ýlk noktadan ikinciye geçiþ yap
        train.transform.DOMove(train.cleanStartPathPoints[1].position, 1f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                // Diðer noktalara geçiþ yap, son noktada dur ve MoveState'e geç
                Vector3[] remainingPath = new Vector3[train.cleanStartPathPoints.Length - 2];
                for (int i = 2; i < train.cleanStartPathPoints.Length; i++)
                {
                    remainingPath[i - 2] = train.cleanStartPathPoints[i].position;
                }

                // Yalnýzca geri kalan noktalar üzerinden bir yol oluþtur
                train.transform.DOPath(remainingPath, (remainingPath.Length) * duration, pathType)
                    .SetEase(Ease.Linear)
                    .SetLookAt(1f, Vector3.forward, Vector3.up)
                    .SetOptions(false, AxisConstraint.None)
                    .OnComplete(() =>
                    {
                        Debug.Log("Tren hedefine ulaþtý!");
                        train.TransitionToState(train.MoveState); // MoveState'e geçiþ
                    });
                train.scoreManager.StartScore();
                train.animator.SetBool("flying", false);
            });
    }
    public override void FixedUpdateState()
    {

    }


    private void FollowBodyParts()
    {
        for (int i = 0; i < train.bodyParts.Count; i++)
        {
            int index = Mathf.Min((i + 1) * Mathf.RoundToInt(train.bodyDistance / train.positionRecordInterval), train.positions.Count - 1);
            Vector3 targetPosition = train.positions[index];
            GameObject bodyPart = train.bodyParts[i];

            if (Vector3.Distance(bodyPart.transform.position, targetPosition) > 0.1f) // Mesafe kontrolü ekledim
            {
                bodyPart.transform.position = Vector3.Lerp(bodyPart.transform.position, targetPosition, 300f * Time.deltaTime);
                bodyPart.transform.LookAt(targetPosition);
            }
            else
            {
                bodyPart.transform.position = targetPosition; // Doðrudan pozisyon atama
            }
        }
    }
}
