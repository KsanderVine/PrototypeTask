using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevTask.Enemies;

namespace DevTask.Bullets
{
    public class PlayerBullet : Bullet
    {
        protected override void TriggerCollision(Collider collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                Enemy enemy = collision.GetComponent<Enemy>();
                enemy.Die();
            }
        }
    }
}
