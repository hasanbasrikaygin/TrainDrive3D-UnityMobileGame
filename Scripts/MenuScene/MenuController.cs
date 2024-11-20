using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button playButton;
    //int maxGoldCount;
    //int currentGoldCount;
    public Image fillImage;
    public TMP_Text valueText;     // Say�y� g�sterecek TextMeshPro eleman�

    public int maxGoldCount = 1000;  // Bar�n maksimum de�eri
    public int currentGoldCount = 75;// Hedef dolum de�eri

    public float fillSpeed = 10.0f; // Dolum i�leminin ba�lang�� h�z�
    public float animationDuration = 0.5f;
    public float fillDuration = 1.5f; // Say� art���n�n s�resi
    public MenuUiAnimationController animationController;
    private void Awake()
    {

          


    }
    void Start()
    {
        IncreaseTextValueWithLeanTween();
    }

    public void OpenPlayGameScene(int mod)
    {
        // Ge�i� modu bilgisini kaydediyoruz
        PlayerPrefs.SetInt("gameModNumber", mod);

        // Animasyonu ba�lat�yoruz ve tamamland���nda sahneyi y�kl�yoruz
        PlayTransitionAnimation();
    }

    // Sahne ge�i�i �ncesi animasyonu ba�latan fonksiyon
    private void PlayTransitionAnimation()
    {
        animationController.BaseUiOut(false);
        animationController.GGImagesIn();

        StartCoroutine(LoadNextScene());
    }

    // Animasyon tamamland�ktan sonra sahne y�kleme fonksiyonu
    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(2.2f);
        SceneManager.LoadScene("2048");
    }
    IEnumerator FillBar(float target)
    {
        // Bar ve metin ba�lang��ta s�f�rdan ba�l�yor
        fillImage.fillAmount = 0;
        valueText.text = "0";

        float currentFill = 0f;
        float currentSpeed = fillSpeed;

        while (currentFill < target)
        {
            // Dolum de�erini ve h�z� art�r
            currentFill = Mathf.MoveTowards(currentFill, target, currentSpeed * Time.deltaTime);
            fillImage.fillAmount = currentFill / maxGoldCount;

            // TMP ile say�y� art�rarak g�ster
            valueText.text = Mathf.RoundToInt(currentFill).ToString();

            // H�z� art�r (iste�e g�re h�zlanma oran�n� de�i�tirebilirsiniz)
            currentSpeed += fillSpeed * Time.deltaTime;

            yield return null; // Bir frame bekle ve i�lemi devam ettir
        }

        // Son de�erleri ayarla
        fillImage.fillAmount = target / maxGoldCount;
        valueText.text = target.ToString();
       
    }
    public void IncreaseTextValueWithLeanTween()
    {
        currentGoldCount = PlayerPrefs.GetInt("GoldAmount", 100);
        // 0'dan targetValue'ye kadar say� art��� yap�l�r
        LeanTween.value(0, currentGoldCount, fillDuration).setDelay(1f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float value) =>
        {
            valueText.text = Mathf.RoundToInt(value).ToString();
        });
    }
}
