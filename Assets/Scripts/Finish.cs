using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevTask
{
    public class Finish : MonoBehaviour
    {
        private void OnTriggerStay(Collider collider)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                if (GameRules.EnemiesCount == 0)
                {
                    GameRules.GetLevelResultsAndReload(true);
                }
            }
        }
    }
}
