using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : Bullet
{
    protected override void Start()
    {
        parentTag = parent.tag;
        Destroy(gameObject, 0.4F);
    }
 
}
