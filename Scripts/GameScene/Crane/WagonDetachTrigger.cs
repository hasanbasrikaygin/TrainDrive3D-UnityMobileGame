using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Android;

public class WagonDetachTrigger : MonoBehaviour
{
    // Start is called before the first frame update

    public Train train;
    public PassengerController passengerController;
    public List<GameObject> releseWagonText;
    int counter =0;
    public ItemManager itemManager;
    public Eagle eagle;


    public GameObject EndBox_Right;
    public GameObject EndBox_Left;
    public CraneController craneController;
    public Animator craneAnimator;

    public GameObject arrow1;
    public GameObject arrow2;
    public GameObject arrow3;

    public GoldValues goldValues;
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (train.mod == 0 && train.bodyParts.Count>9)
        {
            if (other.tag == "Train")
            {
                
                if(counter<releseWagonText.Count)
                {
                    releseWagonText[counter].SetActive(true);
                    counter++;
                }
                train.TransitionToState(train.ReleaseState);
                train.coinEffect.Play();
                passengerController.AddMoney(goldValues.dropWagonsGold);

                passengerController.wagonCount = 0;
                itemManager.isWagonCountFull = false;
                itemManager.ShowFoodWagons();

                LeanTween.cancel(arrow1);
                LeanTween.cancel(arrow2);
                LeanTween.cancel(arrow3);
                arrow1.SetActive(false);
                arrow2.SetActive(false);
                arrow3.SetActive(false);


                if (eagle != null)
                {
                    StartCoroutine(EagleFollowTrain());
                }

            }
        }

    }
   IEnumerator EagleFollowTrain()
    {

        yield return new WaitForSeconds(.5f);
        AudioManager.Instance.PlayEagleScream();
        craneAnimator.SetTrigger("CraneLift");
        eagle.isFollowing = true;
        yield return new WaitForSeconds(6f);
        AudioManager.Instance.PlayEagleScream();
        eagle.isFollowing = false;
        EndBox_Right.SetActive(false);
        EndBox_Left.SetActive(false);

    }
}
