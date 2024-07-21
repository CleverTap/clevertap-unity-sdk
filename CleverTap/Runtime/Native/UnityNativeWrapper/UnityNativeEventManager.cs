#if !UNITY_IOS && !UNITY_ANDROID
using CleverTapSDK.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CleverTapSDK.Native {
    internal class UnityNativeEventManager {
        private static readonly string NATIVE_EVENTS_DB_CACHE = "NativeEventsDbCache";

        private readonly UnityNativePreferenceManager _preferenceManager;
        private readonly UnityNativeDatabaseStore _databaseStore;
        private readonly UnityNativeEventQueueManager _eventQueueManager;
        private readonly UnityNativeDeviceInfo _deviceInfo;
        private readonly UnityNativeCallbackHandler _callbackHandler;

        internal UnityNativeEventManager(UnityNativeCallbackHandler callbackHandler) {
            _preferenceManager = new UnityNativePreferenceManager();
            _databaseStore = new UnityNativeDatabaseStore(NATIVE_EVENTS_DB_CACHE);
            _eventQueueManager = new UnityNativeEventQueueManager(_databaseStore);
            _deviceInfo = new UnityNativeDeviceInfo();
            _callbackHandler = callbackHandler;
        }

        #region Launch

        internal void LaunchWithCredentials(string accountID, string token, string region = null) {
            UnityNativeAccountManager.Instance.SetAccountInfo(accountID, token, region);
            UnityNativeNetworkEngine.Instance.SetRegion(region);
            RecordAppLaunch();
            NotifyUserProfileInitialized();
        }

        internal void RecordAppLaunch() {
            if (UnityNativeSessionManager.Instance.CurrentSession.IsAppLaunched) {
                return;
            }

            var accountInfo = UnityNativeAccountManager.Instance.AccountInfo;
            if (string.IsNullOrEmpty(accountInfo.AccountId) || string.IsNullOrEmpty(accountInfo.AccountToken)) {
                throw new ArgumentNullException("Cannot record App Launched. AccountId and/or AccountToken are not set.");
            }

            UnityNativeNetworkEngine.Instance.SetHeaders(new Dictionary<string, string>() {
                { UnityNativeConstants.Network.HEADER_ACCOUNT_ID_NAME, accountInfo.AccountId },
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

        private readonly HashSet<string> IdentityKeys = new HashSet<string>() {
            UnityNativeConstants.Profile.EMAIL.ToLower(),
            UnityNativeConstants.Profile.IDENTITY.ToLower(),
            UnityNativeConstants.Profile.PHONE.ToLower()
        };

        internal UnityNativeEvent OnUserLogin(Dictionary<string, object> profile) {
            if (profile == null || profile.Count == 0) {
                return null;
            }

            return _OnUserLogin(profile);
        }

        private UnityNativeEvent _OnUserLogin(Dictionary<string, object> profile) {
			try {
				string currentGUID = _deviceInfo.DeviceId;
				bool haveIdentifier = false;
				string cachedGUID = null;

				foreach (var key in profile.Keys) {
					if (IdentityKeys.Contains(key.ToLower())) {
                        var value = profile[key];
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

				// No Identifier or anonymous
				if (!haveIdentifier || IsAnonymousUser()) {
                    CleverTapLogger.Log($"OnUserLogin: No identifier OR device is anonymous, associating profile with current user profile: {currentGUID}");
                    return ProfilePush(profile);
				}
				// Same Profile
				if (cachedGUID != null && cachedGUID.Equals(currentGUID)) {
                    CleverTapLogger.Log($"OnUserLogin: Profile maps to current device id {currentGUID}, using current user profile.");
                    return ProfilePush(profile);
				}

                // New Profile
				SwitchOrCreateProfile(profile, cachedGUID);
			} catch (Exception e) {
				CleverTapLogger.LogError("OnUserLogin failed: " + e);
			}

			return null;
		}

		private bool IsAnonymousUser() {
            return string.IsNullOrEmpty(_preferenceManager.GetUserIdentities());
        }

        private string GetGUIDForIdentifier(string key, string identifier) {
            return _preferenceManager.GetGUIDForIdentifier(key,identifier);
        }

        private void SwitchOrCreateProfile(Dictionary<string, object> profile, string cacheGuid) {
            try
            {
                CleverTapLogger.Log($"asyncProfileSwitchUser:[profile {string.Join(Environment.NewLine, profile)}]" +
                    $" with Cached GUID {(cacheGuid != null ? cacheGuid : "NULL")}");

                // Add all events and flush queue
                ProcessStoredEvents();
                _eventQueueManager.FlushQueue();

                // Reset the session
                UnityNativeSessionManager.Instance.ResetSession();

                if (cacheGuid != null)
                {
                    _deviceInfo.ForceUpdateDeviceId(cacheGuid);
                }
                else
                {
                    _deviceInfo.ForceNewDeviceID();
                }

                NotifyUserProfileInitialized();

                RecordAppLaunch();

                if (profile != null)
                {
                    ProfilePush(profile);
                }
            }
            catch (Exception e)
            {
                CleverTapLogger.LogError("Reset Profile error: " + e);
            }
        }

        internal void NotifyUserProfileInitialized() {
            var eventInfo = new Dictionary<string, string> {
                { "CleverTapID",  _deviceInfo.DeviceId },
                { "CleverTapAccountID", UnityNativeAccountManager.Instance.AccountInfo.AccountId }
            };
            _callbackHandler.CleverTapProfileInitializedCallback(Json.Serialize(eventInfo));
        }

        internal UnityNativeEvent ProfilePush(Dictionary<string, object> properties) {
            if (properties == null || properties.Count == 0) {
                return null;
            }

            // Updating Identity 
            foreach (var key in properties.Keys)
            {
                if (IdentityKeys.Contains(key.ToLower()))
                {
                    var value = properties[key];
                    string identifier = value?.ToString();
                    if (!string.IsNullOrEmpty(identifier))
                    {
                        _preferenceManager.SetGUIDForIdentifier(_deviceInfo.DeviceId, key, identifier);
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

        internal UnityNativeEvent ProfilePush(string key, object value, string command) {
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

        private UnityNativeEvent BuildEvent(UnityNativeEventType eventType, Dictionary<string, object> eventDetails, bool storeEvent = true) {
            if (!UnityNativeSessionManager.Instance.CurrentSession.IsAppLaunched)
            {
                CleverTapLogger.LogError("Record App Launched first");
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

        private UnityNativeEvent BuildEventWithAppFields(UnityNativeEventType eventType, Dictionary<string, object> eventDetails, bool storeEvent = true) {
            if (!UnityNativeSessionManager.Instance.CurrentSession.IsAppLaunched)
            {
                CleverTapLogger.LogError("Record App Launched first");
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

        private async void PushEvent(UnityNativeEvent evt,Action<bool> Success) {
            var metaEvent = Json.Serialize(new UnityNativeMetaEventBuilder().BuildMeta());
            var allEventsJson = new List<string> { metaEvent , evt.JsonContent };
            var jsonContent = "[" + string.Join(",", allEventsJson) + "]";

            var queryParameters = UnityNativeBaseEventQueue.GetQueryStringParameters();

            var request = new UnityNativeRequest(UnityNativeConstants.Network.REQUEST_PATH_RECORD, UnityNativeConstants.Network.REQUEST_POST)
            .SetRequestBody(jsonContent)
            .SetQueryParameters(queryParameters);

            CleverTapLogger.Log($"{GetType().Name}: Executing request with body: {jsonContent} " +
    $"and query parameters: [{string.Join(", ", queryParameters.Select(kv => $"{kv.Key}: {kv.Value}"))}]");

            UnityNativeSessionManager.Instance.UpdateSessionTimestamp();

            var response = await UnityNativeNetworkEngine.Instance.ExecuteRequest(request);
            Success?.Invoke(response.IsSuccess());
        }

        private void StoreEvent(UnityNativeEvent evt) {
            UnityNativeSessionManager.Instance.UpdateSessionTimestamp();
            _databaseStore.AddEvent(evt);
        }

        private void ProcessStoredEvents() {
            _databaseStore.AddEventsFromDB();
        }

        #endregion
    }
}
#endif