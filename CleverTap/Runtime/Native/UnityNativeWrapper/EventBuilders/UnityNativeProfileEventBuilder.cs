#if !UNITY_IOS && !UNITY_ANDROID
using System.Collections.Generic;

namespace CleverTapSDK.Native {
    internal class UnityNativeProfileEventBuilder : UnityNativeBaseEventBuilder {
        internal UnityNativeProfileEventBuilder(UnityNativeSessionManager sessionManager) : base(sessionManager) { }

        internal (Dictionary<string, object> systemFields, Dictionary<string, object> customFields) BuildPushEvent(Dictionary<string, object> properties) {
            if (properties == null) {
                // Log error?
                return (null, null);
            }

            var systemFields = new Dictionary<string, object>();
            var customFields = new Dictionary<string, object>();

            var allEventFields = CleanObjectDictonary(properties, isForProfile: true);
            foreach (var (key, value) in allEventFields) {
                if (UnityNativeConstants.Profile.IsKeyKnownProfileField(key)) {
                    systemFields.Add(key, value);
                    continue;
                }

                customFields.Add(key, value);
            }

            return (customFields, systemFields);
        }
    }
}
#endif