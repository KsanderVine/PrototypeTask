using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevTask
{
    public class ObjectsPool : MonoBehaviour
    {
        public GameObject PoolableInstance;
        public int PoolSize = 20;

        private List<IPoolable> _pool;

        public void Start()
        {
            Initialize();   
        }

        private void Initialize ()
        {
            _pool = new List<IPoolable>();
            for (int i = 0; i < PoolSize; i++)
            {
                AddObjectToPool();
            }
        }

        private void AddObjectToPool ()
        {
            GameObject newObject = Instantiate(PoolableInstance.gameObject);
            newObject.name = PoolableInstance.name;

            newObject.transform.SetParent(transform);
            _pool.Add(newObject.GetComponent<IPoolable>());

            newObject.SetActive(false);
        }

        public IPoolable Create()
        {
            for (int i = 0; i < _pool.Count; i++)
            {
                if (_pool[i].GameObject.activeInHierarchy == false)
                {
                    return Spawn(_pool[i]);
                }
            }

            AddObjectToPool();
            var lastObject = _pool[_pool.Count - 1];
            return Spawn(lastObject);
        }

        public IPoolable Create<T>() where T : IPoolable
        {
            for (int i = 0; i < _pool.Count; i++)
            {
                if (_pool[i].GameObject.activeInHierarchy == false)
                {
                    return Spawn(_pool[i]);
                }
            }

            AddObjectToPool();
            var lastObject = _pool[_pool.Count - 1];
            return Spawn(lastObject);
        }

        private IPoolable Spawn (IPoolable poolable)
        {
            poolable.GameObject.SetActive(true);
            poolable.SpawnWithPool(this);
            return poolable;
        }

        public void ReturnToPool (IPoolable poolable)
        {
            poolable.GameObject.SetActive(false);
            poolable.OnDespawn();
        }
    }
}