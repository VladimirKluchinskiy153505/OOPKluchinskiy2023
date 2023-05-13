using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovableMonster : Monster
{
    [SerializeField] private float speed = 2.0f;
    private Bullet bullet;
    private Vector3 direction;
    private SpriteRenderer sprite;
    protected override void Awake()
    {
        sprite =GetComponentInChildren<SpriteRenderer>();
        bullet = Resources.Load<Bullet>("Bullet");
        lives = 1;
    }
    protected override void Start()
    {
        direction =transform.right;
    }
    protected override void Update()
    {
        Move();
    }
    private void Move()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position+transform.up*0.5f+transform.right*direction.x*0.5f,0.02f);
        if (colliders.Length > 0&& colliders.All(x=>!x.GetComponent<Character>())) { direction.x *= -1.0f; }
        transform.position=Vector3.MoveTowards(transform.position,transform.position+ direction, speed*Time.deltaTime);
    }
}
