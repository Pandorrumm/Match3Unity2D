using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // что бы появились в unity
public class BlankGoal // незаполненная цель
{
    public int numberNeeded; //необходимое кол-во для победы
    public int numberCollected; //собранное кол-во
    public Sprite goalSprite; // какие цвета собрать нужно
    public string matchValue; //тэги кружков
}


public class GoalManager : MonoBehaviour
{
    public BlankGoal[] levelGoals;
    public List<GoalPanel> currentGoals = new List<GoalPanel>();
    public GameObject goalPrefab;
    public GameObject goalIntroParent; //в unity - Goal Container в FadePanel
    public GameObject goalGameParent; //в unity - Goal Container в TopUi
    private Board board;

    private EndGameManager endGame;

    public GameObject goalManagerAnimation;

    void Start ()
    {
        board = FindObjectOfType<Board>();
        endGame = FindObjectOfType<EndGameManager>();
        GetGoals();
        SetupGoals();
    }
	
    void GetGoals() // Получить цели
    {
        if(board != null)
        {
            if(board.world != null)
            {
                if (board.level < board.world.levels.Length)
                {
                    if (board.world.levels[board.level] != null)
                    {
                        levelGoals = board.world.levels[board.level].levelGoals;
                        for(int i = 0; i < levelGoals.Length; i ++)
                        {
                            levelGoals[i].numberCollected = 0;
                        }
                    }
                }
            }
        }
    }

    void SetupGoals() //настройка вводимых целей
    {
        for (int i = 0; i < levelGoals.Length; i++)
        {
            //Создаём панель целей в позиции goalIntroParent
            GameObject goal = Instantiate(goalPrefab, goalIntroParent.transform.position, Quaternion.identity);
            goal.transform.SetParent(goalIntroParent.transform);

            //Установим изображение и текст целей 
            GoalPanel panel = goal.GetComponent<GoalPanel>();
            panel.thisSprite = levelGoals[i].goalSprite;
            panel.thisString = "0/" + levelGoals[i].numberNeeded;


            // Создаём  панель целей в позиции goalGameParent
            GameObject gameGoal = Instantiate(goalPrefab, goalGameParent.transform.position, Quaternion.identity);
            gameGoal.transform.SetParent(goalGameParent.transform);

            //Установим изображение и текст целей в игре
            panel = gameGoal.GetComponent<GoalPanel>();
            currentGoals.Add(panel);
            panel.thisSprite = levelGoals[i].goalSprite;
            panel.thisString = "0/" + levelGoals[i].numberNeeded;
        }
    }

    public void UpdateGoals()
    {
        int goalsCompleted = 0;

        for(int i = 0; i < levelGoals.Length; i++)
        {
            currentGoals[i].thisText.text = "" + levelGoals[i].numberCollected + "/" + levelGoals[i].numberNeeded;

            if(levelGoals[i].numberCollected >= levelGoals[i].numberNeeded)
            {
                goalsCompleted++;
                currentGoals[i].thisText.text = "" + levelGoals[i].numberNeeded + "/" + levelGoals[i].numberNeeded;
            }
        }

        if(goalsCompleted >= levelGoals.Length) //если все цели выполнены
        {
            goalManagerAnimation.GetComponent<Animation>().Play("GoalContainer");

            if (endGame != null )
            {
                endGame.WinGame();
                
                board.currentState = GameState.wait;
            }
            Debug.Log("ПОБЕДААА");
        }
    }

    public void CompareGoal(string goalToCompare) // сравниваем наши цели (цель для сравнения)
    {
        for(int i = 0; i < levelGoals.Length; i++)
        {
            if (goalToCompare == levelGoals[i].matchValue)
            {
                levelGoals[i].numberCollected++;
            }
        }
    }
	
}
