using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState //состояние игры
{
    wait,
    move
}

public class Board : MonoBehaviour
{
   
    public GameState currentState = GameState.move; //текущее состояние
    public int width; //ширина
    public int height; // высота
    public int offSet; // что бы сверху вниз появлялось всё
    public GameObject tilePrefab;
    private TileBackground[,] allTiles; //вся плитка
    public GameObject[] circle;    
    public GameObject[,] allCircle;
    private FindMatches findMatches;

    public GameObject destroyEffect;

    public Circle currentCircle; //текущий

    void Start ()
    {
        findMatches = FindObjectOfType<FindMatches>();
        allTiles = new TileBackground[width, height];
        allCircle = new GameObject[width, height];
        SetUp();
	}
	
	private void SetUp()
    {
        for(int i=0; i<width; i++)
        {
            for(int j=0; j<height; j++)
            {
                // квадраты под сетку

                Vector2 tempPosition = new Vector2(i, j + offSet);               
                GameObject tileBackground = Instantiate(tilePrefab, tempPosition, Quaternion.identity) as GameObject;
                tileBackground.transform.parent = this.transform;
                tileBackground.name = "(" + i + ", " + j + ")";

                // круги в квадратах

                int circlToUse = Random.Range(0, circle.Length);
                int maxIterations = 0; // повторения
                while (MatchesAt(i, j, circle[circlToUse]) && maxIterations < 100)
                {
                    circlToUse = Random.Range(0, circle.Length);
                    maxIterations++;
                    Debug.Log(maxIterations);
                }
                maxIterations = 0;
               
                GameObject circl = Instantiate(circle[circlToUse], tempPosition, Quaternion.identity);
                circl.GetComponent<Circle>().row = j;
                circl.GetComponent<Circle>().column = i;

                circl.transform.parent = this.transform;
                circl.name = "(" + i + ", " + j + ")";

                allCircle[i, j] = circl;
            }
        }
    }

    private bool MatchesAt(int column, int row, GameObject piece) 
    {
        if (column > 1 && row > 1)
        { 
            if (allCircle[column - 1, row].tag == piece.tag && allCircle[column - 2, row].tag == piece.tag)
            {
                return true;
            }
            if (allCircle[column , row-1].tag == piece.tag && allCircle[column , row -2].tag == piece.tag)
            {
                return true;
            }
        } else if(column <=1 || row <=1)
        {
            if (row > 1)
            {
                if( allCircle[column, row - 1].tag == piece.tag && allCircle[column, row - 2].tag == piece.tag)
                {
                    return true;
                }
            }
            if (column > 1)
            {
                if (allCircle[column-1, row].tag == piece.tag && allCircle[column-2, row].tag == piece.tag)
                {
                    return true;
                }
            }
        }
        
        return false;
    }

    private void DestroyMatchesAt(int column, int row)
    {
        if (allCircle[column, row].GetComponent<Circle>().isMatched)
        {
            //сколько элементов в списке совпадений findmatches
            if (findMatches.currentMatches.Count == 4 || findMatches.currentMatches.Count == 7)
            {
                findMatches.CheckBombs();
            }
               
            GameObject particle = Instantiate(destroyEffect, allCircle[column, row].transform.position, Quaternion.identity);
            Destroy(particle, .5f);

            Destroy(allCircle[column, row]);
            allCircle[column, row] = null;

        }
    }

    public void DestroyMatches()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j=0; j < height; j++)
            {
                if(allCircle[i,j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }
        findMatches.currentMatches.Clear();
        StartCoroutine(DecreaseRowCo());
    }

    private IEnumerator DecreaseRowCo() //падение вниз circle 
    {
        int nullCount = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if( allCircle[i,j] == null)
                {
                    nullCount++;
                }
                else if (nullCount > 0)
                {
                    allCircle[i, j].GetComponent<Circle>().row -= nullCount;
                    allCircle[i, j] = null;
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(.4f);
        StartCoroutine(FillBoardCo());
    }

    private void RefillBoard() // пополнение доски
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allCircle[i, j] == null)
                {
                    Vector2 tempPosition = new Vector2(i, j + offSet);
                    int circlToUse = Random.Range(0, circle.Length);
                    //piece - вставка
                    GameObject piece = Instantiate(circle[circlToUse], tempPosition, Quaternion.identity);
                    allCircle[i, j] = piece;
                    piece.GetComponent<Circle>().row = j;
                    piece.GetComponent<Circle>().column = i;
                }
            }
        }
    }

    private bool MatchesOnBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
               if(allCircle[i,j] != null)
                {
                    if (allCircle[i, j].GetComponent<Circle>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private IEnumerator FillBoardCo()
    {
        RefillBoard();
        yield return new WaitForSeconds(.5f);

        while(MatchesOnBoard() == true)
        {
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }
        findMatches.currentMatches.Clear();
        currentCircle = null;
        yield return new WaitForSeconds(.5f);
        currentState = GameState.move;
    }
}
