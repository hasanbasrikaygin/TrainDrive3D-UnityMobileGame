using UnityEngine;

public class WingsFactory : Factory
{
    [SerializeField] private Wings preInstantiatedWings; // Reference to the pre-instantiated object in the scene

    public override IItem CreateItem(Vector3 position)
    {
        // Reposition the pre-instantiated wings and ensure they are active
        preInstantiatedWings.transform.position = position;
        preInstantiatedWings.gameObject.SetActive(true);

        preInstantiatedWings.Initialize();
        preInstantiatedWings.MakeSound();

        return preInstantiatedWings;
    }
}
