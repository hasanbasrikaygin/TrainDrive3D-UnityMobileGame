using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Wings : MonoBehaviour, IItem
{
    public string itemKey; // Itemi ayýrt eden key
    public float itemDuration; // Itemin etkili olduðu süre
    public GameObject itemBar; // Bar objesi
    public Train train;
    public ItemBarManager barManager;
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Initialize()
    {
        // Engelin baþlatýlmasý için gerekli kod
        barManager = FindObjectOfType<ItemBarManager>(); // ItemBarManager'i bul
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
            AudioManager.Instance.PlayItemCollect();
            Debug.Log("Büyüyoruz");
            train.TransitionToState(train.FlyingState);
            barManager.ActivateBar("wings", 10f);
            Hide();
        }
        
    }
}
