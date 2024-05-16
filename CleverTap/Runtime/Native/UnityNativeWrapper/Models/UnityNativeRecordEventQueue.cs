#if !UNITY_IOS && !UNITY_ANDROID
using CleverTapSDK.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CleverTapSDK.Native {

    internal class UnityNativeRecordEventQueue : UnityNativeBaseEventQueue {

        private List<UnityNativeEvent> eventsDuringFlushProccess;

        internal UnityNativeRecordEventQueue(int queueLimit = 49, int defaultTimerInterval = 1000) : base(queueLimit, defaultTimerInterval) {
            eventsDuringFlushProccess = new List<UnityNativeEvent>();
        }

        internal override void QueueEvent(UnityNativeEvent recordEvent) {
            if (isInFlushProcess) {
                eventsDuringFlushProccess.Add(recordEvent);
                return;
            }

            base.QueueEvent(recordEvent);
            ResetAndStartTimer();
        }

        internal async override Task<List<UnityNativeEvent>> FlushEvents() {
            if (isInFlushProcess == true) {
                return null;
            }

            isInFlushProcess = true;
            var proccesedEvents = new List<UnityNativeEvent>();

            while (eventsQueue.Count > 0)
            {
                try
                {
                    var events = eventsQueue.Peek();

                    var metaEvent = Json.Serialize(new UnityNativeMetaEventBuilder().BuildMeta());
                    var allEventsJson = new List<string> { metaEvent };
                    allEventsJson.AddRange(events.Select(e => Json.Serialize(e)));
                    var jsonContent = "[" + string.Join(",", allEventsJson) + "]";


                    var deviceInfo = UnityNativeDeviceManager.Instance.DeviceInfo;
                    var accountInfo = UnityNativeAccountManager.Instance.AccountInfo;
                    var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();

                    var queryParameters = new List<KeyValuePair<string, string>> {
                        new KeyValuePair<string, string>(UnityNativeConstants.Network.QUERY_OS, deviceInfo.OsName),
                        new KeyValuePair<string, string>(UnityNativeConstants.Network.QUERY_SKD_REVISION, UnityNativeConstants.SDK.REVISION),
                        new KeyValuePair<string, string>(UnityNativeConstants.Network.QUERY_ACCOUNT_ID, accountInfo.AccountId),
                        new KeyValuePair<string, string>(UnityNativeConstants.Network.QUERY_CURRENT_TIMESTAMP, timestamp)
                    };

                    var request = new UnityNativeRequest(UnityNativeConstants.Network.REQUEST_PATH_RECORD, UnityNativeConstants.Network.REQUEST_POST)
                    .SetRequestBody(jsonContent)
                    .SetQueryParameters(queryParameters);

                    var response = await UnityNativeNetworkEngine.Instance.ExecuteRequest(request);
                    if (response.StatusCode >= HttpStatusCode.OK && response.StatusCode <= HttpStatusCode.Accepted)
                    {
                        proccesedEvents.AddRange(events);
                        retryCount = 0;
                    }
                    else
                    {
                        OnEventError();
                    }
                }
                catch (Exception ex)
                {
                    OnEventError();
                    return proccesedEvents;
                }
            }

            isInFlushProcess = false;
            QueueEvents(eventsDuringFlushProccess);
            eventsDuringFlushProccess.Clear();
            if (eventsQueue.Any()) {
                ResetAndStartTimer();
            } else {
                StopTimer();
            }

            return proccesedEvents;
        }

        private void OnEventError() {
            retryCount++;
            isInFlushProcess = false;
            QueueEvents(eventsDuringFlushProccess);
            eventsDuringFlushProccess.Clear();
            ResetAndStartTimer();
        }
    }
}
#endif