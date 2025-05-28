#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using CleverTapSDK.Utilities;

namespace CleverTapSDK.Native {
    internal class UnityNativeNotificationViewedEventBuilder {
         private readonly UnityNativeEventValidator _eventValidator;

        internal UnityNativeNotificationViewedEventBuilder(UnityNativeEventValidator eventValidator) {
            _eventValidator = eventValidator;
        }

        internal UnityNativeEventBuilderResult<Dictionary<string, object>> Build(string eventName, Dictionary<string, object> properties = null) {
            var eventValidationResultsWithErrors = new List<UnityNativeValidationResult>();
            if (string.IsNullOrWhiteSpace(eventName)) {
                return new UnityNativeEventBuilderResult<Dictionary<string, object>>(eventValidationResultsWithErrors, null);
            }

            var isRestrictedNameValidationResult = _eventValidator.IsRestrictedName(eventName);
            if (!isRestrictedNameValidationResult.IsSuccess) {
                eventValidationResultsWithErrors.Add(isRestrictedNameValidationResult);
                return new UnityNativeEventBuilderResult<Dictionary<string, object>>(eventValidationResultsWithErrors, null);
            }
            
            var isDiscardedValidationResult = _eventValidator.IsEventDiscarded(eventName);
            if (!isDiscardedValidationResult.IsSuccess) {
                eventValidationResultsWithErrors.Add(isDiscardedValidationResult);
                return new UnityNativeEventBuilderResult<Dictionary<string, object>>(eventValidationResultsWithErrors, null);
            }

            var cleanEventNameValidationResult = _eventValidator.CleanEventName(eventName, out var cleanEventName);
            if (!cleanEventNameValidationResult.IsSuccess) {
                eventValidationResultsWithErrors.Add(cleanEventNameValidationResult);
                return new UnityNativeEventBuilderResult<Dictionary<string, object>>(eventValidationResultsWithErrors, null);
            }

            var eventDetails = new Dictionary<string, object>();
            eventDetails.Add(UnityNativeConstants.Event.EVENT_NAME, cleanEventName);

            if (properties == null || properties.Count == 0) {
                return new UnityNativeEventBuilderResult<Dictionary<string, object>>(eventValidationResultsWithErrors, eventDetails);
            }

            var cleanObjectDictionaryValidationResult = CleanObjectDictonary(properties);
            if (cleanObjectDictionaryValidationResult.ValidationResults.Any(vr => !vr.IsSuccess)) {
                eventValidationResultsWithErrors.AddRange(cleanObjectDictionaryValidationResult.ValidationResults.Where(vr => !vr.IsSuccess));
            }

            eventDetails.Add(UnityNativeConstants.Event.EVENT_DATA, cleanObjectDictionaryValidationResult.EventResult);

            return new UnityNativeEventBuilderResult<Dictionary<string, object>>(eventValidationResultsWithErrors, eventDetails);
        }

        internal UnityNativeEventBuilderResult<Dictionary<string, object>> BuildFetchEvent(int type) {
            // Validate that type is a supported fetch type
            // Currently only Variables fetch is supported
            if (type != UnityNativeConstants.Event.WZRK_FETCH_TYPE_VARIABLES) {
                var validationError = new UnityNativeValidationResult(512,
                $"Unsupported fetch type: {type}");
                return new UnityNativeEventBuilderResult<Dictionary<string, object>>(new List<UnityNativeValidationResult> { validationError }, null);
            }

            var eventDetails = new Dictionary<string, object> {
                { UnityNativeConstants.Event.EVENT_NAME, UnityNativeConstants.Event.EVENT_WZRK_FETCH }
            };

            var properties = new Dictionary<string, object> {
                { "t", type }
            };

            eventDetails.Add(UnityNativeConstants.Event.EVENT_DATA, properties);

            return new UnityNativeEventBuilderResult<Dictionary<string, object>>(new List<UnityNativeValidationResult>(), eventDetails);
        }

        private UnityNativeEventBuilderResult<Dictionary<string, object>> CleanObjectDictonary(Dictionary<string, object> objectDictonary) {
            var cleanObjectDictonary = new Dictionary<string, object>();
            var eventValidationResultsWithErrors = new List<UnityNativeValidationResult>();
            foreach (var (key, value) in objectDictonary) {
                var cleanObjectKeyValdaitonReuslt = _eventValidator.CleanObjectKey(key, out var cleanKey);
                if (!cleanObjectKeyValdaitonReuslt.IsSuccess) {
                    eventValidationResultsWithErrors.Add(cleanObjectKeyValdaitonReuslt);
                    continue;
                }

                var cleanObjectValue = _eventValidator.CleanObjectValue(value, out var cleanValue, false);
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