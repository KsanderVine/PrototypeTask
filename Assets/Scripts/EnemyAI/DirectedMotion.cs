using UnityEngine;
using System.Collections;

namespace DevTask.Enemies.EnemyAI
{
    public class DirectedMotion : MonoBehaviour, IMotion
    {
        public float MotionSpeed;
        public bool IsMovingRight { get; private set; }

        public virtual void Move(Rigidbody rigidbody, float deltaTime)
        {
            if (IsMovingRight)
            {
                rigidbody.AddForce(new Vector3(MotionSpeed, 0, 0) * deltaTime, ForceMode.VelocityChange);
            }
            else
            {
                rigidbody.AddForce(new Vector3(-MotionSpeed, 0, 0) * deltaTime, ForceMode.VelocityChange);
            }
        }

        public void OnCollisionEnter(Collision collision)
        {
            ContactPoint contact = collision.contacts[0];
            if (contact.otherCollider.transform.CompareTag("Obstacle"))
            {
                ChangeDirection();
            }
        }

        protected virtual void ChangeDirection()
        {
            IsMovingRight = !IsMovingRight;
        }

        public void OnDeath()
        {
        }
    }
}