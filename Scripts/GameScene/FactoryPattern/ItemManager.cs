using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private Vector3 baseOffset; // Eþyalarýn konumlarýna uygulanacak ofset
    [SerializeField] private Vector3 scoreBoostOffset; // Eþyalarýn konumlarýna uygulanacak ofset

    [SerializeField] private Factory obstacleFactory; // Engel fabrikasý referansý
    [SerializeField] private Factory foodFactory; // Yiyecek fabrikasý referansý
    [SerializeField] private Factory wingsFactory; // Kanat fabrikasý referansý
    [SerializeField] private Factory zoomOutFactory; // Kanat fabrikasý referansý
    [SerializeField] private Factory scoreBoostFactory; // Kanat fabrikasý referansý
    [SerializeField] private Factory speedBoostFactory; // Kanat fabrikasý referansý
    [SerializeField] private Factory shieldFactory; // Kanat fabrikasý referansý
    [SerializeField] private Factory slowMotionFactory; // Kanat fabrikasý referansý
    

    [SerializeField] private int totalObstacles = 12; // Toplam üretilecek engel sayýsý
    [SerializeField] private int totalFoodItems = 5; // Toplam üretilecek yiyecek sayýsý
    [SerializeField] private int totalWingsItems = 1; // Toplam üretilecek kanat sayýsý
    [SerializeField] private int totalScoreBoostItems = 1; // Toplam üretilecek kanat sayýsý
    [SerializeField] private int totalZoomOutItems = 1; // Toplam üretilecek kanat sayýsý

    [SerializeField] private Collider[] obstacleSpawnAreas; // Engellerin spawn alanlarý
    [SerializeField] private Collider[] foodSpawnAreas; // Yiyeceklerin spawn alanlarý
    [SerializeField] private Collider[] wingsSpawnAreas; // Kanatlarýn spawn alanlarý
    [SerializeField] private Collider[] zoomOutSpawnAreas; // Kanatlarýn spawn alanlarý
    [SerializeField] private Collider[] scoreBoostSpawnAreas; // Kanatlarýn spawn alanlarý
    [SerializeField] private Collider[] speedBoostSpawnAreas; // Kanatlarýn spawn alanlarý
    [SerializeField] private Collider[] shieldSpawnAreas; // Kanatlarýn spawn alanlarý
    [SerializeField] private Collider[] slowMotionSpawnAreas; // Kanatlarýn spawn alanlarý

    [SerializeField] private Transform player; // Oyuncunun transform referansý
    [SerializeField] private float playerAvoidanceRadius = 3f; // Oyuncudan kaçýnýlacak mesafe
    [SerializeField] private float checkRadius = 2f; // Etrafýn kontrol edileceði yarýçap
    [SerializeField] private float foodAndWingsRespawnInterval = 15f; // Yiyecek ve kanatlarýn yeniden spawnlanma süresi

    private List<IItem> obstacles; // Engellerin listesi
    private List<IItem> foodItems; // Yiyeceklerin listesi
    private List<IItem> wingsItems; // Kanatlarýn listesi
    private List<IItem> zoomOutItems; // Kanatlarýn listesi
    private List<IItem> scoreBoostItems; // Kanatlarýn listesi
    private List<IItem> speedBoostItems; // Kanatlarýn listesi
    private List<IItem> shieldItems; // Kanatlarýn listesi
    private List<IItem> slowMotionItems; // Kanatlarýn listesi


    private int obstaclesPlaced = 0; // Yerleþtirilen engel sayýsý


    [SerializeField] private Factory mergeWagonFactory; // Kanat fabrikasý referansý
    private List<IItem> mergeWagonItems; // Kanatlarýn listesi
    public int lastWagonNumber;

    public bool isWagonCountFull = false;
    
    
    private void Start()
    {
        ///\\\///\\\///\\\  --- PASSENGER MOD ---  ///\\\///\\\///\\\
        // Eþyalarýn listelerini baþlat
        obstacles = new List<IItem>();
        wingsItems = new List<IItem>();
        zoomOutItems = new List<IItem>();
        scoreBoostItems = new List<IItem>();
        speedBoostItems = new List<IItem>();
        shieldItems = new List<IItem>();
        slowMotionItems = new List<IItem>();
       

        // Eþyalarý baþlat ve yerleþtir
        InitializeItems(obstacleFactory, totalObstacles, obstacles, false, obstacleSpawnAreas);
        InitializeItems(wingsFactory, totalWingsItems, wingsItems, true, wingsSpawnAreas);
        InitializeItems(zoomOutFactory, totalZoomOutItems, zoomOutItems, true, zoomOutSpawnAreas);
        InitializeItems(scoreBoostFactory, totalScoreBoostItems, scoreBoostItems, true, scoreBoostSpawnAreas);
        InitializeItems(speedBoostFactory, totalWingsItems, speedBoostItems, true, speedBoostSpawnAreas);
        InitializeItems(shieldFactory, totalWingsItems, shieldItems, true, shieldSpawnAreas);
        InitializeItems(slowMotionFactory, totalWingsItems, slowMotionItems, true, slowMotionSpawnAreas);
        

        // Engelleri yeniden spawnlamak için coroutine baþlat
        StartCoroutine(RespawnObstacleItemsCoroutine());
        // Yiyecek ve kanatlarý yeniden spawnlamak için coroutine baþlat
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

    // Eþyalarý baþlatma ve listeye ekleme fonksiyonu
    private void InitializeItems(Factory factory, int itemCount, List<IItem> itemList, bool isVisibleOnSpawn, Collider[] spawnAreas)
    {
        for (int i = 0; i < itemCount; i++)
        {
            // Yeni bir eþya oluþtur
            IItem item = factory.CreateItem(Vector3.zero);

            // Eðer eþya spawn olduðunda görünür deðilse, onu gizle
            if (!isVisibleOnSpawn)
            {
                item.Hide();
            }
            itemList.Add(item);

            // Eþya spawn olduðunda görünür olacaksa, onun pozisyonunu yeniden ayarla ve göster
            if (isVisibleOnSpawn)
            {
                RepositionItem(item, spawnAreas, baseOffset);
                item.Show(); // Eþyayý görünür yap
            }
        }
    }

    // Engelleri yeniden spawnlama coroutine'i
    private IEnumerator RespawnObstacleItemsCoroutine()
    {
        while (obstaclesPlaced < totalObstacles)
        {
            float waitTime = Random.Range(1f, 2f); // Rastgele bir bekleme süresi
            yield return new WaitForSeconds(waitTime);

            IItem obstacle = obstacles[obstaclesPlaced];
            if (TryGetRandomPosition(out Vector3 randomPosition, obstacleSpawnAreas , baseOffset))
            {
                obstacle.MoveTo(randomPosition);
                obstacle.Show(); // Eþyayý görünür yap
                obstaclesPlaced++;
            }
            else
            {
                Debug.LogWarning("Uygun bir spawn pozisyonu bulunamadý. Respawn atlanýyor.");
            }
        }
    }

    // Yiyecek ve kanatlarý yeniden spawnlama coroutine'i
    private IEnumerator RespawnZoomAndWingsItemsCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(foodAndWingsRespawnInterval);
            // Yiyecek eþyalarýný yeniden pozisyonlandýr ve göster
            // Kanat eþyalarýný yeniden pozisyonlandýr ve göster
            foreach (IItem wingsItem in wingsItems)
            {
                RepositionItem(wingsItem, wingsSpawnAreas, baseOffset);
                wingsItem.Show(); // Eþyayý görünür yap
            }
            // zoom out u yeniden pozisyonlandýr ve göster
            foreach (IItem zoomOutsItems in zoomOutItems)
            {
                RepositionItem(zoomOutsItems, wingsSpawnAreas , baseOffset);
                zoomOutsItems.Show(); // Eþyayý görünür yap
            }
            // score boost u yeniden pozisyonlandýr ve göster
            foreach (IItem scoreBoostsItems in scoreBoostItems)
            {
                RepositionItem(scoreBoostsItems, scoreBoostSpawnAreas , baseOffset);
                scoreBoostsItems.Show(); // Eþyayý görünür yap
            }
            foreach (IItem speedBoostsItems in speedBoostItems)
            {
                RepositionItem(speedBoostsItems, speedBoostSpawnAreas , scoreBoostOffset);
                speedBoostsItems.Show(); // Eþyayý görünür yap
            }
            foreach (IItem shieldsItems in shieldItems)
            {
                RepositionItem(shieldsItems, shieldSpawnAreas, scoreBoostOffset);
                shieldsItems.Show(); // Eþyayý görünür yap
            }
            foreach (IItem slowMotionsItems in slowMotionItems)
            {
                RepositionItem(slowMotionsItems, slowMotionSpawnAreas, baseOffset);
                slowMotionsItems.Show(); // Eþyayý görünür yap
            }
        }
    }   
    private IEnumerator RespawnFoodItemsCoroutine()
    {
        
    while (true)
    {
            
            yield return new WaitForSeconds(foodAndWingsRespawnInterval);
            // Yiyecek eþyalarýný yeniden pozisyonlandýr ve göster
            if (!isWagonCountFull)
            {
                foreach (IItem foodItem in foodItems)
                {
                    RepositionItem(foodItem, foodSpawnAreas, baseOffset);
                    foodItem.Show(); // Eþyayý görünür yap
                }
            }
        }
    }   
    // Yiyecek ve kanatlarý yeniden spawnlama coroutine'i
    private IEnumerator RespawnMergeFoodItemsCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(foodAndWingsRespawnInterval);

            // 2048 eþyalarýný yeniden pozisyonlandýr ve göster
            foreach (IItem mergeWagonItem  in mergeWagonItems)
            {
                RepositionItem(mergeWagonItem, foodSpawnAreas , baseOffset);
                mergeWagonItem.Show(); // Eþyayý görünür yap
            }
        }
    }

    // Eþyayý yeniden konumlandýrma fonksiyonu
    private void RepositionItem(IItem item, Collider[] spawnAreas , Vector3 offset)
    {
        if (TryGetRandomPosition(out Vector3 randomPosition, spawnAreas , offset))
        {
            item.MoveTo(randomPosition);
        }
        else
        {
            Debug.LogWarning("Eþya için uygun bir spawn pozisyonu bulunamadý. Mevcut pozisyon korunuyor.");
        }
    }

    // Rastgele bir pozisyon bulma ve geri döndürme fonksiyonu
    private bool TryGetRandomPosition(out Vector3 position, Collider[] spawnAreas , Vector3 offset)
    {
        int maxAttempts = 10; // Maksimum deneme sayýsý
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

    // Rastgele bir pozisyon oluþturma fonksiyonu
    private Vector3 GetRandomPosition(Collider[] spawnAreas , Vector3 offset)
    {
        if (spawnAreas.Length == 0)
        {
            Debug.LogError("Hiçbir spawn alaný tanýmlanmamýþ!");
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

    // Oyuncunun yakýnýnda olup olmadýðýný kontrol etme fonksiyonu
    private bool IsPlayerNearby(Vector3 position)
    {
        float distanceToPlayer = Vector3.Distance(position, player.position);
        return distanceToPlayer < playerAvoidanceRadius;
    }

    // Pozisyonun dolu olup olmadýðýný kontrol etme fonksiyonu
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
            foodItem.Hide(); // Eþyayý görünür yap
        }
    }  
    public void ShowFoodWagons()
    {
        foreach (IItem foodItem in foodItems)
        {
            RepositionItem(foodItem, foodSpawnAreas, baseOffset);
            foodItem.Show(); // Eþyayý görünür yap
        }
    }
}
