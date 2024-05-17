#if !UNITY_IOS && !UNITY_ANDROID
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleverTapSDK.Native {
    internal class UnityNativeUserEventQueue : UnityNativeBaseEventQueue {

        internal UnityNativeUserEventQueue(int queueLimit = 49, int defaultTimerInterval = 1) : base(queueLimit, defaultTimerInterval) { }

        protected override string RequestPath => UnityNativeConstants.Network.REQUEST_PATH_USER_VARIABLES;

        internal override async Task<List<UnityNativeEvent>> FlushEvents()
        {
            return await FlushEventsCore(path => UnityNativeNetworkEngine.Instance.ExecuteRequest(path));
        }

        protected override bool CanProcessEventResponse(UnityNativeResponse response)
        {
            return response.IsSuccess();
        }
    }
}
#endif