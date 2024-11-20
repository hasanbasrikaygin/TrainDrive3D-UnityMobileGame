using UnityEngine;

public interface IItem
{
    void Initialize();
    void MakeSound();

    void MoveTo(Vector3 newPosition);

    void Show();

    void Hide();
}
