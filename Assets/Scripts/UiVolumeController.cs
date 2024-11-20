using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuVolumeController1 : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Awake()
    {

    }
    void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.35f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.50f);

        SetMusicVolume();
        SetSFXVolume();


        StartCoroutine(StartMusic());
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        audioMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
    }

    IEnumerator StartMusic()
    {
        yield return new WaitForSeconds(.5f);
        AudioManager.Instance.PlayBackgroundMusic();
        AudioManager.Instance.PlayTrainSound();
    }

}
