using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotionFactory : Factory
{
    [SerializeField] private SlowMotion preInstantiatedSlowMotion; // Reference to the pre-instantiated object in the scene}

    public override IItem CreateItem(Vector3 position)
    {
        // Reposition the pre-instantiated wings and ensure they are active
        preInstantiatedSlowMotion.transform.position = position;
        preInstantiatedSlowMotion.gameObject.SetActive(true);

        preInstantiatedSlowMotion.Initialize();
        preInstantiatedSlowMotion.MakeSound();

        return preInstantiatedSlowMotion;
    }
}
