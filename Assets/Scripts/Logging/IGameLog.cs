using System.Collections.Generic;

namespace DevTask
{
    public interface IGameLog
    {
        List<string> GameLogs { get; }

        void Log(string content);
        void SaveLog(ILogWriter logSaver);
    }
}