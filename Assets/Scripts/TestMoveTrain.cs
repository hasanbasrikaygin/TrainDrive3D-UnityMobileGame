using DG.Tweening;
using UnityEngine;

public class TestMoveTrain : MonoBehaviour
{
    public Transform[] pathPoints;  // Trenimizin izleyece�i yolun noktalar�n� belirtiyoruz.
    public float duration = 10f;  // Trenimizin yolu ne kadar s�rede tamamlayaca��n� belirleyen s�re.
    public PathType pathType = PathType.CatmullRom;  // Yolu nas�l olu�turaca��m�z� belirleyen PathType se�ene�i.

    void Start()
    {
        // Nesneyi do�rudan yolun ilk noktas�na yerle�tiriyoruz.
        transform.position = pathPoints[0].position;

        // Treni harekete ge�iriyoruz.
        MoveTrain();
    }

    void MoveTrain()
    {
        Vector3[] path = new Vector3[pathPoints.Length];
        for (int i = 0; i < pathPoints.Length; i++)
        {
            path[i] = pathPoints[i].position;  // Her bir noktan�n pozisyonunu diziye ekliyoruz.
        }

        // Trenimizi belirledi�imiz yol boyunca hareket ettiriyoruz.
        transform.DOPath(path, duration, pathType)
                 .SetEase(Ease.Linear)  // Hareketin h�z� sabit tutuluyor.
                 .SetLookAt(0.01f)  // Nesnenin yol boyunca d�n�� yapmas�n� sa�l�yoruz. (0.01f ile �ok hassas bir takip sa�lan�r)
                 .OnComplete(() => Debug.Log("Tren hedefine ula�t�!"));  // Tren yolun sonuna ula�t���nda bir mesaj g�steriliyor.
    }
}
