using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuVolumeController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;      // AudioMixer referans�
    [SerializeField] private Slider musicSlider;         // M�zik sesini kontrol eden slider
    [SerializeField] private Slider sfxSlider;           // SFX (efekt) sesini kontrol eden slider
    [SerializeField] private AudioMixerGroup musicGroup; // M�zik i�in AudioMixerGroup
    [SerializeField] private AudioMixerGroup sfxGroup;   // Ses efektleri i�in AudioMixerGroup
    [SerializeField] private GameObject musicOn;         // M�zik a��k UI elementi
    [SerializeField] private GameObject musicOff;        // M�zik kapal� UI elementi
    [SerializeField] private GameObject sfxOn;           // SFX a��k UI elementi
    [SerializeField] private GameObject sfxOff;          // SFX kapal� UI elementi

    void Start()
    {
        // �nceden kaydedilmi� m�zik ve SFX ses seviyelerini PlayerPrefs'den y�kle
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.35f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.50f);

        // Ba�lang��ta ses seviyelerini uygula
        SetMusicVolume();
        SetSFXVolume();
        
    }

    // M�zik ses seviyesini musicSlider de�erine g�re ayarlar
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
        audioMixer.SetFloat("music", Mathf.Log10(volume) * 20); // Desibele �evirir
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value); // Yeni de�eri kaydeder
    }

    // SFX ses seviyesini sfxSlider de�erine g�re ayarlar
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
        audioMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);   // Desibele �evirir
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);     // Yeni de�eri kaydeder
    }

    // Bir s�re sonra arka plan m�zi�ini ba�latan coroutine
    IEnumerator StartMusic()
    {
        yield return new WaitForSeconds(.5f);
        MenuAudioManager.Instance.PlayBackgroundMusic();
    }

    // Kaydedilen m�zik ses seviyesini geri y�kler ve slider� g�nceller
    public void MusicOn()
    {
        float savedVolume = PlayerPrefs.GetFloat("currentMusicVolume", .5f);
        audioMixer.SetFloat("music", Mathf.Log10(savedVolume) * 20); // Kaydedilen sesi desibele �evirip geri y�kler
        musicSlider.value = savedVolume;  // Slider� mevcut ses seviyesine g�re g�nceller
    }

    // M�zi�i kapat�r ve mevcut ses seviyesini PlayerPrefs'e kaydeder
    public void MusicOff()
    {
        PlayerPrefs.SetFloat("currentMusicVolume", musicSlider.value); // Mevcut m�zik sesini kaydet
        PlayerPrefs.Save(); // PlayerPrefs de�i�ikliklerini kaydet
        audioMixer.SetFloat("music", -80f); // M�zik sesini minimuma (mute) getir
        musicSlider.value = 0f; // Slider� kapal� duruma getir
    }

    // Kaydedilen SFX ses seviyesini geri y�kler ve slider� g�nceller
    public void SfxOn()
    {
        float savedSfxVolume = PlayerPrefs.GetFloat("currentSfxVolume", .5f);
        sfxSlider.value = savedSfxVolume;  // Slider� mevcut SFX ses seviyesine g�re g�nceller
        audioMixer.SetFloat("sfx", Mathf.Log10(savedSfxVolume) * 20); // Kaydedilen sesi desibele �evirip geri y�kler
    }

    // SFX'i kapat�r ve mevcut ses seviyesini PlayerPrefs'e kaydeder
    public void SfxOff()
    {
        PlayerPrefs.SetFloat("currentSfxVolume", sfxSlider.value); // Mevcut SFX sesini kaydet
        PlayerPrefs.Save();
        sfxSlider.value = 0f;  // Slider� kapal� duruma getir
        audioMixer.SetFloat("sfx", -80f); // SFX sesini minimuma (mute) getir
    }
    public void QuitGame()
    {


        // Editor modunda test ediyorsan �al��mayabilir, bu y�zden editor kontrol� ekliyoruz.
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

}
