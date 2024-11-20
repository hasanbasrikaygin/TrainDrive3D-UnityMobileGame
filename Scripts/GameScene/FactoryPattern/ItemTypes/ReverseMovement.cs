using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseMovement : MonoBehaviour, IItem
{
    private AudioSource audioSource;
    [SerializeField] private GameObject particleSystem;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void Initialize()
    {
        Debug.Log("ReverseMovement Initialized");
        particleSystem.SetActive(true);
    }
    public void MakeSound()
    {

    }

    public void MoveTo(Vector3 newPosition)
    {
        throw new System.NotImplementedException();
    }

    public void Show()
    {
        throw new System.NotImplementedException();
    }

    public void Hide()
    {
        throw new System.NotImplementedException();
    }
}
