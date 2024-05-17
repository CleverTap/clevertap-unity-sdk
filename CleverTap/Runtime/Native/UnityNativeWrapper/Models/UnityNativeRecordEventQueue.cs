#if !UNITY_IOS && !UNITY_ANDROID
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleverTapSDK.Native {

    internal class UnityNativeRecordEventQueue : UnityNativeBaseEventQueue {

        private List<UnityNativeEvent> eventsDuringFlushProccess;

        internal UnityNativeRecordEventQueue(int queueLimit = 49, int defaultTimerInterval = 1) : base(queueLimit, defaultTimerInterval) {
            eventsDuringFlushProccess = new List<UnityNativeEvent>();
        }
        internal UnityNativeRecordEventQueue(int queueLimit = 49, int defaultTimerInterval = 1) : base(queueLimit, defaultTimerInterval) { }

        protected override string RequestPath => UnityNativeConstants.Network.REQUEST_PATH_RECORD;

        internal override async Task<List<UnityNativeEvent>> FlushEvents()
        {
            return await FlushEventsCore(path => UnityNativeNetworkEngine.Instance.ExecuteRequest(path));
        }

        internal override void QueueEvent(UnityNativeEvent recordEvent) {
            if (isInFlushProcess) {
                eventsDuringFlushProccess.Add(recordEvent);
                return;
            }

            base.QueueEvent(recordEvent);
            ResetAndStartTimer();
        protected override bool CanProcessEventResponse(UnityNativeResponse response)
        {
            bool processHeaders = UnityNativeNetworkEngine.Instance.ProcessIncomingHeaders(response);
            return processHeaders && response.IsSuccess();
        }
    }
}
#endif