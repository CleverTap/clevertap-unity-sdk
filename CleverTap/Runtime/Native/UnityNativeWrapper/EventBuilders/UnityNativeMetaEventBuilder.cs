#if !UNITY_IOS && !UNITY_ANDROID
using CleverTapSDK.Utilities;
using System.Collections.Generic;

namespace CleverTapSDK.Native {
    internal class UnityNativeMetaEventBuilder {
        private readonly UnityNativeSessionManager _sessionManager;
        private readonly UnityNativeEventValidator _eventValidator;

        internal UnityNativeMetaEventBuilder(UnityNativeSessionManager sessionManager) {
            sessionManager = _sessionManager;
            _eventValidator = new UnityNativeEventValidator();
        }

        internal string GenerateMetaJson() {
            var metaDict = new Dictionary<string, object>();

            metaDict.Add("g", "__d5e0cae60ea14978b908d717c8aa82bf"); // GUID
            metaDict.Add("type", "meta"); // Event type

            var metaAfDict = new Dictionary<string, object> {
                { "Build", "2412" }, // App build number
                { "Version", "2.4.1.2" }, // App Version
                { "OS Version", "9" }, // OS Version
                { "SDK Version", "30800" }, // SDK Version
                { "Latitude", "19.14182727" }, // Latitude
                { "Longitude", "72.84862182" }, // Longitude
                { "Make", "OnePlus" }, // Device Make
                { "Model", "ONEPLUS A3003" }, // Device Model
                { "Carrier", "" }, // Network Carrier
                { "useIP", "false" }, // Denotes using of IP address to track location
                { "OS", "Android" }, // Type of OS
                { "wdt", 2.68 }, // Width of the screen in inches
                { "hgt", 4.8 }, // Height of the screen in inches
                { "dpi", 380 }, // Density Pixels per Inch of the Device
                { "dt", 1 } // Device Type (1 = mobile, 2 = tablet, 3 = TV, 0 = unknown)
            };
            metaDict.Add("af", metaAfDict);

            metaDict.Add("id", "R59-565-965Z"); // Account ID
            metaDict.Add("tk", "565-c52"); // Account Token

            metaDict.Add("l_ts", 0); // Last Request Timestamp
            metaDict.Add("f_ts", 0); // First Request Timestamp
            metaDict.Add("ddnf_tsd", false); // Device-level DND
            metaDict.Add("rtl", new object[0]); // Rendered Target List - List of campaigns rendered
            metaDict.Add("rct", 0); // Unkown
            metaDict.Add("ait", 0); // Unkown
            metaDict.Add("frs", true); // If First Request in Session
            metaDict.Add("debug", true); // If Debug level set to 3
            metaDict.Add("imp", 0); // Count of InApps shown today
            metaDict.Add("tlc", new object[0]); // An array of Target ID, todayCount and lifetime count

            metaDict.Add("arp", new { }); // We should get this ARP after AppLaunched

            return Json.Serialize(metaDict);

            // Send this once
            //"arp": {
            //    "sv": 40200,
            //    "dh": 725268534,
            //    "wdt": 2,
            //    "d_ts": 1632133806,
            //    "hgt": 4,
            //    "rc_n": 5,
            //    "av": "4.2.0.3-prod-spa_web",
            //    "v": 1,
            //    "e_ts": 0,
            //    "j_n": "Zw==",
            //    "i_n": "ZmhgfgUCBQ==",
            //    "r_ts": 1632133807,
            //    "id": "ZWW-WWW-WWRZ",
            //    "rc_w": 60,
            //    "j_s": "{ }"
            //},


            //{
            //    "g": "__d5e0cae60ea14978b908d717c8aa82bf",
            //    "type": "meta",
            //    "af": {
            //      "Build": "2412",
            //      "Version": "2.4.1.2",
            //      "OS Version": "9",
            //      "SDK Version": 30800,
            //      "Make": "OnePlus",
            //      "Model": "ONEPLUS A3003",
            //      "Carrier": "",
            //      "useIP": false,
            //      "OS": "Android",
            //      "wdt": 2.68,
            //      "hgt": 4.8,
            //      "dpi": 380,
            //      "dt" : 1
            //    },
            //    "id": "R59-565-965Z",
            //    "tk": "565-c52",
            //    "l_ts": 0,
            //    "f_ts": 0,
            //    "ddnd": false,
            //    "rtl": [],
            //    "rct": 0,
            //    "ait": 0,
            //    "frs": true,
            //    "imp": 0,
            //    "tlc": []
            //}
        }
    }
}
#endif