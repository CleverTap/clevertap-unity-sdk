#if !UNITY_IOS && !UNITY_ANDROID
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace CleverTapSDK.Native {

    internal delegate void EventTimerTick();

    internal abstract class UnityNativeBaseEventQueue {
        protected readonly int queueLimit;
        protected readonly int defaultTimerInterval;
        protected readonly Timer timer;

        protected int retryCount = 0;
        protected bool isInFlushProcess = false;
        protected Queue<List<UnityNativeEvent>> eventsQueue;
        protected List<UnityNativeEvent> lastEventsInQueue;

        internal virtual event EventTimerTick OnEventTimerTick;

        internal UnityNativeBaseEventQueue(int queueLimit = 49, int defaultTimerInterval = 0) {
            this.queueLimit = queueLimit;
            this.defaultTimerInterval = defaultTimerInterval;
            timer = new Timer();
            timer.Elapsed += OnTimerTick;

            eventsQueue = new Queue<List<UnityNativeEvent>>();
            lastEventsInQueue = new List<UnityNativeEvent>();
        }

        internal virtual void QueueEvent(UnityNativeEvent newEvent) {
            if (lastEventsInQueue == null || lastEventsInQueue.Count == queueLimit) {
                lastEventsInQueue = new List<UnityNativeEvent>() { newEvent };
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
            if (OnEventTimerTick != null) {
                OnEventTimerTick();
            }

            StopTimer();
        }

        protected virtual void ResetAndStartTimer(bool retry = false) {
            if (retryCount == 0) {
                timer.Stop();
                timer.Interval = defaultTimerInterval;
                timer.Start();
                return;
            }

            if (retry) {
                timer.Stop();
                timer.Interval = 2 ^ (retryCount % 10);
                timer.Start();
            }
        }

        protected virtual void StopTimer() {
            timer.Stop();
            timer.Interval = defaultTimerInterval;
        }
    }
}
#endif