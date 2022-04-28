using System;
using System.Collections.Generic;

namespace DevTask
{
    public class DateTimeGameLog : IGameLog
    {
        public List<string> GameLogs { get; private set; }

        public DateTimeGameLog()
        {
            GameLogs = new List<string>();
        }

        public void Log(string content)
        {
            GameLogs.Add($"[{DateTime.Now.ToShortDateString()} - {DateTime.Now.ToLongTimeString()}] ::: {content}");
        }

        public void SaveLog(ILogWriter logSaver)
        {
            string content = string.Join(Environment.NewLine, GameLogs);
            logSaver.SaveLog(content);
        }
    }
}