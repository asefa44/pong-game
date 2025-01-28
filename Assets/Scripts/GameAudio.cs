using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource backgroundMusicSource;
   
    public AudioClip scoreSound;   
    public AudioClip winnerSound;
      
    public void PlayScoreSound()
    {
        audioSource.PlayOneShot(scoreSound);
    }    
    public void PlayWinnerSound()
    {
        audioSource.PlayOneShot(winnerSound);
    }
    public void StopBackgroundMusic()
    {
        backgroundMusicSource.Stop();
    }
    public void PlayBackgroundMusic()
    {
        backgroundMusicSource.Play();
    }
}
