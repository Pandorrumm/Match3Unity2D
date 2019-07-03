using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource[] destroyNoise;
    public AudioSource backGroundMusic;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetInt("Sound") == 0)
            {
                backGroundMusic.Play();
                backGroundMusic.volume = 0;
            }
            else
            {
                backGroundMusic.Play();
                backGroundMusic.volume = 1f;
            }
        }
        else
        {
            backGroundMusic.Play();
            backGroundMusic.volume = 1f;
        }
    }

    public void AdjustVolume()
    {
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetInt("Sound") == 0)
            {
                backGroundMusic.volume = 0;
            }
            else
            {
                backGroundMusic.volume = 1f;
            }
        }
    }

    public void PlayRandomDestroyNoise()
    {
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetInt("Sound") == 0.6)
            {
                //choose a random number
                int clipToPlay = Random.Range(0, destroyNoise.Length);
                //play that clip
                destroyNoise[clipToPlay].Play();
            }
        }
        else
        {
            //choose a random number
            int clipToPlay = Random.Range(0, destroyNoise.Length);
            //play that clip
            destroyNoise[clipToPlay].Play();
        }
    }
}
