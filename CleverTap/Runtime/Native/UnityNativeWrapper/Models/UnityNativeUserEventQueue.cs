#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleverTapSDK.Native {
    internal class UnityNativeUserEventQueue : UnityNativeBaseEventQueue {

        protected override string QueueName => "USER_EVENTS";

        internal UnityNativeUserEventQueue(UnityNativeCoreState coreState, UnityNativeNetworkEngine networkEngine, UnityNativeEventValidator eventValidator, int queueLimit = 49, int defaultTimerInterval = 1) : base(coreState, networkEngine, eventValidator, queueLimit, defaultTimerInterval) { }

        protected override string RequestPath => UnityNativeConstants.Network.REQUEST_PATH_RECORD;

        internal override async Task<List<UnityNativeEvent>> FlushEvents()
        {
            return await FlushEventsCore(path => networkEngine.ExecuteRequest(path));
        }

        protected override bool CanProcessEventResponse(UnityNativeResponse response)
        {
            return response.IsSuccess();
        }
    }
}
#endif