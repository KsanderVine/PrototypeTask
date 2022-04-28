using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

namespace DevTask
{
    public class DatedGameLog : IGameLog
    {
        public List<string> GameLogs { get; private set; }

        public DatedGameLog()
        {
            GameLogs = new List<string>();
        }

        public void Log(string content)
        {
            GameLogs.Add($"[{DateTime.Now.ToLongTimeString()}]:{content}");
        }

        public void SaveLog(ILogWriter logSaver)
        {
            string content = string.Join(Environment.NewLine, GameLogs);
            logSaver.SaveLog(content);
        }
    }
}