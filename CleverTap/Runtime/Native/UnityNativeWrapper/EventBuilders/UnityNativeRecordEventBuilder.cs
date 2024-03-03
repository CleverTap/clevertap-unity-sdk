#if !UNITY_IOS && !UNITY_ANDROID
using System.Collections.Generic;

namespace CleverTapSDK.Native {
    internal class UnityNativeRecordEventBuilder : UnityNativeBaseEventBuilder {
        internal UnityNativeRecordEventBuilder(UnityNativeSessionManager sessionManager) : base(sessionManager) { }

        internal Dictionary<string, object> BuildEvent(string eventName, Dictionary<string, object> properties = null) {
            if (string.IsNullOrWhiteSpace(eventName)) {
                // Log error
                return null;
            }
            
            if (_eventValidator.IsRestrictedName(eventName)) {
                // Log error
                return null;
            }

            if (_eventValidator.IsDiscardedEvent(eventName)) {
                // Log error
                return null;
            }

            if (!_eventValidator.CleanEventName(eventName, out var cleanEventName)) {
                return null;
            }

            var eventDetails = new Dictionary<string, object>();
            eventDetails.Add(UnityNativeConstants.Event.EVENT_NAME, cleanEventName);

            if (properties == null || properties.Count > 0) {
                return eventDetails;
            }

            var eventData = CleanObjectDictonary(properties);
            eventDetails.Add(UnityNativeConstants.Event.EVENT_DATA, eventData);

            return eventDetails;
        }

        internal Dictionary<string, object> BuildChargedEvent(Dictionary<string, object> details, List<Dictionary<string, object>> items) {
            if (details == null || items == null) {
                return null;
            }

            if (items.Count > 50) {
                // Log error
                return null;
            }

            var eventDetails = new Dictionary<string, object>();
            eventDetails.Add(UnityNativeConstants.Event.EVENT_NAME, UnityNativeConstants.Event.EVENT_CHARGED);

            var eventData = CleanObjectDictonary(details);
            eventDetails.Add(UnityNativeConstants.Event.EVENT_DATA, eventData);

            var itemsDetails = new List<Dictionary<string, object>>();
            foreach (var item in items) {
                itemsDetails.Add(CleanObjectDictonary(item));
            }

            eventData.Add(UnityNativeConstants.Event.EVENT_CHARGED_ITEMS, itemsDetails);

            return eventDetails;
        }
    }
}
#endif