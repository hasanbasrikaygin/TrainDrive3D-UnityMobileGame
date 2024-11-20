using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DoorController : MonoBehaviour
{
    public Animator leftDoorAnimator; // Kapý animatörü
    public Animator rightDoorAnimator; // Kapý animatörü
    public GameObject leftDoor; // Kapý animatörü
    public GameObject rightDoor; // Kapý animatörü
    public Train train;

    void Start()
    {
        if (train.mod == 0)
        {
            CloseDoor(); // Kapýyý baþlangýçta kapalý yapýyoruz
           
        }
        else
        {
            leftDoor.SetActive(false);
            rightDoor.SetActive(false);
            gameObject.SetActive(false);
        }
        

    }

    // Bu fonksiyon sadece kapýyý kapatýr
    public void CloseDoor()
    {
        if (train.bodyParts.Count<8) // Eðer kapý açýksa
        {

            rightDoorAnimator.SetBool("isOpen", false); // Kapanma animasyonu baþlar
            leftDoorAnimator.SetBool("isOpen", false); // Kapanma animasyonu baþlar
            //doorAnimator.SetBool("Open", false); // Açýlma animasyonu devre dýþý
        }
    }

    // Eðer diðer nesneler trigger alanýna girerse kapý kapanýr
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Train")) // Nesne 'Train' etiketine sahipse
        {
            CloseDoor(); // Kapýyý kapat
        }
    }
}

