using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : MonoBehaviour
{
    public float minFlyTime = 2f;     // Minimum u�ma s�resi
    public float maxFlyTime = 5f;     // Maksimum u�ma s�resi
    public float speed = 5f;          // U�ma h�z� (Trenden ba��ms�z sabit h�z)
    public Animator eagleAnimator;    // Ku� animat�r�
    public float minX = -100f;        // Alan�n sol s�n�r�
    public float maxX = 100f;         // Alan�n sa� s�n�r�
    public float minY = 20f;          // Alan�n alt s�n�r� (y�kseklik)
    public float maxY = 35f;          // Alan�n �st s�n�r� (y�kseklik)
    public float minZ = -220f;        // Alan�n �n s�n�r�
    public float maxZ = 120f;         // Alan�n arka s�n�r�
    private Vector3 targetPosition;

    // Takip de�i�kenleri
    public Transform train;  // Takip edece�imiz tren nesnesi
    public Train trainObj;
    public float followDistance = 5f;  // Trene olan takip mesafesi
    public float followHeight = 2f;    // Trenin arkas�nda u�ma y�ksekli�i
    public float followSpeed = 5f;     // Takip h�z�
    private float horizontalOffset;    // Sa�/Sol ofset
    public bool isFollowing = false;   // Tren takip kontrol�
    public bool isFlying = true;
    public bool isFollwerTrain = false;
    void Start()
    {
        StartCoroutine(ChangeOffsetRoutine());
        FlyToRandomPosition();  // Ba�lang��ta rastgele u�ma
    }

    void Update()
    {
        // Trenle ilgili h�z de�i�imi yerine sabit h�z kullan
        // speed = trainObj.moveSpeed; // Bunu kald�r, sabit bir h�z kullan.

        if (isFlying && !isFollowing)
        {
            // Ku�u hedef pozisyona do�ru hareket ettir
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            // Hedefe ula�t���nda yeni bir hedef belirle
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isFlying = false;
                StartCoroutine(PauseBeforeNextFlight());
            }
        }

        // E�er tren takibi yap�l�yorsa
        if (isFollowing)
        {
            FollowTrain();
        }
    }

    // Rastgele bir hedef pozisyon belirle
    void FlyToRandomPosition()
    {
        targetPosition = new Vector3(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY),
            Random.Range(minZ, maxZ)
        );
        transform.LookAt(targetPosition); // Ku�u hedef pozisyona bakacak �ekilde d�nd�r
        isFlying = true;  // Rastgele u�u� ba�las�n
    }

    // Ku� bir s�re duraklad�ktan sonra tekrar u�maya ba�lar
    IEnumerator PauseBeforeNextFlight()
    {
        float waitTime = Random.Range(minFlyTime, maxFlyTime);
        yield return new WaitForSeconds(waitTime);
        FlyToRandomPosition();
    }

    // Trene takip etme fonksiyonu (�u an kullan�lm�yor)
    void FollowTrain()
    {
        targetPosition = train.position - train.forward * followDistance + train.right * horizontalOffset;
        targetPosition.y = train.position.y + followHeight;
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        Vector3 lookAtPosition = train.position;
        lookAtPosition.y += 8f;
        transform.LookAt(lookAtPosition);
    }

    IEnumerator ChangeOffsetRoutine()
    {
        while (true)
        {
            horizontalOffset = Random.Range(-followDistance / 2f, followDistance / 2f);
            float waitTime = Random.Range(5f, 10f);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
