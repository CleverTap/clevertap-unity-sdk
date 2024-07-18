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
        private readonly UnityNativeDeviceInfo _deviceInfo;
       
        internal UnityNativeEventManager()
        {
            _preferenceManager = new UnityNativePreferenceManager();
            _databaseStore = new UnityNativeDatabaseStore(NATIVE_EVENTS_DB_CACHE);
            _eventQueueManager = new UnityNativeEventQueueManager(_databaseStore);
            _deviceInfo = new UnityNativeDeviceInfo();
        }

        #region Launch

        internal void LaunchWithCredentials(string accountID, string token, string region = null)
        {
            UnityNativeAccountManager.Instance.SetAccountInfo(accountID, token, region);
            UnityNativeNetworkEngine.Instance.SetRegion(region);
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
                    // { UnityNativeConstants.Network.HEADER_ACCOUNT_TOKEN_NAME, accountInfo.AccountToken }
                    });
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

        internal UnityNativeEvent OnUserLogin(Dictionary<string, object> profile) {
            if (profile == null || profile.Count == 0) {
                return null;
            }

            return _OnUserLogin(profile);
        }

        private readonly HashSet<string> IdentityKeys = new HashSet<string>(){UnityNativeConstants.Profile.EMAIL,UnityNativeConstants.Profile.IDENTITY};
		private UnityNativeEvent _OnUserLogin(Dictionary<string, object> profile) {
			try {
				string currentGUID = _deviceInfo.DeviceId;
				bool haveIdentifier = false;
				string cachedGUID = null;

				foreach (var key in profile.Keys) {
					var value = profile[key];
					if (IdentityKeys.Contains(key)) {
						string identifier = value?.ToString();
						if (!string.IsNullOrEmpty(identifier)) {
							haveIdentifier = true;
							cachedGUID = GetGUIDForIdentifier(key, identifier);
							if (cachedGUID != null) {
								break;
							}
						}
					}
				}
				//new profile
				if (!haveIdentifier || IsAnonymousUser()) {
					return ProfilePush(profile);
				}
				//Same Profile
				if (cachedGUID != null && cachedGUID.Equals(currentGUID)) {
					return ProfilePush(profile);
				}

				SwitchOrCreateProfile(profile, cachedGUID);
			} catch (Exception e) {
				CleverTapLogger.LogError("onUserLogin failed: " + e);
			}

			return null;
		}

		private bool IsAnonymousUser()
        {
            return string.IsNullOrEmpty(_preferenceManager.GetUserIdentities());
        }

        private string GetGUIDForIdentifier(string key, string identifier)
        {
            return _preferenceManager.GetGUIDForIdentifier(key,identifier);
        }

        private void SwitchOrCreateProfile(Dictionary<string, object> profile, string cacheGuid)
        {
            try
            {
                CleverTapLogger.Log($"asyncProfileSwitchUser:[profile {profile} with Cached GUID {(cacheGuid != null ? cacheGuid : "NULL")}");

                ProcessStoredEvents();
               // UnityNativeLoginInfoProvider.Instance.ClearUser();
                //UnityNativeSessionManager.Instance.ClearSession();
                //old profile switch
                if (cacheGuid != null)
                {
                    _deviceInfo.ForceUpdateDeviceId(cacheGuid);
                   // _callbackManager.NotifyUserProfileInitialized(cacheGuid);
                }
                else
                {
                    _deviceInfo.ForceNewDeviceID();
                }

                //_callbackManager.NotifyUserProfileInitialized(_deviceInfo.DeviceId);
                //_deviceInfo.SetCurrentUserOptOutStateFromStorage();
               
                RecordAppLaunch();

                if (profile != null)
                {
                    ProfilePush(profile);
                }

                //foreach (var callback in _callbackManager.GetChangeUserCallbackList())
                {
                 //   callback.OnChangeUser(_deviceInfo.DeviceId, _config.AccountId);
                }
            }
            catch (Exception e)
            {
                CleverTapLogger.LogError("Reset Profile error: " + e);
            }
        }

        internal UnityNativeEvent ProfilePush(Dictionary<string, object> properties) {
            if (properties == null || properties.Count == 0) {
                return null;
            }
            //Updating Identity 
            foreach (var key in properties.Keys)
                {
                    var value = properties[key];
                    if (IdentityKeys.Contains(key)){
                        string identifier = value?.ToString();
                        if (!string.IsNullOrEmpty(identifier)){
                            _preferenceManager.SetGUIDForIdentifier(_deviceInfo.DeviceId,key,identifier);
                        }
                    }
                }

            var eventBuilderResult = new UnityNativeProfileEventBuilder().BuildPushEvent(properties);
            if (eventBuilderResult.EventResult.SystemFields == null || eventBuilderResult.EventResult.CustomFields == null) {
                return null;
            }

            var eventDetails = new List<IDictionary<string, object>>() {
                eventBuilderResult.EventResult.SystemFields,
                eventBuilderResult.EventResult.CustomFields
            }.SelectMany(d => d).ToDictionary(d => d.Key, d => d.Value);

            Dictionary<string, object> profile = (Dictionary<string, object>)eventDetails["profile"];
            foreach (var key in properties.Keys)
            {
                if (!eventDetails.ContainsKey(key) && !profile.ContainsKey(key))
                {
                    profile.Add(key, properties[key]);
                }
            }
            eventDetails["profile"] = profile;
            return BuildEvent(UnityNativeEventType.ProfileEvent, eventDetails);
        }

        internal UnityNativeEvent ProfilePush(string key, object value, string command)
        {
            if (key == null || value == null || command == null)
            {
                return null;
            }

            var commandObj = new Dictionary<string, object>
            {
                { command, value }
            };

            var properties = new Dictionary<string, object>
            {
                { key, commandObj }
            };

            return ProfilePush(properties);
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

        private UnityNativeEvent BuildEvent(UnityNativeEventType eventType, Dictionary<string, object> eventDetails, bool storeEvent = true)
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

            var metaEvent = Json.Serialize(new UnityNativeMetaEventBuilder().BuildMeta());
            var allEventsJson = new List<string> { metaEvent , evt.JsonContent };
            var jsonContent = "[" + string.Join(",", allEventsJson) + "]";

            var queryParameters = new List<KeyValuePair<string, string>> {
                        new KeyValuePair<string, string>(UnityNativeConstants.Network.QUERY_OS, deviceInfo.OsName),
                        new KeyValuePair<string, string>(UnityNativeConstants.Network.QUERY_SKD_REVISION, UnityNativeConstants.SDK.REVISION),
                        new KeyValuePair<string, string>(UnityNativeConstants.Network.QUERY_ACCOUNT_ID, accountInfo.AccountId),
                        new KeyValuePair<string, string>(UnityNativeConstants.Network.QUERY_CURRENT_TIMESTAMP, timestamp)
                    };

            var request = new UnityNativeRequest(UnityNativeConstants.Network.REQUEST_PATH_RECORD, UnityNativeConstants.Network.REQUEST_POST)
            .SetRequestBody(jsonContent)
            .SetQueryParameters(queryParameters);

            var response = await UnityNativeNetworkEngine.Instance.ExecuteRequest(request);
            Success?.Invoke(response.IsSuccess());
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