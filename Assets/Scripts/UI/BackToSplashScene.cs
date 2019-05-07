using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToSplashScene : MonoBehaviour
{
    public string sceneToLoad;
    private GameData gameData;
    private Board board;
   

    public void WinOK()
    {
        if(gameData != null)
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
    }

    void Start()
    {
     // startManager = FindObjectOfType<GameStartManager>();
        gameData = FindObjectOfType<GameData>();
        board = FindObjectOfType<Board>();
    }
}
