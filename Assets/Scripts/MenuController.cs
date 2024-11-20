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
    public TMP_Text valueText;     // Sayýyý gösterecek TextMeshPro elemaný

    public int maxGoldCount = 1000;  // Barýn maksimum deðeri
    public int currentGoldCount = 75;// Hedef dolum deðeri

    public float fillSpeed = 10.0f; // Dolum iþleminin baþlangýç hýzý
    public float animationDuration = 0.5f;
    public float fillDuration = 1.5f; // Sayý artýþýnýn süresi
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
        // Geçiþ modu bilgisini kaydediyoruz
        PlayerPrefs.SetInt("gameModNumber", mod);

        // Animasyonu baþlatýyoruz ve tamamlandýðýnda sahneyi yüklüyoruz
        PlayTransitionAnimation();
    }

    // Sahne geçiþi öncesi animasyonu baþlatan fonksiyon
    private void PlayTransitionAnimation()
    {
        animationController.BaseUiOut(false);
        animationController.GGImagesIn();

        StartCoroutine(LoadNextScene());
    }

    // Animasyon tamamlandýktan sonra sahne yükleme fonksiyonu
    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(2.2f);
        SceneManager.LoadScene("2048");
    }
    IEnumerator FillBar(float target)
    {
        // Bar ve metin baþlangýçta sýfýrdan baþlýyor
        fillImage.fillAmount = 0;
        valueText.text = "0";

        float currentFill = 0f;
        float currentSpeed = fillSpeed;

        while (currentFill < target)
        {
            // Dolum deðerini ve hýzý artýr
            currentFill = Mathf.MoveTowards(currentFill, target, currentSpeed * Time.deltaTime);
            fillImage.fillAmount = currentFill / maxGoldCount;

            // TMP ile sayýyý artýrarak göster
            valueText.text = Mathf.RoundToInt(currentFill).ToString();

            // Hýzý artýr (isteðe göre hýzlanma oranýný deðiþtirebilirsiniz)
            currentSpeed += fillSpeed * Time.deltaTime;

            yield return null; // Bir frame bekle ve iþlemi devam ettir
        }

        // Son deðerleri ayarla
        fillImage.fillAmount = target / maxGoldCount;
        valueText.text = target.ToString();
       
    }
    public void IncreaseTextValueWithLeanTween()
    {
        currentGoldCount = PlayerPrefs.GetInt("GoldAmount", 100);
        // 0'dan targetValue'ye kadar sayý artýþý yapýlýr
        LeanTween.value(0, currentGoldCount, fillDuration).setDelay(1f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float value) =>
        {
            valueText.text = Mathf.RoundToInt(value).ToString();
        });
    }
}
