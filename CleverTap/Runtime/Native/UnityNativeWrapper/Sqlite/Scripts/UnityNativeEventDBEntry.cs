#if !UNITY_IOS && !UNITY_ANDROID
using SQLite4Unity3d;
using System;

namespace CleverTapSDK.Native
{
    internal class UnityNativeEventDBEntry
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public UnityNativeEventType EventType { get; set; }
        public string JsonContent { get; set; }
        public long Timestamp { get; set; }

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