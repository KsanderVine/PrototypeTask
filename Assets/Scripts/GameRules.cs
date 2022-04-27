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
        public static GameRules Instance { get; private set; }
        public static bool IsRestarting { get; private set; }
        public static List<string> GameLogs { get; private set; } = new List<string>();

        public static int EnemiesCount { get; private set; }

        public UIResults VictoryResult;
        public UIResults DefeatResult;

        private void Awake()
        {
            EnemiesCount = 0;
            Instance = this;
        }

        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            IsRestarting = false;
            Log("Game started");
        }

        public static void ChangeEnemiesCount(int count)
        {
            EnemiesCount += count;
        }

        public static void GetLevelResultsAndReload(bool isVictory)
        {
            if (IsRestarting)
                return;

            IsRestarting = true;

            if (isVictory)
            {
                Log("Game result: Victory");
                Instance.VictoryResult.gameObject.SetActive(true);
                Instance.VictoryResult.Show();
            }
            else
            {
                Log("Game result: Defeat");
                Instance.DefeatResult.gameObject.SetActive(true);
                Instance.DefeatResult.Show();
            }

            Instance.StartCoroutine(Instance.ReloadLevel());
        }

        private IEnumerator ReloadLevel()
        {
            yield return new WaitForSecondsRealtime(2f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }

        public static void Log(string log)
        {
            GameLogs.Add($"[{DateTime.Now.ToLongTimeString()}]:{log}");
            Debug.Log(log);
        }

        private void SaveLog()
        {
            string path = Directory.GetParent(Application.dataPath) + "/GameLogs";
            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);

            int emptyIndex = 0;
            string date = DateTime.Now.ToShortDateString();
            string logName = $"{date}_{emptyIndex}.txt";

            while (File.Exists($"{path}/{logName}"))
            {
                emptyIndex++;
                logName = $"{date}_{emptyIndex}.txt";
            }

            string filePath = $"{path}/{logName}";
            string content = string.Join(Environment.NewLine, GameLogs);
            File.WriteAllText(filePath, content);
            Debug.Log($"Log save location: {filePath}");
        }

        private void OnApplicationQuit()
        {
            SaveLog();
        }
    }
}