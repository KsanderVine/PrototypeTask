using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    private void OnTriggerStay(Collider collider)
    {
        if(collider.gameObject.tag.Equals("Player"))
        {
            if (GameRules.EnemiesCount == 0)
            {
                GameRules.RestartLevel(true);
            }
        }
    }
}
