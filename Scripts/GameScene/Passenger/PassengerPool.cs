using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerPool : MonoBehaviour
{
    public static PassengerPool Instance;

    public GameObject passengerPrefab;
    public int initialPoolSize = 10;
    public List<Transform> waitingPoints = new List<Transform>();

    private List<GameObject> freeList = new List<GameObject>();
    private List<GameObject> usedList = new List<GameObject>();
    public List<GameObject> passengers;
    public Animator chefAnimator;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {

            //GameObject passenger = Instantiate(passengerPrefab);
            passengers[i].SetActive(true); // Yolcu baþlangýçta aktif ve kullanýlabilir olmalý
            passengers[i].transform.position = waitingPoints[i].position;
            freeList.Add(passengers[i]);
            Debug.Log("Passenger instantiated and added to freeList: " + passengers[i].name);
        }
    }


    private void InitializeWaitingPoints()
    {
        for (int i = 0; i < waitingPoints.Count; i++)
        {
            GameObject passenger = GetPassenger();
            if (passenger != null)
            {
                //passenger.transform.position = waitingPoints[i].position;
                passenger.SetActive(true);
            }
        }
    }

    public GameObject GetPassenger()
    {
        if (freeList.Count > 0)
        {
            int lastIndex = freeList.Count - 1;
            GameObject passenger = freeList[lastIndex];
            freeList.RemoveAt(lastIndex);
            usedList.Add(passenger);
            passenger.SetActive(true);
            return passenger;
        }

        return null; // Eðer havuzda kullanýlabilir yolcu yoksa
    }


    public void ReturnPassenger(GameObject passenger)
    {

        passenger.SetActive(false);
        usedList.Remove(passenger);
        freeList.Add(passenger);
        GetPassenger();
        //StartCoroutine(MovePassengerToStation(passenger));
        //StartCoroutine(MovePassengerToStation(passenger));
    }

    //private IEnumerator MovePassengerToStation(GameObject passenger)
    //{
    //    yield return new WaitForSeconds(2f); // Belirli süre bekle
    //    for (int i = 0; i < waitingPoints.Count; i++)
    //    {
    //        passenger.transform.position = waitingPoints[i].position;
    //        GetPassenger();
    //        break;
    //        if (waitingPointsCanUse[i] == true)
    //        {
                
    //            waitingPointsCanUse[i] = false;
                
    //        }
    //    }
    //}

    public List<GameObject> GetAllFreePassengers()
    {
        return new List<GameObject>(freeList);
    }
    public void FinishStatePassengerAnimations()
    {
        chefAnimator.SetBool("isFinished", true);
        foreach (GameObject passenger in passengers)
        {
            if (passenger != null)
            {
                passenger.GetComponent<Passenger>().FinishCheering();
            }
        }
    }
}
