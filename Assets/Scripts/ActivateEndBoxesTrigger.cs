using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateEndBoxesTrigger : MonoBehaviour
{
    private int mod = 0;
    public GameObject EndBox_Right;
    public GameObject EndBox_Left;
    public CraneController craneController;
    public Train train;
    public BoxCollider boxCollider;
    public List<GameObject> releseWagonText;
    private void Start()
    {
        mod = PlayerPrefs.GetInt("gameModNumber", 0);
        foreach (GameObject letter in releseWagonText)
        {
            letter.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (mod == 0 && other.CompareTag("Train")&& train.bodyParts.Count>9)
        {

            EndBox_Left.SetActive(true);
            EndBox_Right.SetActive(true);
            boxCollider.enabled = true;
            AudioManager.Instance.PlayTrainHorn();
        }
    }
}
