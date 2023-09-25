#if UNITY_ANDROID
using CleverTap.Utilities;
using CleverTapUnitySDK.Common;
using System.Collections.Generic;
using UnityEngine;

namespace CleverTapUnitySDK.Android {
    public class AndroidPlatformBinding : CleverTapPlatformBindings {
        private AndroidJavaObject unityActivity;
        private AndroidJavaObject clevertap;
        private AndroidJavaObject CleverTapClass;

        #region Properties
        public AndroidJavaObject unityCurrentActivity {
            get {
                if (unityActivity == null) {
                    using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                        unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                    }
                }
                return unityActivity;
            }
        }

        public AndroidJavaObject CleverTapAPI {
            get {
                if (CleverTapClass == null) {
                    CleverTapClass = new AndroidJavaClass("com.clevertap.unity.CleverTapUnityPlugin");
                }
                return CleverTapClass;
            }
        }

        public AndroidJavaObject CleverTap {
            get {
                if (clevertap == null) {
                    AndroidJavaObject context = unityCurrentActivity.Call<AndroidJavaObject>("getApplicationContext");
                    clevertap = CleverTapAPI.CallStatic<AndroidJavaObject>("getInstance", context);
                }
                return clevertap;
            }
        }
        #endregion

        public AndroidPlatformBinding() {
            CallbackHandler = CreateGameObjectAndAttachCallbackHandler<AndoridCallbackHandler>();
        }

        public override void CreateNotificationChannel(string channelId, string channelName, string channelDescription, int importance, bool showBadge) {
            AndroidJavaObject context = unityCurrentActivity.Call<AndroidJavaObject>("getApplicationContext");
            CleverTapAPI.CallStatic("createNotificationChannel", context, channelId, channelName, channelDescription, importance, showBadge);
        }

        public override void CreateNotificationChannelGroup(string groupId, string groupName) {
            AndroidJavaObject context = unityCurrentActivity.Call<AndroidJavaObject>("getApplicationContext");
            CleverTapAPI.CallStatic("createNotificationChannelGroup", context, groupId, groupName);
        }

        public override void CreateNotificationChannelWithGroup(string channelId, string channelName, string channelDescription, int importance, string groupId, bool showBadge) {
            AndroidJavaObject context = unityCurrentActivity.Call<AndroidJavaObject>("getApplicationContext");
            CleverTapAPI.CallStatic("createNotificationChannelWithGroup", context, channelId, channelName, channelDescription, importance, groupId, showBadge);
        }

        public override void CreateNotificationChannelWithGroupAndSound(string channelId, string channelName, string channelDescription, int importance, string groupId, bool showBadge, string sound) {
            AndroidJavaObject context = unityCurrentActivity.Call<AndroidJavaObject>("getApplicationContext");
            CleverTapAPI.CallStatic("createNotificationChannelWithGroupAndSound", context, channelId, channelName, channelDescription, importance, groupId, showBadge, sound);
        }

        public override void CreateNotificationChannelWithSound(string channelId, string channelName, string channelDescription, int importance, bool showBadge, string sound) {
            AndroidJavaObject context = unityCurrentActivity.Call<AndroidJavaObject>("getApplicationContext");
            CleverTapAPI.CallStatic("createNotificationChannelWithSound", context, channelId, channelName, channelDescription, importance, showBadge, sound);
        }

        public override void DeleteInboxMessageForID(string messageId) {
            CleverTap.Call("deleteInboxMessageForId", messageId);
        }

        public override void DeleteInboxMessagesForIDs(string[] messageIds) {
            CleverTap.Call("deleteInboxMessagesForIDs", messageIds);
        }

        public override void DeleteNotificationChannel(string channelId) {
            AndroidJavaObject context = unityCurrentActivity.Call<AndroidJavaObject>("getApplicationContext");
            CleverTapAPI.CallStatic("deleteNotificationChannel", context, channelId);
        }

        public override void DeleteNotificationChannelGroup(string groupId) {
            AndroidJavaObject context = unityCurrentActivity.Call<AndroidJavaObject>("getApplicationContext");
            CleverTapAPI.CallStatic("deleteNotificationChannelGroup", context, groupId);
        }

        public override void DisablePersonalization() {
            CleverTap.Call("disablePersonalization");
        }

        /**
        * Suspends the display of InApp Notifications and discards any new InApp Notifications to be shown
        * after this method is called.
        * The InApp Notifications will be displayed only once resumeInAppNotifications() is called.
        */
        public override void DiscardInAppNotifications() {
            CleverTap.Call("discardInAppNotifications");
        }

        public override void DismissAppInbox() {
            CleverTap.Call("dismissAppInbox");
        }

        public override void EnableDeviceNetworkInfoReporting(bool value) {
            CleverTap.Call("enableDeviceNetworkInfoReporting", value);
        }

        public override void EnablePersonalization() {
            CleverTap.Call("enablePersonalization");
        }

        public override JSONClass EventGetDetail(string eventName) {
            string jsonString = CleverTap.Call<string>("eventGetDetail", eventName);
            JSONClass json;
            try {
                json = (JSONClass)JSON.Parse(jsonString);
            } catch {
                Debug.Log("Unable to event detail json");
                json = new JSONClass();
            }
            return json;
        }

        public override int EventGetFirstTime(string eventName) {
            return CleverTap.Call<int>("eventGetFirstTime", eventName);
        }

        public override int EventGetLastTime(string eventName) {
            return CleverTap.Call<int>("eventGetLastTime", eventName);
        }

        public override int EventGetOccurrences(string eventName) {
            return CleverTap.Call<int>("eventGetOccurrences", eventName);
        }

        /**
        * requests for a unique, asynchronous CleverTap identifier. The value will be available as json {"cleverTapID" : <value> } via 
        * CleverTapUnity#CleverTapInitCleverTapIdCallback() function
        */
        public override string GetCleverTapID() {
            CleverTap.Call("getCleverTapID");
            // Check this
            return string.Empty;
        }

        public override int GetInboxMessageCount() {
            return CleverTap.Call<int>("getInboxMessageCount");
        }

        public override int GetInboxMessageUnreadCount() {
            return CleverTap.Call<int>("getInboxMessageUnreadCount");
        }

        public override void InitializeInbox() {
            CleverTap.Call("initializeInbox");
        }

        public override bool IsPushPermissionGranted() {
            return CleverTap.Call<bool>("isPushPermissionGranted");
        }

        public override void LaunchWithCredentials(string accountID, string token) {
            CleverTapAPI.CallStatic("initialize", accountID, token, unityCurrentActivity);
        }

        public override void LaunchWithCredentialsForRegion(string accountID, string token, string region) {
            CleverTapAPI.CallStatic("initialize", accountID, token, region, unityCurrentActivity);
        }

        public override void MarkReadInboxMessageForID(string messageId) {
            CleverTap.Call("markReadInboxMessageForId", messageId);
        }

        public override void MarkReadInboxMessagesForIDs(string[] messageIds) {
            CleverTap.Call("markReadInboxMessagesForIDs", messageIds);
        }

        public override void OnUserLogin(Dictionary<string, string> properties) {
            CleverTap.Call("onUserLogin", Json.Serialize(properties));
        }

        public override void ProfileAddMultiValueForKey(string key, string val) {
            CleverTap.Call("profileAddMultiValueForKey", key, val);
        }

        public override void ProfileAddMultiValuesForKey(string key, List<string> values) {
            CleverTap.Call("profileAddMultiValuesForKey", key, values.ToArray());
        }

        /**
        * This method is used to decrement the given value.Number should be in positive range
        */
        public override void ProfileDecrementValueForKey(string key, double val) {
            CleverTap.Call("profileDecrementValueForKey", key, val);
        }

        /**
        * This method is used to decrement the given value.Number should be in positive range
        */
        public override void ProfileDecrementValueForKey(string key, int val) {
            CleverTap.Call("profileDecrementValueForKey", key, val);
        }

        public override string ProfileGet(string key) {
            return CleverTap.Call<string>("profileGet", key);
        }

        /**
        * Returns a unique CleverTap identifier suitable for use with install attribution providers.
        * @return The attribution identifier currently being used to identify this user.
        *
        * Disclaimer: this method may take a long time to return, so you should not call it from the
        * application main thread
        *
        * NOTE: Deprecated as of clevertap android core sdk version 4.2.0 and will be removed
        *  in future versions .
        * instead listen for the id via CleverTapUnity#CleverTapInitCleverTapIdCallback() function
        */
        public override string ProfileGetCleverTapAttributionIdentifier() {
            return CleverTap.Call<string>("profileGetCleverTapAttributionIdentifier");
        }

        /**
        * Returns a unique CleverTap identifier suitable for use with install attribution providers.
        * @return The attribution identifier currently being used to identify this user.
        *
        * Disclaimer: this method may take a long time to return, so you should not call it from the
        * application main thread
        *
        * NOTE: Deprecated as of clevertap android core sdk version 4.2.0 and will be removed
        *  in future versions .
        * instead request for clevertapId via getCleverTapId() call and  listen for response
        * via CleverTapUnity#CleverTapInitCleverTapIdCallback() function
        */
        public override string ProfileGetCleverTapID() {
            return CleverTap.Call<string>("profileGetCleverTapID");
        }

        /**
        * This method is used to increment the given value.Number should be in positive range
        */
        public override void ProfileIncrementValueForKey(string key, double val) {
            CleverTap.Call("profileIncrementValueForKey", key, val);
        }

        /**
        * This method is used to increment the given value.Number should be in positive range
        */
        public override void ProfileIncrementValueForKey(string key, int val) {
            CleverTap.Call("profileIncrementValueForKey", key, val);
        }

        public override void ProfilePush(Dictionary<string, string> properties) {
            CleverTap.Call("profilePush", Json.Serialize(properties));
        }

        public override void ProfileRemoveMultiValueForKey(string key, string val) {
            CleverTap.Call("profileRemoveMultiValueForKey", key, val);
        }

        public override void ProfileRemoveMultiValuesForKey(string key, List<string> values) {
            CleverTap.Call("profileRemoveMultiValuesForKey", key, values.ToArray());
        }

        public override void ProfileRemoveValueForKey(string key) {
            CleverTap.Call("profileRemoveValueForKey", key);
        }

        public override void ProfileSetMultiValuesForKey(string key, List<string> values) {
            CleverTap.Call("profileSetMultiValuesForKey", key, values.ToArray());
        }

        public override void PromptForPushPermission(bool showFallbackSettings) {
            CleverTap.Call("promptForPushPermission", showFallbackSettings);
        }

        public override void PromptPushPrimer(Dictionary<string, object> details) {
            CleverTap.Call("promptPushPrimer", Json.Serialize(details));
        }

        public override void PushInstallReferrerSource(string source, string medium, string campaign) {
            CleverTap.Call("pushInstallReferrer", source, medium, campaign);
        }

        public override void RecordChargedEventWithDetailsAndItems(Dictionary<string, object> details, List<Dictionary<string, object>> items) {
            CleverTap.Call("recordChargedEventWithDetailsAndItems", Json.Serialize(details), Json.Serialize(items));
        }

        public override void RecordEvent(string eventName) {
            CleverTap.Call("recordEvent", eventName, null);
        }

        public override void RecordEvent(string eventName, Dictionary<string, object> properties) {
            CleverTap.Call("recordEvent", eventName, Json.Serialize(properties));
        }

        public override void RecordScreenView(string screenName) {
            CleverTap.Call("recordScreenView", screenName);
        }

        /**
        * Suspends the display of InApp Notifications and discards any new InApp Notifications to be shown
        * after this method is called.
        * The InApp Notifications will be displayed only once resumeInAppNotifications() is called.
        */
        public override void ResumeInAppNotifications() {
            CleverTap.Call("resumeInAppNotifications");
        }

        public override int SessionGetTimeElapsed() {
            return CleverTap.Call<int>("sessionGetTimeElapsed");
        }

        public override JSONClass SessionGetUTMDetails() {
            string jsonString = CleverTap.Call<string>("sessionGetUTMDetails");
            JSONClass json;
            try {
                json = (JSONClass)JSON.Parse(jsonString);
            } catch {
                Debug.Log("Unable to parse session utm details json");
                json = new JSONClass();
            }
            return json;
        }

        public override void SetDebugLevel(int level) {
            CleverTapAPI.CallStatic("setDebugLevel", level);
        }

        public override void SetLocation(double lat, double lon) {
            CleverTap.Call("setLocation", lat, lon);
        }

        public override void SetOffline(bool value) {
            CleverTap.Call("setOffline", value);
        }

        public override void SetOptOut(bool value) {
            CleverTap.Call("setOptOut", value);
        }

        public override void ShowAppInbox(string styleConfig) {
            CleverTap.Call("showAppInbox", styleConfig);
        }

        /**
        * Suspends display of InApp Notifications.
        * The InApp Notifications are queued once this method is called
        * and will be displayed once resumeInAppNotifications() is called.
        */
        public override void SuspendInAppNotifications() {
            CleverTap.Call("suspendInAppNotifications");
        }

        public override JSONClass UserGetEventHistory() {
            string jsonString = CleverTap.Call<string>("userGetEventHistory");
            JSONClass json;
            try {
                json = (JSONClass)JSON.Parse(jsonString);
            } catch {
                Debug.Log("Unable to parse user event history json");
                json = new JSONClass();
            }
            return json;
        }

        public override int UserGetPreviousVisitTime() {
            return CleverTap.Call<int>("userGetPreviousVisitTime");
        }

        public override int UserGetScreenCount() {
            return CleverTap.Call<int>("userGetScreenCount");
        }

        public override int UserGetTotalVisits() {
            return CleverTap.Call<int>("userGetTotalVisits");
        }
    }
}
#endif