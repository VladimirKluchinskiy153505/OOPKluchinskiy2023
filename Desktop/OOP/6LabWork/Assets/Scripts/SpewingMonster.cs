using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpewingMonster : Monster
{
    [SerializeField]
    private float rate = 3.0F;

    private Ball ball;

    protected override void Awake()
    {
        ball = Resources.Load<Ball>("Ball");
        lives = 5;
    }

    protected override void Start()
    {
        InvokeRepeating("Shoot", rate, rate);
    }

    private void Shoot()
    {
        Vector3 position = transform.position; position.y += 3.0F;
        Ball newBall = Instantiate(ball, position, ball.transform.rotation) as Ball;
    }
}
