using UnityEngine;

namespace DevTask
{
    public class PoolableEffect : MonoBehaviour, IPoolable
    {
        public GameObject GameObject => gameObject;
        private ObjectsPool _pool;

        public float TimeToDestroy;
        private float _delayToDestroy;

        public void Update()
        {
            if (_pool == null)
                return;

            _delayToDestroy -= Time.deltaTime;
            if(_delayToDestroy <= 0)
            {
                _pool.ReturnToPool(this);
            }
        }

        public void OnDespawn()
        {
            _pool = null;
        }

        public void SpawnWithPool(ObjectsPool pool)
        {
            _delayToDestroy = TimeToDestroy;
            _pool = pool;
        }
    }
}
