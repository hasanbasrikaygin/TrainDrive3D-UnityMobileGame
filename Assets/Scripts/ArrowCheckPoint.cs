using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCheckPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Train")
        {
            gameObject.SetActive(false);
            LeanTween.cancel(gameObject);
        }
    }
}
