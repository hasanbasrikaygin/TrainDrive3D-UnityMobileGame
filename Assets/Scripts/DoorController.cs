using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DoorController : MonoBehaviour
{
    public Animator leftDoorAnimator; // Kap� animat�r�
    public Animator rightDoorAnimator; // Kap� animat�r�
    public GameObject leftDoor; // Kap� animat�r�
    public GameObject rightDoor; // Kap� animat�r�
    public Train train;

    void Start()
    {
        if (train.mod == 0)
        {
            CloseDoor(); // Kap�y� ba�lang��ta kapal� yap�yoruz
           
        }
        else
        {
            leftDoor.SetActive(false);
            rightDoor.SetActive(false);
            gameObject.SetActive(false);
        }
        

    }

    // Bu fonksiyon sadece kap�y� kapat�r
    public void CloseDoor()
    {
        if (train.bodyParts.Count<8) // E�er kap� a��ksa
        {

            rightDoorAnimator.SetBool("isOpen", false); // Kapanma animasyonu ba�lar
            leftDoorAnimator.SetBool("isOpen", false); // Kapanma animasyonu ba�lar
            //doorAnimator.SetBool("Open", false); // A��lma animasyonu devre d���
        }
    }

    // E�er di�er nesneler trigger alan�na girerse kap� kapan�r
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Train")) // Nesne 'Train' etiketine sahipse
        {
            CloseDoor(); // Kap�y� kapat
        }
    }
}

