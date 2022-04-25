using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    public float PushBackForce;

    protected override void TriggerCollision(Collider collision)
    {
        if (collision.tag.Equals("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.PushVelocityImpulse((Rigidbody.velocity.normalized) * PushBackForce);
        }
    }
}
