#if !UNITY_IOS && !UNITY_ANDROID
using System;
using System.Collections.Generic;
using System.Linq;

namespace CleverTapSDK.Native {
    internal static class UnityNativeConstants {
        internal static class SDK {
            internal const string VERSION = "3.0.0";
            internal const string REVISION = "30000";
        }
        internal static class Profile {
            internal const string NAME = "userName";
            internal const string EMAIL = "userEmail";
            internal const string EDUCATION = "userEducation";
            internal const string MARRIED = "userMarried";
            internal const string DATE_OF_BIRTH = "userDOB";
            internal const string BIRTHDAY = "userBirthday";
            internal const string EMPLOYED = "userEmployed";
            internal const string GENDER = "userGender";
            internal const string PHONE = "userPhone";
            internal const string AGE = "userAge";

            internal static bool IsKeyKnownProfileField(string key) {
                if (string.IsNullOrWhiteSpace(key)) {
                    return false;
                }

                return GetKnownProfileFieldForKey(key) != null;
            }

            internal static string GetKnownProfileFieldForKey(string key) {
                if (string.IsNullOrEmpty(key)) {
                    return null;
                }

                var keyLwc = key.Trim().ToLower();
                if (keyLwc == NAME.ToLower() || keyLwc == "name") {
                    return NAME;
                }
                if (keyLwc == EMAIL.ToLower() || keyLwc == "email") {
                    return EMAIL;
                }
                if (keyLwc == EDUCATION.ToLower() || keyLwc == "education") {
                    return EDUCATION;
                }
                if (keyLwc == DATE_OF_BIRTH.ToLower() || keyLwc == "dob") {
                    return DATE_OF_BIRTH;
                }
                if (keyLwc == BIRTHDAY.ToLower() || keyLwc == "birthday") {
                    return BIRTHDAY;
                }
                if (keyLwc == EMPLOYED.ToLower() || keyLwc == "employed") {
                    return EMPLOYED;
                }
                if (keyLwc == GENDER.ToLower() || keyLwc == "gender") {
                    return GENDER;
                }
                if (keyLwc == PHONE.ToLower() || keyLwc == "phone") {
                    return PHONE;
                }
                if (keyLwc == AGE.ToLower() || keyLwc == "age") {
                    return AGE;
                }

                return null;
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
            internal const string EVENT_TYPE_PROFILE = "profile";
            internal const string EVENT_TYPE_EVENT = "event";
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
        internal static class EventMeta {
            internal const string TYPE = "type";
            internal const string TYPE_NAME = "meta";
            internal const string APPLICATION_FIELDS = "af";
            internal const string GUID = "g";
            internal const string ACCOUNT_ID = "id";
            internal const string ACCOUNT_TOKEN = "tk";
            internal const string STORED_DEVICE_TOKEN = "ddnd";
            internal const string FIRST_REQUEST_IN_SESSION = "frs";
            internal const string FIRST_REQUEST_TIMESTAMP = "f_ts";
            internal const string LAST_REQUEST_TIMESTAMP = "l_ts";
            internal const string DEBUG_LEVEL = "debug";
            
            internal const string SOURCE = "us";
            internal const string MEDIUM = "um";
            internal const string CAMPAIGN = "campaign";
            internal const string REF = "ref";
            internal const string WZRK_REF = "wzrk_ref";
        }
        internal static class Validator {
            internal const int MAX_KEY_CHARS = 120;
            internal const int MAX_VALUE_CHARS = 1024;
            internal const int MAX_VALUE_PROPERTY_ARRAY_COUNT = 100;

            internal static readonly IReadOnlyList<string> KEY_NOT_ALLOWED_CHARS = new List<string> { ".", ":", "$", "'", "\"", "\\" };
            internal static readonly IReadOnlyList<string> VALUE_NOT_ALLOWED_CHARS = new List<string> { "'", "\"", "\\" };

            internal static readonly IReadOnlyList<string> RESTRICTED_NAMES = new List<string>() {
                "Notification Sent", "Notification Viewed", "Notification Clicked",
                "UTM Visited", "App Launched", "Stayed", "App Uninstalled",
                "wzrk_d", "wzrk_fetch", "SCCampaignOptOut", "Geocluster Entered", "Geocluster Exited"
            };

            internal static bool IsRestrictedName(string name) {
                if (string.IsNullOrEmpty(name)) {
                    return false;
                }

                return RESTRICTED_NAMES.Select(rn => rn.ToLower()).Any(rn => rn == name.ToLower());
            }
        }
        internal static class Network {
            internal const string CT_BASE_URL = "https://clevertap-prod.com";
            internal const string CT_TEMP_URL = "https://sk1-clevertap-prod.com"; // remove this
            internal const string HEADER_ACCOUNT_ID_NAME = "X-CleverTap-Account-Id";
            internal const string HEADER_ACCOUNT_TOKEN_NAME = "X-CleverTap-Token";
            internal const string HEADER_DOMAIN_NAME = "X-WZRK-RD";
            internal const string HEADER_DOMAIN_MUTE = "X-WZRK-MUTE";

            internal const string QUERY_OS = "os";
            internal const string QUERY_SKD_REVISION = "os";
            internal const string QUERY_ACCOUNT_ID = "z";
            internal const string QUERY_CURRENT_TIMESTAMP = "ts";

            internal const string REQUEST_POST = "POST";
            internal const string REQUEST_GET = "GET";
            internal const string REQUEST_PATH_RECORD = "a1";
            internal const string REQUEST_PATH_USER_VARIABLES = "defineVars";
            internal const string REQUEST_PATH_HAND_SHAKE = "hello";
        }
    }
}
#endif