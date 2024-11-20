using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreaksOnCollide : MonoBehaviour
{
    public GameObject EffectOnCollide;
    public string CollideMeshName;
    public float EffectScale;
    private Vector3 Scale;
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == CollideMeshName)
        {
            Scale = gameObject.GetComponent<Transform>().localScale * EffectScale;
            EffectOnCollide.transform.localScale = new Vector3(Scale.x, Scale.y, Scale.z);
            Instantiate(EffectOnCollide, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}