#if !UNITY_IOS && !UNITY_ANDROID
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Timers;
using CleverTapSDK.Utilities;

namespace CleverTapSDK.Native {

    internal delegate void EventTimerTick();

    internal abstract class UnityNativeBaseEventQueue {

        protected readonly int queueLimit;
        protected readonly int defaultTimerInterval;
        protected Timer timer;

        protected int retryCount = 0;
        protected bool isInFlushProcess = false;
        protected Queue<List<UnityNativeEvent>> eventsQueue;
        protected List<UnityNativeEvent> lastEventsInQueue;
        protected abstract string RequestPath { get; }
        internal virtual event EventTimerTick OnEventTimerTick;

        private List<UnityNativeEvent> eventsDuringFlushProccess;


        internal UnityNativeBaseEventQueue(int queueLimit = 49, int defaultTimerInterval = 1) {
            this.queueLimit = queueLimit;
            this.defaultTimerInterval = defaultTimerInterval;
            timer = new Timer(this.defaultTimerInterval);
            timer.AutoReset = false;
            timer.Elapsed += OnTimerTick;

            eventsQueue = new Queue<List<UnityNativeEvent>>();
            lastEventsInQueue = new List<UnityNativeEvent>();
            eventsQueue.Enqueue(lastEventsInQueue);
        }

        internal virtual void QueueEvent(UnityNativeEvent newEvent) {
            if (lastEventsInQueue == null || lastEventsInQueue.Count == queueLimit || eventsQueue.Count == 0) {
                if (lastEventsInQueue == null)
                    lastEventsInQueue = new List<UnityNativeEvent>();
                lastEventsInQueue.Add(newEvent);
                eventsQueue.Enqueue(lastEventsInQueue);
            } else if (lastEventsInQueue.Count < queueLimit) {
                lastEventsInQueue.Add(newEvent);
            }
        }

        internal virtual void QueueEvents(List<UnityNativeEvent> newEvents) {
            foreach (var newEvent in newEvents) {
                QueueEvent(newEvent);
            }
        }

        internal abstract Task<List<UnityNativeEvent>> FlushEvents();

        protected virtual void OnTimerTick(object sender, ElapsedEventArgs e) {
            CleverTapLogger.Log("Timer TICK");
            if (OnEventTimerTick != null) {
                OnEventTimerTick();
            }

            StopTimer();
        }

        protected async Task<List<UnityNativeEvent>> FlushEventsCore(Func<UnityNativeRequest, Task<UnityNativeResponse>> executeRequest)
        {
            if (isInFlushProcess)
            {
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

                    var request = new UnityNativeRequest(RequestPath, UnityNativeConstants.Network.REQUEST_POST)
                        .SetRequestBody(jsonContent)
                        .SetQueryParameters(queryParameters);

                    var response = await executeRequest(request);
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
                }
                catch (Exception ex)
                {
                    OnEventError();
                    CleverTapLogger.Log(ex.Message);
                    return proccesedEvents;
                }
            }

            isInFlushProcess = false;
            QueueEvents(eventsDuringFlushProccess);
            eventsDuringFlushProccess.Clear();
            if (eventsQueue.Any())
            {
                ResetAndStartTimer();
            }
            else
            {
                StopTimer();
            }

            return proccesedEvents;
        }

        protected void OnEventError()
        {
            retryCount++;
            isInFlushProcess = false;
            QueueEvents(eventsDuringFlushProccess);
            eventsDuringFlushProccess.Clear();
            ResetAndStartTimer();
        }

        protected virtual void ResetAndStartTimer(bool retry = false) {
            CleverTapLogger.Log("ResetAndStartTimer");
            if (retryCount == 0)
            {
                RestartTimer(defaultTimerInterval);
                return;
            }

            if (retry) {
                RestartTimer((2 ^ (retryCount % 10)) * 1000);
                
            }
        }

        private void RestartTimer(double duration)
        {
            StopTimer();

            timer = new Timer();
            timer.AutoReset = false;
            timer.Elapsed += OnTimerTick;
            timer.Interval = duration;
            timer.Start();
        }

        protected virtual void StopTimer() {
            if (timer != null)
            {
                timer.Stop();
                timer.Elapsed -= OnTimerTick;
                timer.Dispose();
                timer = null;
            }

        }
    }
}
#endif