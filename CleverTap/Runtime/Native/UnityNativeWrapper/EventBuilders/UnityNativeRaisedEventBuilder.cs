#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CleverTapSDK.Utilities;

namespace CleverTapSDK.Native
{
    internal class UnityNativeRaisedEventBuilder
    {
        private readonly UnityNativeEventValidator _eventValidator;

        internal UnityNativeRaisedEventBuilder(UnityNativeEventValidator eventValidator)
        {
            _eventValidator = eventValidator;
        }

        internal UnityNativeEventBuilderResult<Dictionary<string, object>> Build(string eventName, Dictionary<string, object> properties = null)
        {
            var eventValidationResultsWithErrors = new List<UnityNativeValidationResult>();
            if (string.IsNullOrWhiteSpace(eventName))
            {
                return new UnityNativeEventBuilderResult<Dictionary<string, object>>(eventValidationResultsWithErrors, null);
            }

            var isRestrictedNameValidationResult = _eventValidator.IsRestrictedName(eventName);
            if (!isRestrictedNameValidationResult.IsSuccess)
            {
                eventValidationResultsWithErrors.Add(isRestrictedNameValidationResult);
                return new UnityNativeEventBuilderResult<Dictionary<string, object>>(eventValidationResultsWithErrors, null);
            }

            var isDiscardedValidationResult = _eventValidator.IsEventDiscarded(eventName);
            if (!isDiscardedValidationResult.IsSuccess)
            {
                eventValidationResultsWithErrors.Add(isDiscardedValidationResult);
                return new UnityNativeEventBuilderResult<Dictionary<string, object>>(eventValidationResultsWithErrors, null);
            }

            var cleanEventNameValidationResult = _eventValidator.CleanEventName(eventName, out var cleanEventName);
            if (!cleanEventNameValidationResult.IsSuccess)
            {
                eventValidationResultsWithErrors.Add(cleanEventNameValidationResult);
                return new UnityNativeEventBuilderResult<Dictionary<string, object>>(eventValidationResultsWithErrors, null);
            }

            var eventDetails = new Dictionary<string, object>();
            eventDetails.Add(UnityNativeConstants.Event.EVENT_NAME, cleanEventName);

            if (properties == null || properties.Count == 0)
            {
                return new UnityNativeEventBuilderResult<Dictionary<string, object>>(eventValidationResultsWithErrors, eventDetails);
            }

            var cleanObjectDictionaryValidationResult = CleanObjectDictonary(properties);
            if (cleanObjectDictionaryValidationResult.ValidationResults.Any(vr => !vr.IsSuccess))
            {
                eventValidationResultsWithErrors.AddRange(cleanObjectDictionaryValidationResult.ValidationResults.Where(vr => !vr.IsSuccess));
            }

            eventDetails.Add(UnityNativeConstants.Event.EVENT_DATA, cleanObjectDictionaryValidationResult.EventResult);

            return new UnityNativeEventBuilderResult<Dictionary<string, object>>(eventValidationResultsWithErrors, eventDetails);
        }

        internal UnityNativeEventBuilderResult<Dictionary<string, object>> BuildFetchEvent(int type)
        {
            // Validate that type is a supported fetch type
            // Currently only Variables fetch is supported
            if (type != UnityNativeConstants.Event.WZRK_FETCH_TYPE_VARIABLES)
            {
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

        internal UnityNativeEventBuilderResult<Dictionary<string, object>> BuildChargedEvent(Dictionary<string, object> details, List<Dictionary<string, object>> items)
        {
            var eventValidationResultsWithErrors = new List<UnityNativeValidationResult>();
            if (details == null || items == null)
            {
                return new UnityNativeEventBuilderResult<Dictionary<string, object>>(eventValidationResultsWithErrors, null);
            }

            if (items.Count > 50)
            {
                CleverTapLogger.Log("Charged event contained more than 50 items.");
                eventValidationResultsWithErrors.Add(new UnityNativeValidationResult(522, "Charged event contained more than 50 items."));
                return new UnityNativeEventBuilderResult<Dictionary<string, object>>(eventValidationResultsWithErrors, null);
            }

            var eventDetails = new Dictionary<string, object>();
            eventDetails.Add(UnityNativeConstants.Event.EVENT_NAME, UnityNativeConstants.Event.EVENT_CHARGED);

            var detailsCleanObjectDictonaryValidationResult = CleanObjectDictonary(details);
            var eventData = detailsCleanObjectDictonaryValidationResult.EventResult;
            if (detailsCleanObjectDictonaryValidationResult.ValidationResults.Any(vr => !vr.IsSuccess))
            {
                eventValidationResultsWithErrors.AddRange(detailsCleanObjectDictonaryValidationResult.ValidationResults.Where(vr => !vr.IsSuccess));
            }

            eventDetails.Add(UnityNativeConstants.Event.EVENT_DATA, eventData);

            var itemsDetails = new List<Dictionary<string, object>>();
            foreach (var item in items)
            {
                var itemCleanObjectDictronaryValidationResult = CleanObjectDictonary(item);
                if (itemCleanObjectDictronaryValidationResult.ValidationResults.Any(vr => !vr.IsSuccess))
                {
                    eventValidationResultsWithErrors.AddRange(itemCleanObjectDictronaryValidationResult.ValidationResults.Where(vr => !vr.IsSuccess));
                }

                itemsDetails.Add(itemCleanObjectDictronaryValidationResult.EventResult);
            }

            eventData.Add(UnityNativeConstants.Event.EVENT_CHARGED_ITEMS, itemsDetails);

            return new UnityNativeEventBuilderResult<Dictionary<string, object>>(eventValidationResultsWithErrors, eventDetails);
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