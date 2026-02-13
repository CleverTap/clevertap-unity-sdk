#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CleverTapSDK.Native
{
    internal class UnityNativeProfileEventBuilder
    {
        private readonly UnityNativeEventValidator _eventValidator;

        internal UnityNativeProfileEventBuilder(UnityNativeEventValidator eventValidator)
        {
            _eventValidator = eventValidator;
        }

        internal UnityNativeEventBuilderResult<UnityNativePushEventResult> BuildPushEvent(Dictionary<string, object> properties)
        {
            var eventValidationResultsWithErrors = new List<UnityNativeValidationResult>();
            if (properties == null || properties.Count == 0)
            {
                return new UnityNativeEventBuilderResult<UnityNativePushEventResult>(eventValidationResultsWithErrors, new UnityNativePushEventResult(null, null));
            }

            var systemFields = new Dictionary<string, object>();
            var customFields = new Dictionary<string, object>();

            var cleanObjectDictonaryValidationResult = CleanObjectDictonary(properties);
            var allEventFields = cleanObjectDictonaryValidationResult.EventResult;
            if (cleanObjectDictonaryValidationResult.ValidationResults.Any(vr => !vr.IsSuccess))
            {
                eventValidationResultsWithErrors.AddRange(cleanObjectDictonaryValidationResult.ValidationResults.Where(vr => !vr.IsSuccess));
            }


            var profile = new Dictionary<string, object>();
            foreach (var (key, value) in allEventFields)
            {
                if (UnityNativeConstants.Profile.IsKeyKnownProfileField(key))
                {
                    profile.Add(UnityNativeConstants.Profile.GetKnownProfileFieldForKey(key), value);
                    continue;
                }
            }

            customFields.Add("profile", profile);
            return new UnityNativeEventBuilderResult<UnityNativePushEventResult>(eventValidationResultsWithErrors, new UnityNativePushEventResult(systemFields, customFields));
        }


        private UnityNativeEventBuilderResult<Dictionary<string, object>> CleanObjectDictonary(Dictionary<string, object> objectDictionary)
        {
            var (cleanedRoot, errors) = RecursivelyCleanObject(objectDictionary);

            // The root must be a dictionary; if recursion returned something else, fallback to empty
            var cleanedDict = cleanedRoot as Dictionary<string, object> ?? new Dictionary<string, object>();

            return new UnityNativeEventBuilderResult<Dictionary<string, object>>(errors, cleanedDict);
        }

        // Recursively clean any object, returning the cleaned value and a list of all validation errors
        private (object CleanedValue, List<UnityNativeValidationResult> Errors) RecursivelyCleanObject(object value)
        {
            // 1. Dictionary – clean each key and recursively clean each value
            if (value is Dictionary<string, object> dict)
            {
                var cleanedDict = new Dictionary<string, object>();
                var dictErrors = new List<UnityNativeValidationResult>();

                foreach (var (key, val) in dict)
                {
                    // Validate and clean the key
                    var keyResult = _eventValidator.CleanObjectKey(key, out var cleanKey);
                    if (!keyResult.IsSuccess)
                    {
                        dictErrors.Add(keyResult);
                        continue; // Skip this entry if the key is invalid
                    }

                    // Recursively clean the value
                    var (cleanValue, valueErrors) = RecursivelyCleanObject(val);
                    dictErrors.AddRange(valueErrors);

                    cleanedDict[cleanKey] = cleanValue;
                }

                return (cleanedDict, dictErrors);
            }

            // 2. List – recursively clean each element
            if (value is IList list)
            {
                var cleanedList = new List<object>();
                var listErrors = new List<UnityNativeValidationResult>();

                foreach (var item in list)
                {
                    var (cleanItem, itemErrors) = RecursivelyCleanObject(item);
                    listErrors.AddRange(itemErrors);
                    cleanedList.Add(cleanItem);
                }

                return (cleanedList, listErrors);
            }

            // 3. Leaf value – delegate to CleanObjectValue (non‑recursive)
            var valueResult = _eventValidator.CleanObjectValue(value, out var cleanedValue, false);
            var errors = new List<UnityNativeValidationResult>();
            if (!valueResult.IsSuccess)
                errors.Add(valueResult);

            // If CleanObjectValue returns null on failure, fallback to the original value
            return (cleanedValue ?? value, errors);
        }
    }
}
#endif