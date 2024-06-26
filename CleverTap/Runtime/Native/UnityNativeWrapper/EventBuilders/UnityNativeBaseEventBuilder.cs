#if !UNITY_IOS && !UNITY_ANDROID
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CleverTapSDK.Native {
    internal class UnityNativeEventBuilder {

        internal UnityNativeEventBuilder() { }

        internal Dictionary<string, object> BuildEvent(UnityNativeEventType eventType, Dictionary<string, object> eventDetails) {
           
            if (UnityNativeNetworkEngine.Instance.IsMuted()) {
                return null;
            }
                
            var eventData = new Dictionary<string, object>(eventDetails);
            switch (eventType) {
                case UnityNativeEventType.ProfileEvent:
                    eventData.Add(UnityNativeConstants.Event.EVENT_TYPE, UnityNativeConstants.Event.EVENT_TYPE_PROFILE);
                    break;
                case UnityNativeEventType.RecordEvent:
                    eventData.Add(UnityNativeConstants.Event.EVENT_TYPE, UnityNativeConstants.Event.EVENT_TYPE_EVENT);
                    if (!string.IsNullOrEmpty(UnityNativeDeviceManager.Instance.DeviceInfo.BundleId)) {
                        eventData.Add(UnityNativeConstants.Event.BUNDLE_IDENTIFIER, UnityNativeDeviceManager.Instance.DeviceInfo.BundleId);
                    }   
                    break;
                default:
                    // NOT Supported YET
                    throw new NotImplementedException();
            }

            var currentSession = UnityNativeSessionManager.Instance.CurrentSession;

            eventData.Add(UnityNativeConstants.Event.UNIX_EPOCH_TIME, DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            eventData.Add(UnityNativeConstants.Event.SESSION, currentSession.SessionId);
            eventData.Add(UnityNativeConstants.Event.SCREEN_COUNT, UnityNativeSessionManager.Instance.GetScreenCount());
            eventData.Add(UnityNativeConstants.Event.LAST_SESSION_LENGTH_SECONDS, UnityNativeSessionManager.Instance.GetLastSessionLength());
            eventData.Add(UnityNativeConstants.Event.IS_FIRST_SESSION, UnityNativeSessionManager.Instance.IsFirstSession());
            // Check if this is needed
            eventData.Add("n", "_bg");

            return eventData;
        }
        internal Dictionary<string, object> BuildAppFields() {
            var deviceInfo = UnityNativeDeviceManager.Instance.DeviceInfo;

            var data = new Dictionary<string, object>();

            data.Add(UnityNativeConstants.Event.APP_VERSION, deviceInfo.AppVersion);
            data.Add(UnityNativeConstants.Event.BUILD, deviceInfo.AppBuild);
            data.Add(UnityNativeConstants.Event.SDK_VERSION, Regex.Replace(deviceInfo.SdkVersion, "[^0-9]", "0"));

            if (!string.IsNullOrEmpty(deviceInfo.Model)) {
                data.Add(UnityNativeConstants.Event.MODEL, deviceInfo.Model);
            }

            if (!string.IsNullOrEmpty(deviceInfo.Manufacturer)) {
                data.Add(UnityNativeConstants.Event.MANUFACTURER, deviceInfo.Manufacturer);
            }

            data.Add(UnityNativeConstants.Event.OS_VERSION, deviceInfo.OsVersion);

            if (!string.IsNullOrEmpty(deviceInfo.Carrier)) {
                data.Add(UnityNativeConstants.Event.CARRIER, deviceInfo.Carrier);
            }

            // Check this
            var useIp = false;
            data.Add(UnityNativeConstants.Event.USE_IP, useIp);
            if (useIp) {
                // Check network Type
                //data.Add(UnityNativeConstants.Event.NETWORK_TYPE, "")

                data.Add(UnityNativeConstants.Event.CONNECTED_TO_WIFI, deviceInfo.Wifi);
            }

            // Check this
            var runningInsideAppExtension = false;
            if (runningInsideAppExtension) {
                data.Add(UnityNativeConstants.Event.RUNNING_INSIDE_APP_EXTENSION, 1);
            }

            data.Add(UnityNativeConstants.Event.OS_NAME, deviceInfo.OsName);
            data.Add(UnityNativeConstants.Event.SCREEN_WIDTH, deviceInfo.DeviceWidth);
            data.Add(UnityNativeConstants.Event.SCREEN_HEIGHT, deviceInfo.DeviceHeight);

            if (!string.IsNullOrEmpty(deviceInfo.CountryCode)) {
                data.Add(UnityNativeConstants.Event.COUNTRY_CODE, deviceInfo.CountryCode);
            }

            if (!string.IsNullOrEmpty(deviceInfo.Locale)) {
                data.Add(UnityNativeConstants.Event.LOCALE_IDENTIFIER, deviceInfo.Locale);
            }

            // sslpin
            //data.Add(UnityNativeConstants.Event.SSL_PINNING, false);

            // Check this
            //string proxyDomain = null;
            //if (!string.IsNullOrEmpty(proxyDomain)) {
            //    data.Add(UnityNativeConstants.Event.PROXY_DOMAIN, proxyDomain);
            //}

            // Check this
            //string spikyProxyDomain = null;
            //if (!string.IsNullOrEmpty(spikyProxyDomain)) {
            //    data.Add(UnityNativeConstants.Event.SPIKY_PROXY_DOMAIN, spikyProxyDomain);
            //}

            // Check this
            bool wv_init = false;
            if (wv_init) {
                data.Add(UnityNativeConstants.Event.WV_INIT, true);
            }

            return data;
        }
        internal Dictionary<string, object> BuildEventWithAppFields(UnityNativeEventType eventType, Dictionary<string, object> eventDetails) {
            var eventData = BuildEvent(eventType, eventDetails);
            eventData.Add(UnityNativeConstants.Event.EVENT_DATA, BuildAppFields());

            return eventData;
        }
    }
}
#endif