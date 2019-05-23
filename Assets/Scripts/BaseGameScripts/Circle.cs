using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    [Header("Board Variables")] //переменные доски
    public int column; //колонка
    public int row; // строка
    public int previousColumns;
    public int previousRow;
    public int targetX;
    public int targetY;
    public bool isMatched = false;

    private Animator anim;
    private float shineDelay; // задержка анимации движухи персонажей, когда стоят просто так
    private float shineDelaySeconds;
    private EndGameManager endGameManager;
    private HintManager hintManager;
    private FindMatches findMatches;
    private Board board;
    public GameObject otherCircle;
    private Vector2 firstTouchPosition = Vector2.zero;
    private Vector2 finalTouchPosition = Vector2.zero;
    private Vector2 tempPosition;  

    [Header("Swipe Stuff")] //наклоны материала
    public float swipeAngle = 0;
    public float swipeResist = 1f; //сопротивление

    [Header("PowerUp Stuff")]
    public bool isColumnBomb;
    public bool isColorBomb;
    public bool isRowBomb;
    public bool isAdjacentBomb; // бомба для соседних кружков
    public GameObject adjacentMarker; //соседняя метка
    public GameObject rowArrow;
    public GameObject columnArrow;  
    public GameObject colorBomb;


   
    void Start ()
    {
        isColumnBomb = false;
        isRowBomb = false;
        isColorBomb = false;
        isAdjacentBomb = false;

        shineDelay = Random.Range(3f, 6f);
        shineDelaySeconds = shineDelay;

        anim = GetComponent<Animator>();
        endGameManager = FindObjectOfType<EndGameManager>();
        hintManager = FindObjectOfType<HintManager>();
        board = GameObject.FindWithTag("Board").GetComponent<Board>(); // так быстрее работать будет
       // board = FindObjectOfType<Board>();
        findMatches = FindObjectOfType<FindMatches>();
        //targetX = (int)transform.position.x;
       // targetY = (int)transform.position.y;
       // row = targetY;
        // = targetX;
       // previousRow = row;
       // previousColumns = column;
    }

    //Это для тестинга и отладки Bomb
    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(1))
        {
            isAdjacentBomb = true;
            GameObject marker = Instantiate(adjacentMarker, transform.position, Quaternion.identity);
            marker.transform.parent = this.transform;
        }
    }

    void Update ()
    {

        shineDelaySeconds -= Time.deltaTime;
        if(shineDelaySeconds <= 0)
        {
            shineDelaySeconds = shineDelay;
            StartCoroutine(StartShineCo());
        }

        /*
        if (isMatched == true)
        {
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            Color currentColor = mySprite.color;
            mySprite.color = new Color(currentColor.r, currentColor.g, currentColor.b, .5f);
        }
        */
        targetX = column;
        targetY = row;

        // перемещение с заменой места круга по X

        if(Mathf.Abs(targetX - transform.position.x) > .1)
        {
            //движение к цели
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);

            if(board.allCircle[column,row] != this.gameObject)
            {
                board.allCircle[column, row] = this.gameObject;
                findMatches.FindAllMatches();
            }           
        }
        else
        {
            //установить позицию напрямую
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
           // board.allCircle[column, row] = this.gameObject;
        }

        // перемещение с заменой места круга  по Y

        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            //движение к цели
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if (board.allCircle[column, row] != this.gameObject)
            {
                board.allCircle[column, row] = this.gameObject;
                findMatches.FindAllMatches();
            }           
        }
        else
        {
            //установить позицию напрямую
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;           
        }
    }

    IEnumerator StartShineCo()
    {
        anim.SetBool("Двигаются, когда стоят", true);
        yield return null;
        anim.SetBool("Двигаются, когда стоят", false);
    }

    public void PopAnimation()
    {
        anim.SetBool("Взрыв шариков", true);
    }

    public IEnumerator CheckMoveCo() //карантин,если не сходятся цвета, то возврат напредыдущую позицию
    {
        if (isColorBomb) //взрывы по цвету
        {
            //этот кусок - цветная бомба, а другой - цвет, который нужно уничтожить
            findMatches.MatchPiecesOfColor(otherCircle.tag);
            isMatched = true;
        }
        else if (otherCircle.GetComponent<Circle>().isColorBomb)
        {
            //другая часть - цветная бомба, и эта часть имеет цвет, чтобы разрушить
            findMatches.MatchPiecesOfColor(this.gameObject.tag);
            otherCircle.GetComponent<Circle>().isMatched = true;
        }

        yield return new WaitForSeconds(0.5f); //задержка на возвращение на предыдущее место, если не сошлись цвета

        if (otherCircle != null)
        {
            if (!isMatched && !otherCircle.GetComponent<Circle>().isMatched)
            {
                otherCircle.GetComponent<Circle>().row = row;
                otherCircle.GetComponent<Circle>().column = column;
                row = previousRow;
                column = previousColumns;
                yield return new WaitForSeconds(0.5f);//запрет на ход - задержка для возврата кругов на исходные места если не совпали
                board.currentCircle = null;
                board.currentState = GameState.move;
            }
            else
            {
                if(endGameManager != null)
                {
                    if(endGameManager.requirements.gameType == GameType.Moves)
                    {
                        endGameManager.DecreaseCounterValue();
                    }
                }
                board.DestroyMatches();              
            }
           // otherCircle = null;
        }        
    }

    private void OnMouseDown()
    {
        //if(anim != null)
        //{
        //    anim.SetBool("Touched", true);
        //}
        //Уничтожаем подсказку
        if (hintManager != null)
        {
            hintManager.DestroyHint();
        }

        if (board.currentState == GameState.move)
        {
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }            
    }

    private void OnMouseUp()
    {
        //anim.SetBool("Touched", false);
        if (board.currentState == GameState.move)
        {
            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
        }
    }

    void CalculateAngle()
    {
        if (Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > swipeResist ||
            Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeResist)
        {
            board.currentState = GameState.wait;
            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y,
            finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;          
            MovePieces();           
            board.currentCircle = this;
        }
        else
        {
            board.currentState = GameState.move;          
        }
    }

    void MovePiecesActual(Vector2 direction)
    {
        otherCircle = board.allCircle[column + (int)direction.x, row + (int)direction.y];
        previousRow = row;
        previousColumns = column;

        if (board.lockTiles[column, row] == null && board.lockTiles[column + (int)direction.x, row + (int)direction.y] == null)
        {
            if (otherCircle != null)
            {
                otherCircle.GetComponent<Circle>().column += -1 * (int)direction.x;
                otherCircle.GetComponent<Circle>().row += -1 * (int)direction.y;
                column += (int)direction.x;
                row += (int)direction.y;

                StartCoroutine(CheckMoveCo());
            }
            else
            {
                board.currentState = GameState.move;
            }
        }
        else
        {
            board.currentState = GameState.move;
        }

    }

    void MovePieces() //движение круглешков
    {
        if (swipeAngle > -45 && swipeAngle <= 45 && column < board.width - 1)
        {
            //Right swipe
            /*
            otherCircle = board.allCircle[column + 1, row];
            previousRow = row;
            previousColumns = column;
            otherCircle.GetComponent<Circle>().column -= 1;
            column += 1;
            StartCoroutine(CheckMoveCo());
            */
            MovePiecesActual(Vector2.right);
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1)
        {
            //Up swipe
            /*
            otherCircle = board.allCircle[column, row + 1];
            previousRow = row;
            previousColumns = column;
            otherCircle.GetComponent<Circle>().row -= 1;
            row += 1;
            StartCoroutine(CheckMoveCo());
            */
            MovePiecesActual(Vector2.up);
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0)
        {
            //Left swipe
            /*
            otherCircle = board.allCircle[column - 1, row];
            previousRow = row;
            previousColumns = column;
            otherCircle.GetComponent<Circle>().column += 1;
            column -= 1;
            StartCoroutine(CheckMoveCo());
            */
            MovePiecesActual(Vector2.left);
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            //Down swipe
            /*
            otherCircle = board.allCircle[column, row - 1];
            previousRow = row;
            previousColumns = column;
            otherCircle.GetComponent<Circle>().row += 1;
            row -= 1;
            StartCoroutine(CheckMoveCo());
            */
            MovePiecesActual(Vector2.down);
        }
        else
        {
            board.currentState = GameState.move;
        }
      
    }

    void FindMatches()
    {
        if (column > 0 && column < board.width - 1)
        {
            GameObject leftCircle1 = board.allCircle[column - 1, row];
            GameObject rightCircle1 = board.allCircle[column + 1, row];
            if (leftCircle1 != null && rightCircle1 != null)
            {
                if (leftCircle1.tag == this.gameObject.tag && rightCircle1.tag == this.gameObject.tag)
                {
                    leftCircle1.GetComponent<Circle>().isMatched = true;
                    rightCircle1.GetComponent<Circle>().isMatched = true;

                    isMatched = true;
                }
            }
        }

        if (row > 0 && row < board.height - 1)
        {
            GameObject upCircle1 = board.allCircle[column , row + 1];
            GameObject downtCircle1 = board.allCircle[column, row - 1];
            if (upCircle1 != null && downtCircle1 != null)
            {
                if (upCircle1.tag == this.gameObject.tag && downtCircle1.tag == this.gameObject.tag)
                {
                    upCircle1.GetComponent<Circle>().isMatched = true;
                    downtCircle1.GetComponent<Circle>().isMatched = true;

                    isMatched = true;
                }
            }
        }
        
    }

    public void MakeRowBomb() // сделать строчную бомбу
    {
        if (!isColumnBomb && !isColorBomb && !isAdjacentBomb)
        {
            isRowBomb = true;
            GameObject arrow = Instantiate(rowArrow, transform.position, Quaternion.identity);
            arrow.transform.parent = this.transform;
        }
    }

    public void MakeColumnBomb() //Сделать колонну бомбу
    {
        if (!isRowBomb && !isColorBomb && !isAdjacentBomb)
        {
            isColumnBomb = true;
            GameObject arrow = Instantiate(columnArrow, transform.position, Quaternion.identity);
            arrow.transform.parent = this.transform;
        }
    }

    public void MakeColorBomb() //Сделать цветовую бомбу
    {
        if (!isColumnBomb && !isRowBomb && !isAdjacentBomb)
        {
            isColorBomb = true;
            GameObject color = Instantiate(colorBomb, transform.position, Quaternion.identity);
            color.transform.parent = this.transform;
            this.gameObject.tag = "Color";
        }
    }

    public void MakeAdjacentBomb() //Сделать бомбу,кот уничтожает соседние кружки
    {
        if (!isColumnBomb && !isColorBomb && !isRowBomb)
        {
            isAdjacentBomb = true;
            GameObject marker = Instantiate(adjacentMarker, transform.position, Quaternion.identity);
            marker.transform.parent = this.transform;
        }
    }
 
}
