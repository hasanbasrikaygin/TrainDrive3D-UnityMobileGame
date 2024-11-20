using System.Collections.Generic;
using UnityEngine;

public class MergeWagonPool : MonoBehaviour
{
    public GameObject wagonPrefab;
    //public List<GameObject> wagonNumbers;
    //public MeshRenderer meshRenderer;
    // Kullanýlmayan (Free) ve kullanýlan (Used) vagonlarýn listesi
    private List<GameObject> freeList;
    private List<GameObject> usedList;
    public List<GameObject> poolWagons;

    // Pool baþlangýç boyutu
    public int initialPoolSize = 10;

    void Start()
    {
        // FreeList ve UsedList'i initialize ediyoruz 
        freeList = new List<GameObject>();
        usedList = new List<GameObject>();

        // Pool'u baþlangýç boyutuna göre dolduruyoruz 
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = Instantiate(wagonPrefab);
            obj.transform.SetParent(transform);
            obj.SetActive(false);
            freeList.Add(obj);
        }
    }

    // Pool'dan vagon çekme
    public GameObject GetWagon( int number , Color color)
    {
        GameObject wagon;

        // Eðer FreeList'te vagon varsa
        if (freeList.Count > 0)
        {
            wagon = freeList[0];
            freeList.RemoveAt(0); // FreeList'ten çýkar
        }
        else
        {
            // Eðer FreeList'te vagon kalmamýþsa yeni bir vagon instantiate ederiz
            wagon = Instantiate(wagonPrefab);
            wagon.transform.SetParent(transform);
        }

        // Vagonu UsedList'e ekleriz ve aktif hale getiririz
        usedList.Add(wagon);
        //Debug.Log("name " + wagonNumbers[number].name);
        wagon.GetComponent<MergeWagon>().ApplyWagonNumber(number);
        wagon.SetActive(true);
        return wagon;
    }

    // Vagonu pool'a geri koyma
    public void ReturnWagon(GameObject wagon)
    {
        // Vagonu UsedList'ten çýkarýrýz
        if (usedList.Remove(wagon))
        {
            // Vagonu inaktif hale getirip FreeList'e ekleriz
            wagon.SetActive(false);
            freeList.Add(wagon);
        }
        else
        {
            Debug.LogWarning("Vagon UsedList'te bulunamadý.");
        }
    }

    // Pool'daki tüm vagonlarý resetleme ve FreeList'e geri koyma
    public void ResetPool()
    {
        foreach (var wagon in usedList)
        {
            wagon.SetActive(false);
            freeList.Add(wagon);
        }

        // Kullanýlan vagonlar listesini temizliyoruz
        usedList.Clear();
    }

    // Kullanýlan vagon sayýsýný döndürme
    public int GetUsedCount()
    {
        return usedList.Count;
    }

    // Kullanýlmayan vagon sayýsýný döndürme
    public int GetFreeCount()
    {
        return freeList.Count;
    }
}
