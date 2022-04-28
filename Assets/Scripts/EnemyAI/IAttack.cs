using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevTask.Enemies.EnemyAI
{
    public interface IAttack
    {
        bool IsTrackable(Transform target);
        void Attack(Transform target, float deltaTime);
        void OnDeath();
    }
}
