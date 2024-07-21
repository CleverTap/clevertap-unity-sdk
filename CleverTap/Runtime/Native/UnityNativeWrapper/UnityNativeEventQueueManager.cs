#if !UNITY_IOS && !UNITY_ANDROID
using System.Threading.Tasks;
using CleverTapSDK.Utilities;

namespace CleverTapSDK.Native {
    internal class UnityNativeEventQueueManager {
        private readonly UnityNativeDatabaseStore _databaseStore;

        private readonly UnityNativeBaseEventQueue _userEventsQueue;
        private readonly UnityNativeBaseEventQueue _recordEventsQueue;

        internal UnityNativeEventQueueManager(UnityNativeDatabaseStore databaseStore) {
            _databaseStore = databaseStore;
            _databaseStore.OnEventStored += OnDatabaseEventStored;
            _userEventsQueue = new UnityNativeUserEventQueue();
            _userEventsQueue.OnEventTimerTick += OnUserEventTimerTick;
            _recordEventsQueue = new UnityNativeRecordEventQueue();
            _recordEventsQueue.OnEventTimerTick += OnRecordEventTimerTick;
        }

        private void OnDatabaseEventStored(UnityNativeEvent newEvent) {
            switch (newEvent.EventType) {
                case UnityNativeEventType.ProfileEvent:
                    _userEventsQueue.QueueEvent(newEvent);
                    break;
                case UnityNativeEventType.RecordEvent:
                    _recordEventsQueue.QueueEvent(newEvent);
                    break;
                default:
                    break;
            }
        }

        internal async void FlushQueues() {
            CleverTapLogger.Log("Flushing queues");
            await FlushUserEvents();
            await FlushRecordEvents();
        }

        private async void OnUserEventTimerTick() {
            await FlushUserEvents();
        }

        private async void OnRecordEventTimerTick() {
           await FlushRecordEvents();
        }

        private async Task FlushUserEvents() {
            CleverTapLogger.Log("Flushing user events");
            var flushedEvents = await _userEventsQueue.FlushEvents();
            _databaseStore.DeleteEvents(flushedEvents);
        }

        private async Task FlushRecordEvents() {
            CleverTapLogger.Log("Flushing record events");
            var flushedEvents = await _recordEventsQueue.FlushEvents();
            _databaseStore.DeleteEvents(flushedEvents);
        }
    }
}
#endif