#if !UNITY_IOS && !UNITY_ANDROID
using System.Collections.Generic;

namespace CleverTapSDK.Native {
    internal delegate void EventStored(UnityNativeEvent newEvent);

    internal class UnityNativeDatabaseStore {

        internal event EventStored OnEventStored;

        internal UnityNativeDatabaseStore() {
            // TODO : Check if database is already initialized
            // if (check if database is initialized)
            // and initialize it if not
            InitilazeDatabase();
        }

        internal List<UnityNativeEvent> GetEvents() {
            return new List<UnityNativeEvent>();
        }

        internal void AddEvent(UnityNativeEvent newEvent) {
            // TODO : Implement logic for inserting event to database table based on newEvent.EventType

            var dbEventId = 1;
            var storedEvent = new UnityNativeEvent(dbEventId, newEvent.EventType, newEvent.JsonContent, newEvent.Timestamp);

            if (OnEventStored != null) {
                OnEventStored(storedEvent);
            }
        }

        internal void DeleteEvents(List<UnityNativeEvent> eventsToRemove) {
            // TODO : Implement logic for deleting events from database table
        }

        private void InitilazeDatabase() {
            // TODO : Generate database and tables
        }
    }
}
#endif