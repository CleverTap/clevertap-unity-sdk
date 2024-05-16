#if !UNITY_IOS && !UNITY_ANDROID
using System.Collections.Generic;

namespace CleverTapSDK.Native {
    internal class UnityNativeWrapper {        
        private readonly UnityNativeEventManager _eventManager;

        internal UnityNativeWrapper()
        {
            _eventManager = new UnityNativeEventManager();
        }

        internal void LaunchWithCredentials(string accountID, string token, string region = null) {
            _eventManager.LaunchWithCredentials(accountID, token, region);
        }
        internal void OnUserLogin(Dictionary<string, object> properties) {
            _eventManager.OnUserLogin(properties);
        }
        internal void ProfilePush(Dictionary<string, object> properties) {
            _eventManager.ProfilePush(properties);
        }
        internal void RecordEvent(string eventName, Dictionary<string, object> properties = null) {
            _eventManager.RecordEvent(eventName, properties);
        }
        internal void RecordChargedEventWithDetailsAndItems(Dictionary<string, object> details, List<Dictionary<string, object>> items) {
            _eventManager.RecordChargedEventWithDetailsAndItems(details, items);
        }
    }
}
#endif