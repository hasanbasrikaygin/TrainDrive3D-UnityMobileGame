using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldFactory : Factory
{

    [SerializeField] private Shield preInstantiatedShield; // Reference to the pre-instantiated object in the scene}

    public override IItem CreateItem(Vector3 position)
    {
        // Reposition the pre-instantiated wings and ensure they are active
        preInstantiatedShield.transform.position = position;
        preInstantiatedShield.gameObject.SetActive(true);

        preInstantiatedShield.Initialize();
        preInstantiatedShield.MakeSound();

        return preInstantiatedShield;
    }
}
