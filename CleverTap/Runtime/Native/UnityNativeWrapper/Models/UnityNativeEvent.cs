#if !UNITY_IOS && !UNITY_ANDROID
using System;

namespace CleverTapSDK.Native {
    internal class UnityNativeEvent {
        private readonly long _timestamp;
        private readonly UnityNativeEventType _eventType;
        private readonly string _jsonContent;

        public UnityNativeEvent(UnityNativeEventType eventType, string jsonContent) {
            _timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            _eventType = eventType;
            _jsonContent = jsonContent;
        }

        internal long Timestamp => _timestamp;
        internal UnityNativeEventType EventType => _eventType;
        internal string JsonContent => _jsonContent;
    }
}
#endif