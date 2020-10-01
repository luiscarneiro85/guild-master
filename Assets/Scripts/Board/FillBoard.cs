using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillBoard : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject[] ElementsPrefab;
    public GameObject[,] board;

    // Start is called before the first frame update
    void Start()
    {
        board = new GameObject[width, height];
        InitilizeBoard();
    }
    
    void InitilizeBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 pos = new Vector2(i, j);
                int randomElement = Random.Range(0, ElementsPrefab.Length);

                int maxIterations = 0;
                while(MatchesAt(i, j, ElementsPrefab[randomElement]) && maxIterations < 100)
                {
                    randomElement = Random.Range(0, ElementsPrefab.Length);
                    maxIterations++;
                }

                maxIterations = 0;
                GameObject element = Instantiate(ElementsPrefab[randomElement], pos, Quaternion.identity) as GameObject;
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
}
