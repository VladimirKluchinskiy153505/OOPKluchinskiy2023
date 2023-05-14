using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] protected int lives=4;
    public virtual void ReceiveDamage()
    {
        --lives;
        if (lives == 0)
        Die();
    }
    public virtual void Die()
    {
         Destroy(gameObject);
    }
}
