#if !UNITY_IOS && !UNITY_ANDROID
using System.Collections.Generic;
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

        internal virtual event EventTimerTick OnEventTimerTick;

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

        protected virtual void ResetAndStartTimer(bool retry = false) {
            CleverTapLogger.Log("ResetAndStartTimer");
            if (retryCount == 0) {
                StopTimer();
                timer = new Timer();
                timer.AutoReset = false;
                timer.Elapsed += OnTimerTick;
                timer.Interval = defaultTimerInterval;
                timer.Start();
                return;
            }

            if (retry) {
                StopTimer();
                timer = new Timer();
                timer.AutoReset = false;
                timer.Elapsed += OnTimerTick;
                timer.Interval = (2 ^ (retryCount % 10)) * 1000;
                timer.Start();
            }
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