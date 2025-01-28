using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public Rigidbody2D rb;
    public int id;
    public float paddleMoveSpeed = 150f;
    public float maxY = 4.5f; // Paddle'ýn hareket edebileceði maksimum Y deðeri
    float movement;
    float direction = 0f;

    private float aiDeadZone = 1.2f;
    Vector2 paddleStartPosition;
    private void Start()
    {
        paddleStartPosition = transform.position;
        GameManager.instance.OnReset += ResetPaddlePosition;
        GameManager.instance.gameUI.OnStartGame += ResetPaddlePosition;
    }

    private void ResetPaddlePosition()
    {
        transform.position = paddleStartPosition;
    }
    private void FixedUpdate()
    {
        if(id == 1 && GameManager.instance.IsPlayerVsAi())
        {
            MoveAiByBallDirection();   
        }
        else
        {
            if (!GameManager.instance.gameUI.menuObject.activeSelf)
            {
                movement = ProcessInput();
                Move(movement);
            }
        } 
    }

    private void MoveAiByBallDirection()
    {
        Vector2 ballDirection = GameManager.instance.ball.rb.velocity.normalized;
        Vector2 ballStartPoint = GameManager.instance.ball.transform.position;

        Vector2 predictedPoint = ballStartPoint + ballDirection;

        if(Mathf.Abs(predictedPoint.y - transform.position.y) > aiDeadZone)
        {
            direction = predictedPoint.y > transform.position.y ? 1 : -1;
        }

        Move(direction);
    }
    private void MoveAiByBallPosition()
    {
        Vector2 ballPosition = GameManager.instance.ball.transform.position;
        
        if(Mathf.Abs(ballPosition.y - transform.position.y) > aiDeadZone)
        {
            direction = ballPosition.y > transform.position.y ? 1 : -1;
        }
        Move(direction);
    }
    private float ProcessInput()
    {
        float movement = 0f;
        switch (id)
        {
            case 1:
                movement = Input.GetAxis("MovePlayer2");
                break;
            case 2:
                movement = Input.GetAxis("MovePlayer1");
                break;
        }
        return movement;
    }
    private void Move(float movement)
    {
        // Hareket vektörü hesaplanýr
        rb.velocity = new Vector2(0, movement * paddleMoveSpeed);

        // Paddle'ýn ekran dýþýna çýkmasýný engellemek için pozisyon sýnýrlarý kontrol edilir
        Vector2 clampedPosition = rb.position;
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -maxY, maxY);
        rb.position = clampedPosition;
    }

    public bool IsLeftPaddle()
    {
        return id == 2;
    }
}
