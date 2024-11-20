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
    public float maxMultiplier = 5f; // �arpan�n ula�abilece�i maksimum seviye
    public float multiplierIncreaseRate = 0.1f; // Zamanla �arpan�n art�� h�z�
    private float timeElapsed = 0f;
    private bool isMultiplierBoostActive = false;
    public float multiplierBoostDuration = 12f; // �arpan art�r�c� item s�resi
    private float boostEndTime = 0f;
    public bool isGameOver = true; // Game over durumu


    public float baseSpeed = 10f; // Ba�lang�� h�z�
    public float speedIncrease = 2f; // Her seviye i�in h�z art���
    public float maxSpeed = 20f; // Maksimum h�z
    public float timeBetweenLevels = 60f; // Her seviyede ne kadar s�re ge�ecek
    private float speedIncreaseTimer = 0f; // Ge�en s�re
    private int currentLevel = 1; // Mevcut h�z seviyesi
    public float scoreTextSpeed = 10.0f; // Dolum i�leminin ba�lang�� h�z�

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

        speedIncreaseTimer += Time.deltaTime; // Oyun s�resini takip et



        if (isGameOver) return;
        // Zaman ilerledik�e �arpan� art�r
        timeElapsed += Time.deltaTime;

        // E�er �arpan maksimum seviyeye ula�mad�ysa art�r
        if (multiplier < maxMultiplier)
        {
            multiplier += multiplierIncreaseRate * Time.deltaTime;
            multiplier = Mathf.Clamp(multiplier, 1f, maxMultiplier); // Maksimum �arpana ula�t���nda durdur
        }

        // Skoru g�ncelle
        score += scoreIncreaseRate * multiplier * Time.deltaTime;
        scoreText.text = Mathf.FloorToInt(score).ToString();

        // E�er �arpan art�r�c� aktifse, s�resini kontrol et
        if (isMultiplierBoostActive && Time.time >= boostEndTime)
        {
            EndMultiplierBoost();
        }
    }

    // �arpan itemi al�nd���nda �a��r�lacak fonksiyon
    public void ActivateMultiplierBoost()
    {
        Debug.Log("Skorboost ba�lad�");
        if (!isMultiplierBoostActive)
        {
            isMultiplierBoostActive = true;
            multiplier *= 2f; // �arpan� iki kat�na ��kar
            boostEndTime = Time.time + multiplierBoostDuration; // Art�rma s�resi hesaplan�r
        }
    }

    // �arpan art�r�c� sona erdi�inde �a��r�lacak fonksiyon
    private void EndMultiplierBoost()
    {
        isMultiplierBoostActive = false;
        multiplier /= 2f; // �arpan eski haline d�ner
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
        float duration = 2.2f; // Dolum s�resi (saniye cinsinden)
        float elapsedTime = 0f;

        yield return new WaitForSeconds(1f); // Ba�lang��ta k�sa bir bekleme s�resi

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime; // Ge�en s�reyi takip et
            currentScore = Mathf.Lerp(0, score, elapsedTime / duration); // Skoru lineer �ekilde art�r
            finishScoreText.text = Mathf.RoundToInt(currentScore).ToString();

            yield return null; // Her karede g�ncelleme
        }

        // D�ng� bitti�inde son skorun tam olarak e�le�ti�inden emin olun
        finishScoreText.text = Mathf.RoundToInt(score).ToString();
    }

    IEnumerator SpeedUpAtIntervals()
    {
        // 1. dakika (60 saniye) bekle
        yield return new WaitForSeconds(60);
        train.SpeedUpp();

        // 2 dakika bekle (toplamda 3 dakika ge�mi� olacak)
        yield return new WaitForSeconds(120);
        train.SpeedUpp();

        // 3 dakika bekle (toplamda 6 dakika ge�mi� olacak)
        yield return new WaitForSeconds(180);
        train.SpeedUpp();

        // 4 dakika bekle (toplamda 10 dakika ge�mi� olacak)
        yield return new WaitForSeconds(240);
        train.SpeedUpp();

        // 5 dakika bekle (toplamda 15 dakika ge�mi� olacak)
        yield return new WaitForSeconds(300);
        train.SpeedUpp();
    }
}
