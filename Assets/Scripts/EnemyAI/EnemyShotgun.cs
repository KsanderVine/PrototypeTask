using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevTask.Enemies.EnemyAI
{
    public class EnemyShotgun : EnemyGun
    {
        protected override void FireGun()
        {
            CreateShot(FirePoint.position, Quaternion.Euler(0, Random.Range(-5f, 5f), 0) * FirePoint.forward);
            CreateShot(FirePoint.position, Quaternion.Euler(0, Random.Range(-5f, 5f), 0) * FirePoint.forward);
            CreateShot(FirePoint.position, Quaternion.Euler(0, Random.Range(-5f, 5f), 0) * FirePoint.forward);
        }
    }
}
