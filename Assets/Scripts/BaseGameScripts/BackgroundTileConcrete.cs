using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTileConcrete : MonoBehaviour
{
    public int hitPoints; //сколько нужно урона блоку
    private SpriteRenderer sprite;
    private GoalManager goalManager;
   
    private Board board;
    public GameObject concreteDestroy;
    public Sprite hitSprite;

    private void Start()
    {
        goalManager = FindObjectOfType<GoalManager>();
        sprite = GetComponent<SpriteRenderer>();
        board = GameObject.FindWithTag("Board").GetComponent<Board>();
      
    }

    private void Update()
    {
        if (hitPoints <= 0)
        {
            if (goalManager != null)
            {
                goalManager.CompareGoal(this.gameObject.tag);
                goalManager.UpdateGoals();
            }
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(int damage) //урон
    {
        hitPoints -= damage;

        for (int i = 0; i < board.boardLayout.Length; i++)
        {
            //если плитка - "Concrete"

            if (board.boardLayout[i].tileKind == TileKind.Concrete)
            {
                Vector2 tempPosition = new Vector2(board.boardLayout[i].x, board.boardLayout[i].y);
                Instantiate(concreteDestroy, tempPosition, Quaternion.identity);
                GetComponent<SpriteRenderer>().sprite = hitSprite;
            }


        }

    }

  
}
