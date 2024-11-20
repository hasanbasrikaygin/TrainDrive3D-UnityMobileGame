using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainTail : MonoBehaviour
{
    public Train train;
    public GameManager gameManager;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tail")
        {
            if (!train.isShieldActive)
            {
                Debug.Log("Game Over");
                gameManager.GameOver();
                //train.TransitionToState(train.GrowState);

            }

        }
    }
    public void ReturnPool()
    {

    }
}
