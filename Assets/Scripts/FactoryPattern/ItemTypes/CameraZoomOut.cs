using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomOut : MonoBehaviour, IItem
{
    public ItemBarManager barManager;
    public Train train;
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Initialize()
    {
        // Engelin ba�lat�lmas� i�in gerekli kod
    }

    public void MakeSound()
    {

    }

    public void MoveTo(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Train")
        {
            AudioManager.Instance.PlayZoomOut();
            barManager.ActivateBar("zoomout", 15f);
            Debug.Log("B�y�yoruz");
            train.CameraZoomOutState();
            Hide();
        }

    }

}
