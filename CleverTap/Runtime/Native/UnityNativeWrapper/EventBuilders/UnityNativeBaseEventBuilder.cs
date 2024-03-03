#if !UNITY_IOS && !UNITY_ANDROID
using System.Collections.Generic;

namespace CleverTapSDK.Native {
    internal class UnityNativeBaseEventBuilder {
        protected readonly UnityNativeSessionManager _sessionManager;
        protected readonly UnityNativeEventValidator _eventValidator;

        internal UnityNativeBaseEventBuilder(UnityNativeSessionManager sessionManager) {
            sessionManager = _sessionManager;
            _eventValidator = new UnityNativeEventValidator();
        }

        protected Dictionary<string, object> CleanObjectDictonary(Dictionary<string, object> objectDictonary, bool isForProfile = false /*, Action<string, string> onErrorHandler*/) {
            var cleanObjectDictonary = new Dictionary<string, object>();
            foreach (var (key, value) in objectDictonary) {
                if (string.IsNullOrWhiteSpace(key)) {
                    // Log error
                    continue;
                }

                if (!_eventValidator.CleanObjectKey(key, out var cleanKey)) {
                    // Log error
                    continue;
                }

                if (!_eventValidator.CleanObjectValue(value, out var cleanValue, isForProfile)) {
                    // Log erorr
                    continue;
                }

                cleanObjectDictonary.Add(cleanKey, cleanValue);
            }

            return cleanObjectDictonary;
        }
    }
}
#endif