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
    private float swiptSpeed = .4f;
    public float swipeAngle = 0;
    public int column;
    public int row;
    public int targetX;
    public int targetY;


    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<FillBoard>();
        targetX = (int)transform.position.x;
        targetY = (int)transform.position.y;
        row = targetY;
        column = targetX;
    }

    // Update is called once per frame
    void Update()
    {
        targetX = column;
        targetY = row;
        if(Mathf.Abs(targetX - transform.position.x) > .1f)
        {
            //Move towards the target
            targetPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, targetPosition, swiptSpeed);
        }
        else
        {
            //Directly set the position
            targetPosition = new Vector2(targetX, transform.position.y);
            transform.position = targetPosition;
            board.board[column, row] = this.gameObject;
        }

        if (Mathf.Abs(targetY - transform.position.y) > .1f)
        {
            targetPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, targetPosition, swiptSpeed);
        }
        else
        {
            targetPosition = new Vector2(transform.position.x, targetY);
            transform.position = targetPosition;
            board.board[column, row] = this.gameObject;
        }
    }

    void OnMouseDown()
    {
        firstTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(firstTouchPos);
    }

    void OnMouseUp()
    {
        finalTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
    }

    void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(
            finalTouchPos.y - firstTouchPos.y,
            finalTouchPos.x - firstTouchPos.x) * 180 / Mathf.PI;
        //Debug.Log(swipeAngle);
        MoveElements();
    }

    void MoveElements()
    {
        if(swipeAngle > -45 && swipeAngle <= 45 && column < board.width)
        {
            //Right swipe
            otherElement = board.board[column + 1, row];
            otherElement.GetComponent<Element>().column -= 1;
            column += 1;
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height)
        {
            //Up swipe
            otherElement = board.board[column, row + 1];
            otherElement.GetComponent<Element>().row -= 1;
            row += 1;
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0)
        {
            //Left swipe
            otherElement = board.board[column - 1, row];
            otherElement.GetComponent<Element>().column += 1;
            column -= 1;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            //Down swipe
            otherElement = board.board[column, row - 1];
            otherElement.GetComponent<Element>().row += 1;
            row -= 1;
        }
    }
}
