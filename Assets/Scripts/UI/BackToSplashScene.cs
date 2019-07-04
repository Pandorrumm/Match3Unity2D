using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;

public class BackToSplashScene : MonoBehaviour
{
    public string sceneToLoad;
    private GameData gameData;
    private Board board;

    public AdMobInterstitial ad; //реклама
  
    public void WinOK()
    {
        
        ad.ShowAds();
        
        // Music.PlaySound("Button");
        if (gameData != null && board.level < 98)
        {
            gameData.saveData.isActiv[board.level + 1] = true;
            gameData.Save();
        }
        SceneManager.LoadScene(sceneToLoad);

        //startManager.startPanel.SetActive(false);
    }

  
	
    public void LoseOK()
    {
        ad.ShowAds();
       
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
