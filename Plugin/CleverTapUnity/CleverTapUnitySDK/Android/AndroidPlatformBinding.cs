#if UNITY_ANDROID
using CleverTap.Common;
using CleverTap.Utilities;
using System.Collections.Generic;

namespace CleverTap.Android {
    internal class AndroidPlatformBinding : CleverTapPlatformBindings {

        public AndroidPlatformBinding() {
            CallbackHandler = CreateGameObjectAndAttachCallbackHandler<AndroidCallbackHandler>("AndroidCallbackHandler");
            CleverTapLogger.Log("Start: CleverTap binding for Android.");
        }

        public override void CreateNotificationChannel(string channelId, string channelName, string channelDescription, int importance, bool showBadge) {
            CleverTapAndroidJNI.CleverTapClass.CallStatic("createNotificationChannel", CleverTapAndroidJNI.ApplicationContext, channelId, channelName, channelDescription, importance, showBadge);
        }

        public override void CreateNotificationChannelGroup(string groupId, string groupName) {
            CleverTapAndroidJNI.CleverTapClass.CallStatic("createNotificationChannelGroup", CleverTapAndroidJNI.ApplicationContext, groupId, groupName);
        }

        public override void CreateNotificationChannelWithGroup(string channelId, string channelName, string channelDescription, int importance, string groupId, bool showBadge) {
            CleverTapAndroidJNI.CleverTapClass.CallStatic("createNotificationChannelWithGroup", CleverTapAndroidJNI.ApplicationContext, channelId, channelName, channelDescription, importance, groupId, showBadge);
        }

        public override void CreateNotificationChannelWithGroupAndSound(string channelId, string channelName, string channelDescription, int importance, string groupId, bool showBadge, string sound) {
            CleverTapAndroidJNI.CleverTapClass.CallStatic("createNotificationChannelWithGroupAndSound", CleverTapAndroidJNI.ApplicationContext, channelId, channelName, channelDescription, importance, groupId, showBadge, sound);
        }

        public override void CreateNotificationChannelWithSound(string channelId, string channelName, string channelDescription, int importance, bool showBadge, string sound) {
            CleverTapAndroidJNI.CleverTapClass.CallStatic("createNotificationChannelWithSound", CleverTapAndroidJNI.ApplicationContext, channelId, channelName, channelDescription, importance, showBadge, sound);
        }

        public override void DeleteInboxMessageForID(string messageId) {
            CleverTapAndroidJNI.CleverTapJNI.Call("deleteInboxMessageForId", messageId);
        }

        public override void DeleteInboxMessagesForIDs(string[] messageIds) {
            CleverTapAndroidJNI.CleverTapJNI.Call("deleteInboxMessagesForIDs", messageIds);
        }

        public override void DeleteNotificationChannel(string channelId) {
            CleverTapAndroidJNI.CleverTapClass.CallStatic("deleteNotificationChannel", CleverTapAndroidJNI.ApplicationContext, channelId);
        }

        public override void DeleteNotificationChannelGroup(string groupId) {
            CleverTapAndroidJNI.CleverTapClass.CallStatic("deleteNotificationChannelGroup", CleverTapAndroidJNI.ApplicationContext, groupId);
        }

        public override void DisablePersonalization() {
            CleverTapAndroidJNI.CleverTapJNI.Call("disablePersonalization");
        }

        /**
        * Suspends the display of InApp Notifications and discards any new InApp Notifications to be shown
        * after this method is called.
        * The InApp Notifications will be displayed only once resumeInAppNotifications() is called.
        */
        public override void DiscardInAppNotifications() {
            CleverTapAndroidJNI.CleverTapJNI.Call("discardInAppNotifications");
        }

        public override void DismissAppInbox() {
            CleverTapAndroidJNI.CleverTapJNI.Call("dismissAppInbox");
        }

        public override void EnableDeviceNetworkInfoReporting(bool value) {
            CleverTapAndroidJNI.CleverTapJNI.Call("enableDeviceNetworkInfoReporting", value);
        }

        public override void EnablePersonalization() {
            CleverTapAndroidJNI.CleverTapJNI.Call("enablePersonalization");
        }

        public override JSONClass EventGetDetail(string eventName) {
            string jsonString = CleverTapAndroidJNI.CleverTapJNI.Call<string>("eventGetDetail", eventName);
            JSONClass json;
            try {
                json = (JSONClass)JSON.Parse(jsonString);
            } catch {
                CleverTapLogger.LogError("Unable to event detail json");
                json = new JSONClass();
            }
            return json;
        }

        public override int EventGetFirstTime(string eventName) {
            return CleverTapAndroidJNI.CleverTapJNI.Call<int>("eventGetFirstTime", eventName);
        }

        public override int EventGetLastTime(string eventName) {
            return CleverTapAndroidJNI.CleverTapJNI.Call<int>("eventGetLastTime", eventName);
        }

        public override int EventGetOccurrences(string eventName) {
            return CleverTapAndroidJNI.CleverTapJNI.Call<int>("eventGetOccurrences", eventName);
        }

        /**
        * requests for a unique, asynchronous CleverTap identifier. The value will be available as json {"cleverTapID" : <value> } via 
        * CleverTapUnity#CleverTapInitCleverTapIdCallback() function
        */
        public override string GetCleverTapID() {
            // check this
            return CleverTapAndroidJNI.CleverTapJNI.Call<string>("getCleverTapID");
        }

        public override int GetInboxMessageCount() {
            return CleverTapAndroidJNI.CleverTapJNI.Call<int>("getInboxMessageCount");
        }

        public override int GetInboxMessageUnreadCount() {
            return CleverTapAndroidJNI.CleverTapJNI.Call<int>("getInboxMessageUnreadCount");
        }

        public override void InitializeInbox() {
            CleverTapAndroidJNI.CleverTapJNI.Call("initializeInbox");
        }

        public override bool IsPushPermissionGranted() {
            return CleverTapAndroidJNI.CleverTapJNI.Call<bool>("isPushPermissionGranted");
        }

        public override void LaunchWithCredentials(string accountID, string token) {
            CleverTapAndroidJNI.CleverTapClass.CallStatic("initialize", accountID, token, CleverTapAndroidJNI.UnityActivity);
        }

        public override void LaunchWithCredentialsForRegion(string accountID, string token, string region) {
            CleverTapAndroidJNI.CleverTapClass.CallStatic("initialize", accountID, token, region, CleverTapAndroidJNI.UnityActivity);
        }

        public override void MarkReadInboxMessageForID(string messageId) {
            CleverTapAndroidJNI.CleverTapJNI.Call("markReadInboxMessageForId", messageId);
        }

        public override void MarkReadInboxMessagesForIDs(string[] messageIds) {
            CleverTapAndroidJNI.CleverTapJNI.Call("markReadInboxMessagesForIDs", messageIds);
        }

        public override void OnUserLogin(Dictionary<string, object> properties) {
            CleverTapAndroidJNI.CleverTapJNI.Call("onUserLogin", Json.Serialize(properties));
        }

        public override void ProfileAddMultiValueForKey(string key, string val) {
            CleverTapAndroidJNI.CleverTapJNI.Call("profileAddMultiValueForKey", key, val);
        }

        public override void ProfileAddMultiValuesForKey(string key, List<string> values) {
            CleverTapAndroidJNI.CleverTapJNI.Call("profileAddMultiValuesForKey", key, values.ToArray());
        }

        /**
        * This method is used to decrement the given value.Number should be in positive range
        */
        public override void ProfileDecrementValueForKey(string key, double val) {
            CleverTapAndroidJNI.CleverTapJNI.Call("profileDecrementValueForKey", key, val);
        }

        /**
        * This method is used to decrement the given value.Number should be in positive range
        */
        public override void ProfileDecrementValueForKey(string key, int val) {
            CleverTapAndroidJNI.CleverTapJNI.Call("profileDecrementValueForKey", key, val);
        }

        public override string ProfileGet(string key) {
            return CleverTapAndroidJNI.CleverTapJNI.Call<string>("profileGet", key);
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
            return CleverTapAndroidJNI.CleverTapJNI.Call<string>("profileGetCleverTapAttributionIdentifier");
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
            return CleverTapAndroidJNI.CleverTapJNI.Call<string>("profileGetCleverTapID");
        }

        /**
        * This method is used to increment the given value.Number should be in positive range
        */
        public override void ProfileIncrementValueForKey(string key, double val) {
            CleverTapAndroidJNI.CleverTapJNI.Call("profileIncrementValueForKey", key, val);
        }

        /**
        * This method is used to increment the given value.Number should be in positive range
        */
        public override void ProfileIncrementValueForKey(string key, int val) {
            CleverTapAndroidJNI.CleverTapJNI.Call("profileIncrementValueForKey", key, val);
        }

        public override void ProfilePush(Dictionary<string, object> properties) {
            CleverTapAndroidJNI.CleverTapJNI.Call("profilePush", Json.Serialize(properties));
        }

        public override void ProfileRemoveMultiValueForKey(string key, string val) {
            CleverTapAndroidJNI.CleverTapJNI.Call("profileRemoveMultiValueForKey", key, val);
        }

        public override void ProfileRemoveMultiValuesForKey(string key, List<string> values) {
            CleverTapAndroidJNI.CleverTapJNI.Call("profileRemoveMultiValuesForKey", key, values.ToArray());
        }

        public override void ProfileRemoveValueForKey(string key) {
            CleverTapAndroidJNI.CleverTapJNI.Call("profileRemoveValueForKey", key);
        }

        public override void ProfileSetMultiValuesForKey(string key, List<string> values) {
            CleverTapAndroidJNI.CleverTapJNI.Call("profileSetMultiValuesForKey", key, values.ToArray());
        }

        public override void PromptForPushPermission(bool showFallbackSettings) {
            CleverTapAndroidJNI.CleverTapJNI.Call("promptForPushPermission", showFallbackSettings);
        }

        public override void PromptPushPrimer(Dictionary<string, object> details) {
            CleverTapAndroidJNI.CleverTapJNI.Call("promptPushPrimer", Json.Serialize(details));
        }

        public override void PushInstallReferrerSource(string source, string medium, string campaign) {
            CleverTapAndroidJNI.CleverTapJNI.Call("pushInstallReferrer", source, medium, campaign);
        }

        public override void RecordChargedEventWithDetailsAndItems(Dictionary<string, object> details, List<Dictionary<string, object>> items) {
            CleverTapAndroidJNI.CleverTapJNI.Call("recordChargedEventWithDetailsAndItems", Json.Serialize(details), Json.Serialize(items));
        }

        public override void RecordEvent(string eventName) {
            CleverTapAndroidJNI.CleverTapJNI.Call("recordEvent", eventName, null);
        }

        public override void RecordEvent(string eventName, Dictionary<string, object> properties) {
            CleverTapAndroidJNI.CleverTapJNI.Call("recordEvent", eventName, Json.Serialize(properties));
        }

        public override void RecordScreenView(string screenName) {
            CleverTapAndroidJNI.CleverTapJNI.Call("recordScreenView", screenName);
        }

        /**
        * Suspends the display of InApp Notifications and discards any new InApp Notifications to be shown
        * after this method is called.
        * The InApp Notifications will be displayed only once resumeInAppNotifications() is called.
        */
        public override void ResumeInAppNotifications() {
            CleverTapAndroidJNI.CleverTapJNI.Call("resumeInAppNotifications");
        }

        public override int SessionGetTimeElapsed() {
            return CleverTapAndroidJNI.CleverTapJNI.Call<int>("sessionGetTimeElapsed");
        }

        public override JSONClass SessionGetUTMDetails() {
            string jsonString = CleverTapAndroidJNI.CleverTapJNI.Call<string>("sessionGetUTMDetails");
            JSONClass json;
            try {
                json = (JSONClass)JSON.Parse(jsonString);
            } catch {
                CleverTapLogger.LogError("Unable to parse session utm details json");
                json = new JSONClass();
            }
            return json;
        }

        public override void SetDebugLevel(int level) {
            CleverTapAndroidJNI.CleverTapClass.CallStatic("setDebugLevel", level);
        }

        public override void SetLocation(double lat, double lon) {
            CleverTapAndroidJNI.CleverTapJNI.Call("setLocation", lat, lon);
        }

        public override void SetOffline(bool value) {
            CleverTapAndroidJNI.CleverTapJNI.Call("setOffline", value);
        }

        public override void SetOptOut(bool value) {
            CleverTapAndroidJNI.CleverTapJNI.Call("setOptOut", value);
        }

        public override void ShowAppInbox(string styleConfig) {
            CleverTapAndroidJNI.CleverTapJNI.Call("showAppInbox", styleConfig);
        }

        /**
        * Suspends display of InApp Notifications.
        * The InApp Notifications are queued once this method is called
        * and will be displayed once resumeInAppNotifications() is called.
        */
        public override void SuspendInAppNotifications() {
            CleverTapAndroidJNI.CleverTapJNI.Call("suspendInAppNotifications");
        }

        public override JSONClass UserGetEventHistory() {
            string jsonString = CleverTapAndroidJNI.CleverTapJNI.Call<string>("userGetEventHistory");
            JSONClass json;
            try {
                json = (JSONClass)JSON.Parse(jsonString);
            } catch {
                CleverTapLogger.LogError("Unable to parse user event history json");
                json = new JSONClass();
            }
            return json;
        }

        public override int UserGetPreviousVisitTime() {
            return CleverTapAndroidJNI.CleverTapJNI.Call<int>("userGetPreviousVisitTime");
        }

        public override int UserGetScreenCount() {
            return CleverTapAndroidJNI.CleverTapJNI.Call<int>("userGetScreenCount");
        }

        public override int UserGetTotalVisits() {
            return CleverTapAndroidJNI.CleverTapJNI.Call<int>("userGetTotalVisits");
        }
    }
}
#endif