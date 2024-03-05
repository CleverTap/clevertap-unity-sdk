#if !UNITY_IOS && !UNITY_ANDROID
using System.Collections.Generic;
using System.Linq;

namespace CleverTapSDK.Native {
    internal class UnityNativeRecordEventBuilder {
        internal UnityNativeRecordEventBuilder() { }

        internal UnityNativeEventBuilderResult<Dictionary<string, object>> Build(string eventName, Dictionary<string, object> properties = null) {
            var eventValidator = new UnityNativeEventValidator();
            var eventValidationResultsWithErrors = new List<UnityNativeValidationResult>();
            if (string.IsNullOrWhiteSpace(eventName)) {
                return new UnityNativeEventBuilderResult<Dictionary<string, object>>(eventValidationResultsWithErrors, null);
            }

            var isRestrictedNameValidationResult = eventValidator.IsRestrictedName(eventName);
            if (!isRestrictedNameValidationResult.IsSuccess) {
                eventValidationResultsWithErrors.Add(isRestrictedNameValidationResult);
                return new UnityNativeEventBuilderResult<Dictionary<string, object>>(eventValidationResultsWithErrors, null);
            }

            var cleanEventNameValidationResult = eventValidator.CleanEventName(eventName, out var cleanEventName);
            if (!cleanEventNameValidationResult.IsSuccess) {
                eventValidationResultsWithErrors.Add(cleanEventNameValidationResult);
                return new UnityNativeEventBuilderResult<Dictionary<string, object>>(eventValidationResultsWithErrors, null);
            }

            var eventDetails = new Dictionary<string, object>();
            eventDetails.Add(UnityNativeConstants.Event.EVENT_NAME, cleanEventName);

            if (properties == null || properties.Count == 0) {
                return new UnityNativeEventBuilderResult<Dictionary<string, object>>(eventValidationResultsWithErrors, eventDetails);
            }

            var cleanObjectDictonaryValidationResult = CleanObjectDictonary(properties);
            if (cleanObjectDictonaryValidationResult.ValidationResults.Any(vr => !vr.IsSuccess)) {
                eventValidationResultsWithErrors.AddRange(cleanObjectDictonaryValidationResult.ValidationResults.Where(vr => !vr.IsSuccess));
            }

            eventDetails.Add(UnityNativeConstants.Event.EVENT_DATA, cleanObjectDictonaryValidationResult.EventResult);

            return new UnityNativeEventBuilderResult<Dictionary<string, object>>(eventValidationResultsWithErrors, eventDetails);
        }

        internal UnityNativeEventBuilderResult<Dictionary<string, object>> BuildChargedEvent(Dictionary<string, object> details, List<Dictionary<string, object>> items) {
            var eventValidator = new UnityNativeEventValidator();
            var eventValidationResultsWithErrors = new List<UnityNativeValidationResult>();
            if (details == null || items == null) {
                return new UnityNativeEventBuilderResult<Dictionary<string, object>>(eventValidationResultsWithErrors, null);
            }

            if (items.Count > 50) {
                // Log error?
            }

            var eventDetails = new Dictionary<string, object>();
            eventDetails.Add(UnityNativeConstants.Event.EVENT_NAME, UnityNativeConstants.Event.EVENT_CHARGED);

            var detailsCleanObjectDictonaryValidationResult = CleanObjectDictonary(details);
            var eventData = detailsCleanObjectDictonaryValidationResult.EventResult;
            if (detailsCleanObjectDictonaryValidationResult.ValidationResults.Any(vr => !vr.IsSuccess)) {
                eventValidationResultsWithErrors.AddRange(detailsCleanObjectDictonaryValidationResult.ValidationResults.Where(vr => !vr.IsSuccess));
            }

            eventDetails.Add(UnityNativeConstants.Event.EVENT_DATA, eventData);

            var itemsDetails = new List<Dictionary<string, object>>();
            foreach (var item in items) {
                var itemCleanObjectDictronaryValidationResult = CleanObjectDictonary(item);
                if (itemCleanObjectDictronaryValidationResult.ValidationResults.Any(vr => !vr.IsSuccess)) {
                    eventValidationResultsWithErrors.AddRange(itemCleanObjectDictronaryValidationResult.ValidationResults.Where(vr => !vr.IsSuccess));
                }

                itemsDetails.Add(itemCleanObjectDictronaryValidationResult.EventResult);
            }

            eventData.Add(UnityNativeConstants.Event.EVENT_CHARGED_ITEMS, itemsDetails);

            return new UnityNativeEventBuilderResult<Dictionary<string, object>>(eventValidationResultsWithErrors, eventDetails);
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

                var cleanObjectValue = eventValidator.CleanObjectValue(value, out var cleanValue, false);
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