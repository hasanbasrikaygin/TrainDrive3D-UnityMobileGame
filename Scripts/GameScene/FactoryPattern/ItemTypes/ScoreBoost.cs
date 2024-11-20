using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoost : MonoBehaviour, IItem
{
    public ItemBarManager barManager;
    public ScoreManager scoreManager;
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
            Debug.Log("Büyüyoruz");
            scoreManager.ActivateMultiplierBoost();
            AudioManager.Instance.PlayItemCollect();
            AudioManager.Instance.PlayScoreBooster();
            barManager.ActivateBar("x2", 12f);
            Hide();
        }

    }
}
