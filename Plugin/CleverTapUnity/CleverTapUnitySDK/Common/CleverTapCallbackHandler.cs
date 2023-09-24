using CleverTap.Utilities;
using System;
using UnityEngine;

namespace CleverTapUnitySDK.Common
{
    #region Delegates

    public delegate void CleverTapDeepLinkCallbackDelegate(string url);
    
    public delegate void CleverTapProfileInitializedCallbackDelegate(string message);
    
    public delegate void CleverTapProfileUpdatesCallbackDelegate(string message);
    
    public delegate void CleverTapPushOpenedCallbackDelegate(string message);
    
    public delegate void CleverTapInitCleverTapIdCallbackDelegate(string message);
    
    public delegate void CleverTapInAppNotificationDismissedCallbackDelegate(string message);
    
    public delegate void CleverTapInAppNotificationShowCallbackDelegate(string message);
    
    public delegate void CleverTapOnPushPermissionResponseCallbackDelegate(string message);
    
    public delegate void CleverTapInAppNotificationButtonTappedDelegate(string message);
    
    public delegate void CleverTapInboxDidInitializeCallbackDelegate();
    
    public delegate void CleverTapInboxMessagesDidUpdateCallbackDelegate();
    
    public delegate void CleverTapInboxCustomExtrasButtonSelectDelegate(string message);
    
    public delegate void CleverTapInboxItemClickedDelegate(string message);
    
    public delegate void CleverTapNativeDisplayUnitsUpdatedDelegate(string message);
    
    public delegate void CleverTapProductConfigFetchedDelegate(string message);
    
    public delegate void CleverTapProductConfigActivatedDelegate(string message);
    
    public delegate void CleverTapProductConfigInitializedDelegate(string message);
    
    public delegate void CleverTapFeatureFlagsUpdatedDelegate(string message);

    #endregion

    public abstract class CleverTapCallbackHandler : MonoBehaviour
    {
        #region Events
        
        public event CleverTapDeepLinkCallbackDelegate OnCleverTapDeepLinkCallback;
        
        public event CleverTapProfileInitializedCallbackDelegate OnCleverTapProfileInitializedCallback;
        
        public event CleverTapProfileUpdatesCallbackDelegate OnCleverTapProfileUpdatesCallback;
        
        public event CleverTapPushOpenedCallbackDelegate OnCleverTapPushOpenedCallback;
        
        public event CleverTapInitCleverTapIdCallbackDelegate OnCleverTapInitCleverTapIdCallback;
        
        public event CleverTapInAppNotificationDismissedCallbackDelegate OnCleverTapInAppNotificationDismissedCallback;
        
        public event CleverTapInAppNotificationShowCallbackDelegate OnCleverTapInAppNotificationShowCallback;
        
        public event CleverTapOnPushPermissionResponseCallbackDelegate OnCleverTapOnPushPermissionResponseCallback;
        
        public event CleverTapInAppNotificationButtonTappedDelegate OnCleverTapInAppNotificationButtonTapped;
        
        public event CleverTapInboxDidInitializeCallbackDelegate OnCleverTapInboxDidInitializeCallback;
        
        public event CleverTapInboxMessagesDidUpdateCallbackDelegate OnCleverTapInboxMessagesDidUpdateCallback;
        
        public event CleverTapInboxCustomExtrasButtonSelectDelegate OnCleverTapInboxCustomExtrasButtonSelect;
        
        public event CleverTapInboxItemClickedDelegate OnCleverTapInboxItemClicked;
        
        public event CleverTapNativeDisplayUnitsUpdatedDelegate OnCleverTapNativeDisplayUnitsUpdated;
        
        public event CleverTapProductConfigFetchedDelegate OnCleverTapProductConfigFetched;
        
        public event CleverTapProductConfigActivatedDelegate OnCleverTapProductConfigActivated;
        
        public event CleverTapProductConfigInitializedDelegate OnCleverTapProductConfigInitialized;
        
        public event CleverTapFeatureFlagsUpdatedDelegate OnCleverTapFeatureFlagsUpdated;

        #endregion

        #region Default - Callback Methods

        public virtual void CleverTapDeepLinkCallback(string url)
        {
            OnCleverTapDeepLinkCallback(url);
            Debug.Log("unity received deep link: " + (!String.IsNullOrEmpty(url) ? url : "NULL"));
        }

        // called when then the CleverTap user profile is initialized
        // returns {"CleverTapID":<CleverTap unique user id>}
        public virtual void CleverTapProfileInitializedCallback(string message)
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
        public virtual void CleverTapProfileUpdatesCallback(string message)
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
        public virtual void CleverTapPushOpenedCallback(string message)
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
        public virtual void CleverTapInitCleverTapIdCallback(string message)
        {
            OnCleverTapInitCleverTapIdCallback(message);
            Debug.Log("unity received clevertap id: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
        }

        // returns the custom data associated with an in-app notification click
        public virtual void CleverTapInAppNotificationDismissedCallback(string message)
        {
            OnCleverTapInAppNotificationDismissedCallback(message);
            Debug.Log("unity received inapp notification dismissed: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
        }

        // returns the custom data associated with an in-app notification click
        public virtual void CleverTapInAppNotificationShowCallback(string message)
        {
            OnCleverTapInAppNotificationShowCallback(message);
            Debug.Log("unity received inapp notification onShow(): " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
        }

        // returns the status of push permission response after it's granted/denied
        public virtual void CleverTapOnPushPermissionResponseCallback(string message)
        {
            OnCleverTapOnPushPermissionResponseCallback(message);
            //Ensure to create call the `CreateNotificationChannel` once notification permission is granted to register for receiving push notifications for Android 13+ devices.
            Debug.Log("unity received push permission response: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
        }

        // returns when an in-app notification is dismissed by a call to action with custom extras
        public virtual void CleverTapInAppNotificationButtonTapped(string message)
        {
            OnCleverTapInAppNotificationButtonTapped(message);
            Debug.Log("unity received inapp notification button tapped: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
        }

        // returns callback for InitializeInbox
        public virtual void CleverTapInboxDidInitializeCallback()
        {
            OnCleverTapInboxDidInitializeCallback();
            Debug.Log("unity received inbox initialized");
        }

        public virtual void CleverTapInboxMessagesDidUpdateCallback()
        {
            OnCleverTapInboxMessagesDidUpdateCallback();
            Debug.Log("unity received inbox messages updated");
        }

        // returns on the click of app inbox message with a map of custom Key-Value pairs
        public virtual void CleverTapInboxCustomExtrasButtonSelect(string message)
        {
            OnCleverTapInboxCustomExtrasButtonSelect(message);
            Debug.Log("unity received inbox message button with custom extras select: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
        }

        // returns on the click of app inbox message with a string of the inbox payload along with page index and button index
        public virtual void CleverTapInboxItemClicked(string message)
        {
            OnCleverTapInboxItemClicked(message);
            Debug.Log("unity received inbox message clicked callback: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
        }

        // returns native display units data
        public virtual void CleverTapNativeDisplayUnitsUpdated(string message)
        {
            OnCleverTapNativeDisplayUnitsUpdated(message);
            Debug.Log("unity received native display units updated: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
        }

        // invoked when Product Experiences - Product Config are fetched 
        public virtual void CleverTapProductConfigFetched(string message)
        {
            OnCleverTapProductConfigFetched(message);
            Debug.Log("unity received product config fetched: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
        }

        // invoked when Product Experiences - Product Config are activated
        public virtual void CleverTapProductConfigActivated(string message)
        {
            OnCleverTapProductConfigActivated(message);
            Debug.Log("unity received product config activated: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
        }

        // invoked when Product Experiences - Product Config are initialized
        public virtual void CleverTapProductConfigInitialized(string message)
        {
            OnCleverTapProductConfigInitialized(message);
            Debug.Log("unity received product config initialized: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
        }

        // invoked when Product Experiences - Feature Flags are updated 
        public virtual void CleverTapFeatureFlagsUpdated(string message)
        {
            OnCleverTapFeatureFlagsUpdated(message);
            Debug.Log("unity received feature flags updated: " + (!String.IsNullOrEmpty(message) ? message : "NULL"));
        }

        #endregion
    }
}

