#if !UNITY_IOS && !UNITY_ANDROID
using CleverTapSDK.Common;
using CleverTapSDK.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CleverTapSDK.Native {
    internal class UnityNativeEventManager {
        private static readonly string NATIVE_EVENTS_DB_CACHE = "NativeEventsDbCache";
        private static readonly int DEFER_EVENT_SECONDS = 2;

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
            UnityNativeSessionManager.Instance.InitializeSession();
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

            UnityNativeEvent @event = BuildEventWithAppFields(UnityNativeEventType.RaisedEvent, eventDetails, false);
            StoreEvent(@event);
            _eventQueueManager.FlushQueues();
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

            if (ShouldDeferEvent(() =>
            {
                OnUserLogin(profile);
            }))
            {
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

                // Flush the queues
                _eventQueueManager.FlushQueues();

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

            if (ShouldDeferEvent(() =>
            {
                ProfilePush(properties);
            }))
            {
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
            if (ShouldDeferEvent(() =>
            {
                RecordEvent(eventName, properties);
            }))
            {
                return null;
            }

            var eventBuilderResult = new UnityNativeRaisedEventBuilder().Build(eventName, properties);
            var eventDetails = eventBuilderResult.EventResult;
            return BuildEvent(UnityNativeEventType.RaisedEvent, eventDetails);
        }

        internal UnityNativeEvent RecordChargedEventWithDetailsAndItems(Dictionary<string, object> details, List<Dictionary<string, object>> items) {
            if (ShouldDeferEvent(() =>
            {
                RecordChargedEventWithDetailsAndItems(details, items);
            }))
            {
                return null;
            }

            var eventBuilderResult = new UnityNativeRaisedEventBuilder().BuildChargedEvent(details, items);
            var eventDetails = eventBuilderResult.EventResult;
            return BuildEvent(UnityNativeEventType.RaisedEvent, eventDetails);
        }

        #endregion

        #region Private

        private bool ShouldDeferEvent(Action action) {
            if (!UnityNativeSessionManager.Instance.CurrentSession.IsAppLaunched)
            {
                CleverTapLogger.Log($"App Launched not yet processed, re-queuing event after {DEFER_EVENT_SECONDS}s.");
                MonoHelper.Instance.StartCoroutine(DeferEventCoroutine(action));
                return true;
            }
            return false;
        }

        private IEnumerator DeferEventCoroutine(Action action) {
            yield return new WaitForSeconds(DEFER_EVENT_SECONDS);
            action();
        }

        private UnityNativeEvent BuildEvent(UnityNativeEventType eventType, Dictionary<string, object> eventDetails, bool storeEvent = true) {
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
            var eventData = new UnityNativeEventBuilder().BuildEventWithAppFields(eventType, eventDetails);
            var eventDataJSONContent = Json.Serialize(eventData);
            var @event = new UnityNativeEvent(eventType, eventDataJSONContent);
            if (storeEvent)
            {
                StoreEvent(@event);
            }
            return @event;
        }

        private void StoreEvent(UnityNativeEvent evt) {
            UnityNativeSessionManager.Instance.UpdateSessionTimestamp();
            _databaseStore.AddEvent(evt);
        }

        #endregion
    }
}
#endif