using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    WAIT,
    MOVE
}

public class FillBoard : MonoBehaviour
{
    public int width;
    public int height;
    public int offSet;
    public GameObject[] ElementsPrefab;
    public GameObject[,] board;
    public GameState currentState = GameState.MOVE;

    private FindMatches findMatches;

    // Start is called before the first frame update
    void Start()
    {
        findMatches = FindObjectOfType<FindMatches>();
        board = new GameObject[width, height];
        InitilizeBoard();
    }
    
    void InitilizeBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 pos = new Vector2(i, j + offSet);
                int randomElement = Random.Range(0, ElementsPrefab.Length);

                int maxIterations = 0;
                while(MatchesAt(i, j, ElementsPrefab[randomElement]) && maxIterations < 100)
                {
                    randomElement = Random.Range(0, ElementsPrefab.Length);
                    maxIterations++;
                }

                maxIterations = 0;
                GameObject element = Instantiate(ElementsPrefab[randomElement], pos, Quaternion.identity) as GameObject;
                element.GetComponent<Element>().row = j;
                element.GetComponent<Element>().column = i;

                element.transform.parent = this.transform;
                element.name = string.Format("({0},{1})", i, j);
                board[i, j] = element;
            }
        }
    }

    private bool MatchesAt(int column, int row, GameObject element)
    {
        if(column > 1 && row > 1)
        {
            if (board[column - 1, row].tag.Equals(element.tag) && board[column - 2, row].tag.Equals(element.tag))
                return true;

            if (board[column, row - 1].tag.Equals(element.tag) && board[column, row - 2].tag.Equals(element.tag))
                return true; 
        }
        else if(column <= 1 || row <= 1)
        {
            if(row > 1)
            {
                if (board[column, row - 1].tag.Equals(element.tag) && board[column, row - 1].tag.Equals(element.tag))
                    return true;
            }

            if (column > 1)
            {
                if (board[column - 1, row].tag.Equals(element.tag) && board[column - 2, row].tag.Equals(element.tag))
                    return true;
            }
        }
        return false;
    }

    private void DestroyMatchesAt(int column, int row)
    {
        if(board[column, row].GetComponent<Element>().isMatched)
        {
            findMatches.currentMatches.Remove(board[column, row]);
            board[column, row].GetComponent<Animator>().SetTrigger("Matched");
            //Destroy(board[column, row]);
            //board[column, row] = null;
        }
    }

    public void DestroyMatches()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if(board[i, j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }

        StartCoroutine(DecreaseRow());
    }

    private void RefillBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if(board[i, j] == null){
                    Vector2 tempPosition = new Vector2(i, j + offSet);
                    int randomElement = Random.Range(0, ElementsPrefab.Length);
                    GameObject element = Instantiate(ElementsPrefab[randomElement], tempPosition, Quaternion.identity) as GameObject;
                    board[i, j] = element;
                    element.GetComponent<Element>().row = j;
                    element.GetComponent<Element>().column = i;
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
                if(board[i, j] != null)
                {
                    if(board[i, j].GetComponent<Element>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void StartDecreaseRow()
    {
        StartCoroutine(DecreaseRow());
    }

    private IEnumerator DecreaseRow()
    {
        int nullCount = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if(board[i, j] == null)
                {
                    nullCount++;
                }
                else if(nullCount > 0)
                {
                    board[i, j].GetComponent<Element>().row -= nullCount;
                    board[i, j] = null;
                }
            }

            nullCount = 0;
        }
        yield return new WaitForSeconds(.4f);

        StartCoroutine(FillBoardCo());
    }

    private IEnumerator FillBoardCo()
    {
        RefillBoard();
        yield return new WaitForSeconds(.3f);

        while (MatchesOnBoard())
        {
            yield return new WaitForSeconds(.3f);
            DestroyMatches();
        }

        yield return new WaitForSeconds(.3f);
        currentState = GameState.MOVE;
    }
}
