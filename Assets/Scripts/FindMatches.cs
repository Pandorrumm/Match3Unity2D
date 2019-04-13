using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMatches : MonoBehaviour
{
    private Board board;
    public List<GameObject> currentMatches = new List<GameObject>();
	
	void Start ()
    {
        board = FindObjectOfType<Board>();
	}

    public void FindAllMatches()
    {
        StartCoroutine(FindAllMatchesCo());
    }

    private IEnumerator FindAllMatchesCo()
    {
        yield return new WaitForSeconds(.2f);
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                GameObject currentCircl = board.allCircle[i, j];
                if(currentCircl != null)
                {
                    if (i > 0 && i < board.width - 1)
                    {
                        GameObject leftCircle = board.allCircle[i - 1, j];
                        GameObject rightCircle = board.allCircle[i + 1, j];
                        if(leftCircle != null && rightCircle != null)
                        {
                            if(leftCircle.tag == currentCircl.tag && rightCircle.tag == currentCircl.tag)
                            {
                                if(!currentMatches.Contains(leftCircle))
                                {
                                    currentMatches.Add(leftCircle);
                                }
                                leftCircle.GetComponent<Circle>().isMatched = true;

                                if (!currentMatches.Contains(rightCircle))
                                {
                                    currentMatches.Add(rightCircle);
                                }
                                rightCircle.GetComponent<Circle>().isMatched = true;

                                if (!currentMatches.Contains(currentCircl))
                                {
                                    currentMatches.Add(currentCircl);
                                }
                                currentCircl.GetComponent<Circle>().isMatched = true;
                            }
                        }
                    }

                    if (j > 0 && j < board.height - 1)
                    {
                        GameObject upCircle = board.allCircle[i, j + 1];
                        GameObject downCircle = board.allCircle[i, j - 1];
                        if (upCircle != null && downCircle != null)
                        {
                            if (upCircle.tag == currentCircl.tag && downCircle.tag == currentCircl.tag)
                            {
                                if (!currentMatches.Contains(upCircle))
                                {
                                    currentMatches.Add(upCircle);
                                }
                                upCircle.GetComponent<Circle>().isMatched = true;

                                if (!currentMatches.Contains(downCircle))
                                {
                                    currentMatches.Add(downCircle);
                                }
                                downCircle.GetComponent<Circle>().isMatched = true;

                                if (!currentMatches.Contains(currentCircl))
                                {
                                    currentMatches.Add(currentCircl);
                                }
                                currentCircl.GetComponent<Circle>().isMatched = true;
                            }
                        }
                    }
                }
                
            }
        }
    }
}
