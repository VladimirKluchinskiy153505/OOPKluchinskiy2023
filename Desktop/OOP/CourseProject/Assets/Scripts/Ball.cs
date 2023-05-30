using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Vector3 direction;
    [SerializeField] private float speed = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        direction = -transform.right;
    }
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
        if (transform.position.x < -7.0f)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Unit unit = collision.gameObject.GetComponent<Unit>();
        if(unit is Character)
        {
            unit.ReceiveDamage();
        }
    }
}
