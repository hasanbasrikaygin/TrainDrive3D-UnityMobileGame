using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MergeFoodFactory : Factory
{

    [SerializeField] private MergeWagonFood[] wagonsInstance; // Sahnedeki �nceden var olan wagonlar�n referans�
    private int index = 0;
    public override IItem CreateItem(Vector3 position)
    {
        if (index > 15)
        {
            Debug.Log(" ======================================");
        }
        // Yeni yiyece�i initialize et ve sesini oynat
        MergeWagonFood newWagon = wagonsInstance[index];
        newWagon.Hide();
        newWagon.Initialize();
        newWagon.MakeSound();
        index++;
        return newWagon;
    }
}
