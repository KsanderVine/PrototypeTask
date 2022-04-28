using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevTask.Enemies.EnemyAI
{
    public interface IMotion
    {
        bool IsMovingRight { get; }
        void Move(Rigidbody rigidbody, float deltaTime);
        void OnDeath();
    }
}