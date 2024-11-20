using DG.Tweening;
using UnityEngine;

public class TestMoveTrain : MonoBehaviour
{
    public Transform[] pathPoints;  // Trenimizin izleyeceði yolun noktalarýný belirtiyoruz.
    public float duration = 10f;  // Trenimizin yolu ne kadar sürede tamamlayacaðýný belirleyen süre.
    public PathType pathType = PathType.CatmullRom;  // Yolu nasýl oluþturacaðýmýzý belirleyen PathType seçeneði.

    void Start()
    {
        // Nesneyi doðrudan yolun ilk noktasýna yerleþtiriyoruz.
        transform.position = pathPoints[0].position;

        // Treni harekete geçiriyoruz.
        MoveTrain();
    }

    void MoveTrain()
    {
        Vector3[] path = new Vector3[pathPoints.Length];
        for (int i = 0; i < pathPoints.Length; i++)
        {
            path[i] = pathPoints[i].position;  // Her bir noktanýn pozisyonunu diziye ekliyoruz.
        }

        // Trenimizi belirlediðimiz yol boyunca hareket ettiriyoruz.
        transform.DOPath(path, duration, pathType)
                 .SetEase(Ease.Linear)  // Hareketin hýzý sabit tutuluyor.
                 .SetLookAt(0.01f)  // Nesnenin yol boyunca dönüþ yapmasýný saðlýyoruz. (0.01f ile çok hassas bir takip saðlanýr)
                 .OnComplete(() => Debug.Log("Tren hedefine ulaþtý!"));  // Tren yolun sonuna ulaþtýðýnda bir mesaj gösteriliyor.
    }
}
