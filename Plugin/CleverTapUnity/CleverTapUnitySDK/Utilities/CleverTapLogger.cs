using UnityEngine;

namespace CleverTap.Utilities {
    public static class CleverTapLogger {
        public enum LogLevel {
            None,
            Error,
            Debug,
        }

        private static LogLevel Level = LogLevel.Debug;

        public static void SetLogLevel(LogLevel level) {
            Level = level;
        }

        public static void Log(string message) {
            if (Level >= LogLevel.Debug) {
                Debug.Log(message);
            }
        }

        public static void LogError(string message) {
            if (Level >= LogLevel.Error) {
                Debug.LogError(message);
            }
        }
    }
}
