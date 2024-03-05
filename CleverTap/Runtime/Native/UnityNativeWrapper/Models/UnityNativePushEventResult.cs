#if !UNITY_IOS && !UNITY_ANDROID
using System.Collections.Generic;

namespace CleverTapSDK.Native {
    internal class UnityNativePushEventResult {
        private readonly IReadOnlyDictionary<string, object> _systemFields;
        private readonly IReadOnlyDictionary<string, object> _customFields;

        public UnityNativePushEventResult(Dictionary<string, object> systemFields, Dictionary<string, object> customFields) { 
            _systemFields = systemFields;
            _customFields = customFields;
        }

        public IReadOnlyDictionary<string, object> SystemFields => _systemFields;
        public IReadOnlyDictionary<string, object> CustomFields => _customFields;
    }
}
#endif