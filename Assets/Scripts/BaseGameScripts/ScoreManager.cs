using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private Board board;
    public Text scoreText;
    public int score;
    public Image scoreBar;
    private GameData gameData;

	
	void Start ()
    {
        board = FindObjectOfType<Board>();
        gameData = FindObjectOfType<GameData>();
        UpdateBar();

    }
	
	void Update ()
    {
        scoreText.text = "" + score;
	}

    public void IncreaseScore(int amountToIncrease)// увелич-е очков 
                                                   //(int сумма для увеличения) 
    {
        score += amountToIncrease;

        if (gameData != null)
        {
            int highscore = gameData.saveData.highScores[board.level];
            if (score > highscore)
            {
                gameData.saveData.highScores[board.level] = score;
            }
            gameData.Save();
        }
        UpdateBar();
    }

    private void UpdateBar()
    {
        if (board != null && scoreBar != null)
        {
            int length = board.scoreGoals.Length;
            //fillAmount - бегунок в юнити на картинке шкалы очков
            scoreBar.fillAmount = (float)score / (float)board.scoreGoals[length - 1];
        }
    }
}
