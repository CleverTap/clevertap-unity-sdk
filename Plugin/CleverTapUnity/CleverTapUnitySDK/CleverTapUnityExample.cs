using System;
using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CleverTapUnitySDK {
    public class CleverTapUnityExample : MonoBehaviour {
        public String CLEVERTAP_ACCOUNT_ID = "YOUR_CLEVERTAP_ACCOUNT_ID";
        public String CLEVERTAP_ACCOUNT_TOKEN = "YOUR_CLEVERTAP_ACCOUNT_TOKEN";
        public String CLEVERTAP_ACCOUNT_REGION = "";
        public int CLEVERTAP_DEBUG_LEVEL = 0;
        public bool CLEVERTAP_ENABLE_PERSONALIZATION = true;
        public bool CLEVERTAP_DISABLE_IDFV;

        void Awake() {
            DontDestroyOnLoad(gameObject);

            CleverTap.SetDebugLevel(CLEVERTAP_DEBUG_LEVEL);

            // Subscribe and listen to event callbacks
            CleverTap.OnCleverTapProfileInitializedCallback += CleverTapProfileInitialized;
            CleverTap.OnCleverTapProfileUpdatesCallback += CleverTapProfileUpdates;
            CleverTap.OnCleverTapInboxMessagesDidUpdateCallback += CleverTapInboxMessagesDidUpdate; ;
            CleverTap.OnCleverTapInitCleverTapIdCallback += (message) => { Debug.Log(message); };

            //CleverTap.OnCleverTapDeepLinkCallback += (message) => { };
            //CleverTap.OnCleverTapPushOpenedCallback += (message) => { };
            //CleverTap.OnCleverTapInAppNotificationDismissedCallback += (message) => { };
            //CleverTap.OnCleverTapInAppNotificationShowCallback += (message) => { };
            //CleverTap.OnCleverTapOnPushPermissionResponseCallback += (message) => { };
            //CleverTap.OnCleverTapInAppNotificationButtonTapped += (message) => { };
            //CleverTap.OnCleverTapInboxDidInitializeCallback += () => { };
            //CleverTap.OnCleverTapInboxCustomExtrasButtonSelect += (message) => { };
            //CleverTap.OnCleverTapInboxItemClicked += (message) => { };
            //CleverTap.OnCleverTapNativeDisplayUnitsUpdated += (message) => { };
            //CleverTap.OnCleverTapProductConfigFetched += (message) => { };
            //CleverTap.OnCleverTapProductConfigActivated += (message) => { };
            //CleverTap.OnCleverTapProductConfigInitialized += (message) => { };
            //CleverTap.OnCleverTapFeatureFlagsUpdated += (message) => { };

            CleverTap.LaunchWithCredentialsForRegion(CLEVERTAP_ACCOUNT_ID, CLEVERTAP_ACCOUNT_TOKEN, CLEVERTAP_ACCOUNT_REGION);
            CleverTap.GetCleverTapID();

            if (CLEVERTAP_ENABLE_PERSONALIZATION) {
                CleverTap.EnablePersonalization();
            }

            // Clevertap APIs
            //CleverTap.ProfileIncrementValueForKey("add_int", 2);
            //CleverTap.ProfileIncrementValueForKey("add_double", 3.5);
            //CleverTap.ProfileDecrementValueForKey("minus_int", 2);
            //CleverTap.ProfileDecrementValueForKey("minus_double", 3.5);
            //CleverTap.SuspendInAppNotifications();
            //CleverTap.DiscardInAppNotifications();
            //CleverTap.ResumeInAppNotifications();

            // Record special charged event
            //Dictionary<string, object> chargeDetails = new Dictionary<string, object> {
            //    { "Amount", 500 },
            //    { "Currency", "USD" },
            //    { "Payment Mode", "Credit card" }
            //};

            //List<Dictionary<string, object>> items = new List<Dictionary<string, object>>
            //{
            //    new Dictionary<string, object> {
            //        { "price", 100 },
            //        { "Product category", "plants" },
            //        { "Quantity", 10 }
            //    },
            //    new Dictionary<string, object> {
            //        { "price", 50 },
            //        { "Product category", "books" },
            //        { "Quantity", 1 }
            //    }
            //};

            //CleverTap.RecordChargedEventWithDetailsAndItems(chargeDetails, items);
            //CleverTap.RecordEvent("testEventPushAmp");

            // Push Primer APIs usages

            //bool isPushPermissionGranted = CleverTap.IsPushPermissionGranted();
            //Debug.Log("isPushPermissionGranted" + isPushPermissionGranted);

            //Dictionary<string, object> item = new Dictionary<string, object> {
            //    { "inAppType", "half-interstitial" },
            //    { "titleText", "Get Notified" },
            //    { "messageText", "Please enable notifications on your device to use Push Notifications." },
            //    { "followDeviceOrientation", true },
            //    { "positiveBtnText", "Allow" },
            //    { "negativeBtnText", "Cancel" },
            //    { "backgroundColor", "#FFFFFF" },
            //    { "btnBorderColor", "#0000FF" },
            //    { "titleTextColor", "#0000FF" },
            //    { "messageTextColor", "#000000" },
            //    { "btnTextColor", "#FFFFFF" },
            //    { "btnBackgroundColor", "#0000FF" },
            //    { "imageUrl", "https://icons.iconarchive.com/icons/treetog/junior/64/camera-icon.png" },
            //    { "btnBorderRadius", "2" },
            //    { "fallbackToSettings", true }
            //};

            //CleverTap.PromptPushPrimer(item);
            //CleverTap.PromptForPushPermission(false);

            // Push Templates APIs usages
            //CleverTap.RecordEvent("Send Basic Push");
            //CleverTap.RecordEvent("Send Carousel Push");
            //CleverTap.RecordEvent("Send Manual Carousel Push");
            //CleverTap.RecordEvent("Send Filmstrip Carousel Push");
            //CleverTap.RecordEvent("Send Rating Push");
            //CleverTap.RecordEvent("Send Product Display Notification");
            //CleverTap.RecordEvent("Send Linear Product Display Push");
            //CleverTap.RecordEvent("Send CTA Notification");
            //CleverTap.RecordEvent("Send Zero Bezel Notification");
            //CleverTap.RecordEvent("Send Zero Bezel Text Only Notification");
            //CleverTap.RecordEvent("Send Timer Notification");
            //CleverTap.RecordEvent("Send Input Box Notification");
            //CleverTap.RecordEvent("Send Input Box Reply with Event Notification");
            //CleverTap.RecordEvent("Send Input Box Reply with Auto Open Notification");
            //CleverTap.RecordEvent("Send Input Box Remind Notification DOC FALSE");
            //CleverTap.RecordEvent("Send Input Box CTA DOC true");
            //CleverTap.RecordEvent("Send Input Box CTA DOC false");
            //CleverTap.RecordEvent("Send Input Box Reminder DOC true");
            //CleverTap.RecordEvent("Send Input Box Reminder DOC false");
        }

        private void CleverTapProfileInitialized(string message) {
            // Implementation
        }

        private void CleverTapProfileUpdates(string message) {
            // Implementation
        }

        private void CleverTapInboxMessagesDidUpdate() {
            // Implementation
        }

#if UNITY_EDITOR
        private void OnValidate() {
            EditorPrefs.SetBool("CLEVERTAP_DISABLE_IDFV", CLEVERTAP_DISABLE_IDFV);
        }
#endif
    }
}
