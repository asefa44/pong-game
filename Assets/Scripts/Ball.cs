using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody2D rb;
    public BallAudio ballAudio;
    public ParticleSystem collisionParticle;
    public TrailRenderer trailRenderer;
    public GameObject collisionMarkerPrefab;
    private float ballSpeed = 7f;
    
    private float startX = 0f;
    private float maxStartY = 4f;
    private float speedMultiplier = 1.1f;
    private float maxBallSpeed = 18f;
    private float maxCollisionAngle = 45f;

    private void Start()
    {
        GameManager.instance.OnReset += ResetBall;
        GameManager.instance.OnReset += InitialPush;
        GameManager.instance.gameUI.OnStartGame += InitialPush;
        GameManager.instance.gameUI.OnStartGame += ResetBall;

        Transform ballTrailTransform = transform.Find("Trail");
        trailRenderer = ballTrailTransform.GetComponent<TrailRenderer>();
    }
    private void InitialPush()
    {
        float moveDirX = Random.value < 0.5 ? 1f : -1f;
        float moveDirY = Random.Range(-1f, 1f);

        Vector2 moveDir = new Vector2(moveDirX, moveDirY).normalized;
        rb.velocity = moveDir * ballSpeed;

        EmitParticle(32);
        trailRenderer.Clear();
    }
    private void ResetBall()
    {
        float positionY = Random.Range(-maxStartY, maxStartY);
        Vector2 position = new Vector2(startX, positionY).normalized;

        transform.position = position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ScoreZone scoreZone = collision.GetComponent<ScoreZone>(); // carptigin obje score zone mi?

        if (scoreZone)
        {
            GameManager.instance.screenShake.StartShake(0.33f, 0.1f);
            GameManager.instance.OnScoreZoneReached(scoreZone.id);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Paddle paddle = collision.collider.GetComponent<Paddle>();
        Wall wall = collision.collider.GetComponent<Wall>();
       
        // Eðer paddle'a çarptýysa
        if (paddle)
        {
            Debug.Log(rb.velocity.magnitude);
            GameManager.instance.screenShake.StartShake(0.01f, 0.5f);
            ballAudio.PlayBallSound();

            AdjustAngle(paddle, collision);

            if (rb.velocity.magnitude < maxBallSpeed)
            {
                rb.velocity *= speedMultiplier;
            }
            else
            {
                rb.velocity = rb.velocity.normalized * maxBallSpeed;
            }

            EmitParticle(16);
        }

        // Eðer duvara çarptýysa
        if (wall)
        {
            GameManager.instance.screenShake.StartShake(0.033f, 0.033f);
            ballAudio.PlayWallSound();
            EmitParticle(8);

        }
    }

    private void AdjustAngle(Paddle paddle, Collision2D collision)
    {
        Vector2 median = Vector2.zero;  

        foreach(ContactPoint2D contactPoint in collision.contacts)
        {
            median += contactPoint.point;
        }
        median /= collision.contactCount;

        // calculate the relative distance from center (between -1 and 1)
        float absoluteDistanceFromCenter = median.y - paddle.transform.position.y;
        float relativeDistanceFromCenter = absoluteDistanceFromCenter * 2 / paddle.transform.localScale.y;

        int angleSign = paddle.IsLeftPaddle() ? 1 : -1;
        Quaternion rotation = Quaternion.AngleAxis(relativeDistanceFromCenter * maxCollisionAngle * angleSign, Vector3.forward);

        Vector2 dir = paddle.IsLeftPaddle() ? Vector2.right : Vector2.left;
        Vector2 velocity = rotation * dir * rb.velocity.magnitude;
        rb.velocity = velocity;
    }
    private void EmitParticle(int amount)
    {
        collisionParticle.Emit(amount);
    }
}