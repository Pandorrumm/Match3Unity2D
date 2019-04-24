using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FindMatches : MonoBehaviour
{
    private Board board;
    public List<GameObject> currentMatches = new List<GameObject>();

    void Start()
    {
        board = FindObjectOfType<Board>();
    }

    public void FindAllMatches()
    {
        StartCoroutine(FindAllMatchesCo());
    }

    private List<GameObject> IsAdjacentBomb(Circle circle1, Circle circle2, Circle circle3)
    {
        List<GameObject> currentCircles = new List<GameObject>();

        if (circle1.isAdjacentBomb)
        {
            currentMatches.Union(GetAdjacentPieces(circle1.column, circle1.row));
        }
        if (circle2.isAdjacentBomb)
        {
            currentMatches.Union(GetAdjacentPieces(circle2.column, circle2.row));
        }
        if (circle3.isAdjacentBomb)
        {
            currentMatches.Union(GetAdjacentPieces(circle3.column, circle3.row));
        }
        return currentCircles;
    }

    private List<GameObject> IsRowBomb(Circle circle1, Circle circle2, Circle circle3)
    {
        List<GameObject> currentCircles = new List<GameObject>();

        if (circle1.isRowBomb)
        {
            currentMatches.Union(GetRowPieces(circle1.row));
        }
        if (circle2.isRowBomb)
        {
            currentMatches.Union(GetRowPieces(circle2.row));
        }
        if (circle3.isRowBomb)
        {
            currentMatches.Union(GetRowPieces(circle3.row));
        }
        return currentCircles;
    }

    private List<GameObject> IsColumnBomb(Circle circle1, Circle circle2, Circle circle3)
    {
        List<GameObject> currentCircles = new List<GameObject>();

        if (circle1.isColumnBomb)
        {
            currentMatches.Union(GetColumnPieces(circle1.column));
        }
        if (circle2.isColumnBomb)
        {
            currentMatches.Union(GetColumnPieces(circle2.column));
        }
        if (circle3.isColumnBomb)
        {
            currentMatches.Union(GetColumnPieces(circle3.column));
        }
        return currentCircles;
    }

    private void AddToListAndMatch(GameObject circle)
    {
        if (!currentMatches.Contains(circle))
        {
            currentMatches.Add(circle);
        }
        circle.GetComponent<Circle>().isMatched = true;
    }

    //получение кусков поблизости в List
    private void GetNearbyPieces(GameObject circle1, GameObject circle2, GameObject circle3)
    {
        AddToListAndMatch(circle1);
        AddToListAndMatch(circle2);
        AddToListAndMatch(circle3);

    }

    private IEnumerator FindAllMatchesCo()
    {
        yield return new WaitForSeconds(.2f);
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                GameObject currentCircl = board.allCircle[i, j];              

                if (currentCircl != null)
                {
                    Circle currentCirclCircl = currentCircl.GetComponent<Circle>();

                    if (i > 0 && i < board.width - 1)
                    {
                        GameObject leftCircle = board.allCircle[i - 1, j];                       
                        GameObject rightCircle = board.allCircle[i + 1, j];

                        if (leftCircle != null && rightCircle != null)
                        {
                            Circle rightCircleCircle = rightCircle.GetComponent<Circle>();
                            Circle leftCircleCircle = leftCircle.GetComponent<Circle>();

                            if (leftCircle != null && rightCircle != null)
                            {
                                if (leftCircle.tag == currentCircl.tag && rightCircle.tag == currentCircl.tag)
                                {
                                    // взрывы 
                                    currentMatches.Union(IsRowBomb(leftCircleCircle, currentCirclCircl, rightCircleCircle));

                                    //что бы не только солонка взорвалась, но и 2 остальных circla из трёх собранных
                                    currentMatches.Union(IsColumnBomb(leftCircleCircle, currentCirclCircl, rightCircleCircle));

                                    currentMatches.Union(IsAdjacentBomb(leftCircleCircle, currentCirclCircl, rightCircleCircle));

                                    GetNearbyPieces(leftCircle, currentCircl, rightCircle);
                                }
                            }
                        }
                    }

                    if (j > 0 && j < board.height - 1)
                    {
                        GameObject upCircle = board.allCircle[i, j + 1];                        
                        GameObject downCircle = board.allCircle[i, j - 1];

                        if (upCircle != null && downCircle != null)
                        {
                            Circle downCirclCircl = downCircle.GetComponent<Circle>();
                            Circle upCirclCircl = upCircle.GetComponent<Circle>();

                            if (upCircle != null && downCircle != null)
                            {
                                if (upCircle.tag == currentCircl.tag && downCircle.tag == currentCircl.tag)
                                {

                                    // взрывы 

                                    currentMatches.Union(IsColumnBomb(upCirclCircl, currentCirclCircl, downCirclCircl));

                                    currentMatches.Union(IsRowBomb(upCirclCircl, currentCirclCircl, downCirclCircl));

                                    currentMatches.Union(IsAdjacentBomb(upCirclCircl, currentCirclCircl, downCirclCircl));

                                    GetNearbyPieces(upCircle, currentCircl, downCircle);
                                }
                            }
                        }
                    }
                }

            }
        }
    }

    public void MatchPiecesOfColor(string color) //поиск совпадений цвета
    {
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                // существуют ли такие части ?
                if(board.allCircle[i,j] != null)
                {
                    //по Тэгу цвета смотрим
                    if(board.allCircle[i,j].tag == color)
                    {
                        // установить круг, который будет соответствовать
                        board.allCircle[i, j].GetComponent<Circle>().isMatched = true;
                    }
                }
            }
        }
            
    }

    List<GameObject> GetAdjacentPieces(int column, int row) //получаем в список соседние элементы
                                                            // для бомбы, кот уничтожит рядомстоящие круги  
    {
        List<GameObject> circle = new List<GameObject>();
        for (int i = column - 1; i <= column + 1; i++)
        {
            for (int j = row - 1; j <= row + 1; j++)
            {
                //проверяем, есть ли что то внутри доски, т.к. может быть с краю бомба
                if(i >= 0 && i < board.width && j >= 0 && j < board.height)
                {
                    if (board.allCircle[i, j] != null)
                    {
                        circle.Add(board.allCircle[i, j]);
                        board.allCircle[i, j].GetComponent<Circle>().isMatched = true;
                    }
                }

            }
        }
        return circle;
    }

    List<GameObject> GetColumnPieces(int column) // получить кусочки колонки для бомбы, 
                                                 // кот уничтожит столбец
    {
        List<GameObject> circle = new List<GameObject>();
        for (int i = 0; i < board.height; i++)
        {
            if (board.allCircle[column, i] != null)
            {
                //для цепной
                Circle circl = board.allCircle[column, i].GetComponent<Circle>();
                if(circl.isRowBomb)
                {
                    circle.Union(GetRowPieces(i).ToList());
                }

                circle.Add(board.allCircle[column, i]);
                // board.allCircle[column, i].GetComponent<Circle>().isMatched = true;
                circl.isMatched = true;
            }
        }
        return circle;
    }

    List<GameObject> GetRowPieces(int row) // получить кусочки колонки, для бомбы строки 
    {
        List<GameObject> circle = new List<GameObject>();
        for (int i = 0; i < board.width; i++)
        {
            if (board.allCircle[i, row] != null)
            {
                //для цепной
                Circle circl = board.allCircle[i, row].GetComponent<Circle>();
                if (circl.isColumnBomb)
                {
                    circle.Union(GetColumnPieces(i).ToList());
                }

                circle.Add(board.allCircle[i, row]);
                circl.isMatched = true;
            }
        }
        return circle;
    }

    public void CheckBombs() // бомбы row and column
    {
        //если игрок что то двигал
        if(board.currentCircle != null)
        {
            //если совпало
            if(board.currentCircle.isMatched)
            {
                // сделать его не похожим
                board.currentCircle.isMatched = false;

                //решить, какую бомбу сделать
                //первый способ
                /*
                int typeOfBomb = Random.Range(0, 100);
                if(typeOfBomb < 50)
                {
                    //row bomb
                    board.currentCircle.MakeRowBomb();
                }
                else if(typeOfBomb >= 50)
                {
                    //column bomb
                    board.currentCircle.MakeColumnBomb();
                }
                */
                //второй способ
                if ((board.currentCircle.swipeAngle > -45 && board.currentCircle.swipeAngle <= 45)
                    || (board.currentCircle.swipeAngle < -135 || board.currentCircle.swipeAngle >= 135))
                {
                    board.currentCircle.MakeRowBomb();
                }
                else
                {
                    board.currentCircle.MakeColumnBomb();
                }

            }
            //если двигаем не шар совпадающего цвета, а другой цвет
            else if(board.currentCircle.otherCircle != null)
            {
                Circle otherCircle = board.currentCircle.otherCircle.GetComponent<Circle>();
                //другой Круг соответствует?
                if (otherCircle.isMatched)
                {
                    otherCircle.isMatched = false;
                    /*
                    // решить, какую бомбу сделать
                    int typeOfBomb = Random.Range(0, 100);
                    if (typeOfBomb < 50)
                    {
                        //row bomb
                        otherCircle.MakeRowBomb();
                    }
                    else if (typeOfBomb >= 50)
                    {
                        //column bomb
                        otherCircle.MakeColumnBomb();
                    }
                    */
                    if ((board.currentCircle.swipeAngle > -45 && board.currentCircle.swipeAngle <= 45)
                    || (board.currentCircle.swipeAngle < -135 || board.currentCircle.swipeAngle >= 135))
                    {
                        otherCircle.MakeRowBomb();
                    }
                    else
                    {
                        otherCircle.MakeColumnBomb();
                    }
                }
            }
        }
    }

}
