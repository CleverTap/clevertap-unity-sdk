#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;

namespace CleverTapSDK.Native {
    internal class UnityNativeEventValidator {
        internal UnityNativeValidationResult CleanEventName(string eventName, out string cleanEventName) {
            cleanEventName = eventName;
            if (string.IsNullOrWhiteSpace(cleanEventName)) {
                return new UnityNativeValidationResult();
            }

            cleanEventName = ReplaceNotAllowedCharacters(cleanEventName, UnityNativeConstants.Validator.KEY_NOT_ALLOWED_CHARS);
            if (cleanEventName.Length > UnityNativeConstants.Validator.MAX_KEY_CHARS) {
                cleanEventName = cleanEventName.Substring(0, UnityNativeConstants.Validator.MAX_KEY_CHARS);
                return new UnityNativeValidationResult(510, $"{cleanEventName}... exceeded the limit of {UnityNativeConstants.Validator.MAX_KEY_CHARS} characters. Trimmed");
            }

            if (string.IsNullOrWhiteSpace(cleanEventName)) {
                return new UnityNativeValidationResult(512, "Event name is empty.");
            }

            return new UnityNativeValidationResult();
        }

        internal UnityNativeValidationResult CleanObjectKey(string objectKey, out string cleanObjectKey) {
            cleanObjectKey = objectKey;
            if (string.IsNullOrWhiteSpace(cleanObjectKey)) {
                return new UnityNativeValidationResult();
            }

            cleanObjectKey = ReplaceNotAllowedCharacters(cleanObjectKey, UnityNativeConstants.Validator.KEY_NOT_ALLOWED_CHARS);
            if (cleanObjectKey.Length > UnityNativeConstants.Validator.MAX_VALUE_CHARS) {
                cleanObjectKey = cleanObjectKey.Substring(0, UnityNativeConstants.Validator.MAX_VALUE_CHARS);
                return new UnityNativeValidationResult(520, $"{cleanObjectKey}... exceeds the limit of {UnityNativeConstants.Validator.MAX_KEY_CHARS} characters. Trimmed");
            }

            if (string.IsNullOrWhiteSpace(cleanObjectKey)) {
                return new UnityNativeValidationResult(512, "Object key name is empty.");
            }

            return new UnityNativeValidationResult();
        }

        internal UnityNativeValidationResult CleanMultiValuePropertyKey(string multiValuePropertyKey, out string cleanMultiValuePropertyKey) {
            var validationResult = CleanObjectKey(multiValuePropertyKey, out cleanMultiValuePropertyKey);
            if (!validationResult.IsSuccess) {
                return validationResult;
            }

            if (UnityNativeConstants.Profile.IsKeyKnownProfileField(cleanMultiValuePropertyKey)) {
                return new UnityNativeValidationResult(523, $"{cleanMultiValuePropertyKey} is a restricted key for multi-value properties. Operation aborted.");
            }

            return new UnityNativeValidationResult();
        }

        internal UnityNativeValidationResult CleanMultiValuePropertyValue(string multiValuePropertyValue, out string cleanMultiValuePropertyValue) {
            return CleanProperyValue(multiValuePropertyValue, out cleanMultiValuePropertyValue);
        }

        internal UnityNativeValidationResult CleanMultiValuePropertyArray(List<string> multiValuePropertyArray, out List<string> cleanMultiValuePropertyArray, string key) {
            cleanMultiValuePropertyArray = multiValuePropertyArray;
            if (multiValuePropertyArray == null || multiValuePropertyArray.Count == 0) {
                return new UnityNativeValidationResult();
            }

            if (cleanMultiValuePropertyArray.Count > UnityNativeConstants.Validator.MAX_VALUE_PROPERTY_ARRAY_COUNT) {
                cleanMultiValuePropertyArray = cleanMultiValuePropertyArray.Take(UnityNativeConstants.Validator.MAX_VALUE_PROPERTY_ARRAY_COUNT).ToList();
                return new UnityNativeValidationResult(521,
                    $"Multi value user property for key {key} exceeds the limit of {UnityNativeConstants.Validator.MAX_VALUE_PROPERTY_ARRAY_COUNT} items. Trimmed");
            }

            return new UnityNativeValidationResult();
        }

        internal UnityNativeValidationResult CleanObjectValue(object objectValue, out object cleanObjectValue, bool isForProfile = false) {
            cleanObjectValue = objectValue;
            if (cleanObjectValue == null || IsAnyNumericType(objectValue)) {
                return new UnityNativeValidationResult();
            }

            if (cleanObjectValue is string) {
                var validationResult = CleanProperyValue((string)cleanObjectValue, out var cleanObjectValueString);
                cleanObjectValue = cleanObjectValueString;
                return validationResult;
            }

            if (IsAnyDateType(objectValue))
            {
                DateTimeOffset dateTimeOffset;
                if (objectValue is DateTimeOffset)
                {
                    dateTimeOffset = (DateTimeOffset)objectValue;
                }
                else
                {
                    dateTimeOffset = new DateTimeOffset((DateTime)objectValue);
                }

                cleanObjectValue = "$D_" + dateTimeOffset.ToUnixTimeSeconds().ToString();
            }

            if (isForProfile && cleanObjectValue is IEnumerable<string>)
            {
                var cleanObjectValueArray = cleanObjectValue as IEnumerable<string>;
                if (cleanObjectValueArray.Count() > UnityNativeConstants.Validator.MAX_VALUE_PROPERTY_ARRAY_COUNT)
                {
                    return new UnityNativeValidationResult(521,
                        $"Invalid user profile property array count: {cleanObjectValueArray.Count()}; max is {UnityNativeConstants.Validator.MAX_VALUE_PROPERTY_ARRAY_COUNT}");
                }
            }

            return new UnityNativeValidationResult();
        }

        internal UnityNativeValidationResult IsRestrictedName(string eventName) {
            if (string.IsNullOrEmpty(eventName)) {
                return new UnityNativeValidationResult();
            }

            if (UnityNativeConstants.Validator.IsRestrictedName(eventName)) {
                return new UnityNativeValidationResult(513, $"{eventName} is restricted event name. Last event aborted.");
            }

            return new UnityNativeValidationResult();
        }

        private UnityNativeValidationResult CleanProperyValue(string propertyValue, out string cleanProperyValue) {
            cleanProperyValue = propertyValue;
            if (string.IsNullOrWhiteSpace(cleanProperyValue)) {
                return new UnityNativeValidationResult();
            }

            cleanProperyValue = ReplaceNotAllowedCharacters(cleanProperyValue, UnityNativeConstants.Validator.VALUE_NOT_ALLOWED_CHARS, toLower: true);
            if (cleanProperyValue.Length > UnityNativeConstants.Validator.MAX_VALUE_CHARS) {
                cleanProperyValue = cleanProperyValue.Substring(0, UnityNativeConstants.Validator.MAX_VALUE_CHARS);
                return new UnityNativeValidationResult(521, $"{cleanProperyValue}... exceeds the limit of {UnityNativeConstants.Validator.MAX_VALUE_CHARS} characters. Trimmed");
            }

            return new UnityNativeValidationResult();
        }

        private string ReplaceNotAllowedCharacters(string str, IReadOnlyList<string> notAllowedCharacters, bool trim = true, bool toLower = false) {
            if (string.IsNullOrWhiteSpace(str)) {
                return str;
            }

            var newStr = str;
            foreach (var notAllowedCharacter in notAllowedCharacters) {
                newStr = newStr.Replace(notAllowedCharacter, "");
            }

            newStr = trim ? newStr.Trim() : newStr;
            newStr = toLower ? newStr.ToLower() : newStr;

            return newStr;
        }

        private bool IsAnyDateType(object o) {
            return o is DateTime || o is DateTimeOffset;
        }

        private bool IsAnyNumericType(object o) {
            return o is short || o is int || o is long ||
                   o is float || o is double || o is decimal;
        }
    }
}
#endif