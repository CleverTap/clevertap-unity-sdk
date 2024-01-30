#if !UNITY_IOS && !UNITY_ANDROID
using System.Collections.Generic;

namespace CleverTapSDK.Native {
    internal class UnityNativeDatabaseStore {
        internal UnityNativeDatabaseStore() {
            // TODO : Check if database is already initialized
            // if (check if database is initialized)
            // and initialize it if not
            InitilazeDatabase();
        }

        internal List<UnityNativeEvent> GetEvents(UnityNativeEventType eventType, long fromTimestamp, int limit = 49) {
            // TODO : Implement logic that will fetch events from database table from timestamp up to limit(49) by eventType
            return new List<UnityNativeEvent>();
        }

        internal void AddEvent(UnityNativeEventType eventType, UnityNativeEvent @event) {
            // TODO : Implement logic for inserting event to database table based on eventType
        }

        internal void DeleteEvents(UnityNativeEventType eventType, long toTimestamp) {
            // TODO : Implement logic for deleting events from database table between from and now timestamp by eventType
        }

        private void InitilazeDatabase() {
            // TODO : Generate database and tables
        }
    }
}
#endif