using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChestManager : MonoBehaviour
{
    [Serializable]
    public class Chest
    {
        public string chestName;                // Sandýðýn ismi
        public int chestPrice;                // Sandýðýn ismi
        public int cooldownTime;                // Sandýðýn bekleme süresi (saniye cinsinden)
        public Button collectButton;            // Sandýðýn toplama butonu
        public GameObject lockImage;            // Sandýk kilidi (aktifse sandýk toplanamaz)
        public TextMeshProUGUI timerText;       // Geri sayým süresini gösterecek TMP öðesi

        [NonSerialized]
        public DateTime nextCollectTime;        // Bir sonraki toplanma zamaný
    }

    public Chest[] chests;                      // Tüm sandýklarýn listesi
    private float updateInterval = 1f;   // 1 saniyede bir güncelle
    private float timer = 0f;
    public GameObject noInternetPanel;
    public GameObject noInternetImage;
    public ChestAdsManager adsManager;
    private void Start()
    {
        LoadChestData();  // Sandýk verilerini yükle
        UpdateChestUI();  // Baþlangýçta UI'yi güncelle

        // Butonlar için listener ekle
        foreach (var chest in chests)
        {
            Chest localChest = chest;  // Closure (kapanýþ) probleminden kaçýnmak için lokal deðiþken
            chest.collectButton.onClick.AddListener(() => CollectChest(localChest,chest.chestPrice));
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        // Eðer 1 saniye geçtiyse iþlemleri yap
        if (timer >= updateInterval)
        {
            UpdateChestUI();  // Sandýk UI'sini güncelle

            timer = 0f;  // Zamanlayýcýyý sýfýrla
        }
        //UpdateChestUI();  // Her frame'de UI'yi güncelle
    }

    // Sandýk verilerini yükler
    private void LoadChestData()
    {
        foreach (var chest in chests)
        {
            if (PlayerPrefs.HasKey(chest.chestName))
            {
                // Sandýk için kaydedilen zamaný yükle
                if (DateTime.TryParse(PlayerPrefs.GetString(chest.chestName), out DateTime loadedTime))
                {
                    chest.nextCollectTime = loadedTime;
                }
                else
                {
                    // Format hatasý durumunda þu anki zamana ayarla
                    chest.nextCollectTime = DateTime.Now;
                }
            }
            else
            {
                // Eðer veri yoksa, sandýk þu an toplanabilir
                chest.nextCollectTime = DateTime.Now;
            }
        }
    }

    // Sandýklarýn UI'sini güncelleyen fonksiyon
    private void UpdateChestUI()
    {
        foreach (var chest in chests)
        {
            TimeSpan remainingTime = chest.nextCollectTime - DateTime.Now;

            if (remainingTime <= TimeSpan.Zero)
            {
                // Sandýk toplanabilir: Buton aktif, kilit görüntüsü gizli
                chest.collectButton.interactable = true;
                chest.lockImage.SetActive(false);
                chest.timerText.text = "Available!";  // Hazýr olduðunu göster
            }
            else
            {
                // Sandýk toplanamaz: Geri sayým ve kilit görüntüsü göster
                chest.collectButton.interactable = false;
                chest.lockImage.SetActive(true);
                chest.timerText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", (int)remainingTime.TotalHours, remainingTime.Minutes, remainingTime.Seconds);
            }
        }
    }

    // Sandýðý toplama iþlemi
    public void CollectChest(Chest chest, int price)
    {
        if(Application.internetReachability != NetworkReachability.NotReachable)
        {
            if (DateTime.Now >= chest.nextCollectTime)
            {
                ChestAwards(price);
                // Sandýk toplandýysa yeni cooldown süresi ayarlanýr
                chest.nextCollectTime = DateTime.Now.AddSeconds(chest.cooldownTime);

                // Yeni zamaný kaydet
                PlayerPrefs.SetString(chest.chestName, chest.nextCollectTime.ToString("yyyy-MM-dd HH:mm:ss"));
                PlayerPrefs.Save();
                Debug.Log("Next Collect Time Set: " + chest.nextCollectTime.ToString("yyyy-MM-dd HH:mm:ss"));

                // UI'yi hemen güncelle
                UpdateChestUI();
                
            }
        }
        else
        {
            noInternetPanel.SetActive(true);
            NoInterNetAnim(noInternetImage);
            Debug.Log("Ýnternet baðlantýsý yok. Reklam gösterilemedi.");
        }

    }
    public void ChestAwards(int price)
    {
            Debug.Log("Ýnternet baðlantýsý yok. Reklam gösterilemedi.1 1 1 1");

        //if (adsManager.LoadRewardedAddIsNull())
        //{
        //    adsManager.LoadRewardedAd();
        //    Debug.Log("Ýnternet baðlantýsý yok. Reklam gösterilemedi 2 2 2 2 2 2");

        //} 
        if (adsManager.LoadRewardedAddIsNull())
        {
            adsManager.LoadRewardedAd();
        }
        adsManager.ShowRewardedAd(price);
    }
    public void NoInterNetAnim(GameObject obj)
    {
       
        Vector3 originalScale = Vector3.one;
        Vector3 targetScale = originalScale * 1.3f;

        // Önce butonu büyüt
        LeanTween.scale(obj, targetScale, 0.3f)
                 .setEase(LeanTweenType.easeInOutQuad)
                 .setOnComplete(() =>
                 {
                     // Sonra butonu eski haline döndür
                     LeanTween.scale(obj, originalScale, 0.3f)
                              .setEase(LeanTweenType.easeInOutQuad)
                              .setOnComplete(() =>
                              {
                                  // 1 saniye sonra paneli kapat
                                  LeanTween.delayedCall(.5f, () =>
                                  {
                                      noInternetPanel.SetActive(false);
                                  });
                              });
                 });
    }
}
