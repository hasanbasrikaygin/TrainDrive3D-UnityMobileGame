using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour, IItem
{
    public ItemBarManager barManager;
    public Train train;
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
            AudioManager.Instance.PlayItemShiled();
            Debug.Log("Büyüyoruz");
            //scoreManager.ActivateMultiplierBoost();
            //AudioManager.Instance.PlayItemCollect();
            //train.TransitionToState(train.FlyingState);
            barManager.ActivateBar("shield", 7f);
            
            Hide();
        }

    }
}
