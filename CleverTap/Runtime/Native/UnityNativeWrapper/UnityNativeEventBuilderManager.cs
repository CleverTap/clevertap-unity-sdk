#if !UNITY_IOS && !UNITY_ANDROID
using CleverTapSDK.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CleverTapSDK.Native {
    internal class UnityNativeEventManager {
        private readonly UnityNativeSessionManager _sessionManager;
        private readonly UnityNativeRecordEventBuilder _recordEventBuilder;
        private readonly UnityNativeProfileEventBuilder _profileEventBuilder;

        internal UnityNativeEventManager(UnityNativeSessionManager _sessionManager) {
            _recordEventBuilder = new UnityNativeRecordEventBuilder(_sessionManager);
            _profileEventBuilder = new UnityNativeProfileEventBuilder(_sessionManager);
        }

        #region Profile Events

        internal UnityNativeEvent OnUserLogin(Dictionary<string, object> properties) {
            return null;
        }

        internal UnityNativeEvent ProfilePush(Dictionary<string, object> properties) {
            return null;
        }

        #endregion

        #region Record Events

        internal UnityNativeEvent RecordAppLaunch() {
            if (_sessionManager.IsAppLaunched()) {
                // App already lanuched
                return null;
            }

            _sessionManager.SetIsAppLaunched(true);


            return RecordEvent(UnityNativeConstants.Event.EVENT_APP_LUNACH, GenerateApplicationFields());
        }

        internal UnityNativeEvent RecordEvent(string eventName, Dictionary<string, object> properties = null) {
            var eventDetails = _recordEventBuilder.BuildEvent(eventName, properties);
            return BuildEvent(UnityNativeEventType.RecordEvent, eventDetails);

            
        }

        internal UnityNativeEvent RecordChargedEventWithDetailsAndItems(Dictionary<string, object> details, List<Dictionary<string, object>> items) {
            return null;
        }

        #endregion

        private UnityNativeEvent BuildEvent(UnityNativeEventType eventType, Dictionary<string, object> eventData) {

            var isMuted = false; // TODO
            if (isMuted)
                return null;

            var accountId = "";
            var accountToken = "";

            if (string.IsNullOrEmpty(accountId) || string.IsNullOrEmpty(accountToken)) {
                // Log?
                return null;
            }

            var data = new Dictionary<string, object>(eventData);

            string type = null;
            switch (eventType) {
                case UnityNativeEventType.ProfileEvent:
                    type = "profile";
                    break;
                case UnityNativeEventType.RecordEvent:
                    type = "event";
                    // pai
                    string bundleIdentifier = null; // TODO: Check if we need this
                    if (!string.IsNullOrEmpty(bundleIdentifier)) {
                        data.Add(UnityNativeConstants.Event.BUNDLE_IDENTIFIER, bundleIdentifier);
                    }
                    break;
                default:
                    // NOT Supported YET
                    throw new NotImplementedException();
            }

            data.Add(UnityNativeConstants.Event.EVENT_TYPE, type);
            data.Add(UnityNativeConstants.Event.UNIX_EPOCH_TIME, DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            data.Add(UnityNativeConstants.Event.SESSION, _sessionManager.GetSessionId());
            data.Add(UnityNativeConstants.Event.SCREEN_COUNT, _sessionManager.GetScreenCount());
            data.Add(UnityNativeConstants.Event.LAST_SESSION_LENGTH_SECONDS, _sessionManager.GetLastSessionLength());
            data.Add(UnityNativeConstants.Event.IS_FIRST_SESSION, _sessionManager.IsFirstSession());
            // Check if this is needed
            data.Add("n", "_bg");

            var jsonContent = Json.Serialize(data);
            return new UnityNativeEvent(eventType, jsonContent);
        }

        private Dictionary<string, object> GenerateApplicationFields() {
            var data = new Dictionary<string, object>();

            data.Add(UnityNativeConstants.Event.APP_VERSION, "AppVersion"); // maybe we don't need this
            data.Add(UnityNativeConstants.Event.BUILD, "Build"); // Check if we need this
            data.Add(UnityNativeConstants.Event.SDK_VERSION, CleverTap.VERSION); // Move this to UnityNative

            string model = SystemInfo.deviceModel; // Check if we need this
            if (!string.IsNullOrEmpty(model)) {
                data.Add(UnityNativeConstants.Event.MODEL, model);
            }

            // Check if we need Location info
            if (false) {
                data.Add(UnityNativeConstants.Event.LATITUDE, null);
                data.Add(UnityNativeConstants.Event.LONGITUDE, null);
            }

            // Check if we need this and is it correct
            string manufacurer = SystemInfo.deviceModel;
            if (!string.IsNullOrEmpty(manufacurer)) {
                data.Add(UnityNativeConstants.Event.MANUFACTURER, manufacurer);
            }

            data.Add(UnityNativeConstants.Event.OS_VERSION, SystemInfo.operatingSystem); // Maybe need to split this

            string carrier = null; // Check if we need this
            if (!string.IsNullOrEmpty(carrier)) {
                data.Add(UnityNativeConstants.Event.CARRIER, carrier);
            }

            var useIp = false;
            data.Add(UnityNativeConstants.Event.USE_IP, useIp); // Check this
            if (useIp) {
                // Check network Type
                //data.Add(UnityNativeConstants.Event.NETWORK_TYPE, "")
                // Check if connected to WIFI
                //data.Add(UnityNativeConstants.Event.CONNECTED_TO_WIFI, false);
            }


            var runningInsideAppExtension = false; // check this
            if (runningInsideAppExtension) {
                data.Add(UnityNativeConstants.Event.RUNNING_INSIDE_APP_EXTENSION, 1);
            }

            data.Add(UnityNativeConstants.Event.OS_NAME, SystemInfo.operatingSystem); // Maybe need to split this
            data.Add(UnityNativeConstants.Event.SCREEN_WIDTH, Screen.width); // check if we need this
            data.Add(UnityNativeConstants.Event.SCREEN_HEIGHT, Screen.height); // check if we need this



            string countryCode = null; // check this
            if (!string.IsNullOrEmpty(countryCode)) {
                data.Add(UnityNativeConstants.Event.COUNTRY_CODE, countryCode);
            }

            // locale
            //data.Add(UnityNativeConstants.Event.LOCALE_IDENTIFIER, "");
            // sslpin
            //data.Add(UnityNativeConstants.Event.SSL_PINNING, false);

            string proxyDomain = null;
            if (!string.IsNullOrEmpty(proxyDomain)) {
                data.Add(UnityNativeConstants.Event.PROXY_DOMAIN, proxyDomain);
            }

            string spikyProxyDomain = null;
            if (!string.IsNullOrEmpty(spikyProxyDomain)) {
                data.Add(UnityNativeConstants.Event.SPIKY_PROXY_DOMAIN, spikyProxyDomain);
            }

            bool wv_init = false;
            if (wv_init) {
                data.Add(UnityNativeConstants.Event.SPIKY_PROXY_DOMAIN, true);
            }

            return data;
        }
    }
}
#endif