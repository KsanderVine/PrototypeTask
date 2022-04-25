using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    protected override void TriggerCollision(Collider collision)
    {
        if (collision.tag.Equals("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.Die();
        }
    }
}
