using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private Board board;
    public Text scoreText;
    public int score;
    public Image scoreBar; // увеличивающаяся полоска
    private GameData gameData;
    private int numberStars; // кол - во звёзд
    private StarsGame starsGame;
	
	void Start ()
    {
        board = FindObjectOfType<Board>();
        gameData = FindObjectOfType<GameData>();
        starsGame = FindObjectOfType<StarsGame>();

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

        starsGame.ActivateStars(); //звёзды при увеличении очков на слайдере

        for (int i = 0; i < board.scoreGoals.Length; i ++)
        {
            if(score > board.scoreGoals[i] && numberStars < i + 1)
            {
                numberStars++;               
            }
        }

        if (gameData != null)
        {
            int highscore = gameData.saveData.highScores[board.level];

            if (score > highscore)
            {
                gameData.saveData.highScores[board.level] = score;
               // gameData.saveData.stars[board.level] = numberStars;
            }

            int currentStars = gameData.saveData.stars[board.level];

            if(numberStars > currentStars)
            {
                gameData.saveData.stars[board.level] = numberStars;
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
