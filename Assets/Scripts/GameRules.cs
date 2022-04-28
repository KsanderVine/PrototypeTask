using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.IO;
using System;
using DevTask.UI;

namespace DevTask
{
    public class GameRules : MonoBehaviour
    {
        public static IGameLog GameLog { get; private set; }
        public static bool IsRestarting { get; private set; }

        public int EnemiesCount { get; private set; }
        public UIResults VictoryResult;
        public UIResults DefeatResult;

        public void Awake()
        {
            if (GameLog == null)
                GameLog = new DateTimeGameLog();
            EnemiesCount = 0;
        }

        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            IsRestarting = false;
            GameLog.Log("Game started");
        }

        public void ChangeEnemiesCount(int count)
        {
            EnemiesCount += count;
        }

        public void GetLevelResultsAndReload(bool isVictory)
        {
            if (IsRestarting)
                return;

            IsRestarting = true;

            if (isVictory)
            {
                GameLog.Log("Game result: Victory");
                VictoryResult.gameObject.SetActive(true);
                VictoryResult.Show();
            }
            else
            {
                GameLog.Log("Game result: Defeat");
                DefeatResult.gameObject.SetActive(true);
                DefeatResult.Show();
            }

            StartCoroutine(ReloadLevel());
        }

        private IEnumerator ReloadLevel()
        {
            yield return new WaitForSecondsRealtime(2f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }

        private void OnApplicationQuit()
        {
            GameLog.SaveLog(new DatedLogWriter());
        }
    }
}