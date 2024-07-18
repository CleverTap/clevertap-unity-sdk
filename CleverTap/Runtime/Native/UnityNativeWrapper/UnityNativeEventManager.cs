#if !UNITY_IOS && !UNITY_ANDROID
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace CleverTapSDK.Native
{
    internal class UnityNativeEventManager
    {
        private static readonly string NATIVE_EVENTS_DB_CACHE = "NativeEventsDbCache";
        private static readonly object processingUserLoginLock = new object();
        private string processingUserLoginIdentifier = null;

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

        internal UnityNativeEvent OnUserLogin(Dictionary<string, object> profile, string cleverTapID = null)
        {
            if (profile == null || profile.Count == 0)
            {
                return null;
            }

            return _OnUserLogin(profile, cleverTapID);
        }

        private HashSet<string> IdentityKeys = new HashSet<string>(){UnityNativeConstants.Profile.EMAIL,UnityNativeConstants.Profile.IDENTITY};
        private UnityNativeEvent _OnUserLogin(Dictionary<string, object> profile, string cleverTapID)
        {
           
            try
            {
                string currentGUID = _deviceInfo.DeviceId;
                bool haveIdentifier = false;
                string cachedGUID = null;

                foreach (var key in profile.Keys)
                {
                    var value = profile[key];
                    if (IdentityKeys.Contains(key))
                    {
                        string identifier = value?.ToString();
                        if (!string.IsNullOrEmpty(identifier))
                        {
                            haveIdentifier = true;
                            cachedGUID = GetGUIDForIdentifier(key, identifier);
                            if (cachedGUID != null)
                            {
                                break;
                            }
                        }
                    }
                }
                //new profile
                if (!haveIdentifier || IsAnonymousUser())
                {
                    return ProfilePush(profile);
                }
                //Same Profile
                if (cachedGUID != null && cachedGUID.Equals(currentGUID))
                {
                    return ProfilePush(profile);
                }
              
                SwitchOrCreateProfile(profile, cachedGUID, cleverTapID);
            }
            catch (Exception e)
            {
                Debug.LogError("onUserLogin failed: " + e);
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

        private void SwitchOrCreateProfile(Dictionary<string, object> profile, string cacheGuid, string cleverTapID)
        {
            try
            {
                Debug.Log($"asyncProfileSwitchUser:[profile {profile} with Cached GUID {(cacheGuid != null ? cacheGuid : "NULL")}");

                _eventQueueManager.FlushQueue();
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
                Debug.LogError("Reset Profile error: " + e);
            }
        }


        private UnityNativeEvent ProfilePush(Dictionary<string, object> properties)
        {
            if (properties == null || properties.Count == 0)
            {
                return null;
            }

            foreach (var key in properties.Keys)
                {
                    var value = properties[key];
                    if (IdentityKeys.Contains(key))
                    {
                        string identifier = value?.ToString();
                        if (!string.IsNullOrEmpty(identifier))
                        {
                            _preferenceManager.SetGUIDForIdentifier(_deviceInfo.DeviceId,key,identifier);
                        }
                    }
                }

            var eventBuilderResult = new UnityNativeProfileEventBuilder().BuildPushEvent(properties);
            if (eventBuilderResult.EventResult.SystemFields == null || eventBuilderResult.EventResult.CustomFields == null)
            {
                return null;
            }

            var eventDetails = new List<IDictionary<string, object>>() {
                eventBuilderResult.EventResult.SystemFields,
                eventBuilderResult.EventResult.CustomFields
            }.SelectMany(d => d).ToDictionary(d => d.Key, d => d.Value);

            var profile = (Dictionary<string, object>)eventDetails["profile"];
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

        private void RecordAppLaunch()
        {
            if (UnityNativeSessionManager.Instance.CurrentSession.isAppLaunched)
            {
                return;
            }

            var accountInfo = UnityNativeAccountManager.Instance.AccountInfo;
            if (string.IsNullOrEmpty(accountInfo.AccountId) || string.IsNullOrEmpty(accountInfo.AccountToken))
            {
                throw new ArgumentNullException("accountId || accountToken");
            }

            UnityNativeNetworkEngine.Instance.SetHeaders(new Dictionary<string, string>() {
                { UnityNativeConstants.Network.HEADER_ACCOUNT_ID_NAME, accountInfo.AccountId }
            });

            UnityNativeSessionManager.Instance.CurrentSession.SetIsAppLaunched(true);
            var eventDetails = new Dictionary<string, object> {
                { UnityNativeConstants.Event.EVENT_NAME, UnityNativeConstants.Event.EVENT_APP_LAUNCH }
            };

            var appLaunchEvent = BuildEvent(UnityNativeEventType.RecordEvent, eventDetails);
            PushEvent(appLaunchEvent, (isPushed) =>
            {
                UnityNativeSessionManager.Instance.CurrentSession.SetIsAppLaunched(isPushed);
            });
        }

        private UnityNativeEvent BuildEvent(UnityNativeEventType eventType, Dictionary<string, object> eventDetails)
        {
            if (!UnityNativeSessionManager.Instance.CurrentSession.isAppLaunched)
            {
                return null;
            }

            var eventData = new UnityNativeEventBuilder().BuildEvent(eventType, eventDetails);
            var eventDataJSONContent = JsonUtility.ToJson(eventData);
            var evt = new UnityNativeEvent(eventType, eventDataJSONContent);
            StoreEvent(evt);
            return evt;
        }

        private async void PushEvent(UnityNativeEvent evt, Action<bool> callback)
        {
            var deviceInfo = UnityNativeDeviceManager.Instance.DeviceInfo;
            var accountInfo = UnityNativeAccountManager.Instance.AccountInfo;
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();

            var metaEvent = JsonUtility.ToJson(new UnityNativeMetaEventBuilder().BuildMeta());
            var allEventsJson = new List<string> { metaEvent, evt.JsonContent };
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
            callback?.Invoke(response.IsSuccess());
        }

        private void StoreEvent(UnityNativeEvent evt)
        {
            _databaseStore.AddEvent(evt);
        }

    }
}
#endif
