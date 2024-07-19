#if !UNITY_IOS && !UNITY_ANDROID
using System.Threading.Tasks;

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
                    _recordEventsQueue.QueueEvent(newEvent);
                    break;
                case UnityNativeEventType.RecordEvent:
                    _recordEventsQueue.QueueEvent(newEvent);
                    break;
                default:
                    break;
            }
        }

        private async void OnUserEventTimerTick() {
            await FlushUserEvents();
        }

        private async void OnRecordEventTimerTick() {
           await FlushRecordEvents();
        }

        private async Task FlushUserEvents() {
            var flushedEvents = await _userEventsQueue.FlushEvents();
            _databaseStore.DeleteEvents(flushedEvents);
        }

        private async Task FlushRecordEvents() {
            var flushedEvents = await _recordEventsQueue.FlushEvents();
            _databaseStore.DeleteEvents(flushedEvents);
        }

        public async void FlushQueue(){
            await FlushUserEvents();
            await FlushRecordEvents();
        }
    }
}
#endif