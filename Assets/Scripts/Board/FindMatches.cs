using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMatches : MonoBehaviour
{
    private FillBoard board;
    public List<GameObject> currentMatches = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<FillBoard>();
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
                GameObject currentElement = board.board[i, j];
                if(currentElement != null)
                {
                    if(i > 0 && i < board.width - 1)
                    {
                        GameObject leftElement = board.board[i - 1, j];
                        GameObject rightElement = board.board[i + 1, j];
                        if(leftElement != null && rightElement != null)
                        {
                            if(leftElement.tag.Equals(currentElement.tag) && rightElement.tag.Equals(currentElement.tag))
                            {
                                if (!currentMatches.Contains(leftElement))
                                    currentMatches.Add(leftElement);
                                leftElement.GetComponent<Element>().isMatched = true;

                                if (!currentMatches.Contains(rightElement))
                                    currentMatches.Add(rightElement);
                                rightElement.GetComponent<Element>().isMatched = true;

                                if (!currentMatches.Contains(currentElement))
                                    currentMatches.Add(currentElement);
                                currentElement.GetComponent<Element>().isMatched = true;
                            }
                        }
                    }

                    if (j > 0 && j < board.height - 1)
                    {
                        GameObject downElement = board.board[i, j - 1];
                        GameObject upElement = board.board[i, j + 1];
                        if (downElement != null && upElement != null)
                        {
                            if (downElement.tag.Equals(currentElement.tag) && upElement.tag.Equals(currentElement.tag))
                            {
                                if (!currentMatches.Contains(downElement))
                                    currentMatches.Add(downElement);
                                downElement.GetComponent<Element>().isMatched = true;

                                if (!currentMatches.Contains(upElement))
                                    currentMatches.Add(upElement);
                                upElement.GetComponent<Element>().isMatched = true;

                                if (!currentMatches.Contains(currentElement))
                                    currentMatches.Add(currentElement);
                                currentElement.GetComponent<Element>().isMatched = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
