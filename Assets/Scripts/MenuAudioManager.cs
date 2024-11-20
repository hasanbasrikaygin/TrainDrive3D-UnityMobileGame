using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudioManager : MonoBehaviour
{
    public static MenuAudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource buttonSource;
    public AudioSource backgroundMusicSource;

    [Header("Audio Clips")]
    public AudioClip buttonClip;
    public AudioClip buyClip;
    public AudioClip insufficientCoinClip;
    public AudioClip backgroundMusicClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        StartCoroutine(StartMusic());
    }

    public void PlayBackgroundMusic()
    {
        backgroundMusicSource.clip = backgroundMusicClip;
        backgroundMusicSource.loop = true;
        backgroundMusicSource.Play();
    }
    public void StopBackgroundMusic()
    {
        backgroundMusicSource.clip = backgroundMusicClip;
        backgroundMusicSource.Stop();
        // Update is called once per frame
    }

    IEnumerator StartMusic()
    {
        yield return new WaitForSeconds(.5f);
        PlayBackgroundMusic();
    }
    public void PlaySFX(AudioClip clip)
    {
        buttonSource.PlayOneShot(clip);
    }
    public void PlayButtonSound()
    {
        PlaySFX(buttonClip);
    }
    public void PlayBuySound()
    {
        PlaySFX(buyClip);
    }   
    public void PlayInsufficientCoinSound()
    {
        PlaySFX(insufficientCoinClip);
    }
}
