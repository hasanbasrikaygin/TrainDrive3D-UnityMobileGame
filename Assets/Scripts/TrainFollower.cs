using DG.Tweening.Core.Easing;
using UnityEngine;

public class TrainFollower : MonoBehaviour
{
    public Transform trainTransform; // Tren objesinin referansý
    public GameObject stationChef;
    public GameManager gameManager;
    public Train train;
    public BoxCollider boxCollider;
    void LateUpdate()
    {
        if (trainTransform != null)
        {
            // TrainRig objesini trenin konumuyla eþitle
            transform.position = trainTransform.position;
            stationChef.transform.LookAt(trainTransform.transform.position);      
        }

    }
    //private void OnTriggerEnter(Collider other)
    //{   
    //    if (other.tag == "Tail")
    //    {
    //        if (!train.isShieldActive)
    //        {
    //            Debug.Log("Game Over");
    //            gameManager.GameOver();
    //            //train.TransitionToState(train.GrowState);

    //        }

    //    }
    //}
}
