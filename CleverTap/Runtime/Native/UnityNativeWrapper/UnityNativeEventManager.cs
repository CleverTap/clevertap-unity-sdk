#if !UNITY_IOS && !UNITY_ANDROID
using CleverTapSDK.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CleverTapSDK.Native {
    internal class UnityNativeEventManager {
        private static readonly string NATIVE_EVENTS_DB_CACHE = "NativeEventsDbCache";

        private readonly UnityNativePreferenceManager _preferenceManager;
        private readonly UnityNativeDatabaseStore _databaseStore;
        private readonly UnityNativeEventQueueManager _eventQueueManager;

        internal UnityNativeEventManager() {
            _preferenceManager = new UnityNativePreferenceManager();
            _databaseStore = new UnityNativeDatabaseStore(NATIVE_EVENTS_DB_CACHE);
            _eventQueueManager = new UnityNativeEventQueueManager(_databaseStore);
        }

        #region Launch

        internal void LaunchWithCredentials(string accountID, string token, string region = null) {
            UnityNativeAccountManager.Instance.SetAccountInfo(accountID, token, region);
            RecordAppLaunch();
        }

        internal void RecordAppLaunch() {
            if (UnityNativeSessionManager.Instance.CurrentSession.isAppLaunched) {
                return;
            }

            var accountInfo = UnityNativeAccountManager.Instance.AccountInfo;
            if (string.IsNullOrEmpty(accountInfo.AccountId) || string.IsNullOrEmpty(accountInfo.AccountToken)) {
                // Log error?
                throw new ArgumentNullException("accountId || accountToken");
            }

            UnityNativeNetworkEngine.Instance
                .SetHeaders(new Dictionary<string, string>() {
                    { UnityNativeConstants.Network.HEADER_ACCOUNT_ID_NAME, accountInfo.AccountId },
                    { UnityNativeConstants.Network.HEADER_ACCOUNT_TOKEN_NAME, accountInfo.AccountToken }})
                .SetBaseURI(UnityNativeConstants.Network.CT_TEMP_URL); // Remove this when hello ep is introduced

            UnityNativeSessionManager.Instance.CurrentSession.SetIsAppLaunched(true);
            var eventDetails = new Dictionary<string, object> {
                { UnityNativeConstants.Event.EVENT_NAME, UnityNativeConstants.Event.EVENT_APP_LUNACH }
            };

            UnityNativeEvent @event = BuildEventWithAppFields(UnityNativeEventType.RecordEvent, eventDetails, false);
            PushEvent(@event, (isPushed) =>
            {
                UnityNativeSessionManager.Instance.CurrentSession.SetIsAppLaunched(isPushed);
            });
        } 

        #endregion

        #region Profile Events

        internal UnityNativeEvent OnUserLogin(Dictionary<string, object> properties) {
            if (properties == null || properties.Count == 0) {
                return null;
            }

            // TODO : 
            // Check if user is already login
            // Flush / Remove all existing events
            // Reset session
            // Get info for user if exsits
            UnityNativeSessionManager.Instance.CurrentSession.SetIsAppLaunched(false);
            RecordAppLaunch();

            //Processing Stored Events On App Launch
            ProcessStoredEvents();

            return ProfilePush(properties);
        }

        internal UnityNativeEvent ProfilePush(Dictionary<string, object> properties) {
            if (properties == null || properties.Count == 0) {
                return null;
            }

            var eventBuilderResult = new UnityNativeProfileEventBuilder().BuildPushEvent(properties);
            if (eventBuilderResult.EventResult.SystemFields == null || eventBuilderResult.EventResult.CustomFields == null) {
                return null;
            }

            var eventDetails = new List<IDictionary<string, object>>() { 
                eventBuilderResult.EventResult.SystemFields, 
                eventBuilderResult.EventResult.CustomFields 
            }.SelectMany(d => d).ToDictionary(d => d.Key, d => d.Value);

            return BuildEvent(UnityNativeEventType.ProfileEvent, eventDetails);
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

        private UnityNativeEvent BuildEvent(UnityNativeEventType eventType, Dictionary<string, object> eventDetails, bool storeEvent = false)
        {
            if (!UnityNativeSessionManager.Instance.CurrentSession.isAppLaunched)
            {
                return null;
            }

            var eventData = new UnityNativeEventBuilder().BuildEvent(eventType, eventDetails);
            var eventDataJSONContent = Json.Serialize(eventData);
            var @event = new UnityNativeEvent(eventType, eventDataJSONContent);
            if (storeEvent)
            {
                StoreEvent(@event);
            }
            return @event;
        }

        private UnityNativeEvent BuildEventWithAppFields(UnityNativeEventType eventType, Dictionary<string, object> eventDetails, bool storeEvent = true)
        {
            if (!UnityNativeSessionManager.Instance.CurrentSession.isAppLaunched)
            {
                return null;
            }

            var eventData = new UnityNativeEventBuilder().BuildEventWithAppFields(eventType, eventDetails);
            var eventDataJSONContent = Json.Serialize(eventData);
            var @event = new UnityNativeEvent(eventType, eventDataJSONContent);
            if (storeEvent)
            {
                StoreEvent(@event);
            }
            return @event;
        }

        private async void PushEvent(UnityNativeEvent evt,Action<bool> Success)
        {
            var deviceInfo = UnityNativeDeviceManager.Instance.DeviceInfo;
            var accountInfo = UnityNativeAccountManager.Instance.AccountInfo;
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            var queryParameters = new List<KeyValuePair<string, string>> {
                        new KeyValuePair<string, string>(UnityNativeConstants.Network.QUERY_OS, deviceInfo.OsName),
                        new KeyValuePair<string, string>(UnityNativeConstants.Network.QUERY_SKD_REVISION, UnityNativeConstants.SDK.REVISION),
                        new KeyValuePair<string, string>(UnityNativeConstants.Network.QUERY_ACCOUNT_ID, accountInfo.AccountId),
                        new KeyValuePair<string, string>(UnityNativeConstants.Network.QUERY_CURRENT_TIMESTAMP, timestamp)
                    };

            var request = new UnityNativeRequest(UnityNativeConstants.Network.REQUEST_PATH_USER_VARIABLES, UnityNativeConstants.Network.REQUEST_POST)
            .SetRequestBody(evt.JsonContent)
            .SetQueryParameters(queryParameters);

            var response = await UnityNativeNetworkEngine.Instance.ExecuteRequest(request);
            if (response.StatusCode >= HttpStatusCode.OK && response.StatusCode <= HttpStatusCode.Accepted)
            {
                Success?.Invoke(true);
            }
            else
            {
                Success?.Invoke(false);
            }
        }

        private void StoreEvent(UnityNativeEvent evt) {
            _databaseStore.AddEvent(evt);
        }

        private void ProcessStoredEvents()
        {
            _databaseStore.AddEventsFromDB();
        }

        #endregion
    }
}
#endif