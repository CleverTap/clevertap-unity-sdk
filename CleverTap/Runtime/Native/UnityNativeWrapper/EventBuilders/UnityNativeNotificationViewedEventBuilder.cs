#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR
using System.Collections.Generic;

namespace CleverTapSDK.Native
{
    internal class UnityNativeNotificationViewedEventBuilder
    {
        internal UnityNativeNotificationViewedEventBuilder(UnityNativeEventValidator eventValidator) { }

        internal UnityNativeEventBuilderResult<Dictionary<string, object>> Build(Dictionary<string, object> properties = null)
        {
            var validationResults = new List<UnityNativeValidationResult>();

            var eventDetails = new Dictionary<string, object>
            {
                { UnityNativeConstants.Event.EVENT_NAME, UnityNativeConstants.Event.CLTAP_NOTIFICATION_VIEWED_EVENT_NAME }
            };

            if (properties == null || properties.Count == 0)
            {
                return new UnityNativeEventBuilderResult<Dictionary<string, object>>(validationResults, eventDetails);
            }

            eventDetails.Add(UnityNativeConstants.Event.EVENT_DATA, properties);

            return new UnityNativeEventBuilderResult<Dictionary<string, object>>(validationResults, eventDetails);
        }
    }
}
#endif