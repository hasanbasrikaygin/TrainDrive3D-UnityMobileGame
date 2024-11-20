using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private Vector3 baseOffset; // E�yalar�n konumlar�na uygulanacak ofset
    [SerializeField] private Vector3 scoreBoostOffset; // E�yalar�n konumlar�na uygulanacak ofset

    [SerializeField] private Factory obstacleFactory; // Engel fabrikas� referans�
    [SerializeField] private Factory foodFactory; // Yiyecek fabrikas� referans�
    [SerializeField] private Factory wingsFactory; // Kanat fabrikas� referans�
    [SerializeField] private Factory zoomOutFactory; // Kanat fabrikas� referans�
    [SerializeField] private Factory scoreBoostFactory; // Kanat fabrikas� referans�
    [SerializeField] private Factory speedBoostFactory; // Kanat fabrikas� referans�
    [SerializeField] private Factory shieldFactory; // Kanat fabrikas� referans�
    [SerializeField] private Factory slowMotionFactory; // Kanat fabrikas� referans�
    

    [SerializeField] private int totalObstacles = 12; // Toplam �retilecek engel say�s�
    [SerializeField] private int totalFoodItems = 5; // Toplam �retilecek yiyecek say�s�
    [SerializeField] private int totalWingsItems = 1; // Toplam �retilecek kanat say�s�
    [SerializeField] private int totalScoreBoostItems = 1; // Toplam �retilecek kanat say�s�
    [SerializeField] private int totalZoomOutItems = 1; // Toplam �retilecek kanat say�s�

    [SerializeField] private Collider[] obstacleSpawnAreas; // Engellerin spawn alanlar�
    [SerializeField] private Collider[] foodSpawnAreas; // Yiyeceklerin spawn alanlar�
    [SerializeField] private Collider[] wingsSpawnAreas; // Kanatlar�n spawn alanlar�
    [SerializeField] private Collider[] zoomOutSpawnAreas; // Kanatlar�n spawn alanlar�
    [SerializeField] private Collider[] scoreBoostSpawnAreas; // Kanatlar�n spawn alanlar�
    [SerializeField] private Collider[] speedBoostSpawnAreas; // Kanatlar�n spawn alanlar�
    [SerializeField] private Collider[] shieldSpawnAreas; // Kanatlar�n spawn alanlar�
    [SerializeField] private Collider[] slowMotionSpawnAreas; // Kanatlar�n spawn alanlar�

    [SerializeField] private Transform player; // Oyuncunun transform referans�
    [SerializeField] private float playerAvoidanceRadius = 3f; // Oyuncudan ka��n�lacak mesafe
    [SerializeField] private float checkRadius = 2f; // Etraf�n kontrol edilece�i yar��ap
    [SerializeField] private float foodAndWingsRespawnInterval = 15f; // Yiyecek ve kanatlar�n yeniden spawnlanma s�resi

    private List<IItem> obstacles; // Engellerin listesi
    private List<IItem> foodItems; // Yiyeceklerin listesi
    private List<IItem> wingsItems; // Kanatlar�n listesi
    private List<IItem> zoomOutItems; // Kanatlar�n listesi
    private List<IItem> scoreBoostItems; // Kanatlar�n listesi
    private List<IItem> speedBoostItems; // Kanatlar�n listesi
    private List<IItem> shieldItems; // Kanatlar�n listesi
    private List<IItem> slowMotionItems; // Kanatlar�n listesi


    private int obstaclesPlaced = 0; // Yerle�tirilen engel say�s�


    [SerializeField] private Factory mergeWagonFactory; // Kanat fabrikas� referans�
    private List<IItem> mergeWagonItems; // Kanatlar�n listesi
    public int lastWagonNumber;

    public bool isWagonCountFull = false;
    
    
    private void Start()
    {
        ///\\\///\\\///\\\  --- PASSENGER MOD ---  ///\\\///\\\///\\\
        // E�yalar�n listelerini ba�lat
        obstacles = new List<IItem>();
        wingsItems = new List<IItem>();
        zoomOutItems = new List<IItem>();
        scoreBoostItems = new List<IItem>();
        speedBoostItems = new List<IItem>();
        shieldItems = new List<IItem>();
        slowMotionItems = new List<IItem>();
       

        // E�yalar� ba�lat ve yerle�tir
        InitializeItems(obstacleFactory, totalObstacles, obstacles, false, obstacleSpawnAreas);
        InitializeItems(wingsFactory, totalWingsItems, wingsItems, true, wingsSpawnAreas);
        InitializeItems(zoomOutFactory, totalZoomOutItems, zoomOutItems, true, zoomOutSpawnAreas);
        InitializeItems(scoreBoostFactory, totalScoreBoostItems, scoreBoostItems, true, scoreBoostSpawnAreas);
        InitializeItems(speedBoostFactory, totalWingsItems, speedBoostItems, true, speedBoostSpawnAreas);
        InitializeItems(shieldFactory, totalWingsItems, shieldItems, true, shieldSpawnAreas);
        InitializeItems(slowMotionFactory, totalWingsItems, slowMotionItems, true, slowMotionSpawnAreas);
        

        // Engelleri yeniden spawnlamak i�in coroutine ba�lat
        StartCoroutine(RespawnObstacleItemsCoroutine());
        // Yiyecek ve kanatlar� yeniden spawnlamak i�in coroutine ba�lat
        StartCoroutine(RespawnZoomAndWingsItemsCoroutine());

        ///\\\///\\\///\\\  --- PASSENGER MOD ---  ///\\\///\\\///\\\
        if (foodFactory != null)
        {
            foodItems = new List<IItem>();
            InitializeItems(foodFactory, totalFoodItems, foodItems, true, foodSpawnAreas);
            StartCoroutine(RespawnFoodItemsCoroutine());
        }

        ///\\\///\\\///\\\  --- 2048 MOD ---  ///\\\///\\\///\\\
        if (mergeWagonFactory != null)
        {
            mergeWagonItems = new List<IItem>();
            InitializeItems(mergeWagonFactory, 14, mergeWagonItems, true, foodSpawnAreas);
            StartCoroutine(RespawnMergeFoodItemsCoroutine());
        }


    }

    // E�yalar� ba�latma ve listeye ekleme fonksiyonu
    private void InitializeItems(Factory factory, int itemCount, List<IItem> itemList, bool isVisibleOnSpawn, Collider[] spawnAreas)
    {
        for (int i = 0; i < itemCount; i++)
        {
            // Yeni bir e�ya olu�tur
            IItem item = factory.CreateItem(Vector3.zero);

            // E�er e�ya spawn oldu�unda g�r�n�r de�ilse, onu gizle
            if (!isVisibleOnSpawn)
            {
                item.Hide();
            }
            itemList.Add(item);

            // E�ya spawn oldu�unda g�r�n�r olacaksa, onun pozisyonunu yeniden ayarla ve g�ster
            if (isVisibleOnSpawn)
            {
                RepositionItem(item, spawnAreas, baseOffset);
                item.Show(); // E�yay� g�r�n�r yap
            }
        }
    }

    // Engelleri yeniden spawnlama coroutine'i
    private IEnumerator RespawnObstacleItemsCoroutine()
    {
        while (obstaclesPlaced < totalObstacles)
        {
            float waitTime = Random.Range(1f, 2f); // Rastgele bir bekleme s�resi
            yield return new WaitForSeconds(waitTime);

            IItem obstacle = obstacles[obstaclesPlaced];
            if (TryGetRandomPosition(out Vector3 randomPosition, obstacleSpawnAreas , baseOffset))
            {
                obstacle.MoveTo(randomPosition);
                obstacle.Show(); // E�yay� g�r�n�r yap
                obstaclesPlaced++;
            }
            else
            {
                Debug.LogWarning("Uygun bir spawn pozisyonu bulunamad�. Respawn atlan�yor.");
            }
        }
    }

    // Yiyecek ve kanatlar� yeniden spawnlama coroutine'i
    private IEnumerator RespawnZoomAndWingsItemsCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(foodAndWingsRespawnInterval);
            // Yiyecek e�yalar�n� yeniden pozisyonland�r ve g�ster
            // Kanat e�yalar�n� yeniden pozisyonland�r ve g�ster
            foreach (IItem wingsItem in wingsItems)
            {
                RepositionItem(wingsItem, wingsSpawnAreas, baseOffset);
                wingsItem.Show(); // E�yay� g�r�n�r yap
            }
            // zoom out u yeniden pozisyonland�r ve g�ster
            foreach (IItem zoomOutsItems in zoomOutItems)
            {
                RepositionItem(zoomOutsItems, wingsSpawnAreas , baseOffset);
                zoomOutsItems.Show(); // E�yay� g�r�n�r yap
            }
            // score boost u yeniden pozisyonland�r ve g�ster
            foreach (IItem scoreBoostsItems in scoreBoostItems)
            {
                RepositionItem(scoreBoostsItems, scoreBoostSpawnAreas , baseOffset);
                scoreBoostsItems.Show(); // E�yay� g�r�n�r yap
            }
            foreach (IItem speedBoostsItems in speedBoostItems)
            {
                RepositionItem(speedBoostsItems, speedBoostSpawnAreas , scoreBoostOffset);
                speedBoostsItems.Show(); // E�yay� g�r�n�r yap
            }
            foreach (IItem shieldsItems in shieldItems)
            {
                RepositionItem(shieldsItems, shieldSpawnAreas, scoreBoostOffset);
                shieldsItems.Show(); // E�yay� g�r�n�r yap
            }
            foreach (IItem slowMotionsItems in slowMotionItems)
            {
                RepositionItem(slowMotionsItems, slowMotionSpawnAreas, baseOffset);
                slowMotionsItems.Show(); // E�yay� g�r�n�r yap
            }
        }
    }   
    private IEnumerator RespawnFoodItemsCoroutine()
    {
        
    while (true)
    {
            
            yield return new WaitForSeconds(foodAndWingsRespawnInterval);
            // Yiyecek e�yalar�n� yeniden pozisyonland�r ve g�ster
            if (!isWagonCountFull)
            {
                foreach (IItem foodItem in foodItems)
                {
                    RepositionItem(foodItem, foodSpawnAreas, baseOffset);
                    foodItem.Show(); // E�yay� g�r�n�r yap
                }
            }
        }
    }   
    // Yiyecek ve kanatlar� yeniden spawnlama coroutine'i
    private IEnumerator RespawnMergeFoodItemsCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(foodAndWingsRespawnInterval);

            // 2048 e�yalar�n� yeniden pozisyonland�r ve g�ster
            foreach (IItem mergeWagonItem  in mergeWagonItems)
            {
                RepositionItem(mergeWagonItem, foodSpawnAreas , baseOffset);
                mergeWagonItem.Show(); // E�yay� g�r�n�r yap
            }
        }
    }

    // E�yay� yeniden konumland�rma fonksiyonu
    private void RepositionItem(IItem item, Collider[] spawnAreas , Vector3 offset)
    {
        if (TryGetRandomPosition(out Vector3 randomPosition, spawnAreas , offset))
        {
            item.MoveTo(randomPosition);
        }
        else
        {
            Debug.LogWarning("E�ya i�in uygun bir spawn pozisyonu bulunamad�. Mevcut pozisyon korunuyor.");
        }
    }

    // Rastgele bir pozisyon bulma ve geri d�nd�rme fonksiyonu
    private bool TryGetRandomPosition(out Vector3 position, Collider[] spawnAreas , Vector3 offset)
    {
        int maxAttempts = 10; // Maksimum deneme say�s�
        int attempts = 0;
        position = Vector3.zero;

        while (attempts < maxAttempts)
        {
            Vector3 randomPosition = GetRandomPosition(spawnAreas , offset);
            if (!IsPlayerNearby(randomPosition) && !IsOccupied(randomPosition))
            {
                position = randomPosition;
                return true;
            }
            attempts++;
        }

        return false;
    }

    // Rastgele bir pozisyon olu�turma fonksiyonu
    private Vector3 GetRandomPosition(Collider[] spawnAreas , Vector3 offset)
    {
        if (spawnAreas.Length == 0)
        {
            Debug.LogError("Hi�bir spawn alan� tan�mlanmam��!");
            return Vector3.zero;
        }

        Collider randomArea = spawnAreas[Random.Range(0, spawnAreas.Length)];
        Bounds bounds = randomArea.bounds;
        Vector3 randomPosition = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            bounds.center.y,
            Random.Range(bounds.min.z, bounds.max.z)
        );

        return randomPosition + offset;
    }

    // Oyuncunun yak�n�nda olup olmad���n� kontrol etme fonksiyonu
    private bool IsPlayerNearby(Vector3 position)
    {
        float distanceToPlayer = Vector3.Distance(position, player.position);
        return distanceToPlayer < playerAvoidanceRadius;
    }

    // Pozisyonun dolu olup olmad���n� kontrol etme fonksiyonu
    private bool IsOccupied(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, checkRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Train") || collider.CompareTag("Wagon"))
            {
                return true;
            }
        }
        return false;
    }
    public void HideFoodWagons()
    {
        foreach (IItem foodItem in foodItems)
        {
            RepositionItem(foodItem, foodSpawnAreas, baseOffset);
            foodItem.Hide(); // E�yay� g�r�n�r yap
        }
    }  
    public void ShowFoodWagons()
    {
        foreach (IItem foodItem in foodItems)
        {
            RepositionItem(foodItem, foodSpawnAreas, baseOffset);
            foodItem.Show(); // E�yay� g�r�n�r yap
        }
    }
}
