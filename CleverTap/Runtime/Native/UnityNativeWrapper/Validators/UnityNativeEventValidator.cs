#if !UNITY_IOS && !UNITY_ANDROID
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CleverTapSDK.Native {
    internal class UnityNativeEventValidator {

        private const int MAX_KEY_CHARS = 120;
        private const int MAX_VALUE_CHARS = 1024;
        private const int MAX_VALUE_PROPERTY_ARRAY_COUNT = 100;

        private readonly IReadOnlyList<string> _notAllowedKeyCharacters = new List<string>() { ".", ":", "$", "'", "\"", "\\" };
        private readonly IReadOnlyList<string> _notAllowedValueCharacters = new List<string>() { "'", "\"", "\\" };

        internal bool CleanEventName(string eventName, out string cleanEventName) {
            cleanEventName = eventName;
            if (string.IsNullOrWhiteSpace(cleanEventName)) {
                return false;
            }

            cleanEventName = ReplaceNotAllowedCharacters(cleanEventName, _notAllowedKeyCharacters);
            if (cleanEventName == string.Empty) {
                return false;
            }

            if (cleanEventName.Length > MAX_KEY_CHARS) {
                cleanEventName = cleanEventName.Substring(0, MAX_KEY_CHARS);
                //     NSString *errStr = [NSString stringWithFormat:@"%@%@", name, [NSString stringWithFormat:@"... exceeded the limit of %d characters. Trimmed", kMaxKeyChars]];
                //     [vr setErrorDesc:errStr];
                //     [vr setErrorCode:510];
                return false;
            }

            return true;
        }

        internal bool CleanObjectKey(string objectKey, out string cleanObjectKey) {
            cleanObjectKey = objectKey;
            if (string.IsNullOrWhiteSpace(cleanObjectKey)) {
                return false;
            }

            cleanObjectKey = ReplaceNotAllowedCharacters(cleanObjectKey, _notAllowedKeyCharacters);

            return true;
        }

        internal bool CleanMultiValuePropertyKey(string multiValuePropertyKey, out string cleanMultiValuePropertyKey) {
            if (!CleanObjectKey(multiValuePropertyKey, out cleanMultiValuePropertyKey)) {
                return true;
            }

            return UnityNativeConstants.Profile.IsKeyKnownProfileField(cleanMultiValuePropertyKey);
        }

        internal bool CleanMultiValuePropertyValue(string multiValuePropertyValue, out string cleanMultiValuePropertyValue) {
            cleanMultiValuePropertyValue = multiValuePropertyValue;
            return CleanProperyValue(multiValuePropertyValue, out cleanMultiValuePropertyValue);
        }

        internal bool CleanMultiValuePropertyArray(List<string> multiValuePropertyArray, out List<string> cleanMultiValuePropertyArray) {
            cleanMultiValuePropertyArray = multiValuePropertyArray;
            if (multiValuePropertyArray == null || multiValuePropertyArray.Count == 0) {
                return true;
            }

            if (cleanMultiValuePropertyArray.Count > MAX_VALUE_PROPERTY_ARRAY_COUNT) {
                cleanMultiValuePropertyArray = cleanMultiValuePropertyArray.Take(MAX_VALUE_PROPERTY_ARRAY_COUNT).ToList();
                //     NSString *errStr = [NSString stringWithFormat:@"Multi value user property for key %@ exceeds the limit of %d items. Trimmed", key, kMaxMultiValuePropertyArrayCount];
                //     [vr setErrorDesc:errStr];
                //     [vr setErrorCode:521];
                return false;
            }

            return true;
        }

        internal bool CleanObjectValue(object objectValue, out object cleanObjectValue, bool isForProfile = false) {
            cleanObjectValue = objectValue;
            if (cleanObjectValue == null) {
                return true;
            }

            if (IsAnyNumbericType(objectValue)) {
                return true;
            }

            if (cleanObjectValue is string) {
                var objectValueString = (string)cleanObjectValue;
                var cleanObjectValueString = objectValueString;
                var result = CleanProperyValue(cleanObjectValueString, out cleanObjectValueString);
                cleanObjectValue = cleanObjectValueString;
                return result;
            }

            if (IsAnyDateType(objectValue)) {
                var objectValueDate = (DateTimeOffset)cleanObjectValue;
                var cleanObjectValueUnixDate = objectValueDate.ToUnixTimeSeconds().ToString("$D_%d");
                cleanObjectValue = cleanObjectValueUnixDate;
                return true;
            }

            // Allow only for profile
            if (isForProfile && cleanObjectValue is IEnumerable<string>) {
                if ((cleanObjectValue as IEnumerable<string>).Count() > MAX_VALUE_PROPERTY_ARRAY_COUNT) {
                    //NSString* errStr = [NSString stringWithFormat:@"Invalid user profile property array count: %lu; max is: %d", (unsigned long)values.count, kMaxMultiValuePropertyArrayCount];
                    //             [vr setErrorDesc:errStr];
                    //             [vr setErrorCode:521];
                    return false;
                }
                return true;
            }

            return false;
        }

        internal bool IsRestrictedName(string eventName) {
            if (string.IsNullOrEmpty(eventName))
                return false;

            var restrictedNames = new List<string>() {
                "Notification Sent", "Notification Viewed", "Notification Clicked",
                "UTM Visited", "App Launched", "Stayed", "App Uninstalled",
                "wzrk_d", "wzrk_fetch", "SCCampaignOptOut", "Geocluster Entered", "Geocluster Exited"
            };

            if (restrictedNames.Select(rn => rn.ToLower()).Any(rn => rn == eventName.ToLower())) {
                // [error setErrorCode:513];
                // NSString *errStr = [NSString stringWithFormat:@"%@%@", name, @" is a restricted event name. Last event aborted."]
                return true;
            }

            return false;
        }

        internal bool IsDiscardedEvent(string eventName) {
            // for (NSString *x in discardedEvents)
            //     if ([name.lowercaseString isEqualToString:x.lowercaseString]) {
            //         // The event name is discarded
            //         CTValidationResult *error = [[CTValidationResult alloc] init];
            //         [error setErrorCode:513];
            //         NSString *errStr = [NSString stringWithFormat:@"%@%@%@", name, @" is a discarded event, dropping event: ", name];
            //         [error setErrorDesc:errStr];
            //         return true;
            //     }
            // return false;

            return false;
        }

        internal void SetDiscardedEvents(List<string> eventNames) {

        }

        // Check this when user enable and specifiy custom cleverTapId
        internal bool IsValidCleverTapId(string cleverTapId) {

            if (string.IsNullOrEmpty(cleverTapId))
                // Log ex. CleverTapUseCustomId has been specified to true in config but custom CleverTap ID is null or empty.
                return false;
            if (cleverTapId.Length > 64)
                // Log ex. Custom CleverTap ID is greater than 64 characters
                return false;

            // TODO : Check if this work properly
            var allowedCharactersRegex = @"[=|<>;+.A-Za-z0-9()!:$@_-]*";
            if (!Regex.IsMatch(cleverTapId, allowedCharactersRegex))
                // Log ex. Custom CleverTap ID cannot contain special characters apart from (, ), !, :, @, $, _, and -
                return false;

            return true;
        }

        private bool CleanProperyValue(string propertyValue, out string cleanProperyValue) {
            cleanProperyValue = propertyValue;
            if (string.IsNullOrWhiteSpace(cleanProperyValue)) {
                return true;
            }

            cleanProperyValue = ReplaceNotAllowedCharacters(cleanProperyValue, _notAllowedValueCharacters, toLower: true);
            if (cleanProperyValue.Length > MAX_VALUE_CHARS) {
                cleanProperyValue = cleanProperyValue.Substring(0, MAX_VALUE_CHARS);
                //     NSString *errStr = [NSString stringWithFormat:@"%@%@", value, [NSString stringWithFormat:@"... exceeds the limit of %d characters. Trimmed", kMaxValueChars]];
                //     [vr setErrorDesc:errStr];
                //     [vr setErrorCode:521];
                return false;
            }

            return true;
        }

        private string ReplaceNotAllowedCharacters(string str, IReadOnlyList<string> notAllowedCharacters, bool trim = true, bool toLower = false) {
            var newStr = str;
            if (string.IsNullOrWhiteSpace(str)) {
                return newStr;
            }

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

        private bool IsAnyNumbericType(object o) {
            return o is short || o is int || o is long ||
                   o is float || o is double || o is decimal;
        }
    }
}
#endif