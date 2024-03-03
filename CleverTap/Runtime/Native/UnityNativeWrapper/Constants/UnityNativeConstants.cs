#if !UNITY_IOS && !UNITY_ANDROID
using System.Collections.Generic;

namespace CleverTapSDK.Native {
    internal static class UnityNativeConstants {
        internal static class Profile {
            internal const string NAME = "Name";
            internal const string EMAIL = "Email";
            internal const string EDUCATION = "Education";
            internal const string MARRIED = "Married";
            internal const string DATE_OF_BIRTH = "DOB";
            internal const string BIRTHDAY = "Birthday";
            internal const string EMPLOYED = "Employed";
            internal const string GENDER = "Gender";
            internal const string PHONE = "Phone";
            internal const string AGE = "Age";

            internal static bool IsKeyKnownProfileField(string key) {
                if (string.IsNullOrWhiteSpace(key)) {
                    return false;
                }

                var knownProfileFields = new List<string>() { 
                    NAME.ToLower(), 
                    EMAIL.ToLower(), 
                    EDUCATION.ToLower(), 
                    MARRIED.ToLower(), 
                    DATE_OF_BIRTH.ToLower(), 
                    BIRTHDAY.ToLower(), 
                    EMPLOYED.ToLower(), 
                    GENDER.ToLower(), 
                    PHONE.ToLower(), 
                    AGE.ToLower() 
                };

                return knownProfileFields.Contains(key.Trim().ToLower());
            }
        }
        internal static class Event
        {
            internal const string EVENT_NAME = "evtName";
            internal const string EVENT_DATA = "evtData";

            internal const string EVENT_APP_LUNACH = "App Launched";
            internal const string EVENT_CHARGED = "Charged";
            internal const string EVENT_CHARGED_ITEMS = "Items";

            internal const string EVENT_TYPE = "type";
            internal const string UNIX_EPOCH_TIME = "ep";
            internal const string SESSION = "s";
            internal const string SCREEN_COUNT = "pg";
            internal const string LAST_SESSION_LENGTH_SECONDS = "lsl";
            internal const string IS_FIRST_SESSION = "f";

            internal const string GEO_FENCE_LOCATION = "gf";
            internal const string GEO_FENCE_SDK_VERSION = "gfSDKVersion";

            internal const string BUNDLE_IDENTIFIER = "pai";

            internal const string APP_VERSION = "Version";
            internal const string BUILD = "Build";
            internal const string SDK_VERSION = "SDK Version";
            internal const string OS_NAME = "OS";
            internal const string OS_VERSION = "OS Version";
            internal const string MODEL = "Model";
            internal const string MANUFACTURER = "Make";

            internal const string CARRIER = "Carrier";
            internal const string USE_IP = "useIP";
            internal const string NETWORK_TYPE = "Radio";
            internal const string CONNECTED_TO_WIFI = "wifi";
            internal const string SSL_PINNING = "sslpin";
            internal const string PROXY_DOMAIN = "proxyDomain";
            internal const string SPIKY_PROXY_DOMAIN = "spikyProxyDomain";

            internal const string SCREEN_WIDTH = "wdt";
            internal const string SCREEN_HEIGHT = "hgt";

            internal const string LATITUDE = "Latitude";
            internal const string LONGITUDE = "Longitude";
            internal const string COUNTRY_CODE = "cc";

            internal const string LIBRARY = "lib";

            internal const string RUNNING_INSIDE_APP_EXTENSION = "appex";
            internal const string LOCALE_IDENTIFIER = "locale";
            internal const string WV_INIT = "wv_init";
        }
    }
}
#endif