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
                GameObject element = Instantiate(ElementsPrefab[randomElement], pos, Quaternion.identity) as GameObject;
                element.transform.parent = this.transform;
                element.name = string.Format("({0},{1})", i, j);
                board[i, j] = element;
            }
        }
    }
}
