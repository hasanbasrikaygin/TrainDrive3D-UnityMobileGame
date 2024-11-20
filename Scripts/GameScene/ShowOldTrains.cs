using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOldTrains : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<MeshRenderer>().enabled = true;
    }
}
