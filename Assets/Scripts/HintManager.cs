using System.Collections;
using System.Collections.Generic;
using UnityEngine;

  // подсказки

public class HintManager : MonoBehaviour 
{
    private Board board;
    public float hintDelay; // задержка подсказки(секунды)
    private float hintDelaySeconds;
    public GameObject hintParticle; //particleSystem
    public GameObject currentHint; //текущая подсказка

    void Start ()
    {
        board = FindObjectOfType<Board>();
        hintDelaySeconds = hintDelay;
	}
	
	void Update ()
    {
        hintDelaySeconds -= Time.deltaTime;
        if(hintDelaySeconds <= 0 && currentHint == null)
        {
            MarkHint();
            hintDelaySeconds = hintDelay; 
        }
	}

    //находим все возможные совпадения на доске и кидаем их в список
    List<GameObject> FindAllMatches()
    {
        List<GameObject> possibleMoves = new List<GameObject>();

        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                if (board.allCircle[i, j] != null)
                {
                    if (i < board.width - 1)
                    {
                        if (board.SwitchAndCheck(i, j, Vector2.right))
                        {
                            possibleMoves.Add(board.allCircle[i, j]);
                        }
                    }

                    if (j < board.height - 1)
                    {
                        if (board.SwitchAndCheck(i, j, Vector2.up))
                        {
                            possibleMoves.Add(board.allCircle[i, j]);
                        }
                    }
                }
            }
        }
        return possibleMoves;       
    }

    //выбрать один из этих совпадений случайным образом
    GameObject PickOneRandomly()
    {
        List<GameObject> possibleMoves = new List<GameObject>();
        possibleMoves = FindAllMatches();

        if(possibleMoves.Count > 0)
        {
            int pieceToUse = Random.Range(0, possibleMoves.Count);
            return possibleMoves[pieceToUse];
        }
        return null;
    }

    //создать подсказку для этого совпадения
    private void MarkHint()
    {
        GameObject move = PickOneRandomly();

        if(move != null)
        {
            currentHint = Instantiate(hintParticle, move.transform.position, Quaternion.identity);

        }
    }

    //уничтожить подсказку
    public void DestroyHint()
    {
        if(currentHint != null)
        {
            Destroy(currentHint);
            currentHint = null;
            hintDelaySeconds = hintDelay;
        }
    }
}
