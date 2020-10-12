using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    private Vector2 firstTouchPos;
    private Vector2 finalTouchPos;
    private Vector2 targetPosition;
    private GameObject otherElement;
    private FillBoard board;
    private float swipeSpeed = .4f;
    private FindMatches findMatches;

    [Header("Board Variables")]
    public float swipeAngle = 0;
    public int column;
    public int row;
    public int previousColumn;
    public int previousRow;
    public int targetX;
    public int targetY;
    public bool isMatched = false;
    public float swipeResist = 1f;



    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<FillBoard>();
        findMatches = FindObjectOfType<FindMatches>();
    }

    // Update is called once per frame
    void Update()
    {
        //FindMatches();

        //if (isMatched)
        //{

        //}

        targetX = column;
        targetY = row;
        if(Mathf.Abs(targetX - transform.position.x) > .1f)
        {
            //Move towards the target
            targetPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, targetPosition, swipeSpeed);
            if(board.board[column, row] != this.gameObject)
            {
                board.board[column, row] = this.gameObject;
            }

            findMatches.FindAllMatches();
        }
        else
        {
            //Directly set the position
            targetPosition = new Vector2(targetX, transform.position.y);
            transform.position = targetPosition;    
        }

        if (Mathf.Abs(targetY - transform.position.y) > .1f)
        {
            //Move towards the target
            targetPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, targetPosition, swipeSpeed);
            if (board.board[column, row] != this.gameObject)
            {
                board.board[column, row] = this.gameObject;
            }

            findMatches.FindAllMatches();
        }
        else
        {
            //Directly set the position
            targetPosition = new Vector2(transform.position.x, targetY);
            transform.position = targetPosition;
        }
    }

    public void DestroyElement()
    {
        Destroy(this.gameObject);
        board.board[column, row] = null;
        board.GetComponent<FillBoard>().StartDecreaseRow();
    }

    void OnMouseDown()
    {
        if(board.currentState == GameState.MOVE)
        {
            firstTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }      
    }

    void OnMouseUp()
    {
        if (board.currentState == GameState.MOVE)
        {
            finalTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
        }
    }

    void CalculateAngle()
    {
        if(Mathf.Abs(finalTouchPos.y - firstTouchPos.y) > swipeResist || Mathf.Abs(finalTouchPos.x - firstTouchPos.x) > swipeResist)
        {
            swipeAngle = Mathf.Atan2(
                finalTouchPos.y - firstTouchPos.y,
                finalTouchPos.x - firstTouchPos.x) * 180 / Mathf.PI;
            MoveElements();
            board.currentState = GameState.WAIT;
        }
        else
        {
            board.currentState = GameState.MOVE;
        }

    }

    void MoveElements()
    {
        if(swipeAngle > -45 && swipeAngle <= 45 && column < board.width - 1)
        {
            //Right swipe
            otherElement = board.board[column + 1, row];
            previousColumn = column;
            previousRow = row;
            otherElement.GetComponent<Element>().column -= 1;
            column += 1;
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1)
        {
            //Up swipe
            otherElement = board.board[column, row + 1];
            previousColumn = column;
            previousRow = row;
            otherElement.GetComponent<Element>().row -= 1;
            row += 1;
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0)
        {
            //Left swipe
            otherElement = board.board[column - 1, row];
            previousColumn = column;
            previousRow = row;
            otherElement.GetComponent<Element>().column += 1;
            column -= 1;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            //Down swipe
            otherElement = board.board[column, row - 1];
            previousColumn = column;
            previousRow = row;
            otherElement.GetComponent<Element>().row += 1;
            row -= 1;
        }

        StartCoroutine(CheckMove());
    }

    public IEnumerator CheckMove()
    {
        yield return new WaitForSeconds(.5f);
        if (otherElement != null)
        {
            if (!isMatched && !otherElement.GetComponent<Element>().isMatched)
            {
                otherElement.GetComponent<Element>().row = row;
                otherElement.GetComponent<Element>().column = column;
                row = previousRow;
                column = previousColumn;
                yield return new WaitForSeconds(.5f);
                board.currentState = GameState.MOVE;
            }
            else
            {
                board.DestroyMatches();
            }

            otherElement = null;
        }
    }

    //void FindMatches()
    //{
    //    if(column > 0 && column < board.width - 1)
    //    {
    //        GameObject leftElement1 = board.board[column - 1, row];
    //        GameObject rightElement1 = board.board[column + 1, row];

    //        if(leftElement1 != null && rightElement1 != null)
    //        {
    //            if (leftElement1.tag.Equals(this.gameObject.tag) && rightElement1.tag.Equals(this.gameObject.tag))
    //            {
    //                leftElement1.GetComponent<Element>().isMatched = true;
    //                rightElement1.GetComponent<Element>().isMatched = true;
    //                isMatched = true;
    //            }
    //        }
    //    }

    //    if (row > 0 && row < board.height - 1)
    //    {
    //        GameObject upElement1 = board.board[column, row + 1];
    //        GameObject downElement1 = board.board[column, row - 1];

    //        if(upElement1 != null && downElement1 != null)
    //        {
    //            if (upElement1.tag.Equals(this.gameObject.tag) && downElement1.tag.Equals(this.gameObject.tag))
    //            {
    //                upElement1.GetComponent<Element>().isMatched = true;
    //                downElement1.GetComponent<Element>().isMatched = true;
    //                isMatched = true;
    //            }
    //        }
    //    }
    //}
}
