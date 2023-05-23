using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected GameObject parent;
    public GameObject Parent { set { parent = value; } get { return parent; } }
    protected float speed=10.0f;
    protected Vector3 direction;
    public Vector3 Direction { set { direction = value; } }
    protected SpriteRenderer sprite;
    protected string parentTag;
    public Color Color
    {
        set { sprite.color = value; }
    }
    protected void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }
    protected virtual void Start()
    { 
            parentTag = parent.tag;
            Destroy(gameObject, 2.0f);
    }
    protected void Update()
    {
       transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
    }
    protected void OnTriggerEnter2D(Collider2D collider)
    {
         Unit unit = collider.GetComponent<Unit>();
            if (unit && unit.gameObject.tag != parentTag)
            {
                //Debug.Log(parentTag);
                if (!(unit is MovableMonster))
                { unit.ReceiveDamage(); }
                Destroy(gameObject);
            }

    }
}
