using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostFactory : Factory
{

    [SerializeField] private SpeedBoost preInstantiatedSpeedBoost; // Reference to the pre-instantiated object in the scene}

    public override IItem CreateItem(Vector3 position)
    {
        // Reposition the pre-instantiated wings and ensure they are active
        preInstantiatedSpeedBoost.transform.position = position;
        preInstantiatedSpeedBoost.gameObject.SetActive(true);

        preInstantiatedSpeedBoost.Initialize();
        preInstantiatedSpeedBoost.MakeSound();

        return preInstantiatedSpeedBoost;
    }
}
