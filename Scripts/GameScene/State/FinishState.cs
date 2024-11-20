using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FinishState : TrainState
{
    private int initialWagonCount = 0;
    public FinishState(Train train) : base(train) { }
    private float duration = .6f;  // Her bir noktaya ge�i� s�resi
    private PathType pathType = PathType.CatmullRom;  // Yol olu�turma �ekli
    private bool wagonsRemoved = false; // Silme i�lemi i�in bayrak
    
    
    public override void EnterState()
    {
        AudioManager.Instance.StopBackgroundMusic();
        initialWagonCount = train.bodyParts.Count;
        train.cinemachineState.SetBool("isFinished", true);




            for (int i = 0; i < train.bodyParts.Count; i++)
            {
                train.bodyParts[i].SetActive(false); // Vagonu g�r�nmez yap
            }


        Vector3 startPosition = train.finishPathPoints[0].position;
        train.transform.position = startPosition;
        train.transform.LookAt(train.finishLookTarget.transform);
        train.positions.Insert(0, startPosition);
        
        train.StartCoroutine(MoveTrain());
    }

    public override void ExitState()
    {
        
    }

    public override void FixedUpdateState()
    {
        //train.transform.LookAt(train.finishLookTarget.transform);
    }

    public override void UpdateState()
    {

    }

    private IEnumerator MoveTrain()
    {
        yield return new WaitForSeconds(.1f);
        AudioManager.Instance.PlayFinish();
        // �lk noktadan ikinciye ge�i� yap
        train.transform.DOMove(train.finishPathPoints[1].position, 1f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                // Di�er noktalara ge�i� yap, son noktada dur ve MoveState'e ge�
                Vector3[] remainingPath = new Vector3[train.finishPathPoints.Length - 2];
                for (int i = 2; i < train.finishPathPoints.Length; i++)
                {
                    remainingPath[i - 2] = train.finishPathPoints[i].position;
                }

                // Yaln�zca geri kalan noktalar �zerinden bir yol olu�tur
                train.transform.DOPath(remainingPath, (remainingPath.Length) * duration, pathType)
                    .SetEase(Ease.Linear)
                    .SetLookAt(1f, Vector3.forward, Vector3.up)
                    .SetOptions(false, AxisConstraint.None)
                    .OnComplete(() =>
                    {
                        Debug.Log("Tren hedefine ula�t�!");
                        train.TransitionToState(train.IdleState); // MoveState'e ge�i�
                    });

                train.scorePanel.SetActive(true);
            });
    }
}
