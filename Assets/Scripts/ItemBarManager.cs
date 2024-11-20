using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class ItemBarManager : MonoBehaviour
{
    public GameObject wingsPanel;
    public GameObject zoomoutPanel;
    public GameObject x2Panel;
    public GameObject shieldPanel;
    public GameObject speedBoostPanel;
    public GameObject slowMotionPanel;
    

    public Image wingsBar;
    public Image zoomoutBar;
    public Image x2Bar;
    public Image shieldBar;
    public Image speedBoostBar;
    public Image slowMotionBar;

    // Her bir bar için ayrý coroutine tanýmlýyoruz.
    private Coroutine wingsCoroutine;
    private Coroutine zoomoutCoroutine;
    private Coroutine x2Coroutine;
    private Coroutine shieldCoroutine;
    private Coroutine speedBoostCoroutine;
    private Coroutine slowMotionCoroutine;

    public Train train;

    public float slowdownFactor = 0.05f;
    public float slowdownLength = 2f;
    // Genel fonksiyon, hem bar hem de paneli yönetiyor.
    public void ActivateBar(string abilityName, float duration)
    {
        GameObject selectedPanel = null;
        Image selectedBar = null;
        Coroutine selectedCoroutine = null;

        // Hangi yeteneðe ait panel ve bar'ýn seçileceðini belirliyoruz.
        switch (abilityName)
        {
            case "wings":
                selectedPanel = wingsPanel;
                selectedBar = wingsBar;
                selectedCoroutine = wingsCoroutine;
                break;
            case "zoomout":
                selectedPanel = zoomoutPanel;
                selectedBar = zoomoutBar;
                selectedCoroutine = zoomoutCoroutine;
                break;
            case "x2":
                selectedPanel = x2Panel;
                selectedBar = x2Bar;
                selectedCoroutine = x2Coroutine;
                break;
            case "shield":
                selectedPanel = shieldPanel;
                selectedBar = shieldBar;
                selectedCoroutine = shieldCoroutine;
                break;
            case "speedBoost":
                selectedPanel = speedBoostPanel;
                selectedBar = speedBoostBar;
                selectedCoroutine = speedBoostCoroutine;
                break;
            case "slowMotion":
                selectedPanel = slowMotionPanel;
                selectedBar = slowMotionBar;
                selectedCoroutine = slowMotionCoroutine;
                break;
            default:
                Debug.LogWarning("Geçersiz yetenek ismi!");
                return;
        }

        if (selectedCoroutine != null)
            StopCoroutine(selectedCoroutine); // Ayný bar için devam eden coroutine varsa durdur.

        selectedPanel.SetActive(true);  // Paneli aktif et.
        selectedBar.fillAmount = 1f;    // Bar'ý tamamen doldur.

        // Her yetenek için ayrý coroutine çalýþtýrýyoruz.
        switch (abilityName)
        {
            case "wings":
                wingsCoroutine = StartCoroutine(UpdateBar(selectedPanel, selectedBar, duration, abilityName));
                break;
            case "zoomout":
                zoomoutCoroutine = StartCoroutine(UpdateBar(selectedPanel, selectedBar, duration , abilityName));
                break;
            case "x2":
                x2Coroutine = StartCoroutine(UpdateBar(selectedPanel, selectedBar, duration, abilityName));
                break;
            case "shield":
                shieldCoroutine = StartCoroutine(UpdateBar(selectedPanel, selectedBar, duration , abilityName));
                break;
            case "speedBoost":
                speedBoostCoroutine = StartCoroutine(UpdateBar(selectedPanel, selectedBar, duration, "speedBoost"));
                break;
            case "slowMotion":
                slowMotionCoroutine = StartCoroutine(UpdateBar(selectedPanel, selectedBar, duration, "slowMotion"));
                break;
        }
    }

    // Bar'ý ve paneli güncelleyen geri sayým fonksiyonu
    private IEnumerator UpdateBar(GameObject panel, Image bar, float duration,string abilityNames)
    {
        if (abilityNames== "speedBoost")
        {
            train.SpeedUpp();
        }
        if (abilityNames== "shield")
        {
            train.isShieldActive = true;
        }
        if (abilityNames == "slowMotion")
        {
            StartSlowmotion();
        }
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            bar.fillAmount = 1f - (elapsed / duration); // Geri sayým ile barýn doluluðunu ayarla.
            yield return null;
        }
        panel.SetActive(false); // Süre dolunca paneli kapat.
        if (abilityNames == "speedBoost")
        {
            train.SpeedDownn();
        }
        if (abilityNames == "shield")
        {
            train.isShieldActive = false;
        }
        if (abilityNames == "slowMotion")
        {
            StopSlowmotion();
        }
        
    }

    public void StartSlowmotion()
    {
        Time.timeScale = slowdownFactor;  // Zamaný yavaþlat
        Time.fixedDeltaTime = Time.timeScale * 0.2f;  // Fizik adýmlarýný da yavaþlat
    }

    // Oyunu normal hýza döndüren fonksiyon
    public void StopSlowmotion()
    {
        Time.timeScale = 1f;  // Zamaný normale döndür
        Time.fixedDeltaTime = Time.timeScale * 0.2f;  // Fizik adýmlarýný da normale döndür
    }
}
