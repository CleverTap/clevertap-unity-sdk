#if !UNITY_IOS && !UNITY_ANDROID
using System.Threading.Tasks;

namespace CleverTapSDK.Native {
    internal class UnityNativeEventQueueManager {
        private readonly UnityNativeDatabaseStore _databaseStore;
        private readonly UnityNativeSessionManager _sessionManager;

        private readonly UnityNativeBaseEventQueue _userEventsQueue;
        private readonly UnityNativeBaseEventQueue _recordEventsQueue;

        internal UnityNativeEventQueueManager(UnityNativeSessionManager sessionManager, UnityNativeDatabaseStore databaseStore) {
            _databaseStore = databaseStore;
            _sessionManager = sessionManager;
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

        private void OnUserEventTimerTick() {
            FlushUserEvents();
        }

        private void OnRecordEventTimerTick() {
            FlushRecordEvents();
        }

        private async Task FlushUserEvents() {
            var flushedEvents = await _userEventsQueue.FlushEvents();
            _databaseStore.DeleteEvents(flushedEvents);
        }

        private async Task FlushRecordEvents() {
            var flushedEvents = await _recordEventsQueue.FlushEvents();
            _databaseStore.DeleteEvents(flushedEvents);
        }
    }
}
#endif