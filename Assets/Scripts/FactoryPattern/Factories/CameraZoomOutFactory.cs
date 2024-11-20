using UnityEngine;

public class CameraZoomOutFactory : Factory
{
    [SerializeField] private CameraZoomOut preInstantiatedZoomOut;
    public override IItem CreateItem(Vector3 position)
    {
        // Reposition the pre-instantiated wings and ensure they are active
        preInstantiatedZoomOut.transform.position = position;
        preInstantiatedZoomOut.gameObject.SetActive(true);

        preInstantiatedZoomOut.Initialize();
        preInstantiatedZoomOut.MakeSound();



        preInstantiatedZoomOut.Initialize();
        preInstantiatedZoomOut.MakeSound();
        return preInstantiatedZoomOut;
    }
}


