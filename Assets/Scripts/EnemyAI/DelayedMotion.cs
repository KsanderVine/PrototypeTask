using UnityEngine;
using System.Collections;

namespace DevTask.Enemies.EnemyAI
{
    public class DelayedMotion : DirectedMotion
    {
        public float ChangeDirectionCooldown = 2f;
        private float _changeDirectionDelay;

        public override void Move(Rigidbody rigidbody, float deltaTime)
        {
            _changeDirectionDelay -= deltaTime;

            if(_changeDirectionDelay < 0)
            {
                ChangeDirection();
            }
            
            base.Move(rigidbody, deltaTime);
        }

        protected override void ChangeDirection()
        {
            _changeDirectionDelay = ChangeDirectionCooldown;
            base.ChangeDirection();
        }
    }
}
