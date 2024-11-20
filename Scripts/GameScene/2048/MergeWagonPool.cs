using System.Collections.Generic;
using UnityEngine;

public class MergeWagonPool : MonoBehaviour
{
    public GameObject wagonPrefab;
    //public List<GameObject> wagonNumbers;
    //public MeshRenderer meshRenderer;
    // Kullan�lmayan (Free) ve kullan�lan (Used) vagonlar�n listesi
    private List<GameObject> freeList;
    private List<GameObject> usedList;
    public List<GameObject> poolWagons;

    // Pool ba�lang�� boyutu
    public int initialPoolSize = 10;

    void Start()
    {
        // FreeList ve UsedList'i initialize ediyoruz 
        freeList = new List<GameObject>();
        usedList = new List<GameObject>();

        // Pool'u ba�lang�� boyutuna g�re dolduruyoruz 
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = Instantiate(wagonPrefab);
            obj.transform.SetParent(transform);
            obj.SetActive(false);
            freeList.Add(obj);
        }
    }

    // Pool'dan vagon �ekme
    public GameObject GetWagon( int number , Color color)
    {
        GameObject wagon;

        // E�er FreeList'te vagon varsa
        if (freeList.Count > 0)
        {
            wagon = freeList[0];
            freeList.RemoveAt(0); // FreeList'ten ��kar
        }
        else
        {
            // E�er FreeList'te vagon kalmam��sa yeni bir vagon instantiate ederiz
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
        // Vagonu UsedList'ten ��kar�r�z
        if (usedList.Remove(wagon))
        {
            // Vagonu inaktif hale getirip FreeList'e ekleriz
            wagon.SetActive(false);
            freeList.Add(wagon);
        }
        else
        {
            Debug.LogWarning("Vagon UsedList'te bulunamad�.");
        }
    }

    // Pool'daki t�m vagonlar� resetleme ve FreeList'e geri koyma
    public void ResetPool()
    {
        foreach (var wagon in usedList)
        {
            wagon.SetActive(false);
            freeList.Add(wagon);
        }

        // Kullan�lan vagonlar listesini temizliyoruz
        usedList.Clear();
    }

    // Kullan�lan vagon say�s�n� d�nd�rme
    public int GetUsedCount()
    {
        return usedList.Count;
    }

    // Kullan�lmayan vagon say�s�n� d�nd�rme
    public int GetFreeCount()
    {
        return freeList.Count;
    }
}
