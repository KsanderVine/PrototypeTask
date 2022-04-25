using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.IO;
using System;

public class GameRules : MonoBehaviour
{
    public static GameRules Instance { get; private set; }
    public static bool IsRestarting { get; private set; }
    public static List<string> GameLogs { get; private set; }

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
        GameLogs = new List<string>();

        Log("Game started");
    }

    public static void ChangeEnemiesCount (int count)
    {
        EnemiesCount += count;
    }

    public static void RestartLevel (bool isVictory)
    {
        if (IsRestarting)
            return;

        IsRestarting = true;

        if(isVictory)
        {
            Log("Game result: Victory");
            Instance.VictoryResult.Show();
        }
        else
        {
            Log("Game result: Defeat");
            Instance.DefeatResult.Show();
        }

        Instance.Invoke(nameof(Instance.ReloadLevel), 2f);
    }

    public static void Log (string log)
    {
        GameLogs.Add($"[{DateTime.Now.ToLongTimeString()}]:{log}");
        Debug.Log(log);
    }

    private void SaveLog ()
    {
        string path = Directory.GetParent(Application.dataPath) + "/GameLogs";
        if (Directory.Exists(path) == false)
            Directory.CreateDirectory(path);

        int emptyIndex = 0;
        string logName = $"GameLog_{emptyIndex}.txt";

        while (File.Exists($"{path}/{logName}"))
        {
            emptyIndex++;
            logName = $"GameLog_{emptyIndex}.txt";
        }

        string filePath = $"{path}/{logName}";
        string content = string.Join(Environment.NewLine, GameLogs);
        File.WriteAllText(filePath, content);
        Debug.Log($"Log save location: {filePath}");
    }

    private void ReloadLevel ()
    {
        SaveLog();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
}
