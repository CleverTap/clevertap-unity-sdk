using UnityEngine;

namespace CTIntegrationTests
{
    internal enum LoggerLevel
    {
        Off,
        Error,
        Debug,
    }

    internal static class Logger
    {
        private static LoggerLevel Level = LoggerLevel.Debug;

        internal static void SetLogLevel(LoggerLevel level)
        {
            Level = level;
        }

        internal static void Log(string message)
        {
            if (Level >= LoggerLevel.Debug)
            {
                Debug.Log($"[CTIntegrationTests]: {message}");
            }
        }

        internal static void LogWarning(string message)
        {
            if (Level >= LoggerLevel.Error)
            {
                Debug.LogWarning($"[CTIntegrationTests][Warning]: {message}");
            }
        }

        internal static void LogError(string message)
        {
            if (Level >= LoggerLevel.Error)
            {
                Debug.Log($"[CTIntegrationTests][Error]: {message}");
            }
        }
    }
}
