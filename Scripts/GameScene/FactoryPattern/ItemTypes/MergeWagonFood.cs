using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeWagonFood : MonoBehaviour, IItem
{
    [SerializeField] int wagonNumber;
    [SerializeField] Color wagonColor = Color.white;
    public Train train;
    public GoldValues goldValues;
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Initialize()
    {
        
    }

    public void MakeSound()
    {
       
    }

    public void MoveTo(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Train")
        {

            Debug.Log("Büyüyoruz");
            train.AddMoney(goldValues.wagonGold);
            AudioManager.Instance.PlayItemCollect();
            //int number = (int)Math.Pow(2, wagonNumber);
            Debug.Log("Wagon numberssssssssss : " + 2 * (1 + wagonNumber));
            //if(number==1)
            //    number = 2;
            train.GetColorAndNumber(wagonNumber, wagonColor);
            train.wagonNumbers.Add(wagonNumber);
            train.TransitionToState(train.WagonMergeState);
            Hide();
        }

    }

}
