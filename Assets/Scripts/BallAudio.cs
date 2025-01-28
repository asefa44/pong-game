using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAudio : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip wallSound;
    public AudioClip ballSound;

    public void PlayWallSound()
    {
        audioSource.PlayOneShot(wallSound);
    }
    public void PlayBallSound()
    {
        audioSource.PlayOneShot(ballSound);
    }
}
