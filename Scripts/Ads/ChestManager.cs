using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChestManager : MonoBehaviour
{
    [Serializable]
    public class Chest
    {
        public string chestName;                // Sand���n ismi
        public int chestPrice;                // Sand���n ismi
        public int cooldownTime;                // Sand���n bekleme s�resi (saniye cinsinden)
        public Button collectButton;            // Sand���n toplama butonu
        public GameObject lockImage;            // Sand�k kilidi (aktifse sand�k toplanamaz)
        public TextMeshProUGUI timerText;       // Geri say�m s�resini g�sterecek TMP ��esi

        [NonSerialized]
        public DateTime nextCollectTime;        // Bir sonraki toplanma zaman�
    }

    public Chest[] chests;                      // T�m sand�klar�n listesi
    private float updateInterval = 1f;   // 1 saniyede bir g�ncelle
    private float timer = 0f;
    public GameObject noInternetPanel;
    public GameObject noInternetImage;
    public ChestAdsManager adsManager;
    private void Start()
    {
        LoadChestData();  // Sand�k verilerini y�kle
        UpdateChestUI();  // Ba�lang��ta UI'yi g�ncelle

        // Butonlar i�in listener ekle
        foreach (var chest in chests)
        {
            Chest localChest = chest;  // Closure (kapan��) probleminden ka��nmak i�in lokal de�i�ken
            chest.collectButton.onClick.AddListener(() => CollectChest(localChest,chest.chestPrice));
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        // E�er 1 saniye ge�tiyse i�lemleri yap
        if (timer >= updateInterval)
        {
            UpdateChestUI();  // Sand�k UI'sini g�ncelle

            timer = 0f;  // Zamanlay�c�y� s�f�rla
        }
        //UpdateChestUI();  // Her frame'de UI'yi g�ncelle
    }

    // Sand�k verilerini y�kler
    private void LoadChestData()
    {
        foreach (var chest in chests)
        {
            if (PlayerPrefs.HasKey(chest.chestName))
            {
                // Sand�k i�in kaydedilen zaman� y�kle
                if (DateTime.TryParse(PlayerPrefs.GetString(chest.chestName), out DateTime loadedTime))
                {
                    chest.nextCollectTime = loadedTime;
                }
                else
                {
                    // Format hatas� durumunda �u anki zamana ayarla
                    chest.nextCollectTime = DateTime.Now;
                }
            }
            else
            {
                // E�er veri yoksa, sand�k �u an toplanabilir
                chest.nextCollectTime = DateTime.Now;
            }
        }
    }

    // Sand�klar�n UI'sini g�ncelleyen fonksiyon
    private void UpdateChestUI()
    {
        foreach (var chest in chests)
        {
            TimeSpan remainingTime = chest.nextCollectTime - DateTime.Now;

            if (remainingTime <= TimeSpan.Zero)
            {
                // Sand�k toplanabilir: Buton aktif, kilit g�r�nt�s� gizli
                chest.collectButton.interactable = true;
                chest.lockImage.SetActive(false);
                chest.timerText.text = "Available!";  // Haz�r oldu�unu g�ster
            }
            else
            {
                // Sand�k toplanamaz: Geri say�m ve kilit g�r�nt�s� g�ster
                chest.collectButton.interactable = false;
                chest.lockImage.SetActive(true);
                chest.timerText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", (int)remainingTime.TotalHours, remainingTime.Minutes, remainingTime.Seconds);
            }
        }
    }

    // Sand��� toplama i�lemi
    public void CollectChest(Chest chest, int price)
    {
        if(Application.internetReachability != NetworkReachability.NotReachable)
        {
            if (DateTime.Now >= chest.nextCollectTime)
            {
                ChestAwards(price);
                // Sand�k topland�ysa yeni cooldown s�resi ayarlan�r
                chest.nextCollectTime = DateTime.Now.AddSeconds(chest.cooldownTime);

                // Yeni zaman� kaydet
                PlayerPrefs.SetString(chest.chestName, chest.nextCollectTime.ToString("yyyy-MM-dd HH:mm:ss"));
                PlayerPrefs.Save();
                Debug.Log("Next Collect Time Set: " + chest.nextCollectTime.ToString("yyyy-MM-dd HH:mm:ss"));

                // UI'yi hemen g�ncelle
                UpdateChestUI();
                
            }
        }
        else
        {
            noInternetPanel.SetActive(true);
            NoInterNetAnim(noInternetImage);
            Debug.Log("�nternet ba�lant�s� yok. Reklam g�sterilemedi.");
        }

    }
    public void ChestAwards(int price)
    {
            Debug.Log("�nternet ba�lant�s� yok. Reklam g�sterilemedi.1 1 1 1");

        //if (adsManager.LoadRewardedAddIsNull())
        //{
        //    adsManager.LoadRewardedAd();
        //    Debug.Log("�nternet ba�lant�s� yok. Reklam g�sterilemedi 2 2 2 2 2 2");

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

        // �nce butonu b�y�t
        LeanTween.scale(obj, targetScale, 0.3f)
                 .setEase(LeanTweenType.easeInOutQuad)
                 .setOnComplete(() =>
                 {
                     // Sonra butonu eski haline d�nd�r
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
