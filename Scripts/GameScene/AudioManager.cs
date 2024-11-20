using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource backgroundMusicSource;
    public AudioSource trainSoundSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip gameStartClip;
    public AudioClip gameOverClip;
    public AudioClip pauseClip;
    public AudioClip wagonPickupClip;
    public AudioClip wagonDropoffClip;
    public AudioClip passengerPickupClip;
    public AudioClip passengerDropoffClip;
    public AudioClip coinCollectClip;
    public AudioClip buttonClickClip;
    public AudioClip trainLoopClip;
    public AudioClip backgroundMusicClip;
    public AudioClip itemCollectClip;
    public AudioClip itemSpeedUpClip;
    public AudioClip itemShiledClip;
    public AudioClip eagleScreamClip;
    public AudioClip scoreBoosterClip;
    public AudioClip trainHornClip;
    public AudioClip jumpClip;
    public AudioClip slowMotionClip;
    public AudioClip zoomOutClip;
    public AudioClip finishStateClip;
    public AudioClip countdownClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Prevent AudioManager from being destroyed on scene change
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Start background music and train sound

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
    }   
    public void StopTrainMusic()
    {
        trainSoundSource.clip = trainLoopClip;
        trainSoundSource.Stop();
    }

    public void PlayTrainSound()
    {
        trainSoundSource.clip = trainLoopClip;
        trainSoundSource.loop = true;
        trainSoundSource.pitch = 0.35f;
        //trainSoundSource.volume = 0.1f;
        trainSoundSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    // Individual methods for playing specific sound effects
    public void PlayGameStart()
    {
        PlaySFX(gameStartClip);
    }

    public void PlayGameOver()
    {
        PlaySFX(gameOverClip);
    }

    public void PlayPause()
    {
        PlaySFX(pauseClip);
    }

    public void PlayWagonPickup()
    {
        PlaySFX(wagonPickupClip);
    }

    public void PlayWagonDropoff()
    {
        PlaySFX(wagonDropoffClip);
    }

    public void PlayPassengerPickup()
    {
        PlaySFX(passengerPickupClip); 
    }

    public void PlayPassengerDropoff()
    {
        PlaySFX(passengerDropoffClip);
    }

    public void PlayCoinCollect()
    {
        PlaySFX(coinCollectClip);
    }

    public void PlayButtonClick()
    {
        PlaySFX(buttonClickClip);
    }

    public void PlayItemCollect()
    {
        PlaySFX(itemCollectClip);
    }   
    public void PlayItemSpeedUp()
    {
        PlaySFX(itemSpeedUpClip);
    } 
    public void PlayItemShiled()
    {
        PlaySFX(itemShiledClip);
    }   
    public void PlayEagleScream()
    {
        PlaySFX(eagleScreamClip);
    }  
    public void PlayScoreBooster()
    {
        PlaySFX(scoreBoosterClip);
    } 
    public void PlayTrainHorn()
    {
        PlaySFX(trainHornClip);
    }  
    public void PlayJump()
    {
        PlaySFX(jumpClip);
    }   
    public void PlaySlowMotion()
    {
        PlaySFX(slowMotionClip);
    }   
    public void PlayZoomOut()
    {
        PlaySFX(zoomOutClip);
    }  
    public void PlayFinish()
    {
        PlaySFX(finishStateClip);
    }  
    public void PlayCountdown()
    {
        PlaySFX(countdownClip);
    }
}
