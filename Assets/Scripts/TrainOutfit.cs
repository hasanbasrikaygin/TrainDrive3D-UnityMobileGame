using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainOutfit : MonoBehaviour
{
    [SerializeField] GameObject parentObj;
    [SerializeField] List<GameObject> trainList;
    int selectedTrainIndex;
    void Start()
    {
        
        selectedTrainIndex = PlayerPrefs.GetInt("SelectedTrainIndex", 0);
        Debug.Log(selectedTrainIndex + " tiren");
        Debug.Log(trainList.Count + " tiren");
        if (selectedTrainIndex >= trainList.Count)
            selectedTrainIndex = 0;

        GameObject obj = Instantiate(trainList[selectedTrainIndex], parentObj.transform.position, Quaternion.identity);
        obj.transform.parent = parentObj.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
           // int random = Random.Range(0, trainList.Count);
            
            //GameObject foodInstance = Instantiate(foodPrefab.gameObject, spawnPosition, Quaternion.identity);

        }
    }

}
