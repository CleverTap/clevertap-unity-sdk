#if !UNITY_IOS && !UNITY_ANDROID
using CleverTapSDK.Utilities;
using System;
using System.Collections.Generic;

namespace CleverTapSDK.Native {
    internal class UnityNativeEventManager {
        private readonly UnityNativePreferenceManager _preferenceManager;
        private readonly UnityNativeDatabaseStore _databaseStore;
        private readonly UnityNativeEventQueueManager _eventQueueManager;

        internal UnityNativeEventManager() {
            _preferenceManager = new UnityNativePreferenceManager();
            _databaseStore = new UnityNativeDatabaseStore();
            _eventQueueManager = new UnityNativeEventQueueManager(_databaseStore);
        }

        #region Launch

        internal void LaunchWithCredentials(string accountID, string token, string region = null) {
            UnityNativeAccountManager.Instance.SetAccountInfo(accountID, token, region);
            RecordAppLaunch();
        }

        internal UnityNativeEvent RecordAppLaunch() {
            var accountInfo = UnityNativeAccountManager.Instance.AccountInfo;
            if (string.IsNullOrEmpty(accountInfo.AccountId) || string.IsNullOrEmpty(accountInfo.AccountToken)) {
                // Log error?
                throw new ArgumentNullException("accountId || accountToken");
            }

            if (UnityNativeSessionManager.Instance.CurrentSession.isAppLaunched) {
                // App already lanuched
                return null;
            }

            UnityNativeSessionManager.Instance.CurrentSession.SetIsAppLaunched(true);
            var eventDetails = new Dictionary<string, object> {
                { UnityNativeConstants.Event.EVENT_NAME, UnityNativeConstants.Event.EVENT_APP_LUNACH }
            };

            return BuildEventWithAppFields(UnityNativeEventType.RecordEvent, eventDetails);
        } 

        #endregion

        #region Profile Events

        internal UnityNativeEvent OnUserLogin(Dictionary<string, object> properties) {
            return null;
        }

        internal UnityNativeEvent ProfilePush(Dictionary<string, object> properties) {
            return null;
        }

        #endregion

        #region Record Events

        internal UnityNativeEvent RecordEvent(string eventName, Dictionary<string, object> properties = null) {
            var eventBuilderResult = new UnityNativeRecordEventBuilder().Build(eventName, properties);
            var eventDetails = eventBuilderResult.EventResult;
            return BuildEvent(UnityNativeEventType.RecordEvent, eventDetails);
        }

        internal UnityNativeEvent RecordChargedEventWithDetailsAndItems(Dictionary<string, object> details, List<Dictionary<string, object>> items) {
            var eventBuilderResult = new UnityNativeRecordEventBuilder().BuildChargedEvent(details, items);
            var eventDetails = eventBuilderResult.EventResult;
            return BuildEvent(UnityNativeEventType.RecordEvent, eventDetails);
        }

        #endregion

        #region Private

        private UnityNativeEvent BuildEvent(UnityNativeEventType eventType, Dictionary<string, object> eventDetails) {
            if (UnityNativeSessionManager.Instance.CurrentSession.isAppLaunched) {
                RecordAppLaunch();
            }
            
            var eventData = new UnityNativeEventBuilder().BuildEvent(eventType, eventDetails);
            var eventDataJSONContent = Json.Serialize(eventData);
            return new UnityNativeEvent(eventType, eventDataJSONContent);
        }

        private UnityNativeEvent BuildEventWithAppFields(UnityNativeEventType eventType, Dictionary<string, object> eventDetails) {
            if (!UnityNativeSessionManager.Instance.CurrentSession.isAppLaunched) {
                RecordAppLaunch();
            }

            var eventData = new UnityNativeEventBuilder().BuildEventWithAppFields(eventType, eventDetails);
            var eventDataJSONContent = Json.Serialize(eventData);
            return new UnityNativeEvent(eventType, eventDataJSONContent);
        }

        #endregion
    }
}
#endif