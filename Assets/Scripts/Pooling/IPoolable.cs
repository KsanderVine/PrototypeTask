using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevTask
{
    public interface IPoolable
    {
        GameObject GameObject { get; }

        void OnDespawn();
        void SpawnWithPool(ObjectsPool pool);
    }
}