using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarsGame : MonoBehaviour
{

    // private int starsActive;
    public Image[] stars; //заполненные звёзды, дочерние в юнити
    private ScoreManager scoreManager;
    //public int currentScore;
    private Board board;

    private StarsAnimations starsAnimations;
    

    void Start()
    {
        starsAnimations = FindObjectOfType<StarsAnimations>();
        
        board = FindObjectOfType<Board>();
        scoreManager = FindObjectOfType<ScoreManager>();
        if (stars[0].enabled == false)
        {
            ActivateStars();
        }

    }
    private void Update()
    {
       // ActivateStars();
    }

    public void ActivateStars()
    {
        
        if (scoreManager.score > board.scoreGoals[0] && scoreManager.score < board.scoreGoals[0] + 200)
        {

            stars[0].enabled = true;
            starsAnimations.StartAnimationsBigLitl();

            //Debug.Log(board.scoreGoals[0] + 20);

            //if (scoreManager.score < board.scoreGoals[0] + 50)
            //{
            //    stars[0].enabled = true;
            //    starsAnimations.StartAnimationsBigLitl();
            //    //Debug.Log(board.scoreGoals[0] + 20);
            //}
            //if(scoreManager.score > board.scoreGoals[1])
            //{
            //    if(scoreManager.score < board.scoreGoals[1] + 50)
            //    {
            //        stars[1].enabled = true;
            //        starsAnimations.StartAnimationsBigLitl1();
            //    }
            //}
            //if (scoreManager.score > board.scoreGoals[2])
            //{
            //    if (scoreManager.score < board.scoreGoals[2] + 50)
            //    {
            //        stars[2].enabled = true;
            //        starsAnimations.StartAnimationsBigLitl2();
            //    }
            //}
        }
        if (scoreManager.score > board.scoreGoals[1] && scoreManager.score < board.scoreGoals[1] + 200)
        {

            stars[1].enabled = true;
            starsAnimations.StartAnimationsBigLitlA();
            // Debug.Log("Открыть вторую звезду!!");

        }
        if (scoreManager.score > board.scoreGoals[2] && scoreManager.score < board.scoreGoals[2] + 100)
        {

            stars[2].enabled = true;
            starsAnimations.StartAnimationsBigLitlB();
            // Debug.Log("Открыть третью звезду!!");

        }


    }

}
