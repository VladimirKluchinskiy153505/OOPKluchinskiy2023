using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Unit
{

    protected virtual void Awake() { }
    protected virtual void Start() { }
    protected virtual void Update() { }
    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        Unit unit = collider.GetComponent<Unit>();
        if (unit!=null && unit is Character)
        {
            if (Mathf.Abs(unit.transform.position.x - transform.position.x) < 0.7f) { ReceiveDamage(); }
            else { unit.ReceiveDamage(); }
        }
    }
}
