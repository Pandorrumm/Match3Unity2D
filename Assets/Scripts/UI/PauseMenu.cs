using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
   
    public string newLevel;
    public bool paused = false;
    private Board board;
    

    public static bool GameIsPaused;
   // public GameObject pauseMenuUi;


    //public AdMobInterstitial ad;

    private void Start()
    {
        //pausePanel.SetActive(false);
        GameIsPaused = false;
        Time.timeScale = 1;
        board = FindObjectOfType<Board>();
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Continue();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Continue()
    {
        Music.PlaySound("Button");
        GameState holder1 = board.currentState;
        board.currentState = GameState.move;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
       // pausePanel.SetActive(true);
       // Time.timeScale = 0f;
        GameIsPaused = true;
        GameState holder = board.currentState;
        if (board.currentState != GameState.pause)
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

    public void LoadMenu()
    {
       // ad.ShowAds();
        Time.timeScale = 1f;
        SceneManager.LoadScene("ChoiceLevels");
    }

    public void QuitGame()
    {
        Music.PlaySound("Button");
        SceneManager.LoadScene("Splash");
        Music.PlaySound("StartGameFon)");


    }
}
