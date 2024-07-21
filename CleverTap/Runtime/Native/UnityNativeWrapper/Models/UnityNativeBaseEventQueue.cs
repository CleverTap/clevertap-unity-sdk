#if !UNITY_IOS && !UNITY_ANDROID
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleverTapSDK.Utilities;
using UnityEngine;
using CleverTapSDK.Common;

namespace CleverTapSDK.Native
{

    internal delegate void EventTimerTick();

    internal abstract class UnityNativeBaseEventQueue
    {

        protected readonly int queueLimit;
        protected readonly int defaultTimerInterval;
        private Coroutine timerCoroutine;

        protected int retryCount = 0;
        protected bool isInFlushProcess = false;
        protected Queue<List<UnityNativeEvent>> eventsQueue;
        protected abstract string RequestPath { get; }
        internal virtual event EventTimerTick OnEventTimerTick;

        internal UnityNativeBaseEventQueue(int queueLimit = 49, int defaultTimerInterval = 1)
        {
            this.queueLimit = queueLimit;
            this.defaultTimerInterval = defaultTimerInterval;
            eventsQueue = new Queue<List<UnityNativeEvent>>();
        }

        internal virtual void QueueEvent(UnityNativeEvent newEvent)
        {
            if (eventsQueue.Count == 0 || eventsQueue.Peek().Count == queueLimit)
            {
                eventsQueue.Enqueue(new List<UnityNativeEvent>());
            }

            eventsQueue.Peek().Add(newEvent);
            ResetAndStartTimer();
        }

        internal virtual void QueueEvents(List<UnityNativeEvent> newEvents)
        {
            foreach (var newEvent in newEvents)
            {
                QueueEvent(newEvent);
            }
        }

        internal abstract Task<List<UnityNativeEvent>> FlushEvents();

        protected virtual void OnTimerTick()
        {
            CleverTapLogger.Log("Timer TICK");
            OnEventTimerTick?.Invoke();
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
                    var events = eventsQueue.Dequeue();
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

                    if (CanProcessEventResponse(response))
                    {
                        proccesedEvents.AddRange(events);
                        retryCount = 0;
                    }
                    else
                    {
                        // Re-enqueue events in case of error
                        QueueEvents(events);
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

        protected abstract bool CanProcessEventResponse(UnityNativeResponse response);

        protected void OnEventError()
        {
            retryCount++;
            isInFlushProcess = false;
            ResetAndStartTimer();
        }

        protected virtual void ResetAndStartTimer(bool retry = false)
        {
            CleverTapLogger.Log("ResetAndStartTimer");
            if (retryCount == 0)
            {
                RestartTimer(defaultTimerInterval);
                return;
            }

            if (retry)
            {
                RestartTimer((Mathf.Pow(2, retryCount % 10)) * 1000);
            }
        }

        private void RestartTimer(float duration)
        {
            StopTimer();
            timerCoroutine = MonoHelper.Instance.StartCoroutine(TimerCoroutine(duration));
            CleverTapLogger.Log("Timer Restarted");
        }

        private IEnumerator TimerCoroutine(float duration)
        {
            yield return new WaitForSeconds(duration);
            OnTimerTick();
        }

        protected virtual void StopTimer()
        {
            if (timerCoroutine != null)
            {
                MonoHelper.Instance.StopCoroutine(timerCoroutine);
                timerCoroutine = null;
            }
        }
    }
}
#endif
