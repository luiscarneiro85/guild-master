using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMatrizGenerate : MonoBehaviour
{
    [SerializeField]
    public int[,] field = new int[6, 5];

    enum ELEMENTOS
    {
        FOGO = 0,
        AGUA = 1,
        AR = 2,
        TERRA = 3,
        LUZ = 4,
        ESCURIDAO = 5
    }

    // Start is called before the first frame update
    void Start()
    {
        //PreencheMatriz();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PreencheMatriz()
    {
        int randomElement;
        const int COLUMNS = 5;
        const int ROWS = 6;
        const int NUMBERELEMENTS = 5;

        for (int i = 0; i < ROWS; i++)
        {
            for (int j = 0; j < COLUMNS; j++)
            {
                field[i, j] = -1;
            }
        }

        for (int i = 0; i < ROWS; i++)
        {
            for (int j = 0; j < COLUMNS; j++)
            {
                if((i == 0 || i == 1) && (j == 0 || j ==1))
                    field[i,j] = randomElement = Random.Range(0, NUMBERELEMENTS);
                else if((i == 0 || i == 1) && j > 1)
                {
                    //TODO 
                    //sorteia o valor e compara com o elemento da esquerda
                    //Nao precisa comparar com o elemento de cima
                }
                else if((j == 0 || j ==1) && i > 1)
                {
                    //TODO
                    //Sorteia o valor e compara com o elemento de cima
                    //Nao precisa comparar com o elemento da esquerda
                }
                else
                {
                    //TODO
                    //Sorteia o Elemento e compara com o elemento de cima e o da esqueda
                }
            }
        }

        for (int i = 0; i < ROWS; i++)
        {
            for (int j = 0; j < COLUMNS; j++)
            {
                print(field[i, j] + ",");
            }
            print("\n");
        }
    }
}
