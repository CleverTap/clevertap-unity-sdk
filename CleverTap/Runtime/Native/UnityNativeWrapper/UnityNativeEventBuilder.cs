#if !UNITY_IOS && !UNITY_ANDROID
namespace CleverTapSDK.Native {
    internal class UnityNativeEventBuilder {
        private readonly UnityNativeSessionManager _sessionManager;

        internal UnityNativeEventBuilder(UnityNativeSessionManager sessionManager) {
            sessionManager = _sessionManager;
        }

        internal UnityNativeEvent GenerateEvent(UnityNativeEventType eventType, object eventDetails = null) {
            // TODO : impelemnt logic to generate event json content
            var sessionId = _sessionManager.GetSessionId();
            var jsonContent = string.Empty;

            return new UnityNativeEvent(eventType, jsonContent);
        }
    }
}
#endif