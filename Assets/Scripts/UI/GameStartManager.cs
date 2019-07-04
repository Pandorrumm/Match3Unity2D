using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject levelPanel;
    public GameObject trainingPanel;
    private GameData gameData;
    

    void Start ()
    {
        //if (StartGamePanel == true)
        //{
        //    Music.PlaySound("StartGameFon)");
        //}
        gameData = FindObjectOfType<GameData>();

        startPanel.SetActive(true);        
        levelPanel.SetActive(false);
    }
	
	public void PlayGame()
    {
        if (!gameData.saveData.isActiv[7]) // что бы тренировочное меню появлялось первые 7 уровней 
        {
            // Music.PlaySound("Button_1");
            startPanel.SetActive(false);
            trainingPanel.SetActive(true);
            // levelPanel.SetActive(true);
            // gameData.Load();
        }
        else
        {
            startPanel.SetActive(false);
            levelPanel.SetActive(true);
        }

    }

    public void Next()
    {
      //  Music.PlaySound("Button_1");
        trainingPanel.SetActive(false);
         levelPanel.SetActive(true);
        // gameData.Load();
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Вышли из игры");
    }

    public void Home()
    {
        startPanel.SetActive(true);
        levelPanel.SetActive(false);
    }

	void Update ()
    {
        //if (startPanel == true)
        //{
        //    MusicStartPanel.PlaySound("StartGameFon)");
        //}
    }
}
