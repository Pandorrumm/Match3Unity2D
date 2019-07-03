using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicStartPanel : MonoBehaviour {

    static AudioSource myMusicStartPanel;
    public static AudioClip musicStartPanel;
    private void Start()
    {
        musicStartPanel = Resources.Load<AudioClip>("StartGameFon)");

        myMusicStartPanel = GetComponent<AudioSource>();
    }


    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "StartGameFon)":
              //  myMusicStartPanel.PlayOneShot(musicStartPanel);
                break;

        }
    }

}
