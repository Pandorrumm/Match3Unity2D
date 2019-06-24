using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickDamage : MonoBehaviour {

    public int hitsToBreak;
    public Sprite hitSprite;
    public GameObject concreteDestroy;
    private Board board;
    //public GameObject slimeDestroy;
    //public GameObject lockDestroy;

    void Start () {
        board = GameObject.FindWithTag("Board").GetComponent<Board>();
    }

    public void BreakBrick()
    {
        hitsToBreak--;
        for (int i = 0; i < board.boardLayout.Length; i++)
        {
            //если плитка - "Concrete"
            //if (board.boardLayout[i].tileKind == TileKind.Concrete)
            if (board.boardLayout[i].tileKind == TileKind.Concrete)
            {
                Vector2 tempPosition = new Vector2(board.boardLayout[i].x, board.boardLayout[i].y);
                Instantiate(concreteDestroy, tempPosition, Quaternion.identity);
                GetComponent<SpriteRenderer>().sprite = hitSprite;
            }

        }
    }
}
