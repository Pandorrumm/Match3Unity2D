using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Music : MonoBehaviour {

  //  public Slider volume;
    static AudioSource myMusic;
    public static AudioClip concretDamage, concreteDestroy, lockTile, slim, slimStart, breakable, lose, win;

    private void Start()
    {
        concretDamage = Resources.Load<AudioClip>("Concrete Damage");
        concreteDestroy = Resources.Load<AudioClip>("Concrete Destroy");
        lockTile = Resources.Load<AudioClip>("Lock");
        slim = Resources.Load<AudioClip>("Slim");
        slimStart = Resources.Load<AudioClip>("Slim Start");
        breakable = Resources.Load<AudioClip>("Breakable");
        lose = Resources.Load<AudioClip>("Lose");
        win = Resources.Load<AudioClip>("Win");

        myMusic = GetComponent<AudioSource>();
    }

    void Update()
    {
       // myMusic.volume = volume.value;
    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "Concrete Damage":
                myMusic.PlayOneShot(concretDamage);
                break;
            case "Concrete Destroy":
                myMusic.PlayOneShot(concreteDestroy);
                break;
            case "Lock":
                myMusic.PlayOneShot(lockTile);
                break;
            case "Slim":
                myMusic.PlayOneShot(slim);
                break;
            case "Slim Start":
                myMusic.PlayOneShot(slimStart);
                break;
            case "Breakable":
                myMusic.PlayOneShot(breakable);
                break;
            case "Lose":
                myMusic.PlayOneShot(lose);
                break;
            case "Win":
                myMusic.PlayOneShot(win);
                break;



        }
    }
}
