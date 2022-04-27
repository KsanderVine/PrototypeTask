using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevTask
{
    public class ObjectsPoolsManager : MonoBehaviour
    {
        public ObjectsPool[] ObjectsPools;

        public ObjectsPool GetPoolWithName(string poolName)
        {
            if (ObjectsPools != null && ObjectsPools.Length > 0)
            {
                for (int i = 0; i < ObjectsPools.Length; i++)
                {
                    if(ObjectsPools[i].name.Equals(poolName))
                    {
                        return ObjectsPools[i];
                    }
                }
            }

            return null;
        }
    }
}
