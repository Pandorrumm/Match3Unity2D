using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectManager : MonoBehaviour
{
    public GameObject[] panels; // блоки с уровнями
    public GameObject currentPanel;
    public GameObject LeftButton;
    public int page;
    private GameData gameData;
    public int currentLevel = 0;
	
	void Start ()
    {
        LeftButton.SetActive(false);

        gameData = FindObjectOfType<GameData>();

        for(int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }

        if (gameData != null)
        {
            for (int i = 0; i < gameData.saveData.isActiv.Length; i++)
            {
                if(gameData.saveData.isActiv[i])
                {
                    currentLevel = i;
                }
            }
        }
        page = (int)Mathf.Floor(currentLevel / 9);
        currentPanel = panels[page];
        panels[page].SetActive(true);
        
	}
		
	void Update ()
    {
		
	}

    public void PageRight() // стрелка в право
    {
        if (page < panels.Length - 1)
        {

            currentPanel.SetActive(false);
            page++;
            currentPanel = panels[page];
            currentPanel.SetActive(true);
            LeftButton.SetActive(true);
        }
    }

    public void PageLeft()
    {       
        if (page > 0)
        {          
            currentPanel.SetActive(false);
            page--;

            currentPanel = panels[page];
            currentPanel.SetActive(true);
            if (page == 0 && currentPanel == panels[0])
            {
                LeftButton.SetActive(false);
            }
        }
    }

    public void QuitGame()
    {
        gameData.Save();
        Application.Quit();        
        Debug.Log("Вышли из игры");
    }
}
