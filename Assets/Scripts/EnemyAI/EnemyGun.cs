using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevTask.Bullets;

namespace DevTask.Enemies.EnemyAI
{
    public class EnemyGun : MonoBehaviour, IAttack
    {
        public GameObject Gun;
        public Transform FirePoint;

        public float AttackCooldown;
        private float _attackCooldown;

        private ObjectsPool _bulletsPool;
        private ObjectsPool _gunFirePool;

        public void Awake()
        {
            _bulletsPool = FindObjectOfType<ObjectsPoolsManager>().GetPoolWithName("@EnemiesBulletsPool");
            _gunFirePool = FindObjectOfType<ObjectsPoolsManager>().GetPoolWithName("@GunFirePool");
        }

        public bool IsTrackable(Transform target)
        {
            Vector3 targetDirection = (target.transform.position - FirePoint.position).normalized;

            if (Physics.Raycast(FirePoint.position, targetDirection, out RaycastHit hit))
            {
                if (hit.transform == target.transform)
                {
                    Vector3 targetPosition = target.position;
                    targetPosition.y = transform.position.y;

                    transform.LookAt(targetPosition);
                    return true;
                }
            }
            
            return false;
        }

        public void Attack(Transform target, float deltaTime)
        {
            _attackCooldown -= deltaTime;
            if (_attackCooldown <= 0)
            {
                _attackCooldown = AttackCooldown;
                FireGun();
            }
        }

        protected virtual void FireGun()
        {
            CreateShot(FirePoint.position, FirePoint.forward);
        }

        protected virtual void CreateShot (Vector3 position, Vector3 direction)
        {
            GameObject fireEffect = _gunFirePool.Create().GameObject;
            fireEffect.transform.position = position;
            fireEffect.transform.forward = direction;

            Bullet bullet = (Bullet)_bulletsPool.Create<Bullet>();
            bullet.transform.position = position;
            bullet.Fire(direction);
        }

        public void OnDeath()
        {
            Gun.gameObject.SetActive(false);
        }
    }
}
