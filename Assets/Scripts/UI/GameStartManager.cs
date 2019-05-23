using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject levelPanel;
    private GameData gameData;
	
	void Start ()
    {
        gameData = FindObjectOfType<GameData>();
        startPanel.SetActive(true);
        levelPanel.SetActive(false);
    }
	
	public void PlayGame()
    {
        startPanel.SetActive(false);
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
		
	}
}
