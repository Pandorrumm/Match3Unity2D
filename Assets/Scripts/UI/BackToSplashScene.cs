using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToSplashScene : MonoBehaviour
{
    public string sceneToLoad;
    private GameData gameData;
    private Board board;

   // public GameObject startGamePanel;

    public void WinOK()
    {

        if(gameData != null && board.level < 98)
        {
            gameData.saveData.isActiv[board.level + 1] = true;
            gameData.Save();
        }
        SceneManager.LoadScene(sceneToLoad);
        //startManager.startPanel.SetActive(false);
    }
	
    public void LoseOK()
    {       
        SceneManager.LoadScene(sceneToLoad);
        //startGamePanel.SetActive(false);
    }

    void Start()
    {
     // startManager = FindObjectOfType<GameStartManager>();
        gameData = FindObjectOfType<GameData>();
        board = FindObjectOfType<Board>();
    }
}
