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

    // Her bir bar i�in ayr� coroutine tan�ml�yoruz.
    private Coroutine wingsCoroutine;
    private Coroutine zoomoutCoroutine;
    private Coroutine x2Coroutine;
    private Coroutine shieldCoroutine;
    private Coroutine speedBoostCoroutine;
    private Coroutine slowMotionCoroutine;

    public Train train;

    public float slowdownFactor = 0.05f;
    public float slowdownLength = 2f;
    // Genel fonksiyon, hem bar hem de paneli y�netiyor.
    public void ActivateBar(string abilityName, float duration)
    {
        GameObject selectedPanel = null;
        Image selectedBar = null;
        Coroutine selectedCoroutine = null;

        // Hangi yetene�e ait panel ve bar'�n se�ilece�ini belirliyoruz.
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
                Debug.LogWarning("Ge�ersiz yetenek ismi!");
                return;
        }

        if (selectedCoroutine != null)
            StopCoroutine(selectedCoroutine); // Ayn� bar i�in devam eden coroutine varsa durdur.

        selectedPanel.SetActive(true);  // Paneli aktif et.
        selectedBar.fillAmount = 1f;    // Bar'� tamamen doldur.

        // Her yetenek i�in ayr� coroutine �al��t�r�yoruz.
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

    // Bar'� ve paneli g�ncelleyen geri say�m fonksiyonu
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
            bar.fillAmount = 1f - (elapsed / duration); // Geri say�m ile bar�n dolulu�unu ayarla.
            yield return null;
        }
        panel.SetActive(false); // S�re dolunca paneli kapat.
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
        Time.timeScale = slowdownFactor;  // Zaman� yava�lat
        Time.fixedDeltaTime = Time.timeScale * 0.2f;  // Fizik ad�mlar�n� da yava�lat
    }

    // Oyunu normal h�za d�nd�ren fonksiyon
    public void StopSlowmotion()
    {
        Time.timeScale = 1f;  // Zaman� normale d�nd�r
        Time.fixedDeltaTime = Time.timeScale * 0.2f;  // Fizik ad�mlar�n� da normale d�nd�r
    }
}
