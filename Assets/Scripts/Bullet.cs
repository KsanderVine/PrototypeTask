using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevTask.Bullets
{
    public abstract class Bullet : MonoBehaviour, IPoolable
    {
        public Rigidbody Rigidbody;
        public GameObject BreakingEffect;
        public float BulletSpeed;

        private ObjectsPool _pool;
        private float _lifeTime = 10f;

        private ObjectsPool _breakingEffectPool;
        public GameObject GameObject
        {
            get => gameObject;
        }

        public void Awake()
        {
            _breakingEffectPool = FindObjectOfType<ObjectsPoolsManager>().GetPoolWithName("@BulletsBreakingPool");
        }

        public void Fire(Vector3 direction)
        {
            Rigidbody.velocity = direction * BulletSpeed;
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (_pool == null)
                return;

            TriggerCollision(collision);
            CreateBreakingEffect();

            _pool.ReturnToPool(this);
        }

        protected void CreateBreakingEffect()
        {
            GameObject breakingEffect = _breakingEffectPool.Create<IPoolable>().GameObject;
            breakingEffect.transform.position = transform.position;
        }

        protected abstract void TriggerCollision(Collider collision);

        public void FixedUpdate()
        {
            _lifeTime -= Time.fixedDeltaTime;
            if (_lifeTime < 0 || transform.position.y < -50f)
            {
                CreateBreakingEffect();
                _pool.ReturnToPool(this);
            }
        }

        public void OnDespawn()
        {
            _pool = null;
        }

        public void SpawnWithPool(ObjectsPool pool)
        {
            _lifeTime = 10f;
            Rigidbody.velocity = Vector3.zero;
            _pool = pool;
        }
    }
}