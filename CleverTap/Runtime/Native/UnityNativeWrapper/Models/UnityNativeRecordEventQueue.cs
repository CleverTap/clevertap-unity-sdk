#if !UNITY_IOS && !UNITY_ANDROID
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

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

            while (eventsQueue.Count > 0) {
                try {
                    var events = eventsQueue.Peek();

                    // Refactor this
                    var metaEvent = "{}";
                    var jsonContent = "[";
                    jsonContent += metaEvent;
                    foreach (var json in events.Select(e => jsonContent)) {
                        jsonContent += ("," + jsonContent);
                    }
                    jsonContent += "]";

                    // WIP : Missing routes and event builder
                    //var request = new UnityNativeRequest("recordEvent", "POST").SetRequestBody(jsonContent);
                    //var response = await UnityNativeNetworkEngine.Instance.ExecuteRequest(request);
                    if (true /*response.StatusCode == System.Net.HttpStatusCode.OK*/) {
                        proccesedEvents.AddRange(eventsQueue.Dequeue());
                        retryCount = 0;
                    } else {
                        OnEventError();
                    }
                } catch (Exception ex) {
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