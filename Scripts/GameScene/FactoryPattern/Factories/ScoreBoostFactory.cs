using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoostFactory : Factory
{
    [SerializeField] private ScoreBoost preInstantiatedScoreBoost; // Reference to the pre-instantiated object in the scene}

    public override IItem CreateItem(Vector3 position)
    {
        // Reposition the pre-instantiated wings and ensure they are active
        preInstantiatedScoreBoost.transform.position = position;
        preInstantiatedScoreBoost.gameObject.SetActive(true);

        preInstantiatedScoreBoost.Initialize();
        preInstantiatedScoreBoost.MakeSound();

        return preInstantiatedScoreBoost;
    }
}
