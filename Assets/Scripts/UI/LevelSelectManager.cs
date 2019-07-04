using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectManager : MonoBehaviour
{
    public GameObject[] panels; // блоки с уровнями
    public GameObject currentPanel;
    public GameObject LeftButton;
    public GameObject RightButton;
    public int page;
    private GameData gameData;
    public int currentLevel = 0;
    public GameObject levelSelectPanel;  //Для музыки
   

    void Start ()
    {
        if (currentLevel < 9)
        {
            LeftButton.SetActive(false);
        }
        

        gameData = FindObjectOfType<GameData>();

        for(int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }

        // что бы при запуске открывались туры, до которых дошёл
        if (gameData != null)
        {
            for (int i = 0; i < gameData.saveData.isActiv.Length; i++)
            {
                if (gameData.saveData.isActiv[i])
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
		if(page >= 1)
        {
            LeftButton.SetActive(true);
        }
        if (/*currentLevel > 90 && */page == 10)
        {
            RightButton.SetActive(false);
        }
        else
        {
            RightButton.SetActive(true);
        }

        if(levelSelectPanel == true)
        {
            MusicStartPanel.PlaySound("StartGameFon)");
        }
     

    }

    public void PageRight() // стрелка в право
    {
       // Music.PlaySound("Button_1");
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
      //  Music.PlaySound("Button_1");
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
       // Music.PlaySound("Button_1");
        gameData.Save();
        Application.Quit();        
        Debug.Log("Вышли из игры");
    }
    
}
