#if !UNITY_IOS && !UNITY_ANDROID
using CleverTapSDK.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CleverTapSDK.Native {
    internal class UnityNativeUserEventQueue : UnityNativeBaseEventQueue {

        private List<UnityNativeEvent> eventsDuringFlushProccess;

        internal UnityNativeUserEventQueue(int queueLimit = 49, int defaultTimerInterval = 1) : base(queueLimit, defaultTimerInterval) {
            eventsDuringFlushProccess = new List<UnityNativeEvent>();
        }

        internal override void QueueEvent(UnityNativeEvent userEvent)
        {
            if (isInFlushProcess)
            {
                eventsDuringFlushProccess.Add(userEvent);
                return;
            }

            base.QueueEvent(userEvent);
            ResetAndStartTimer();
        }

        internal async override Task<List<UnityNativeEvent>> FlushEvents() {
            if (isInFlushProcess == true) {
                return null;
            }

            isInFlushProcess = true;
            var proccesedEvents = new List<UnityNativeEvent>();

            while (eventsQueue.Count > 0) {
                
                try {
                    var events = eventsQueue.Peek();

                    var metaEvent = Json.Serialize(new UnityNativeMetaEventBuilder().BuildMeta());
                    var allEventsJson = new List<string> { metaEvent };
                    allEventsJson.AddRange(events.Select(e => e.JsonContent));
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


                    var request = new UnityNativeRequest(UnityNativeConstants.Network.REQUEST_PATH_USER_VARIABLES, UnityNativeConstants.Network.REQUEST_POST)
                    .SetRequestBody(jsonContent)
                    .SetQueryParameters(queryParameters);

                    var response = await UnityNativeNetworkEngine.Instance.ExecuteRequest(request);
                    bool processHeaders = UnityNativeNetworkEngine.Instance.ProcessIncomingHeaders(response);

                    if (processHeaders && response.StatusCode >= HttpStatusCode.OK && response.StatusCode <= HttpStatusCode.Accepted)
                    {
                        proccesedEvents.AddRange(events);
                        lastEventsInQueue.Clear();
                        eventsQueue.Dequeue();
                        retryCount = 0;
                    }
                    else
                    {
                        OnEventError();
                    }
                } catch (Exception ex) {
                    OnEventError();
                    CleverTapLogger.Log(ex.Message);
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