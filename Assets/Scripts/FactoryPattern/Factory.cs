using UnityEngine;

public abstract class Factory : MonoBehaviour
{
    public abstract IItem CreateItem(Vector3 position);
}
