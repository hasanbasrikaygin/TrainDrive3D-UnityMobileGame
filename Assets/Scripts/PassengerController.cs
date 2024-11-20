using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;  

public class PassengerController : MonoBehaviour
{
    public int wagonCount = 0;
    public Train train;
    [SerializeField]
    public GameObject chef;
    public Animator  chefAnimator;
    bool isThrow = false;
    public ParticleSystem chefMoneyEffect;
    private int currentGoldCount = 0;

    public TextMeshProUGUI goldCountText;
    public int gameGoldCount = 0;
    public GoldValues goldValues;
    private void Start()
    {
        chefAnimator = chef.GetComponent<Animator>();
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Degdi");
        if (other.CompareTag("Train"))
        {
            AudioManager.Instance.PlayTrainHorn();
            Debug.Log("Eslesti");
            SendTrainPosition();
        }
    }

    void SendTrainPosition()
    {
        isThrow = true;
        // Kullanýlabilir tüm yolcularý bir kez alýn ve bir listeye kaydedin
        List<GameObject> freePassengers = PassengerPool.Instance.GetAllFreePassengers();
        Debug.Log(freePassengers.Count + " -- " + train.bodyParts.Count + PassengerPool.Instance.GetAllFreePassengers().Count );
        // bosta yolcu varsa ve bosta vagon varsa
        for (int i = 0; i < freePassengers.Count && wagonCount < train.bodyParts.Count; i++) //&&  i < snake.bodyParts.Count
        {

            AddMoney(goldValues.passengerGold);

            GameObject passenger = freePassengers[i];
            // Yolcunun hedef objesini tren parçasý olarak ata
            passenger.GetComponent<Passenger>().targetObj = train.bodyParts[wagonCount];
            wagonCount++;
            Debug.Log(train.bodyParts[i].name);
            if(isThrow)
            {
                chefAnimator.SetTrigger("Throw");
                isThrow = false;
                StartCoroutine(ThrowMoney());

            }
            // Yolcunun hareket etmesini saðla
            passenger.GetComponent<Passenger>().PassengerMove(passenger.transform.position);
        }
    }
    IEnumerator ThrowMoney()
    {
        yield return new WaitForSeconds(1.5f);
        chefMoneyEffect.Play();
    }
    public void AddMoney(int money)
    {
        currentGoldCount = PlayerPrefs.GetInt("GoldAmount") + money;
        gameGoldCount += money;
        goldCountText.text = gameGoldCount.ToString();
        PlayerPrefs.SetInt("GoldAmount", currentGoldCount);
    }
}
  