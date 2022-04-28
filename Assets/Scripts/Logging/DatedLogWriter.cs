using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

public class DatedLogWriter : ILogWriter
{
    public bool SaveLog(string logContent)
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

        try
        {
            File.WriteAllText(filePath, logContent);
        }
        catch
        {
            Debug.LogError($"Saving log error; Path: {filePath}");
            return false;
        }

        Debug.Log($"Log save location: {filePath}");
        return true;
    }
}