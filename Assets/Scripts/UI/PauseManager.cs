using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    private Board board;
    public string newLevel;
    public bool paused = false;

    public Image musicButton;
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;
    private SoundManager sound;

	void Start ()
    {
        sound = FindObjectOfType<SoundManager>();
        pausePanel.SetActive(false);
        // board = GameObject.FindWithTag("Board").GetComponent<Board>();
        board = FindObjectOfType<Board>();

        // в настройках плеера звуковая клавиша предназначена для звука
        // если звук = 0, то без звука

        if (PlayerPrefs.HasKey("Sound"))
        {
            if(PlayerPrefs.GetInt("Sound") == 0)
            {
                musicButton.sprite = musicOffSprite;
            }
            else
            {
                musicButton.sprite = musicOnSprite;
            }
        }
        else
        {
            musicButton.sprite = musicOnSprite;
        }       
	}
	
	void Update ()
    {
		//if(paused && !pausePanel.activeInHierarchy)
  //      {
  //          pausePanel.SetActive(true);
  //          board.currentState = GameState.pause;
  //      }

  //      if(!paused && pausePanel.activeInHierarchy)
  //      {
  //          pausePanel.SetActive(false);
  //          board.currentState = GameState.move;
  //      }
	}

    public void SoundButton()
    {
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetInt("Sound") == 0)
            {
                PlayerPrefs.SetInt("Sound", 1);
                musicButton.sprite = musicOnSprite;
                sound.AdjustVolume();
            }
            else
            {
                PlayerPrefs.SetInt("Sound", 0);
                musicButton.sprite = musicOffSprite;
                sound.AdjustVolume();
            }
        }
        else
        {
            PlayerPrefs.SetInt("Sound", 0);
            musicButton.sprite = musicOffSprite;
            sound.AdjustVolume();
        }
    }

    //public void PauseGame()
    //{
    //    paused = !paused;
    //}

    public void PauseGame()
    {
        GameState holder = board.currentState;
        if(board.currentState != GameState.pause)
        {
            holder = board.currentState;
            board.currentState = GameState.pause;
            pausePanel.SetActive(true);
        }
        else
        {
            board.currentState = GameState.move;
            pausePanel.SetActive(false);
            PlayerPrefs.Save();
        }
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("Splash");
    }
}
