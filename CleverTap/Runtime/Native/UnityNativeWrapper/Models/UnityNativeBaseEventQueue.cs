#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleverTapSDK.Common;
using CleverTapSDK.Utilities;
using UnityEngine;

namespace CleverTapSDK.Native
{
    internal delegate void EventTimerTick();

    internal abstract class UnityNativeBaseEventQueue
    {
        internal virtual event EventTimerTick OnEventTimerTick;
        protected readonly int queueLimit;
        protected readonly int defaultTimerInterval;

        protected int retryCount = 0;
        protected bool isInFlushProcess = false;
        protected Queue<List<UnityNativeEvent>> eventsQueue;
        protected abstract string QueueName { get; }

        protected UnityNativeCoreState coreState;
        protected UnityNativeNetworkEngine networkEngine;
        private Coroutine timerCoroutine;

        internal UnityNativeBaseEventQueue(UnityNativeCoreState coreState, UnityNativeNetworkEngine networkEngine, int queueLimit = 49, int defaultTimerInterval = 1)
        {
            this.coreState = coreState;
            this.networkEngine = networkEngine;
            this.queueLimit = queueLimit;
            this.defaultTimerInterval = defaultTimerInterval;
            eventsQueue = new Queue<List<UnityNativeEvent>>();
        }

        internal virtual void QueueEvent(UnityNativeEvent newEvent)
        {
            if (!eventsQueue.TryPeek(out List<UnityNativeEvent> currentList) || currentList.Count == queueLimit)
            {
                currentList = new List<UnityNativeEvent>();
                eventsQueue.Enqueue(currentList);
            }

            currentList.Add(newEvent);
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
            OnEventTimerTick?.Invoke();
            StopTimer();
        }

        protected async Task<List<UnityNativeEvent>> FlushEventsCore(Func<UnityNativeRequest, Task<UnityNativeResponse>> executeRequest)
        {
            var proccesedEvents = new List<UnityNativeEvent>();
            if (isInFlushProcess)
            {
                return proccesedEvents;
            }

            isInFlushProcess = true;

            bool willRetry = false;
            List<UnityNativeEvent> events = new List<UnityNativeEvent>();
            while (eventsQueue.Count > 0 && !willRetry)
            {
                try
                {
                    events = eventsQueue.Peek();
                    var metaEvent = Json.Serialize(BuildMeta());
                    var allEventsJson = new List<string> { metaEvent };
                    allEventsJson.AddRange(events.Select(e => e.JsonContent));
                    var jsonContent = "[" + string.Join(",", allEventsJson) + "]";

                    string requestPath = GetRequestPath(events);
                    var queryStringParameters = GetQueryStringParameters();
                    var request = new UnityNativeRequest(requestPath, UnityNativeConstants.Network.REQUEST_POST)
                        .SetRequestBody(jsonContent)
                        .SetQueryParameters(queryStringParameters);

                    var response = await executeRequest(request);

                    if (CanProcessEventResponse(response, request, events))
                    {
                        proccesedEvents.AddRange(events);
                        retryCount = 0;
                        eventsQueue.Dequeue();
                    }
                    else
                    {
                        willRetry = true;
                        OnEventsError();
                        CleverTapLogger.Log($"Error sending queue");
                        return proccesedEvents;
                    }
                }
                catch (Exception ex)
                {
                    CleverTapLogger.Log($"Exception: {ex.Message}, Stack Trace: {ex.StackTrace}");
                    if (ShouldRetryOnException(ex, events))
                    {
                        willRetry = true;
                        OnEventsError();
                    }
                    else
                    {
                        // Drop the events
                        CleverTapLogger.Log($"ShouldRetryOnException returned false. Dropping {events.Count} events from: {QueueName}.");
                        proccesedEvents.AddRange(events);
                        retryCount = 0;
                        eventsQueue.Dequeue();
                    }

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

        protected virtual bool ShouldRetryOnException(Exception ex, List<UnityNativeEvent> sentEvents)
        {
            return true;
        }

        internal List<KeyValuePair<string, string>> GetQueryStringParameters()
        {
            var deviceInfo = coreState.DeviceInfo;
            var accountInfo = coreState.AccountInfo;
            if (deviceInfo == null || accountInfo == null)
            {
                CleverTapLogger.Log("Cannot generate query string parameters.");
                return new List<KeyValuePair<string, string>>();
            }

            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            // Default Query String parameters
            var queryParameters = new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>(UnityNativeConstants.Network.QUERY_OS, deviceInfo.OsName),
                new KeyValuePair<string, string>(UnityNativeConstants.Network.QUERY_SDK_REVISION, deviceInfo.SdkVersion.ToString()),
                new KeyValuePair<string, string>(UnityNativeConstants.Network.QUERY_ACCOUNT_ID, accountInfo.AccountId),
                new KeyValuePair<string, string>(UnityNativeConstants.Network.QUERY_CURRENT_TIMESTAMP, timestamp)
            };
            return queryParameters;
        }

        internal Dictionary<string, object> BuildMeta()
        {
            var deviceInfo = coreState.DeviceInfo;
            var accountInfo = coreState.AccountInfo;

            var metaDetails = new Dictionary<string, object>
            {
                { UnityNativeConstants.EventMeta.GUID, deviceInfo.DeviceId },
                { UnityNativeConstants.EventMeta.TYPE, UnityNativeConstants.EventMeta.TYPE_NAME },
                { UnityNativeConstants.EventMeta.APPLICATION_FIELDS, UnityNativeEventBuilder.BuildAppFields(deviceInfo) },
                { UnityNativeConstants.EventMeta.ACCOUNT_ID, accountInfo.AccountId },
                { UnityNativeConstants.EventMeta.ACCOUNT_TOKEN, accountInfo.AccountToken },
                { UnityNativeConstants.EventMeta.FIRST_REQUEST_IN_SESSION, coreState.SessionManager.IsFirstSession() },
                { UnityNativeConstants.EventMeta.ARP_KEY, GetARP() },
                { UnityNativeConstants.EventMeta.KEY_I, GetI() },
                { UnityNativeConstants.EventMeta.KEY_J, GetJ() }
            };
            return metaDetails;
        }

        private Dictionary<string, object> GetARP()
        {
            string arpNamespaceKey = string.Format(UnityNativeConstants.EventMeta.ARP_NAMESPACE_KEY,
                coreState.DeviceInfo.DeviceId);
            var preferenceManager = UnityNativePreferenceManager.GetPreferenceManager(coreState.AccountInfo.AccountId);
            string arpJson = preferenceManager.GetString(arpNamespaceKey, string.Empty);
            if (!string.IsNullOrEmpty(arpJson)
                && Json.Deserialize(arpJson) is Dictionary<string, object> arpDictionary
                && arpDictionary.Count > 0)
            {
                return arpDictionary;
            }

            return new Dictionary<string, object>();
        }

        private long GetI()
        {
            var preferenceManager = UnityNativePreferenceManager.GetPreferenceManager(coreState.AccountInfo.AccountId);
            return preferenceManager.GetLong(UnityNativeConstants.EventMeta.KEY_I, 0);
        }

        private long GetJ()
        {
            var preferenceManager = UnityNativePreferenceManager.GetPreferenceManager(coreState.AccountInfo.AccountId);
            return preferenceManager.GetLong(UnityNativeConstants.EventMeta.KEY_J, 0);
        }

        protected virtual bool CanProcessEventResponse(UnityNativeResponse response,
            UnityNativeRequest request, List<UnityNativeEvent> sentEvents)
        {
            return response.IsSuccess();
        }

        protected virtual string GetRequestPath(List<UnityNativeEvent> events)
        {
            return UnityNativeConstants.Network.REQUEST_PATH_RECORD;
        }

        protected void OnEventsError()
        {
            retryCount++;
            isInFlushProcess = false;
            ResetAndStartTimer();
        }

        protected virtual void ResetAndStartTimer()
        {
            CleverTapLogger.Log($"Calling {QueueName} ResetAndStartTimer, retryCount: {retryCount}");
            if (retryCount == 0)
            {
                RestartTimer(defaultTimerInterval);
                return;
            }

            float delay = Mathf.Pow(2, retryCount % 10);
            CleverTapLogger.Log($"Will retry sending events from queue {QueueName} in {delay}s.");
            RestartTimer(delay);
        }

        private void RestartTimer(float duration)
        {
            StopTimer();
            timerCoroutine = MonoHelper.Instance.StartCoroutine(TimerCoroutine(duration));
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
