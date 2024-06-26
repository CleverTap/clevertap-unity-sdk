#if !UNITY_IOS && !UNITY_ANDROID
using SQLite4Unity3d;
using System;
using UnityEngine;

namespace CleverTapSDK.Native
{
    [System.Serializable]
    internal class UnityNativeEventDBEntry
    {
        //[PrimaryKey, AutoIncrement]
        public int Id;
        public UnityNativeEventType EventType;  // Changed from property to public field
        public string JsonContent;  // Changed from property to public field
        public long Timestamp;  // Changed from property to public field

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