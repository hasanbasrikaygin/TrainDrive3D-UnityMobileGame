using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseMovementFactory : Factory
{
    [SerializeField] private ReverseMovement reverseMovementPrefab;

    public override IItem CreateItem(Vector3 position)
    {
        GameObject reverseMovementInstance = Instantiate(reverseMovementPrefab.gameObject,position,Quaternion.identity);
        ReverseMovement newReverseMovement = reverseMovementInstance.GetComponent<ReverseMovement>();

        newReverseMovement.Initialize();
        newReverseMovement.MakeSound();
        return newReverseMovement;
    }
}
