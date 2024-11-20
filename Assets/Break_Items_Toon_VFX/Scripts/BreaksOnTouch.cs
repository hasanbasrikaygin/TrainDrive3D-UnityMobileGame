using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreaksOnTouch : MonoBehaviour
{
    public GameObject EffectOnTouch;
    public float EffectScale;
    private Vector3 Scale;
    void OnMouseDown()
    {
        Scale = gameObject.GetComponent<Transform>().localScale* EffectScale;
        EffectOnTouch.transform.localScale = new Vector3(Scale.x, Scale.y, Scale.z);
        Instantiate(EffectOnTouch, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
 