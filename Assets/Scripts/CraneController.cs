using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneController : MonoBehaviour
{
    public Transform craneHook; // Vincin ucu
    public bool isWagonLoaded = false;
    public Train train;
    int index = 0;
    public List<GameObject> wagons = new List<GameObject>();
    public WagonCleaner cleaner;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tail")) // Vagonlar� 'Wagon' tag'� ile ay�rabilirsiniz
        {
            other.transform.SetParent(craneHook);
            wagons.Add(other.gameObject);
        }
        if (other.CompareTag("WagonDropZone"))
        {
            foreach (GameObject wagon in wagons)
            {
                train.ReturnWagonToPool(wagon);
            }
            wagons.Clear();
            GetComponent<BoxCollider>().enabled = false;
           StartCoroutine(cleaner.CleanTimer());
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WagonDropZone"))
        {

            GetComponent<BoxCollider>().enabled = false;
        }
    }
    void AttachWagonToCrane(GameObject wagon)
    {
        // Vagonun vin�le birlikte hareket etmesini sa�lamak i�in parent-child ili�kisi kuruyoruz.

        // Vagonun vin� ucuna sabitlenmesini sa�l�yoruz
        //wagon.transform.localPosition = Vector3.zero; // Kancaya merkezde yap���r
        //wagon.transform.localRotation = Quaternion.identity; // Vagonun rotasyonu vin�le hizalan�r
    }

    
    void ReleaseWagon(GameObject wagon)
    {
        // Vagonu serbest b�rak�yoruz (art�k ba��ms�z olacak)
        //wagon.transform.SetParent(null);
        //wagon.transform.SetParent(null);
    }

}
