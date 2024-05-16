#if !UNITY_IOS && !UNITY_ANDROID
using System.Collections.Generic;
using CleverTapSDK.Utilities;

namespace CleverTapSDK.Native {
    internal delegate void EventStored(UnityNativeEvent newEvent);
 
    internal class UnityNativeDatabaseStore {

        internal event EventStored OnEventStored;
        private UnityDataService dataService;

        internal UnityNativeDatabaseStore(string databaseName) {
            InitilazeDatabase(databaseName);
        }

        internal List<UnityNativeEvent> GetEvents() {
            return new List<UnityNativeEvent>();
        }

        internal void AddEvent(UnityNativeEvent newEvent) {

            if(newEvent == null || newEvent.JsonContent == null || dataService == null || dataService.GetConnection() == null)
            {
                CleverTapLogger.LogError("Database not intiallised");
                return;
            }

            var dbEventId = dataService.Insert(new UnityNativeEventDBEntry(newEvent.EventType, newEvent.JsonContent, newEvent.Timestamp));
            var storedEvent = new UnityNativeEvent(dbEventId, newEvent.EventType, newEvent.JsonContent, newEvent.Timestamp);

            if (OnEventStored != null) {
                OnEventStored(storedEvent);
            }
        }

        internal void AddEventsFromDB()
        {
            if (dataService == null || dataService.GetConnection() == null)
            {
                CleverTapLogger.LogError("Database not intiallised");
                return;
            }

            List<UnityNativeEventDBEntry> entries = dataService.GetAllEntries<UnityNativeEventDBEntry>();

            foreach (var entry in entries)
            {
                if (OnEventStored != null)
                {
                    OnEventStored(new UnityNativeEvent(entry.Id, entry));
                }
            }

            CleverTapLogger.Log("Cache events added to processing queue from Database");
        }

        internal void DeleteEvents(List<UnityNativeEvent> eventsToRemove) {

            if (dataService == null || dataService.GetConnection() == null)
            {
                CleverTapLogger.LogError("Database not intiallised");
                return;
            }
            
            foreach(var events in eventsToRemove)
            {
                int id =  events.Id ?? -1;
                if (id != -1)
                {
                    dataService.Delete<UnityNativeEventDBEntry>(id);
                }
            }
        }

        private void InitilazeDatabase(string databaseName)
        {
            if (dataService == null || dataService.GetConnection() == null)
            {   
                dataService = new UnityDataService(databaseName);
                CleverTapLogger.Log("Database connection created");
            }

            //Create Table If Doesn't Exist
            dataService.CreateTable<UnityNativeEventDBEntry>();
        }
    }
}
#endif