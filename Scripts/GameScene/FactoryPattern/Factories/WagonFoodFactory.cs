using System.Collections.Generic;
using UnityEngine;

public class WagonFoodFactory : Factory
{
    [SerializeField] private WagonFood foodPrefab;
    [SerializeField] List<WagonFood> foodList;
    int selectedTrainIndex;

    public override IItem CreateItem(Vector3 position)
    {
        selectedTrainIndex = PlayerPrefs.GetInt("SelectedTrainIndex", 0);

        GameObject foodInstance = Instantiate(foodList[selectedTrainIndex].gameObject, position, Quaternion.identity);

        foodInstance.transform.SetParent(transform);

        // Yeni yiyeceði initialize et ve sesini oynat
        WagonFood newFood = foodInstance.GetComponent<WagonFood>();
        newFood.Hide();
        newFood.Initialize();
        newFood.MakeSound();

        return newFood;
    }

}
