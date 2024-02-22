#if !UNITY_IOS && !UNITY_ANDROID

using System.Collections.Generic;

namespace CleverTapSDK.Native {
    internal class UnityNativeWrapper {        
        private readonly UnityNativeSessionManager _sessionManager;
        private readonly UnityNativePreferenceManager _preferenceManager;
        private readonly UnityNativeEventBuilder _eventBuilder;
        private readonly UnityNativeDatabaseStore _databaseStore;
        private readonly UnityNativeEventQueueManager _eventEventQueueManager;

        internal UnityNativeWrapper()
        {
            _sessionManager = new UnityNativeSessionManager();
            _preferenceManager = new UnityNativePreferenceManager(_sessionManager);
            _eventBuilder = new UnityNativeEventBuilder(_sessionManager);
            _databaseStore = new UnityNativeDatabaseStore();
            _eventEventQueueManager = new UnityNativeEventQueueManager(_sessionManager, _databaseStore);
        }

        internal void LaunchWithCredentials(string accountID, string token, string region = null) {

        }
        
        internal void OnUserLogin(Dictionary<string, object> properties) {
            _sessionManager.UpdateSessionUserIdentity("TODO");
            var @event = _eventBuilder.GenerateEvent(UnityNativeEventType.ProfileEvent, new { eventName = "OnUserLogin", properties = properties });
            _databaseStore.AddEvent(@event);
        }
        internal void ProfilePush(Dictionary<string, object> properties) {

        }
        internal void RecordEvent(string eventName, Dictionary<string, object> properties = null) {
            
        }
        internal void RecordChargedEventWithDetailsAndItems(Dictionary<string, object> details, List<Dictionary<string, object>> items) {

        }
    }
}
#endif