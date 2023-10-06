using UnityEngine;
using System.Collections.Generic;
using CleverTap.Utilities;

/// <summary>
/// These methods can be called by Unity applications to record
/// events and set and get user profile attributes.
/// </summary>

namespace CleverTap {
    public class CleverTapBinding : MonoBehaviour {

        public const string Version = "2.4.1";

#if UNITY_IOS

        public static void LaunchWithCredentials(string accountID, string token) =>
            CleverTapUnitySDK.CleverTap.LaunchWithCredentials(accountID, token);

        public static void LaunchWithCredentialsForRegion(string accountID, string token, string region) =>
            CleverTapUnitySDK.CleverTap.LaunchWithCredentialsForRegion(accountID, token, region);

        public static void OnUserLogin(Dictionary<string, string> properties) => 
            CleverTapUnitySDK.CleverTap.OnUserLogin(properties);

        public static void ProfilePush(Dictionary<string, string> properties) => 
            CleverTapUnitySDK.CleverTap.ProfilePush(properties);

        public static string ProfileGet(string key) => 
            CleverTapUnitySDK.CleverTap.ProfileGet(key);

        public static string ProfileGetCleverTapAttributionIdentifier() =>
            CleverTapUnitySDK.CleverTap.ProfileGetCleverTapAttributionIdentifier();

        public static string ProfileGetCleverTapID() =>
            CleverTapUnitySDK.CleverTap.ProfileGetCleverTapID();

        public static void ProfileRemoveValueForKey(string key) => 
            CleverTapUnitySDK.CleverTap.ProfileRemoveValueForKey(key);

        public static void ProfileSetMultiValuesForKey(string key, List<string> values) => 
            CleverTapUnitySDK.CleverTap.ProfileSetMultiValuesForKey(key, values);

        public static void ProfileAddMultiValuesForKey(string key, List<string> values) => 
            CleverTapUnitySDK.CleverTap.ProfileAddMultiValuesForKey(key, values);

        public static void ProfileRemoveMultiValuesForKey(string key, List<string> values) => 
            CleverTapUnitySDK.CleverTap.ProfileRemoveMultiValuesForKey(key, values);

        public static void ProfileAddMultiValueForKey(string key, string val) => 
            CleverTapUnitySDK.CleverTap.ProfileAddMultiValueForKey(key, val);

        public static void ProfileRemoveMultiValueForKey(string key, string val) => 
            CleverTapUnitySDK.CleverTap.ProfileRemoveMultiValueForKey(key, val);

        public static void ProfileIncrementValueForKey(string key, double val) => 
            CleverTapUnitySDK.CleverTap.ProfileIncrementValueForKey(key, val);

        public static void ProfileIncrementValueForKey(string key, int val) => 
            CleverTapUnitySDK.CleverTap.ProfileIncrementValueForKey(key, val);

        public static void ProfileDecrementValueForKey(string key, double val) => 
            CleverTapUnitySDK.CleverTap.ProfileDecrementValueForKey(key, val);

        public static void ProfileDecrementValueForKey(string key, int val) => 
            CleverTapUnitySDK.CleverTap.ProfileDecrementValueForKey(key, val);

        public static void SuspendInAppNotifications() => 
            CleverTapUnitySDK.CleverTap.SuspendInAppNotifications();

        public static void DiscardInAppNotifications() => 
            CleverTapUnitySDK.CleverTap.DiscardInAppNotifications();

        public static void ResumeInAppNotifications() => 
            CleverTapUnitySDK.CleverTap.ResumeInAppNotifications();

        public static string GetCleverTapID() => 
            CleverTapUnitySDK.CleverTap.GetCleverTapID();

        public static void RecordScreenView(string screenName) => 
            CleverTapUnitySDK.CleverTap.RecordScreenView(screenName);

        public static void RecordEvent(string eventName) => 
            CleverTapUnitySDK.CleverTap.RecordEvent(eventName);

        public static void RecordEvent(string eventName, Dictionary<string, object> properties) => 
            CleverTapUnitySDK.CleverTap.RecordEvent(eventName, properties);

        public static void RecordChargedEventWithDetailsAndItems(Dictionary<string, object> details, List<Dictionary<string, object>> items) => 
            CleverTapUnitySDK.CleverTap.RecordChargedEventWithDetailsAndItems(details, items);

        public static int EventGetFirstTime(string eventName) =>
            CleverTapUnitySDK.CleverTap.EventGetFirstTime(eventName);

        public static int EventGetLastTime(string eventName) => 
            CleverTapUnitySDK.CleverTap.EventGetLastTime(eventName);

        public static int EventGetOccurrences(string eventName) => 
            CleverTapUnitySDK.CleverTap.EventGetOccurrences(eventName);

        public static JSONClass UserGetEventHistory() => 
            CleverTapUnitySDK.CleverTap.UserGetEventHistory();

        public static JSONClass SessionGetUTMDetails() => 
            CleverTapUnitySDK.CleverTap.SessionGetUTMDetails();

        public static int SessionGetTimeElapsed() => 
            CleverTapUnitySDK.CleverTap.SessionGetTimeElapsed();

        public static JSONClass EventGetDetail(string eventName) => 
            CleverTapUnitySDK.CleverTap.EventGetDetail(eventName);

        public static int UserGetTotalVisits() => 
            CleverTapUnitySDK.CleverTap.UserGetTotalVisits();

        public static int UserGetScreenCount() => 
            CleverTapUnitySDK.CleverTap.UserGetScreenCount();

        public static int UserGetPreviousVisitTime() => 
            CleverTapUnitySDK.CleverTap.UserGetPreviousVisitTime();

        public static void RegisterPush() => 
            CleverTapUnitySDK.CleverTap.RegisterPush();

        public static void SetApplicationIconBadgeNumber(int num) => 
            CleverTapUnitySDK.CleverTap.SetApplicationIconBadgeNumber(num);

        public static void SetDebugLevel(int level) => 
            CleverTapUnitySDK.CleverTap.SetDebugLevel(level);

        public static void EnablePersonalization() => 
            CleverTapUnitySDK.CleverTap.EnablePersonalization();

        public static void DisablePersonalization() => 
            CleverTapUnitySDK.CleverTap.DisablePersonalization();

        public static void SetLocation(double lat, double lon) => 
            CleverTapUnitySDK.CleverTap.SetLocation(lat, lon);

        public static void PushInstallReferrerSource(string source, string medium, string campaign) => 
            CleverTapUnitySDK.CleverTap.PushInstallReferrerSource(source, medium, campaign);

        public static void SetOffline(bool enabled) => 
            CleverTapUnitySDK.CleverTap.SetOffline(enabled);

        public static void SetOptOut(bool enabled) => 
            CleverTapUnitySDK.CleverTap.SetOptOut(enabled);

        public static void EnableDeviceNetworkInfoReporting(bool enabled) => 
            CleverTapUnitySDK.CleverTap.EnableDeviceNetworkInfoReporting(enabled);

        public static void InitializeInbox() => 
            CleverTapUnitySDK.CleverTap.InitializeInbox();

        public static void ShowAppInbox(Dictionary<string, object> styleConfig) => 
            CleverTapUnitySDK.CleverTap.ShowAppInbox(styleConfig);

        public static void DismissAppInbox() => 
            CleverTapUnitySDK.CleverTap.DismissAppInbox();

        public static int GetInboxMessageCount() => 
            CleverTapUnitySDK.CleverTap.GetInboxMessageCount();

        public static int GetInboxMessageUnreadCount() => 
            CleverTapUnitySDK.CleverTap.GetInboxMessageUnreadCount();

        public static JSONArray GetAllInboxMessages() => 
            CleverTapUnitySDK.CleverTap.GetAllInboxMessages();

        public static JSONArray GetUnreadInboxMessages() => 
            CleverTapUnitySDK.CleverTap.GetUnreadInboxMessages();

        public static JSONClass GetInboxMessageForId(string messageId) => 
            CleverTapUnitySDK.CleverTap.GetInboxMessageForId(messageId);

        public static void DeleteInboxMessageForID(string messageId) => 
            CleverTapUnitySDK.CleverTap.DeleteInboxMessageForID(messageId);

        public static void DeleteInboxMessagesForIDs(string[] messageIds) => 
            CleverTapUnitySDK.CleverTap.DeleteInboxMessagesForIDs(messageIds);

        public static void MarkReadInboxMessageForID(string messageId) => 
            CleverTapUnitySDK.CleverTap.MarkReadInboxMessageForID(messageId);

        public static void MarkReadInboxMessagesForIDs(string[] messageIds) => 
            CleverTapUnitySDK.CleverTap.MarkReadInboxMessagesForIDs(messageIds);

        public static void RecordInboxNotificationViewedEventForID(string messageId) => 
            CleverTapUnitySDK.CleverTap.RecordInboxNotificationViewedEventForID(messageId);

        public static void RecordInboxNotificationClickedEventForID(string messageId) => 
            CleverTapUnitySDK.CleverTap.RecordInboxNotificationClickedEventForID(messageId);

        public static JSONArray GetAllDisplayUnits() => 
            CleverTapUnitySDK.CleverTap.GetAllDisplayUnits();

        public static JSONClass GetDisplayUnitForID(string unitID) => 
            CleverTapUnitySDK.CleverTap.GetDisplayUnitForID(unitID);

        public static void RecordDisplayUnitViewedEventForID(string unitID) => 
            CleverTapUnitySDK.CleverTap.RecordDisplayUnitViewedEventForID(unitID);

        public static void RecordDisplayUnitClickedEventForID(string unitID) => 
            CleverTapUnitySDK.CleverTap.RecordDisplayUnitClickedEventForID(unitID);

        public static void FetchProductConfig() => 
            CleverTapUnitySDK.CleverTap.FetchProductConfig();

        public static void FetchProductConfigWithMinimumInterval(double minimumInterval) => 
            CleverTapUnitySDK.CleverTap.FetchProductConfigWithMinimumInterval(minimumInterval);

        public static void SetProductConfigMinimumFetchInterval(double minimumFetchInterval) => 
            CleverTapUnitySDK.CleverTap.SetProductConfigMinimumFetchInterval(minimumFetchInterval);

        public static void ActivateProductConfig() => 
            CleverTapUnitySDK.CleverTap.ActivateProductConfig();

        public static void FetchAndActivateProductConfig() => 
            CleverTapUnitySDK.CleverTap.FetchAndActivateProductConfig();

        public static void SetProductConfigDefaults(Dictionary<string, object> defaults) => 
            CleverTapUnitySDK.CleverTap.SetProductConfigDefaults(defaults);

        public static void SetProductConfigDefaultsFromPlistFileName(string fileName) => 
            CleverTapUnitySDK.CleverTap.SetProductConfigDefaultsFromPlistFileName(fileName);

        public static JSONClass GetProductConfigValueFor(string key) => 
            CleverTapUnitySDK.CleverTap.GetProductConfigValueFor(key);

        public static double GetProductConfigLastFetchTimeStamp() => 
            CleverTapUnitySDK.CleverTap.GetProductConfigLastFetchTimeStamp();

        public static void ResetProductConfig() => 
            CleverTapUnitySDK.CleverTap.ResetProductConfig();

        public static bool GetFeatureFlag(string key, bool defaultValue) => 
            CleverTapUnitySDK.CleverTap.GetFeatureFlag(key, defaultValue);

        public static void PromptPushPrimer(Dictionary<string, object> json) => 
            CleverTapUnitySDK.CleverTap.PromptPushPrimer(json);

        public static void PromptForPushPermission(bool showFallbackSettings) => 
            CleverTapUnitySDK.CleverTap.PromptForPushPermission(showFallbackSettings);

        public static void IsPushPermissionGranted() => 
            CleverTapUnitySDK.CleverTap.IsPushPermissionGranted();

#elif UNITY_ANDROID

        private static AndroidJavaObject unityActivity;
        private static AndroidJavaObject clevertap;
        private static AndroidJavaObject CleverTapClass;

        #region Properties
        public static AndroidJavaObject unityCurrentActivity {
            get {
                if (unityActivity == null) {
                    using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                        unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                    }
                }
                return unityActivity;
            }
        }

        public static AndroidJavaObject CleverTapAPI {
            get {
                if (CleverTapClass == null) {
                    CleverTapClass = new AndroidJavaClass("com.clevertap.unity.CleverTapUnityPlugin");
                }
                return CleverTapClass;
            }
        }

        public static AndroidJavaObject CleverTap {
            get {
                if (clevertap == null) {
                    AndroidJavaObject context = unityCurrentActivity.Call<AndroidJavaObject>("getApplicationContext");
                    clevertap = CleverTapAPI.CallStatic<AndroidJavaObject>("getInstance", context);
                }
                return clevertap;
            }
        }
        #endregion

        public static void SetDebugLevel(int level) => 
            CleverTapUnitySDK.CleverTap.SetDebugLevel(level);

        public static void Initialize(string accountID, string accountToken) => 
            CleverTapUnitySDK.CleverTap.LaunchWithCredentials(accountID, accountToken);

        public static void Initialize(string accountID, string accountToken, string region) => 
            CleverTapUnitySDK.CleverTap.LaunchWithCredentialsForRegion(accountID, accountToken, region);

        public static void LaunchWithCredentials(string accountID, string token, string region) {            
            //no op only supported on ios
        }

        public static void LaunchWithCredentials(string accountID, string token) {
            //no op only supported on ios
        }

        public static void RegisterPush() {
            //no op only supported on ios
        }

        public static void CreateNotificationChannel(string channelId, string channelName, string channelDescription, int importance, bool showBadge) => 
            CleverTapUnitySDK.CleverTap.CreateNotificationChannel(channelId, channelName, channelDescription, importance, showBadge);

        public static void CreateNotificationChannelWithSound(string channelId, string channelName, string channelDescription, int importance, bool showBadge, string sound) => 
            CleverTapUnitySDK.CleverTap.CreateNotificationChannelWithSound(channelId, channelName, channelDescription, importance, showBadge, sound);

        public static void CreateNotificationChannelWithGroup(string channelId, string channelName, string channelDescription, int importance, string groupId, bool showBadge) => 
            CleverTapUnitySDK.CleverTap.CreateNotificationChannelWithGroup(channelId, channelName, channelDescription, importance, groupId, showBadge);

        public static void CreateNotificationChannelWithGroupAndSound(string channelId, string channelName, string channelDescription, int importance, string groupId, bool showBadge, string sound) => 
            CleverTapUnitySDK.CleverTap.CreateNotificationChannelWithGroupAndSound(channelId, channelName, channelDescription, importance, groupId, showBadge, sound);

        public static void CreateNotificationChannelGroup(string groupId, string groupName) => 
            CleverTapUnitySDK.CleverTap.CreateNotificationChannelGroup(groupId, groupName);

        public static void DeleteNotificationChannel(string channelId) => 
            CleverTapUnitySDK.CleverTap.DeleteNotificationChannel(channelId);

        public static void DeleteNotificationChannelGroup(string groupId) => 
            CleverTapUnitySDK.CleverTap.DeleteNotificationChannelGroup(groupId);

        public static void SetOptOut(bool value) => 
            CleverTapUnitySDK.CleverTap.SetOptOut(value);

        public static void SetOffline(bool value) => 
            CleverTapUnitySDK.CleverTap.SetOffline(value);

        public static void EnableDeviceNetworkInfoReporting(bool value) => 
            CleverTapUnitySDK.CleverTap.EnableDeviceNetworkInfoReporting(value);

        public static void EnablePersonalization() => 
            CleverTapUnitySDK.CleverTap.EnablePersonalization();

        public static void DisablePersonalization() => 
            CleverTapUnitySDK.CleverTap.DisablePersonalization();

        public static void SetLocation(double lat, double lon) => 
            CleverTapUnitySDK.CleverTap.SetLocation(lat, lon);

        public static void OnUserLogin(Dictionary<string, string> properties) => 
            CleverTapUnitySDK.CleverTap.OnUserLogin(properties);

        public static void ProfilePush(Dictionary<string, string> properties) => 
            CleverTapUnitySDK.CleverTap.ProfilePush(properties);

        public static string ProfileGet(string key) => 
            CleverTapUnitySDK.CleverTap.ProfileGet(key);

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
        public static string ProfileGetCleverTapAttributionIdentifier() => 
            CleverTapUnitySDK.CleverTap.ProfileGetCleverTapAttributionIdentifier();

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
        public static string ProfileGetCleverTapID() => 
            CleverTapUnitySDK.CleverTap.ProfileGetCleverTapID();

        /*
         * requests for a unique, asynchronous CleverTap identifier. The value will be available as json {"cleverTapID" : <value> } via 
         * CleverTapUnity#CleverTapInitCleverTapIdCallback() function
         */
        public static void GetCleverTapId() => 
            CleverTapUnitySDK.CleverTap.GetCleverTapID();

        /**
         * This method is used to increment the given value.Number should be in positive range
         */
        public static void ProfileIncrementValueForKey(string key, double val) => 
            CleverTapUnitySDK.CleverTap.ProfileIncrementValueForKey(key, val);

        /**
         * This method is used to increment the given value.Number should be in positive range
         */
        public static void ProfileIncrementValueForKey(string key, int val) => 
            CleverTapUnitySDK.CleverTap.ProfileIncrementValueForKey(key, val);

        /**
         * This method is used to decrement the given value.Number should be in positive range
         */
        public static void ProfileDecrementValueForKey(string key, double val) => 
            CleverTapUnitySDK.CleverTap.ProfileDecrementValueForKey(key, val);

        /**
         * This method is used to decrement the given value.Number should be in positive range
         */
        public static void ProfileDecrementValueForKey(string key, int val) => 
            CleverTapUnitySDK.CleverTap.ProfileDecrementValueForKey(key, val);

        /**
         * Suspends display of InApp Notifications.
         * The InApp Notifications are queued once this method is called
         * and will be displayed once resumeInAppNotifications() is called.
         */
        public static void SuspendInAppNotifications() => 
            CleverTapUnitySDK.CleverTap.SuspendInAppNotifications();

        /**
         * Suspends the display of InApp Notifications and discards any new InApp Notifications to be shown
         * after this method is called.
         * The InApp Notifications will be displayed only once resumeInAppNotifications() is called.
         */
        public static void DiscardInAppNotifications() => 
            CleverTapUnitySDK.CleverTap.DiscardInAppNotifications();

        /**
         * Suspends the display of InApp Notifications and discards any new InApp Notifications to be shown
         * after this method is called.
         * The InApp Notifications will be displayed only once resumeInAppNotifications() is called.
         */
        public static void ResumeInAppNotifications() => 
            CleverTapUnitySDK.CleverTap.ResumeInAppNotifications();

        public static void ProfileRemoveValueForKey(string key) => 
            CleverTapUnitySDK.CleverTap.ProfileRemoveValueForKey(key);

        public static void ProfileSetMultiValuesForKey(string key, List<string> values) => 
            CleverTapUnitySDK.CleverTap.ProfileSetMultiValuesForKey(key, values);

        public static void ProfileAddMultiValuesForKey(string key, List<string> values) => 
            CleverTapUnitySDK.CleverTap.ProfileAddMultiValuesForKey(key, values);

        public static void ProfileRemoveMultiValuesForKey(string key, List<string> values) => 
            CleverTapUnitySDK.CleverTap.ProfileRemoveMultiValuesForKey(key, values);

        public static void ProfileAddMultiValueForKey(string key, string val) => 
            CleverTapUnitySDK.CleverTap.ProfileAddMultiValueForKey(key, val);

        public static void ProfileRemoveMultiValueForKey(string key, string val) => 
            CleverTapUnitySDK.CleverTap.ProfileRemoveMultiValueForKey(key, val);

        public static void RecordScreenView(string screenName) => 
            CleverTapUnitySDK.CleverTap.RecordScreenView(screenName);

        public static void RecordEvent(string eventName) => 
            CleverTapUnitySDK.CleverTap.RecordEvent(eventName);

        public static void RecordEvent(string eventName, Dictionary<string, object> properties) => 
            CleverTapUnitySDK.CleverTap.RecordEvent(eventName, properties);

        public static void RecordChargedEventWithDetailsAndItems(Dictionary<string, object> details, List<Dictionary<string, object>> items) => 
            CleverTapUnitySDK.CleverTap.RecordChargedEventWithDetailsAndItems(details, items);

        public static int EventGetFirstTime(string eventName) => 
            CleverTapUnitySDK.CleverTap.EventGetFirstTime(eventName);

        public static int EventGetLastTime(string eventName) => 
            CleverTapUnitySDK.CleverTap.EventGetLastTime(eventName);

        public static int EventGetOccurrences(string eventName) => 
            CleverTapUnitySDK.CleverTap.EventGetOccurrences(eventName);

        public static JSONClass EventGetDetail(string eventName) => 
            CleverTapUnitySDK.CleverTap.EventGetDetail(eventName);

        public static JSONClass UserGetEventHistory() => 
            CleverTapUnitySDK.CleverTap.UserGetEventHistory();

        public static JSONClass SessionGetUTMDetails() => 
            CleverTapUnitySDK.CleverTap.SessionGetUTMDetails();

        public static int SessionGetTimeElapsed() => 
            CleverTapUnitySDK.CleverTap.SessionGetTimeElapsed();

        public static int UserGetTotalVisits() => 
            CleverTapUnitySDK.CleverTap.UserGetTotalVisits();

        public static int UserGetScreenCount() => 
            CleverTapUnitySDK.CleverTap.UserGetScreenCount();

        public static int UserGetPreviousVisitTime() => 
            CleverTapUnitySDK.CleverTap.UserGetPreviousVisitTime();

        public static void SetApplicationIconBadgeNumber(int num) {
            // no-op for Android
        }

        public static void PushInstallReferrerSource(string source, string medium, string campaign) => 
            CleverTapUnitySDK.CleverTap.PushInstallReferrerSource(source, medium, campaign);

        public static void InitializeInbox() => 
            CleverTapUnitySDK.CleverTap.InitializeInbox();

        public static void ShowAppInbox(string styleConfig) => 
            CleverTapUnitySDK.CleverTap.ShowAppInbox(styleConfig);

        public static void DismissAppInbox() => 
            CleverTapUnitySDK.CleverTap.DismissAppInbox();

        public static int GetInboxMessageCount() => 
            CleverTapUnitySDK.CleverTap.GetInboxMessageCount();

        public static void DeleteInboxMessagesForIDs(string[] messageIds) => 
            CleverTapUnitySDK.CleverTap.DeleteInboxMessagesForIDs(messageIds);

        public static void DeleteInboxMessageForID(string messageId) => 
            CleverTapUnitySDK.CleverTap.DeleteInboxMessageForID(messageId);

        public static void MarkReadInboxMessagesForIDs(string[] messageIds) => 
            CleverTapUnitySDK.CleverTap.MarkReadInboxMessagesForIDs(messageIds);

        public static void MarkReadInboxMessageForID(string messageId) => 
            CleverTapUnitySDK.CleverTap.MarkReadInboxMessageForID(messageId);

        public static int GetInboxMessageUnreadCount() => 
            CleverTapUnitySDK.CleverTap.GetInboxMessageUnreadCount();

        public static void PromptPushPrimer(Dictionary<string, object> details) => 
            CleverTapUnitySDK.CleverTap.PromptPushPrimer(details);

        public static void PromptForPushPermission(bool showFallbackSettings) => 
            CleverTapUnitySDK.CleverTap.PromptForPushPermission(showFallbackSettings);

        public static bool IsPushPermissionGranted() => 
            CleverTapUnitySDK.CleverTap.IsPushPermissionGranted();

#else

        public static void LaunchWithCredentials(string accountID, string token, string region) => 
            CleverTapUnitySDK.CleverTap.LaunchWithCredentialsForRegion(accountID, token, region);

        public static void OnUserLogin(Dictionary<string, string> properties) => 
            CleverTapUnitySDK.CleverTap.OnUserLogin(properties);

        public static void ProfilePush(Dictionary<string, string> properties) => 
            CleverTapUnitySDK.CleverTap.ProfilePush(properties);

        public static string ProfileGet(string key) => 
            CleverTapUnitySDK.CleverTap.ProfileGet(key);

        public static string ProfileGetCleverTapAttributionIdentifier() => 
            CleverTapUnitySDK.CleverTap.ProfileGetCleverTapAttributionIdentifier();

        public static string ProfileGetCleverTapID() => 
            CleverTapUnitySDK.CleverTap.ProfileGetCleverTapID();

        public static void ProfileRemoveValueForKey(string key) => 
            CleverTapUnitySDK.CleverTap.ProfileRemoveValueForKey(key);

        public static void ProfileSetMultiValuesForKey(string key, List<string> values) =>
            CleverTapUnitySDK.CleverTap.ProfileSetMultiValuesForKey(key, values);

        public static void ProfileAddMultiValuesForKey(string key, List<string> values) => 
            CleverTapUnitySDK.CleverTap.ProfileAddMultiValuesForKey(key, values);

        public static void ProfileRemoveMultiValuesForKey(string key, List<string> values) => 
            CleverTapUnitySDK.CleverTap.ProfileRemoveMultiValuesForKey(key, values);

        public static void ProfileAddMultiValueForKey(string key, string val) => 
            CleverTapUnitySDK.CleverTap.ProfileAddMultiValueForKey(key, val);

        public static void ProfileRemoveMultiValueForKey(string key, string val) => 
            CleverTapUnitySDK.CleverTap.ProfileRemoveMultiValueForKey(key, val);

        public static void ProfileIncrementValueForKey(string key, double val) => 
            CleverTapUnitySDK.CleverTap.ProfileIncrementValueForKey(key, val);

        public static void ProfileIncrementValueForKey(string key, int val) => 
            CleverTapUnitySDK.CleverTap.ProfileIncrementValueForKey(key, val);

        public static void ProfileDecrementValueForKey(string key, double val) => 
            CleverTapUnitySDK.CleverTap.ProfileDecrementValueForKey(key, val);

        public static void ProfileDecrementValueForKey(string key, int val) => 
            CleverTapUnitySDK.CleverTap.ProfileDecrementValueForKey(key, val);

        public static void SuspendInAppNotifications() => 
            CleverTapUnitySDK.CleverTap.SuspendInAppNotifications();

        public static void DiscardInAppNotifications() => 
            CleverTapUnitySDK.CleverTap.DiscardInAppNotifications();

        public static void ResumeInAppNotifications() => 
            CleverTapUnitySDK.CleverTap.ResumeInAppNotifications();

        public static string GetCleverTapID() => 
            CleverTapUnitySDK.CleverTap.GetCleverTapID();

        public static void RecordScreenView(string screenName) => 
            CleverTapUnitySDK.CleverTap.RecordScreenView(screenName);

        public static void RecordEvent(string eventName) => 
            CleverTapUnitySDK.CleverTap.RecordEvent(eventName);

        public static void RecordEvent(string eventName, Dictionary<string, object> properties) => 
            CleverTapUnitySDK.CleverTap.RecordEvent(eventName, properties);

        public static void RecordChargedEventWithDetailsAndItems(Dictionary<string, object> details, List<Dictionary<string, object>> items) => 
            CleverTapUnitySDK.CleverTap.RecordChargedEventWithDetailsAndItems(details, items);

        public static int EventGetFirstTime(string eventName) => 
            CleverTapUnitySDK.CleverTap.EventGetFirstTime(eventName);

        public static int EventGetLastTime(string eventName) => 
            CleverTapUnitySDK.CleverTap.EventGetLastTime(eventName);

        public static int EventGetOccurrences(string eventName) => 
            CleverTapUnitySDK.CleverTap.EventGetOccurrences(eventName);

        public static JSONClass EventGetDetail(string eventName) => 
            CleverTapUnitySDK.CleverTap.EventGetDetail(eventName);

        public static JSONClass UserGetEventHistory() => 
            CleverTapUnitySDK.CleverTap.UserGetEventHistory();

        public static JSONClass SessionGetUTMDetails() => 
            CleverTapUnitySDK.CleverTap.SessionGetUTMDetails();

        public static int SessionGetTimeElapsed() => 
            CleverTapUnitySDK.CleverTap.SessionGetTimeElapsed();

        public static int UserGetTotalVisits() => 
            CleverTapUnitySDK.CleverTap.UserGetTotalVisits();

        public static int UserGetScreenCount() => 
            CleverTapUnitySDK.CleverTap.UserGetScreenCount();

        public static int UserGetPreviousVisitTime() => 
            CleverTapUnitySDK.CleverTap.UserGetPreviousVisitTime();

        public static void EnablePersonalization() => 
            CleverTapUnitySDK.CleverTap.EnablePersonalization();

        public static void DisablePersonalization() => 
            CleverTapUnitySDK.CleverTap.DisablePersonalization();

        public static void RegisterPush() => 
            CleverTapUnitySDK.CleverTap.RegisterPush();

        public static void SetApplicationIconBadgeNumber(int num) => 
            CleverTapUnitySDK.CleverTap.SetApplicationIconBadgeNumber(num);

        public static void SetDebugLevel(int level) => 
            CleverTapUnitySDK.CleverTap.SetDebugLevel(level);

        public static void SetLocation(double lat, double lon) => 
            CleverTapUnitySDK.CleverTap.SetLocation(lat, lon);

        public static void PushInstallReferrerSource(string source, string medium, string campaign) => 
            CleverTapUnitySDK.CleverTap.PushInstallReferrerSource(source, medium, campaign);

        public static void EnableDeviceNetworkInfoReporting(bool value) => 
            CleverTapUnitySDK.CleverTap.EnableDeviceNetworkInfoReporting(value);

        public static void SetOptOut(bool value) => 
            CleverTapUnitySDK.CleverTap.SetOptOut(value);

        public static void SetOffline(bool value) => 
            CleverTapUnitySDK.CleverTap.SetOffline(value);

        public static void CreateNotificationChannel(string channelId, string channelName, string channelDescription, int importance, bool showBadge) => 
            CleverTapUnitySDK.CleverTap.CreateNotificationChannel(channelId, channelName, channelDescription, importance, showBadge);

        public static void CreateNotificationChannelWithSound(string channelId, string channelName, string channelDescription, int importance, bool showBadge, string sound) => 
            CleverTapUnitySDK.CleverTap.CreateNotificationChannelWithSound(channelId, channelName, channelDescription, importance, showBadge, sound);

        public static void CreateNotificationChannelWithGroup(string channelId, string channelName, string channelDescription, int importance, string groupId, bool showBadge) => 
            CleverTapUnitySDK.CleverTap.CreateNotificationChannelWithGroup(channelId, channelName, channelDescription, importance, groupId, showBadge);

        public static void CreateNotificationChannelWithGroupAndSound(string channelId, string channelName, string channelDescription, int importance, string groupId, bool showBadge, string sound) => 
            CleverTapUnitySDK.CleverTap.CreateNotificationChannelWithGroupAndSound(channelId, channelName, channelDescription, importance, groupId, showBadge, sound);

        public static void CreateNotificationChannelGroup(string groupId, string groupName) => 
            CleverTapUnitySDK.CleverTap.CreateNotificationChannelGroup(groupId, groupName);

        public static void DeleteNotificationChannel(string channelId) => 
            CleverTapUnitySDK.CleverTap.DeleteNotificationChannel(channelId);

        public static void DeleteNotificationChannelGroup(string groupId) => 
            CleverTapUnitySDK.CleverTap.DeleteNotificationChannelGroup(groupId);

        public static void InitializeInbox() => 
            CleverTapUnitySDK.CleverTap.InitializeInbox();

        public static void ShowAppInbox(string styleConfig) => 
            CleverTapUnitySDK.CleverTap.ShowAppInbox(styleConfig);

        public static void DismissAppInbox() => 
            CleverTapUnitySDK.CleverTap.DismissAppInbox();

        public static int GetInboxMessageCount() => 
            CleverTapUnitySDK.CleverTap.GetInboxMessageCount();

        public static int GetInboxMessageUnreadCount() => 
            CleverTapUnitySDK.CleverTap.GetInboxMessageUnreadCount();

        public static void DeleteInboxMessagesForIDs(string[] messageIds) => 
            CleverTapUnitySDK.CleverTap.DeleteInboxMessagesForIDs(messageIds);

        public static void DeleteInboxMessageForID(string messageId) => 
            CleverTapUnitySDK.CleverTap.DeleteInboxMessageForID(messageId);

        public static void MarkReadInboxMessagesForIDs(string[] messageIds) => 
            CleverTapUnitySDK.CleverTap.MarkReadInboxMessagesForIDs(messageIds);

        public static void MarkReadInboxMessageForID(string messageId) => 
            CleverTapUnitySDK.CleverTap.MarkReadInboxMessageForID(messageId);

        public static void PromptPushPrimer(Dictionary<string, object> json) => 
            CleverTapUnitySDK.CleverTap.PromptPushPrimer(json);

        public static void PromptForPushPermission(bool showFallbackSettings) => 
            CleverTapUnitySDK.CleverTap.PromptForPushPermission(showFallbackSettings);

        public static bool IsPushPermissionGranted() => 
            CleverTapUnitySDK.CleverTap.IsPushPermissionGranted();

#endif
    }
}
