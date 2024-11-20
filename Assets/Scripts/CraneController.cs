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
        if (other.CompareTag("Tail")) // Vagonlarý 'Wagon' tag'ý ile ayýrabilirsiniz
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
        // Vagonun vinçle birlikte hareket etmesini saðlamak için parent-child iliþkisi kuruyoruz.

        // Vagonun vinç ucuna sabitlenmesini saðlýyoruz
        //wagon.transform.localPosition = Vector3.zero; // Kancaya merkezde yapýþýr
        //wagon.transform.localRotation = Quaternion.identity; // Vagonun rotasyonu vinçle hizalanýr
    }

    
    void ReleaseWagon(GameObject wagon)
    {
        // Vagonu serbest býrakýyoruz (artýk baðýmsýz olacak)
        //wagon.transform.SetParent(null);
        //wagon.transform.SetParent(null);
    }

}
