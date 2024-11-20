using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    public Train train;
    public TextMeshProUGUI scoreText;
    public float score = 0f;
    public float scoreIncreaseRate = 10f; // Her saniye artacak temel skor
    public float multiplier = 1f;
    public float maxMultiplier = 5f; // Çarpanýn ulaþabileceði maksimum seviye
    public float multiplierIncreaseRate = 0.1f; // Zamanla çarpanýn artýþ hýzý
    private float timeElapsed = 0f;
    private bool isMultiplierBoostActive = false;
    public float multiplierBoostDuration = 12f; // Çarpan artýrýcý item süresi
    private float boostEndTime = 0f;
    public bool isGameOver = true; // Game over durumu


    public float baseSpeed = 10f; // Baþlangýç hýzý
    public float speedIncrease = 2f; // Her seviye için hýz artýþý
    public float maxSpeed = 20f; // Maksimum hýz
    public float timeBetweenLevels = 60f; // Her seviyede ne kadar süre geçecek
    private float speedIncreaseTimer = 0f; // Geçen süre
    private int currentLevel = 1; // Mevcut hýz seviyesi
    public float scoreTextSpeed = 10.0f; // Dolum iþleminin baþlangýç hýzý

    public TextMeshProUGUI finishScoreText;

    public GameObject itemBarUi; 
    public GameObject gameOverCanvas;
    public GameObject baseUi;
    public GameObject mergeCanvas;
    public GameObject scoreCanvas;
    public GameObject passengerController;
    public GameObject passengerPlatform;
    public PassengerPool passengerPool;
    public UIAnimationManager uiAnimationManager;
    private float[] intervals = { 60, 180, 360, 600, 900 }; // Dakika olarak 1, 3, 6, 10, 15
    private int intervalIndex = 0;
    private void Start()
    {
        scoreCanvas.SetActive(false);
        StartCoroutine(SpeedUpAtIntervals());
    }
    void Update()
    {

        speedIncreaseTimer += Time.deltaTime; // Oyun süresini takip et



        if (isGameOver) return;
        // Zaman ilerledikçe çarpaný artýr
        timeElapsed += Time.deltaTime;

        // Eðer çarpan maksimum seviyeye ulaþmadýysa artýr
        if (multiplier < maxMultiplier)
        {
            multiplier += multiplierIncreaseRate * Time.deltaTime;
            multiplier = Mathf.Clamp(multiplier, 1f, maxMultiplier); // Maksimum çarpana ulaþtýðýnda durdur
        }

        // Skoru güncelle
        score += scoreIncreaseRate * multiplier * Time.deltaTime;
        scoreText.text = Mathf.FloorToInt(score).ToString();

        // Eðer çarpan artýrýcý aktifse, süresini kontrol et
        if (isMultiplierBoostActive && Time.time >= boostEndTime)
        {
            EndMultiplierBoost();
        }
    }

    // Çarpan itemi alýndýðýnda çaðýrýlacak fonksiyon
    public void ActivateMultiplierBoost()
    {
        Debug.Log("Skorboost baþladý");
        if (!isMultiplierBoostActive)
        {
            isMultiplierBoostActive = true;
            multiplier *= 2f; // Çarpaný iki katýna çýkar
            boostEndTime = Time.time + multiplierBoostDuration; // Artýrma süresi hesaplanýr
        }
    }

    // Çarpan artýrýcý sona erdiðinde çaðýrýlacak fonksiyon
    private void EndMultiplierBoost()
    {
        isMultiplierBoostActive = false;
        multiplier /= 2f; // Çarpan eski haline döner
        Debug.Log("Skorboost bitti");
    }
    public void StopScore()
    {


        isGameOver = true;
        Debug.Log("Game Over! Skor durduruldu.");
    }
    public void StartScore()
    {
        isGameOver = false;
    }
    public void FinishState()
    {
        passengerController.SetActive(true);
        passengerPlatform.GetComponent<BoxCollider>().enabled = false;
        passengerPool.FinishStatePassengerAnimations();
        uiAnimationManager.ChangeScaleText();
        gameOverCanvas.SetActive(false);
        baseUi.SetActive(false);
        itemBarUi.SetActive(false); 
        mergeCanvas.SetActive(false);
        scoreCanvas.SetActive(false);
        train.isShieldActive = true;

        train.TransitionToState(train.FinishState);

        PlayerPrefs.SetInt("lastScore", (int)score);
        PlayerPrefs.Save();
        //ScoreBoardManager.Instance.AddScore(());                                                                                                             
        StartCoroutine(ScoreUpdaterUi());
    }
    IEnumerator ScoreUpdaterUi()
    {
        finishScoreText.text = "0";
        float currentScore = 0f;
        float duration = 2.2f; // Dolum süresi (saniye cinsinden)
        float elapsedTime = 0f;

        yield return new WaitForSeconds(1f); // Baþlangýçta kýsa bir bekleme süresi

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime; // Geçen süreyi takip et
            currentScore = Mathf.Lerp(0, score, elapsedTime / duration); // Skoru lineer þekilde artýr
            finishScoreText.text = Mathf.RoundToInt(currentScore).ToString();

            yield return null; // Her karede güncelleme
        }

        // Döngü bittiðinde son skorun tam olarak eþleþtiðinden emin olun
        finishScoreText.text = Mathf.RoundToInt(score).ToString();
    }

    IEnumerator SpeedUpAtIntervals()
    {
        // 1. dakika (60 saniye) bekle
        yield return new WaitForSeconds(60);
        train.SpeedUpp();

        // 2 dakika bekle (toplamda 3 dakika geçmiþ olacak)
        yield return new WaitForSeconds(120);
        train.SpeedUpp();

        // 3 dakika bekle (toplamda 6 dakika geçmiþ olacak)
        yield return new WaitForSeconds(180);
        train.SpeedUpp();

        // 4 dakika bekle (toplamda 10 dakika geçmiþ olacak)
        yield return new WaitForSeconds(240);
        train.SpeedUpp();

        // 5 dakika bekle (toplamda 15 dakika geçmiþ olacak)
        yield return new WaitForSeconds(300);
        train.SpeedUpp();
    }
}
