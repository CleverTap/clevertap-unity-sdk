using UnityEngine;
using System;
using CleverTap.Utilities;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CleverTapTest.NewBindings
{
    public class CleverTapCallback : MonoBehaviour
    {
        public String CLEVERTAP_ACCOUNT_ID = "YOUR_CLEVERTAP_ACCOUNT_ID";
        public String CLEVERTAP_ACCOUNT_TOKEN = "YOUR_CLEVERTAP_ACCOUNT_TOKEN";
        public String CLEVERTAP_ACCOUNT_REGION = "";
        public int CLEVERTAP_DEBUG_LEVEL = 0;
        public bool CLEVERTAP_ENABLE_PERSONALIZATION = true;
        public bool CLEVERTAP_DISABLE_IDFV;

        public delegate void CleverTapDeepLinkCallbackDelegate(string url);
        public static event CleverTapDeepLinkCallbackDelegate OnCleverTapDeepLinkCallback;

        public delegate void CleverTapProfileInitializedCallbackDelegate(string message);
        public static event CleverTapProfileInitializedCallbackDelegate OnCleverTapProfileInitializedCallback;

        public delegate void CleverTapProfileUpdatesCallbackDelegate(string message);
        public static event CleverTapProfileUpdatesCallbackDelegate OnCleverTapProfileUpdatesCallback;

        public delegate void CleverTapPushOpenedCallbackDelegate(string message);
        public static event CleverTapPushOpenedCallbackDelegate OnCleverTapPushOpenedCallback;

        public delegate void CleverTapInitCleverTapIdCallbackDelegate(string message);
        public static event CleverTapInitCleverTapIdCallbackDelegate OnCleverTapInitCleverTapIdCallback;

        public delegate void CleverTapInAppNotificationDismissedCallbackDelegate(string message);
        public static event CleverTapInAppNotificationDismissedCallbackDelegate OnCleverTapInAppNotificationDismissedCallback;

        public delegate void CleverTapInAppNotificationShowCallbackDelegate(string message);
        public static event CleverTapInAppNotificationShowCallbackDelegate OnCleverTapInAppNotificationShowCallback;

        public delegate void CleverTapOnPushPermissionResponseCallbackDelegate(string message);
        public static event CleverTapOnPushPermissionResponseCallbackDelegate OnCleverTapOnPushPermissionResponseCallback;

        public delegate void CleverTapInAppNotificationButtonTappedDelegate(string message);
        public static event CleverTapInAppNotificationButtonTappedDelegate OnCleverTapInAppNotificationButtonTapped;

        public delegate void CleverTapInboxDidInitializeCallbackDelegate();
        public static event CleverTapInboxDidInitializeCallbackDelegate OnCleverTapInboxDidInitializeCallback;

        public delegate void CleverTapInboxMessagesDidUpdateCallbackDelegate();
        public static event CleverTapInboxMessagesDidUpdateCallbackDelegate OnCleverTapInboxMessagesDidUpdateCallback;

        public delegate void CleverTapInboxCustomExtrasButtonSelectDelegate(string message);
        public static event CleverTapInboxCustomExtrasButtonSelectDelegate OnCleverTapInboxCustomExtrasButtonSelect;

        public delegate void CleverTapInboxItemClickedDelegate(string message);
        public static event CleverTapInboxItemClickedDelegate OnCleverTapInboxItemClicked;

        public delegate void CleverTapNativeDisplayUnitsUpdatedDelegate(string message);
        public static event CleverTapNativeDisplayUnitsUpdatedDelegate OnCleverTapNativeDisplayUnitsUpdated;

        public delegate void CleverTapProductConfigFetchedDelegate(string message);
        public static event CleverTapProductConfigFetchedDelegate OnCleverTapProductConfigFetched;

        public delegate void CleverTapProductConfigActivatedDelegate(string message);
        public static event CleverTapProductConfigActivatedDelegate OnCleverTapProductConfigActivated;

        public delegate void CleverTapProductConfigInitializedDelegate(string message);
        public static event CleverTapProductConfigInitializedDelegate OnCleverTapProductConfigInitialized;

        public delegate void CleverTapFeatureFlagsUpdatedDelegate(string message);
        public static event CleverTapFeatureFlagsUpdatedDelegate OnCleverTapFeatureFlagsUpdated;

        void CleverTapDeepLinkCallback(string url)
        {
            OnCleverTapDeepLinkCallback(url);
            Debug.Log("unity received deep link: " + (!String.IsNullOrEmpty(url) ? url : "NULL"));
        }

        // called when then the CleverTap user profile is initialized
        // returns {"CleverTapID":<CleverTap unique user id>}
        void CleverTapProfileInitializedCallback(string message)
        {
            OnCleverTapProfileInitializedCallback(message);
            Debug.Log("unity received profile initialized: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));

            if (String.IsNullOrEmpty(message))
            {
                return;
            }

            try
            {
                JSONClass json = (JSONClass)JSON.Parse(message);
                Debug.Log(String.Format("unity parsed profile initialized {0}", json));
            }
            catch
            {
                Debug.LogError("unable to parse json");
            }
        }

        // called when the user profile is updated as a result of a server sync
        /**
            returns dict in the form:
            {
                "profile":{"<property1>":{"oldValue":<value>, "newValue":<value>}, ...},
                "events:{"<eventName>":
                            {"count":
                                {"oldValue":(int)<old count>, "newValue":<new count>},
                            "firstTime":
                                {"oldValue":(double)<old first time event occurred>, "newValue":<new first time event occurred>},
                            "lastTime":
                                {"oldValue":(double)<old last time event occurred>, "newValue":<new last time event occurred>},
                        }, ...
                    }
            }
        */
        void CleverTapProfileUpdatesCallback(string message)
        {
            OnCleverTapProfileUpdatesCallback(message);

            Debug.Log("unity received profile updates: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));

            if (String.IsNullOrEmpty(message))
            {
                return;
            }

            try
            {
                JSONClass json = (JSONClass)JSON.Parse(message);
                Debug.Log(String.Format("unity parsed profile updates {0}", json));
            }
            catch
            {
                Debug.LogError("unable to parse json");
            }
        }

        // returns the data associated with the push notification
        void CleverTapPushOpenedCallback(string message)
        {
            OnCleverTapPushOpenedCallback(message);
            Debug.Log("unity received push opened: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));

            if (String.IsNullOrEmpty(message))
            {
                return;
            }

            try
            {
                JSONClass json = (JSONClass)JSON.Parse(message);
                Debug.Log(String.Format("push notification data is {0}", json));
            }
            catch
            {
                Debug.LogError("unable to parse json");
            }
        }

        // returns a unique CleverTap identifier suitable for use with install attribution providers.
        void CleverTapInitCleverTapIdCallback(string message)
        {
            OnCleverTapInitCleverTapIdCallback(message);
            Debug.Log("unity received clevertap id: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
        }

        // returns the custom data associated with an in-app notification click
        void CleverTapInAppNotificationDismissedCallback(string message)
        {
            OnCleverTapInAppNotificationDismissedCallback(message);
            Debug.Log("unity received inapp notification dismissed: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
        }

        // returns the custom data associated with an in-app notification click
        void CleverTapInAppNotificationShowCallback(string message)
        {
            OnCleverTapInAppNotificationShowCallback(message);
            Debug.Log("unity received inapp notification onShow(): " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
        }

        // returns the status of push permission response after it's granted/denied
        void CleverTapOnPushPermissionResponseCallback(string message)
        {
            OnCleverTapOnPushPermissionResponseCallback(message);
            //Ensure to create call the `CreateNotificationChannel` once notification permission is granted to register for receiving push notifications for Android 13+ devices.
            Debug.Log("unity received push permission response: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
        }

        // returns when an in-app notification is dismissed by a call to action with custom extras
        void CleverTapInAppNotificationButtonTapped(string message)
        {
            OnCleverTapInAppNotificationButtonTapped(message);
            Debug.Log("unity received inapp notification button tapped: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
        }

        // returns callback for InitializeInbox
        void CleverTapInboxDidInitializeCallback()
        {
            OnCleverTapInboxDidInitializeCallback();
            Debug.Log("unity received inbox initialized");
        }

        void CleverTapInboxMessagesDidUpdateCallback()
        {
            OnCleverTapInboxMessagesDidUpdateCallback();
            Debug.Log("unity received inbox messages updated");
        }

        // returns on the click of app inbox message with a map of custom Key-Value pairs
        void CleverTapInboxCustomExtrasButtonSelect(string message)
        {
            OnCleverTapInboxCustomExtrasButtonSelect(message);
            Debug.Log("unity received inbox message button with custom extras select: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
        }

        // returns on the click of app inbox message with a string of the inbox payload along with page index and button index
        void CleverTapInboxItemClicked(string message)
        {
            OnCleverTapInboxItemClicked(message);
            Debug.Log("unity received inbox message clicked callback: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
        }

        // returns native display units data
        void CleverTapNativeDisplayUnitsUpdated(string message)
        {
            OnCleverTapNativeDisplayUnitsUpdated(message);
            Debug.Log("unity received native display units updated: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
        }

        // invoked when Product Experiences - Product Config are fetched 
        void CleverTapProductConfigFetched(string message)
        {
            OnCleverTapProductConfigFetched(message);
            Debug.Log("unity received product config fetched: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
        }

        // invoked when Product Experiences - Product Config are activated
        void CleverTapProductConfigActivated(string message)
        {
            OnCleverTapProductConfigActivated(message);
            Debug.Log("unity received product config activated: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
        }

        // invoked when Product Experiences - Product Config are initialized
        void CleverTapProductConfigInitialized(string message)
        {
            OnCleverTapProductConfigInitialized(message);
            Debug.Log("unity received product config initialized: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
        }

        // invoked when Product Experiences - Feature Flags are updated 
        void CleverTapFeatureFlagsUpdated(string message)
        {
            OnCleverTapFeatureFlagsUpdated(message);
            Debug.Log("unity received feature flags updated: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
        }

    #if UNITY_EDITOR
    private void OnValidate() {
        EditorPrefs.SetBool("CLEVERTAP_DISABLE_IDFV", CLEVERTAP_DISABLE_IDFV);
    }
    #endif
    }
}
