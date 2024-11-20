using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : MonoBehaviour
{
    public float minFlyTime = 2f;     // Minimum uçma süresi
    public float maxFlyTime = 5f;     // Maksimum uçma süresi
    public float speed = 5f;          // Uçma hýzý (Trenden baðýmsýz sabit hýz)
    public Animator eagleAnimator;    // Kuþ animatörü
    public float minX = -100f;        // Alanýn sol sýnýrý
    public float maxX = 100f;         // Alanýn sað sýnýrý
    public float minY = 20f;          // Alanýn alt sýnýrý (yükseklik)
    public float maxY = 35f;          // Alanýn üst sýnýrý (yükseklik)
    public float minZ = -220f;        // Alanýn ön sýnýrý
    public float maxZ = 120f;         // Alanýn arka sýnýrý
    private Vector3 targetPosition;

    // Takip deðiþkenleri
    public Transform train;  // Takip edeceðimiz tren nesnesi
    public Train trainObj;
    public float followDistance = 5f;  // Trene olan takip mesafesi
    public float followHeight = 2f;    // Trenin arkasýnda uçma yüksekliði
    public float followSpeed = 5f;     // Takip hýzý
    private float horizontalOffset;    // Sað/Sol ofset
    public bool isFollowing = false;   // Tren takip kontrolü
    public bool isFlying = true;
    public bool isFollwerTrain = false;
    void Start()
    {
        StartCoroutine(ChangeOffsetRoutine());
        FlyToRandomPosition();  // Baþlangýçta rastgele uçma
    }

    void Update()
    {
        // Trenle ilgili hýz deðiþimi yerine sabit hýz kullan
        // speed = trainObj.moveSpeed; // Bunu kaldýr, sabit bir hýz kullan.

        if (isFlying && !isFollowing)
        {
            // Kuþu hedef pozisyona doðru hareket ettir
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            // Hedefe ulaþtýðýnda yeni bir hedef belirle
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isFlying = false;
                StartCoroutine(PauseBeforeNextFlight());
            }
        }

        // Eðer tren takibi yapýlýyorsa
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
        transform.LookAt(targetPosition); // Kuþu hedef pozisyona bakacak þekilde döndür
        isFlying = true;  // Rastgele uçuþ baþlasýn
    }

    // Kuþ bir süre durakladýktan sonra tekrar uçmaya baþlar
    IEnumerator PauseBeforeNextFlight()
    {
        float waitTime = Random.Range(minFlyTime, maxFlyTime);
        yield return new WaitForSeconds(waitTime);
        FlyToRandomPosition();
    }

    // Trene takip etme fonksiyonu (þu an kullanýlmýyor)
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
