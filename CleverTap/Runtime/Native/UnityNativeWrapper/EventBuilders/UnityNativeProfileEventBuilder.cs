#if !UNITY_IOS && !UNITY_ANDROID
using System.Collections.Generic;
using System.Linq;

namespace CleverTapSDK.Native {
    internal class UnityNativeProfileEventBuilder {
        internal UnityNativeProfileEventBuilder() { }

        internal UnityNativeEventBuilderResult<UnityNativePushEventResult> BuildPushEvent(Dictionary<string, object> properties) {
            var eventValidator = new UnityNativeEventValidator();
            var eventValidationResultsWithErrors = new List<UnityNativeValidationResult>();
            if (properties == null || properties.Count == 0) {
                return new UnityNativeEventBuilderResult<UnityNativePushEventResult>(eventValidationResultsWithErrors, new UnityNativePushEventResult(null, null));
            }

            var systemFields = new Dictionary<string, object>();
            var customFields = new Dictionary<string, object>();

            var cleanObjectDictonaryValidationResult = CleanObjectDictonary(properties);
            var allEventFields = cleanObjectDictonaryValidationResult.EventResult;
            if (cleanObjectDictonaryValidationResult.ValidationResults.Any(vr => !vr.IsSuccess)) {
                eventValidationResultsWithErrors.AddRange(cleanObjectDictonaryValidationResult.ValidationResults.Where(vr => !vr.IsSuccess));
            }

            foreach (var (key, value) in allEventFields) {
                if (UnityNativeConstants.Profile.IsKeyKnownProfileField(key)) {
                    systemFields.Add(UnityNativeConstants.Profile.GetKnownProfileFieldForKey(key), value);
                    continue;
                }

                customFields.Add(key, value);
            }
            
            return new UnityNativeEventBuilderResult<UnityNativePushEventResult>(eventValidationResultsWithErrors, new UnityNativePushEventResult(systemFields, customFields));
        }

        private UnityNativeEventBuilderResult<Dictionary<string, object>> CleanObjectDictonary(Dictionary<string, object> objectDictonary) {
            var eventValidator = new UnityNativeEventValidator();
            var cleanObjectDictonary = new Dictionary<string, object>();
            var eventValidationResultsWithErrors = new List<UnityNativeValidationResult>();
            foreach (var (key, value) in objectDictonary) {
                var cleanObjectKeyValdaitonReuslt = eventValidator.CleanObjectKey(key, out var cleanKey);
                if (!cleanObjectKeyValdaitonReuslt.IsSuccess) {
                    eventValidationResultsWithErrors.Add(cleanObjectKeyValdaitonReuslt);
                    continue;
                }

                var cleanObjectValue = eventValidator.CleanObjectValue(value, out var cleanValue, true);
                if (!cleanObjectValue.IsSuccess) {
                    eventValidationResultsWithErrors.Add(cleanObjectValue);
                    continue;
                }

                cleanObjectDictonary.Add(cleanKey, cleanValue);
            }

            return new UnityNativeEventBuilderResult<Dictionary<string, object>>(eventValidationResultsWithErrors, cleanObjectDictonary);
        }
    }
}
#endif