using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevTask
{
    public class Finish : MonoBehaviour
    {
        private GameRules _gameRules;

        public void Awake()
        {
            _gameRules = FindObjectOfType<GameRules>();
        }

        private void OnTriggerStay(Collider collider)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                if (_gameRules.EnemiesCount == 0)
                {
                    _gameRules.GetLevelResultsAndReload(true);
                }
            }
        }
    }
}
