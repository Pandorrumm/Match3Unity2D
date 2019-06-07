using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameType
{
    Moves, // сколько можно раз сделать ход в игре
    Time
}

[System.Serializable]
public class EndGameRequirements //требования к концу игры
{
    public GameType gameType;
    public int counterValue; //значение счетчика
}

public class EndGameManager : MonoBehaviour
{
    
    public GameObject movesLabel;
    public GameObject timeLabel;
    public GameObject youWinPanel;
    public GameObject tryAgainPanel;
    public GameObject starsWinPanel;
    public Text counter; //счётчик
    public EndGameRequirements requirements;
    public int currentCounterValue;
    private float timerSeconds;
    private Board board;
    //private LevelButton levelButton;

    void Start ()
    {
        board = FindObjectOfType<Board>();
        //levelButton = FindObjectOfType<LevelButton>();
        SetGameType();
        SetupGame();

	}
	
    void SetGameType() // Установить тип игры
    {
        if(board.world != null)
        {
            if (board.level < board.world.levels.Length)
            {
                if (board.world.levels[board.level] != null)
                {
                    requirements = board.world.levels[board.level].endGameRequirements;
                }
            }
        }
    }


    void SetupGame()
    {
        currentCounterValue = requirements.counterValue;

        if (requirements.gameType == GameType.Moves)
        {
            movesLabel.SetActive(true);
            timeLabel.SetActive(false);
        }
        else
        {
            timerSeconds = 1;
            movesLabel.SetActive(false);
            timeLabel.SetActive(true);
        }
        counter.text = "" + currentCounterValue;
    }

    public void DecreaseCounterValue() // уменьшение значения счётчика(очки или время) и проигрыш
    {
        if (board.currentState != GameState.pause)
        {
            currentCounterValue--;
            counter.text = "" + currentCounterValue;

            if (currentCounterValue <= 0) //если кончились ходы
            {
                LoseGame();
            }
        }    
    }
	
    public void WinGame()
    {
        //youWinPanel.SetActive(true);
        Invoke("YouWinPanel", 2); // задержка в появлении панели
        board.currentState = GameState.win;
        currentCounterValue = 0;
        counter.text = "" + currentCounterValue;
        FadePanelController fade = FindObjectOfType<FadePanelController>();
        fade.GameOver();

    }

    public void YouWinPanel()
    {
        youWinPanel.SetActive(true);
        // starsWinPanel.SetActive(true);
        Invoke("ActivStarsWinPanel", 1);
    }
    public void ActivStarsWinPanel()
    {
        starsWinPanel.SetActive(true);
    }

    public void LoseGame()
    {
        tryAgainPanel.SetActive(true);
        board.currentState = GameState.lose;
        Debug.Log("Вы проиграли!");
        currentCounterValue = 0;
        counter.text = "" + currentCounterValue;
        FadePanelController fade = FindObjectOfType<FadePanelController>();
        fade.GameOver(); // вылет панели анимация
    }

	void Update ()
    {
		if(requirements.gameType == GameType.Time && currentCounterValue > 0)
        {
            timerSeconds -= Time.deltaTime;

            if(timerSeconds <= 0)
            {
                DecreaseCounterValue();
                timerSeconds = 1;
            }
        }
	}
}
