#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR
using System;
using SQLite4Unity3d;
using UnityEngine;
using UnityEngine.Scripting;

namespace CleverTapSDK.Native
{
    internal class UnityNativeEventDBEntry
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [Preserve] public int Id { get; set; }
#else
        [PrimaryKey, AutoIncrement]
        [Preserve] public int Id { get; set; }
#endif

        [Preserve] public UnityNativeEventType EventType { get; set; }
        [Preserve] public string JsonContent { get; set; }
        [Preserve] public long Timestamp { get; set; }

        public UnityNativeEventDBEntry()
        {
        }

        public UnityNativeEventDBEntry(UnityNativeEventType eventType, string jsonContent, long timestamp)
        {
            EventType = eventType;
            JsonContent = jsonContent;
            Timestamp = timestamp;
        }
    }
}
#endif
