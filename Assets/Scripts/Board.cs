using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState //состояние игры
{
    wait,
    move,
    win,
    lose,
    pause
}

public enum TileKind
{
    Breakable, //разбиваемые плитки
    Blank, //пробел
    Normal
}

[System.Serializable]
public class TileType //Тип плитки, для Blank Spaces
{
    public int x;
    public int y;
    public TileKind tileKind;
}


public class Board : MonoBehaviour
{
    [Header ("Scriptable Object Stuff")] //скриптовые объекты
    public World world;
    public int level;

    public GameState currentState = GameState.move; //текущее состояние

    [Header("Board Dimensions")] //размеры 
    public int width; //ширина
    public int height; // высота
    public int offSet; // что бы сверху вниз появлялось всё

    [Header ("Prefabs")]
    public GameObject tilePrefab;
    public GameObject breakableTilePrefab;
    public GameObject[] circle;
    public GameObject destroyEffect;

    [Header("Layout")] //расположение
    public GameObject[,] allCircle;
    private FindMatches findMatches;   
    public Circle currentCircle; //текущий
    public TileType[] boardLayout; //расположение, для Blank Spaces 
    private bool[,] blankSpaces;
    private BackgroundTile[,] breakableTiles;
   
    private ScoreManager scoreManager;
    public int basePieceValue = 20; // по 20 очков
    private int streakValue = 1; // Значение полосы
    public float refillDelay = 0.5f; //задержка пополнения доски
    public int[] scoreGoals; //goals - цель
    private SoundManager soundManager;
    private GoalManager goalManager;


    private void Awake()
    {
        if(world != null)
        {
            if(world.levels[level] != null)
            {
                width = world.levels[level].width;
                height = world.levels[level].height;
            }
        }
    }


    void Start()
    {
        
        goalManager = FindObjectOfType<GoalManager>();
        soundManager = FindObjectOfType<SoundManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        breakableTiles = new BackgroundTile[width, height];
        findMatches = FindObjectOfType<FindMatches>();
        blankSpaces = new bool[width, height];
        allCircle = new GameObject[width, height];
        SetUp();
        currentState = GameState.pause;
    }

    public void GenerateBlankSpaces()
    {
        for (int i = 0; i < boardLayout.Length; i++)
        {
            if (boardLayout[i].tileKind == TileKind.Blank)
            {
                blankSpaces[boardLayout[i].x, boardLayout[i].y] = true;
            }
        }
    }

    public void GenerateBreakableTiles() //бьющаяся плитка
    {
        //смотрим на все плитки 
        for (int i = 0; i < boardLayout.Length; i++)
        {
            //если плитка - "желе"
            if (boardLayout[i].tileKind == TileKind.Breakable)
            {
                //создать бьющуюся плитку на позиции
                Vector2 tempPosition = new Vector2(boardLayout[i].x, boardLayout[i].y);
                GameObject tile = Instantiate(breakableTilePrefab, tempPosition, Quaternion.identity);
                breakableTiles[boardLayout[i].x, boardLayout[i].y] = tile.GetComponent<BackgroundTile>();
            }
        }
    }

    private void SetUp()
    {
        GenerateBlankSpaces();
        GenerateBreakableTiles();

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                // квадраты под сетку
                if (!blankSpaces[i, j])
                {
                    Vector2 tempPosition = new Vector2(i, j + offSet);

                    Vector2 tilePosition = new Vector2(i, j);

                    GameObject tileBackground = Instantiate(tilePrefab, tilePosition, Quaternion.identity) as GameObject;
                    tileBackground.transform.parent = this.transform;
                    tileBackground.name = "(" + i + ", " + j + ")";

                    // круги в квадратах

                    int circlToUse = Random.Range(0, circle.Length);

                    //что бы в начале игры не было 3 в ряд
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
    }

    private bool MatchesAt(int column, int row, GameObject piece)
    {
        if (column > 1 && row > 1)
        {
            if (allCircle[column - 1, row] != null && allCircle[column - 2, row] != null)
            {
                if (allCircle[column - 1, row].tag == piece.tag && allCircle[column - 2, row].tag == piece.tag)
                {
                    return true;
                }
            }

            if (allCircle[column, row - 1] != null && allCircle[column, row - 2] != null)
            {
                if (allCircle[column, row - 1].tag == piece.tag && allCircle[column, row - 2].tag == piece.tag)
                {
                    return true;
                }
            }

        } else if (column <= 1 || row <= 1)
        {
            if (row > 1)
            {
                if (allCircle[column, row - 1] != null && allCircle[column, row - 2] != null)
                {
                    if (allCircle[column, row - 1].tag == piece.tag && allCircle[column, row - 2].tag == piece.tag)
                    {
                        return true;
                    }
                }
            }
            if (column > 1)
            {
                if (allCircle[column - 1, row] != null && allCircle[column - 2, row] != null)
                {
                    if (allCircle[column - 1, row].tag == piece.tag && allCircle[column - 2, row].tag == piece.tag)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private bool ColumnOrRow() // столбец или строка
    {
        int numberHorizontal = 0;
        int numberVertical = 0;
        Circle firstPiece = findMatches.currentMatches[0].GetComponent<Circle>();

        if (firstPiece != null)
        {
            foreach (GameObject currentPiece in findMatches.currentMatches)
            {
                Circle circle = currentPiece.GetComponent<Circle>();

                if (circle.row == firstPiece.row)
                {
                    numberHorizontal++;
                }

                if (circle.column == firstPiece.column)
                {
                    numberVertical++;
                }
            }
        }
        return (numberVertical == 5 || numberHorizontal == 5);
    }

    private void CheckToMakeBombs() // Делаем бомбы по ко-ву совпавших кружкоы
    {
        if (findMatches.currentMatches.Count == 4 || findMatches.currentMatches.Count == 7)
        {
            findMatches.CheckBombs();
        }

        if (findMatches.currentMatches.Count == 5 || findMatches.currentMatches.Count == 8)
        {
            if (ColumnOrRow())
            {
                //сделать цветную бомбу
                //текущая точка соответствует?
                if (currentCircle != null)
                {
                    if (currentCircle.isMatched)
                    {
                        if (!currentCircle.isColorBomb)
                        {
                            currentCircle.isMatched = false;
                            currentCircle.MakeColorBomb();
                        }
                    }
                    else
                    {
                        if (currentCircle.otherCircle != null)
                        {
                            Circle otherCircle = currentCircle.otherCircle.GetComponent<Circle>();
                            if (otherCircle.isMatched)
                            {
                                if (!otherCircle.isColorBomb)
                                {
                                    otherCircle.isMatched = false;
                                    otherCircle.MakeColorBomb();
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                //сделать для соседних кругов бомбу
                //текущая точка соответствует?
                if (currentCircle != null)
                {
                    if (currentCircle.isMatched)
                    {
                        if (!currentCircle.isAdjacentBomb)
                        {
                            currentCircle.isMatched = false;
                            currentCircle.MakeAdjacentBomb();
                        }
                    }
                    else
                    {
                        if (currentCircle.otherCircle != null)
                        {
                            Circle otherCircle = currentCircle.otherCircle.GetComponent<Circle>();
                            if (otherCircle.isMatched)
                            {
                                if (!otherCircle.isAdjacentBomb)
                                {
                                    otherCircle.isMatched = false;
                                    otherCircle.MakeAdjacentBomb();
                                }
                            }
                        }
                    }
                }
            }
        }

    }


    private void DestroyMatchesAt(int column, int row) // нанесение урона
    {
        if (allCircle[column, row].GetComponent<Circle>().isMatched)
        {
            //сколько элементов в списке совпадений findmatches
            if (findMatches.currentMatches.Count >= 4)
            {
                CheckToMakeBombs();
            }
            // нужно ли ломать плитку?

            if (breakableTiles[column, row] != null)
            {
                //если да, нанесите один урон

                breakableTiles[column, row].TakeDamage(1);
                if (breakableTiles[column, row].hitPoints <= 0)
                {
                    breakableTiles[column, row] = null;
                }
            }
            // GoalManager
            if(goalManager != null)
            {
                goalManager.CompareGoal(allCircle[column, row].tag.ToString());
                goalManager.UpdateGoals();
            }

            // Менеджер звука существует?
            if(soundManager != null)
            {
                soundManager.PlayRandomDestroyNoise();
            }

            GameObject particle = Instantiate(destroyEffect, allCircle[column, row].transform.position,
                                              Quaternion.identity);

            Destroy(particle, .5f);
            Destroy(allCircle[column, row]);
            scoreManager.IncreaseScore(basePieceValue * streakValue);
            allCircle[column, row] = null;

        }
    }

    public void DestroyMatches()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allCircle[i, j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }
        findMatches.currentMatches.Clear();
        StartCoroutine(DecreaseRowCo2());
    }

    private IEnumerator DecreaseRowCo2() //падение вниз circle Не в пустоты
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                //если текущее место не пустое ... и не пустота
                if (!blankSpaces[i, j] && allCircle[i, j] == null)
                {
                    //пщвторы из пустоты сверху до вершины столбца
                    for (int k = j + 1; k < height; k++)
                    {
                        //если круг найден..
                        if (allCircle[i, k] != null)
                        {
                            //сдвинуть круг в пустое место
                            allCircle[i, k].GetComponent<Circle>().row = j;

                            //установить это место равным нулю
                            allCircle[i, k] = null;
                            //выйти из петли
                            break;

                        }
                    }
                }
            }
        }
        yield return new WaitForSeconds(refillDelay * 0.5f);
        StartCoroutine(FillBoardCo());
    }
    private IEnumerator DecreaseRowCo() //падение вниз circle 
    {
        int nullCount = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allCircle[i, j] == null)
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
        yield return new WaitForSeconds(refillDelay * 0.5f);
        StartCoroutine(FillBoardCo());
    }

    private void RefillBoard() // пополнение доски
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allCircle[i, j] == null && !blankSpaces[i, j])
                {
                    Vector2 tempPosition = new Vector2(i, j + offSet);
                    int circlToUse = Random.Range(0, circle.Length);

                    int maxIterations = 0;
                    while(MatchesAt(i, j, circle[circlToUse]) && maxIterations < 100)
                    {
                        maxIterations++;
                        circlToUse = Random.Range(0, circle.Length);
                    }
                    maxIterations = 0;

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
                if (allCircle[i, j] != null)
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

    private IEnumerator FillBoardCo() // карантин заполнения доски
    {
        RefillBoard();
        yield return new WaitForSeconds(refillDelay);

        while (MatchesOnBoard() == true)
        {
            streakValue ++;
            DestroyMatches();
            yield return new WaitForSeconds(2 * refillDelay);            
        }

        findMatches.currentMatches.Clear();
        currentCircle = null;
       // yield return new WaitForSeconds(0.5f);

        if(IsDeadLocked()) //нет ходов
        {
            StartCoroutine(ShuffleBoard()); // перемешиваем круги на доске
            Debug.Log("НЕТ ХОДОВ!!");
        }

        yield return new WaitForSeconds(refillDelay);
        currentState = GameState.move;
        streakValue = 1;
    }

    private void SwitchPieces(int column, int row, Vector2 direction) //Переключить части
    {
        //Возьмите второй кусок и сохраните его в держателе - в holder
        GameObject holder = allCircle[column + (int)direction.x, row + (int)direction.y] as GameObject;

        //переключение первого круга на вторую позицию
        allCircle[column + (int)direction.x, row + (int)direction.y] = allCircle[column, row];

        //установить первый круг, чтобы быть второй круг
        allCircle[column, row] = holder;
    }

    private bool CheckForMatches() //Проверим на совпадения, вдруг ничего нету!
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allCircle[i, j] != null)
                {
                    //убедимся, что на доске один и два справа
                    if (i < width - 2)
                    {
                        //проверьте, существуют ли круг справа и две справа
                        if (allCircle[i + 1, j] != null && allCircle[i + 2, j] != null)
                        {
                            if (allCircle[i + 1, j].tag == allCircle[i, j].tag
                                && allCircle[i + 2, j].tag == allCircle[i, j].tag)
                            {
                                return true;
                            }
                        }
                    }
                    if (j < height - 2)
                    {
                        //проверка, существуют ли круги выше
                        if (allCircle[i, j + 1] != null && allCircle[i, j + 2] != null)
                        {
                            if (allCircle[i, j + 1].tag == allCircle[i, j].tag
                                && allCircle[i, j + 2].tag == allCircle[i, j].tag)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }

    public bool SwitchAndCheck(int column, int row, Vector2 direction)
    {
        SwitchPieces(column, row, direction);
        if (CheckForMatches())
        {
            SwitchPieces(column, row, direction);
            return true;
        }
        SwitchPieces(column, row, direction);
        return false;
    }    

    private bool IsDeadLocked() //нет ходов, тупик
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allCircle[i, j] != null)
                {
                    if (i < width - 1)
                    {
                        if (SwitchAndCheck(i, j, Vector2.right))
                        {
                            return false;
                        }
                    }

                    if(j < height - 1)
                    {
                        if(SwitchAndCheck(i, j, Vector2.up))
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }

    private IEnumerator ShuffleBoard() //перемешивание кругов на доске, когда нет ходов
    {
        yield return new WaitForSeconds(0.5f);
        List<GameObject> newBoard = new List<GameObject>();
        //добавляем все в список в этот
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allCircle[i, j] != null)
                {
                    newBoard.Add(allCircle[i, j]);
                }
            }
        }

        yield return new WaitForSeconds(0.5f);
        //за каждое место на доске
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                //если это место не пустое
                if(!blankSpaces[i,j])
                {
                    //выбрать случайные числа
                    int pieceToUse = Random.Range(0, newBoard.Count);                   

                    //что бы в начале игры не было 3 в ряд
                    int maxIterations = 0; // повторения
                    while (MatchesAt(i, j, newBoard[pieceToUse]) && maxIterations < 100)
                    {
                        pieceToUse = Random.Range(0, newBoard.Count);
                        maxIterations++;
                        Debug.Log(maxIterations);
                    }
                    // сделать контейнер для частей
                    Circle piece = newBoard[pieceToUse].GetComponent<Circle>();
                    maxIterations = 0;

                    //назначить столбец для частей
                    piece.column = i;
                    //назначить строку для частей
                    piece.row = j;

                    //заполнить массив кругов этими новыми частями
                    allCircle[i, j] = newBoard[pieceToUse];

                    //удалить из списка его
                    newBoard.Remove(newBoard[pieceToUse]);
                }
            }
        }
        //Проверить, вдруг после перемешивания опять тупик, то заново
        if(IsDeadLocked())
        {
            ShuffleBoard();
        }
    }
}
