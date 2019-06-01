using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SrarsWinPanel : MonoBehaviour
{

    // private int starsActive;
    public Image[] stars; //заполненные звёзды, дочерние в юнити
    private ScoreManager scoreManager;
    //public int currentScore;
    private Board board;

   // private StarsAnimations starsAnimations;


    void Start()
    {
        //starsAnimations = FindObjectOfType<StarsAnimations>();

        board = FindObjectOfType<Board>();
        scoreManager = FindObjectOfType<ScoreManager>();
        ActivateStars();

    }
    private void Update()
    {
        // ActivateStars();
    }

    public void ActivateStars()
    {
        //for (int i = 0; i < board.scoreGoals.Length; i++)
        //{

        // scoreManager.IncreaseScore(currentScore);
        //currentScore += scoreManager.score;
        //if (currentScore == board.scoreGoals[0])
        //{
        //    stars[0].enabled = true;
        //    Debug.Log("Открыть первую звезду!!");

        //}
        if (scoreManager.score > board.scoreGoals[0])
        {
            stars[0].enabled = true;
            //starsAnimations.StartAnimationsBigLitl();
            //Debug.Log(board.scoreGoals[0] + 20);
          
        }

        if (scoreManager.score > board.scoreGoals[1] )
        {
            stars[1].enabled = true;
            //starsAnimations.StartAnimationsBigLitlA();
            // Debug.Log("Открыть вторую звезду!!");
        }

        if (scoreManager.score > board.scoreGoals[2] )
        {
            stars[2].enabled = true;
           // starsAnimations.StartAnimationsBigLitlB();
            // Debug.Log("Открыть третью звезду!!");
        }


    }

}
