using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagonCleaner : MonoBehaviour
{
    public bool isCleanTime=false;
    public Train train;
    public void OnTriggerEnter(Collider other)
    {
     
        if(isCleanTime && other.CompareTag("Tail"))
        {
            train.ReturnWagonToPool(other.gameObject);
        }
    }
    public IEnumerator CleanTimer()
    {
        isCleanTime = true;
        yield return new WaitForSeconds(1);
        isCleanTime=false;
    }
}
