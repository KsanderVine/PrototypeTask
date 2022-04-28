using UnityEngine;
using System.Collections;

public interface ILogWriter
{
    bool SaveLog(string logContent);
}