using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuVolumeController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;      // AudioMixer referansý
    [SerializeField] private Slider musicSlider;         // Müzik sesini kontrol eden slider
    [SerializeField] private Slider sfxSlider;           // SFX (efekt) sesini kontrol eden slider
    [SerializeField] private AudioMixerGroup musicGroup; // Müzik için AudioMixerGroup
    [SerializeField] private AudioMixerGroup sfxGroup;   // Ses efektleri için AudioMixerGroup
    [SerializeField] private GameObject musicOn;         // Müzik açýk UI elementi
    [SerializeField] private GameObject musicOff;        // Müzik kapalý UI elementi
    [SerializeField] private GameObject sfxOn;           // SFX açýk UI elementi
    [SerializeField] private GameObject sfxOff;          // SFX kapalý UI elementi

    void Start()
    {
        // Önceden kaydedilmiþ müzik ve SFX ses seviyelerini PlayerPrefs'den yükle
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.35f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.50f);

        // Baþlangýçta ses seviyelerini uygula
        SetMusicVolume();
        SetSFXVolume();
        
    }

    // Müzik ses seviyesini musicSlider deðerine göre ayarlar
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        if(volume < 0.0002)
        {
            musicOff.SetActive(true);
            musicOn.SetActive(false);
        }
        else
        {
            musicOff.SetActive(false);
            musicOn.SetActive(true);
        }
        audioMixer.SetFloat("music", Mathf.Log10(volume) * 20); // Desibele çevirir
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value); // Yeni deðeri kaydeder
    }

    // SFX ses seviyesini sfxSlider deðerine göre ayarlar
    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        if (volume < 0.0002)
        {
            sfxOff.SetActive(true);
            sfxOn.SetActive(false);
        }
        else
        {
            sfxOff.SetActive(false);
            sfxOn.SetActive(true);
        }
        audioMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);   // Desibele çevirir
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);     // Yeni deðeri kaydeder
    }

    // Bir süre sonra arka plan müziðini baþlatan coroutine
    IEnumerator StartMusic()
    {
        yield return new WaitForSeconds(.5f);
        MenuAudioManager.Instance.PlayBackgroundMusic();
    }

    // Kaydedilen müzik ses seviyesini geri yükler ve sliderý günceller
    public void MusicOn()
    {
        float savedVolume = PlayerPrefs.GetFloat("currentMusicVolume", .5f);
        audioMixer.SetFloat("music", Mathf.Log10(savedVolume) * 20); // Kaydedilen sesi desibele çevirip geri yükler
        musicSlider.value = savedVolume;  // Sliderý mevcut ses seviyesine göre günceller
    }

    // Müziði kapatýr ve mevcut ses seviyesini PlayerPrefs'e kaydeder
    public void MusicOff()
    {
        PlayerPrefs.SetFloat("currentMusicVolume", musicSlider.value); // Mevcut müzik sesini kaydet
        PlayerPrefs.Save(); // PlayerPrefs deðiþikliklerini kaydet
        audioMixer.SetFloat("music", -80f); // Müzik sesini minimuma (mute) getir
        musicSlider.value = 0f; // Sliderý kapalý duruma getir
    }

    // Kaydedilen SFX ses seviyesini geri yükler ve sliderý günceller
    public void SfxOn()
    {
        float savedSfxVolume = PlayerPrefs.GetFloat("currentSfxVolume", .5f);
        sfxSlider.value = savedSfxVolume;  // Sliderý mevcut SFX ses seviyesine göre günceller
        audioMixer.SetFloat("sfx", Mathf.Log10(savedSfxVolume) * 20); // Kaydedilen sesi desibele çevirip geri yükler
    }

    // SFX'i kapatýr ve mevcut ses seviyesini PlayerPrefs'e kaydeder
    public void SfxOff()
    {
        PlayerPrefs.SetFloat("currentSfxVolume", sfxSlider.value); // Mevcut SFX sesini kaydet
        PlayerPrefs.Save();
        sfxSlider.value = 0f;  // Sliderý kapalý duruma getir
        audioMixer.SetFloat("sfx", -80f); // SFX sesini minimuma (mute) getir
    }
    public void QuitGame()
    {


        // Editor modunda test ediyorsan çalýþmayabilir, bu yüzden editor kontrolü ekliyoruz.
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

}
