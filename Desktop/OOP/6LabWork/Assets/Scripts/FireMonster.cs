using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMonster : Monster
{
    [SerializeField]
    private float rate = 0.1F;
    [SerializeField]
    private Color bulletColor = Color.white;

    private FireBullet bullet;

    protected override void Awake()
    {
        bullet = Resources.Load<FireBullet>("FireBullet");
        lives = 2;
    }

    protected override void Start()
    {
        InvokeRepeating("Shoot", rate, rate);
    }

    private void Shoot()
    {
        Vector3 position = transform.position; position.y += 0.5F;
        FireBullet newBullet = Instantiate(bullet, position, bullet.transform.rotation) as FireBullet;

        newBullet.Parent = gameObject;
        newBullet.Direction = -newBullet.transform.right;
        newBullet.Color = bulletColor;
    }

}
