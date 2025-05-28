#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleverTapSDK.Native
{
    internal class UnityNativeNotificationViewedEventQueue : UnityNativeBaseEventQueue
    {
        protected override string QueueName => "NOTIFICATION_VIEW_EVENT";

        private UnityNativeNetworkEngine _networkEngine;

        internal UnityNativeNotificationViewedEventQueue(UnityNativeCoreState coreState,
            UnityNativeNetworkEngine networkEngine) :
            base(coreState, networkEngine)
        {
            _networkEngine = networkEngine;
        }

        internal override async Task<List<UnityNativeEvent>> FlushEvents()
        {
            return await FlushEventsCore(request => networkEngine.ExecuteRequest(request, UnityNativeEventType.NotificationViewEvent));
        }
    }
}
#endif