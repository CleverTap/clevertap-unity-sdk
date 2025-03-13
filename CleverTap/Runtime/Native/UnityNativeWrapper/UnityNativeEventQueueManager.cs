#if (!UNITY_IOS && !UNITY_ANDROID) || UNITY_EDITOR
using System.Threading.Tasks;
using CleverTapSDK.Utilities;

namespace CleverTapSDK.Native {
    internal class UnityNativeEventQueueManager {
        private readonly UnityNativeDatabaseStore _databaseStore;

        private readonly UnityNativeBaseEventQueue _userEventsQueue;
        private readonly UnityNativeBaseEventQueue _raisedEventsQueue;

        private readonly UnityNativeBaseEventQueue _singleEventsQueue;

        internal UnityNativeEventQueueManager(UnityNativeCoreState coreState, UnityNativeNetworkEngine networkEngine, UnityNativeDatabaseStore databaseStore) {
            _databaseStore = databaseStore;
            _databaseStore.OnEventStored += OnDatabaseEventStored;
            _userEventsQueue = new UnityNativeUserEventQueue(coreState, networkEngine);
            _userEventsQueue.OnEventTimerTick += OnUserEventTimerTick;
            _raisedEventsQueue = new UnityNativeRaisedEventQueue(coreState, networkEngine);
            _raisedEventsQueue.OnEventTimerTick += OnRaisedEventTimerTick;
            _singleEventsQueue = new UnityNativeSingleEventQueue(coreState, networkEngine);
            _singleEventsQueue.OnEventTimerTick += OnSingleEventTimerTick;
            // Add the events stored in the DB
            _databaseStore.AddEventsFromDB();
        }

        private void OnDatabaseEventStored(UnityNativeEvent newEvent) {
            QueueEvent(newEvent);
        }

        internal void QueueEvent(UnityNativeEvent newEvent)
        {
            switch (newEvent.EventType)
            {
                case UnityNativeEventType.ProfileEvent:
                    _userEventsQueue.QueueEvent(newEvent);
                    break;
                case UnityNativeEventType.RaisedEvent:
                    _raisedEventsQueue.QueueEvent(newEvent);
                    break;
                case UnityNativeEventType.DefineVarsEvent:
                    _singleEventsQueue.QueueEvent(newEvent);
                    break;
                default:
                    break;
            }
        }

        internal async void FlushQueues() {
            CleverTapLogger.Log("Flushing queues");
            await FlushUserEvents();
            await FlushRaisedEvents();
        }

        private async void OnUserEventTimerTick() {
            await FlushUserEvents();
        }

        private async void OnRaisedEventTimerTick() {
           await FlushRaisedEvents();
        }

        private async void OnSingleEventTimerTick()
        {
            await FlushSingleEvents();
        }

        private async Task FlushUserEvents() {
            CleverTapLogger.Log("Flushing user events");
            var flushedEvents = await _userEventsQueue.FlushEvents();
            _databaseStore.DeleteEvents(flushedEvents);
        }

        private async Task FlushRaisedEvents() {
            CleverTapLogger.Log("Flushing raised events");
            var flushedEvents = await _raisedEventsQueue.FlushEvents();
            _databaseStore.DeleteEvents(flushedEvents);
        }

        private async Task FlushSingleEvents()
        {
            CleverTapLogger.Log("Flushing single events");
            var flushedEvents = await _singleEventsQueue.FlushEvents();
        }
    }
}
#endif