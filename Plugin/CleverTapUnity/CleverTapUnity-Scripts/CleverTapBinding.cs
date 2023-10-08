using UnityEngine;
using System.Collections.Generic;
using CleverTap.Utilities;
using System;

/// <summary>
/// These methods can be called by Unity applications to record
/// events and set and get user profile attributes.
/// </summary>

namespace CleverTap {
    public class CleverTapBinding : MonoBehaviour {

        public const string Version = "2.4.1";

#if UNITY_IOS
        
        [Obsolete("Please use CleverTapUnitySDK.CleverTap.LaunchWithCredentials(string, string) instead.")]
        public static void LaunchWithCredentials(string accountID, string token) =>
            CleverTapUnitySDK.CleverTap.LaunchWithCredentials(accountID, token);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.LaunchWithCredentialsForRegion(string, string, string) instead.")]
        public static void LaunchWithCredentialsForRegion(string accountID, string token, string region) =>
            CleverTapUnitySDK.CleverTap.LaunchWithCredentialsForRegion(accountID, token, region);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.OnUserLogin(Dictionary<string, string>) instead.")]
        public static void OnUserLogin(Dictionary<string, string> properties) =>
            CleverTapUnitySDK.CleverTap.OnUserLogin(properties);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfilePush(Dictionary<string, string>) instead.")]
        public static void ProfilePush(Dictionary<string, string> properties) =>
            CleverTapUnitySDK.CleverTap.ProfilePush(properties);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileGet(string) instead.")]
        public static string ProfileGet(string key) =>
            CleverTapUnitySDK.CleverTap.ProfileGet(key);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileGetCleverTapAttributionIdentifier() instead.")]
        public static string ProfileGetCleverTapAttributionIdentifier() =>
            CleverTapUnitySDK.CleverTap.ProfileGetCleverTapAttributionIdentifier();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileGetCleverTapID() instead.")]
        public static string ProfileGetCleverTapID() =>
            CleverTapUnitySDK.CleverTap.ProfileGetCleverTapID();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileRemoveValueForKey(string) instead.")]
        public static void ProfileRemoveValueForKey(string key) =>
            CleverTapUnitySDK.CleverTap.ProfileRemoveValueForKey(key);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileSetMultiValuesForKey(string, List<string>) instead.")]
        public static void ProfileSetMultiValuesForKey(string key, List<string> values) =>
            CleverTapUnitySDK.CleverTap.ProfileSetMultiValuesForKey(key, values);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileAddMultiValuesForKey(string, List<string>) instead.")]
        public static void ProfileAddMultiValuesForKey(string key, List<string> values) =>
            CleverTapUnitySDK.CleverTap.ProfileAddMultiValuesForKey(key, values);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileRemoveMultiValuesForKey(string, List<string>) instead.")]
        public static void ProfileRemoveMultiValuesForKey(string key, List<string> values) =>
            CleverTapUnitySDK.CleverTap.ProfileRemoveMultiValuesForKey(key, values);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileAddMultiValueForKey(string, string) instead.")]
        public static void ProfileAddMultiValueForKey(string key, string val) =>
            CleverTapUnitySDK.CleverTap.ProfileAddMultiValueForKey(key, val);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileRemoveMultiValueForKey(string, string) instead.")]
        public static void ProfileRemoveMultiValueForKey(string key, string val) =>
            CleverTapUnitySDK.CleverTap.ProfileRemoveMultiValueForKey(key, val);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileIncrementValueForKey(string, double) instead.")]
        public static void ProfileIncrementValueForKey(string key, double val) =>
            CleverTapUnitySDK.CleverTap.ProfileIncrementValueForKey(key, val);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileIncrementValueForKey(string, int) instead.")]
        public static void ProfileIncrementValueForKey(string key, int val) =>
            CleverTapUnitySDK.CleverTap.ProfileIncrementValueForKey(key, val);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileDecrementValueForKey(string, double) instead.")]
        public static void ProfileDecrementValueForKey(string key, double val) =>
            CleverTapUnitySDK.CleverTap.ProfileDecrementValueForKey(key, val);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileDecrementValueForKey(string, int) instead.")]
        public static void ProfileDecrementValueForKey(string key, int val) =>
            CleverTapUnitySDK.CleverTap.ProfileDecrementValueForKey(key, val);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.SuspendInAppNotifications() instead.")]
        public static void SuspendInAppNotifications() =>
            CleverTapUnitySDK.CleverTap.SuspendInAppNotifications();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.DiscardInAppNotifications() instead.")]
        public static void DiscardInAppNotifications() =>
            CleverTapUnitySDK.CleverTap.DiscardInAppNotifications();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ResumeInAppNotifications() instead.")]
        public static void ResumeInAppNotifications() =>
            CleverTapUnitySDK.CleverTap.ResumeInAppNotifications();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.GetCleverTapID() instead.")]
        public static string GetCleverTapID() =>
            CleverTapUnitySDK.CleverTap.GetCleverTapID();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.RecordScreenView(string) instead.")]
        public static void RecordScreenView(string screenName) =>
            CleverTapUnitySDK.CleverTap.RecordScreenView(screenName);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.RecordEvent(string) instead.")]
        public static void RecordEvent(string eventName) =>
            CleverTapUnitySDK.CleverTap.RecordEvent(eventName);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.RecordEvent(string, Dictionary<string, object>) instead.")]
        public static void RecordEvent(string eventName, Dictionary<string, object> properties) =>
            CleverTapUnitySDK.CleverTap.RecordEvent(eventName, properties);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.RecordChargedEventWithDetailsAndItems(Dictionary<string, object>, List<Dictionary<string, object>>) instead.")]
        public static void RecordChargedEventWithDetailsAndItems(Dictionary<string, object> details, List<Dictionary<string, object>> items) =>
            CleverTapUnitySDK.CleverTap.RecordChargedEventWithDetailsAndItems(details, items);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.EventGetFirstTime(string) instead.")]
        public static int EventGetFirstTime(string eventName) =>
            CleverTapUnitySDK.CleverTap.EventGetFirstTime(eventName);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.EventGetLastTime(string) instead.")]
        public static int EventGetLastTime(string eventName) =>
            CleverTapUnitySDK.CleverTap.EventGetLastTime(eventName);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.EventGetOccurrences(string) instead.")]
        public static int EventGetOccurrences(string eventName) =>
            CleverTapUnitySDK.CleverTap.EventGetOccurrences(eventName);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.UserGetEventHistory() instead.")]
        public static JSONClass UserGetEventHistory() =>
            CleverTapUnitySDK.CleverTap.UserGetEventHistory();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.SessionGetUTMDetails() instead.")]
        public static JSONClass SessionGetUTMDetails() =>
            CleverTapUnitySDK.CleverTap.SessionGetUTMDetails();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.SessionGetTimeElapsed() instead.")]
        public static int SessionGetTimeElapsed() =>
            CleverTapUnitySDK.CleverTap.SessionGetTimeElapsed();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.EventGetDetail(string eventName) instead.")]
        public static JSONClass EventGetDetail(string eventName) =>
            CleverTapUnitySDK.CleverTap.EventGetDetail(eventName);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.UserGetTotalVisits() instead.")]
        public static int UserGetTotalVisits() =>
            CleverTapUnitySDK.CleverTap.UserGetTotalVisits();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.UserGetScreenCount() instead.")]
        public static int UserGetScreenCount() =>
            CleverTapUnitySDK.CleverTap.UserGetScreenCount();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.UserGetPreviousVisitTime() instead.")]
        public static int UserGetPreviousVisitTime() =>
            CleverTapUnitySDK.CleverTap.UserGetPreviousVisitTime();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.RegisterPush() instead.")]
        public static void RegisterPush() =>
            CleverTapUnitySDK.CleverTap.RegisterPush();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.SetApplicationIconBadgeNumber(int) instead.")]
        public static void SetApplicationIconBadgeNumber(int num) =>
            CleverTapUnitySDK.CleverTap.SetApplicationIconBadgeNumber(num);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.SetDebugLevel(int) instead.")]
        public static void SetDebugLevel(int level) =>
            CleverTapUnitySDK.CleverTap.SetDebugLevel(level);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.EnablePersonalization() instead.")]
        public static void EnablePersonalization() =>
            CleverTapUnitySDK.CleverTap.EnablePersonalization();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.DisablePersonalization() instead.")]
        public static void DisablePersonalization() =>
            CleverTapUnitySDK.CleverTap.DisablePersonalization();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.SetLocation(double, double) instead.")]
        public static void SetLocation(double lat, double lon) =>
            CleverTapUnitySDK.CleverTap.SetLocation(lat, lon);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.PushInstallReferrerSource(string, string, string) instead.")]
        public static void PushInstallReferrerSource(string source, string medium, string campaign) =>
            CleverTapUnitySDK.CleverTap.PushInstallReferrerSource(source, medium, campaign);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.SetOffline(bool) instead.")]
        public static void SetOffline(bool enabled) =>
            CleverTapUnitySDK.CleverTap.SetOffline(enabled);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.SetOptOut(bool) instead.")]
        public static void SetOptOut(bool enabled) =>
            CleverTapUnitySDK.CleverTap.SetOptOut(enabled);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.EnableDeviceNetworkInfoReporting(bool) instead.")]
        public static void EnableDeviceNetworkInfoReporting(bool enabled) =>
            CleverTapUnitySDK.CleverTap.EnableDeviceNetworkInfoReporting(enabled);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.InitializeInbox() instead.")]
        public static void InitializeInbox() =>
            CleverTapUnitySDK.CleverTap.InitializeInbox();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ShowAppInbox(Dictionary<string, object>) instead.")]
        public static void ShowAppInbox(Dictionary<string, object> styleConfig) =>
            CleverTapUnitySDK.CleverTap.ShowAppInbox(styleConfig);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.DismissAppInbox() instead.")]
        public static void DismissAppInbox() =>
            CleverTapUnitySDK.CleverTap.DismissAppInbox();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.GetInboxMessageCount() instead.")]
        public static int GetInboxMessageCount() =>
            CleverTapUnitySDK.CleverTap.GetInboxMessageCount();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.GetInboxMessageUnreadCount() instead.")]
        public static int GetInboxMessageUnreadCount() =>
            CleverTapUnitySDK.CleverTap.GetInboxMessageUnreadCount();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.GetAllInboxMessages() instead.")]
        public static JSONArray GetAllInboxMessages() =>
            CleverTapUnitySDK.CleverTap.GetAllInboxMessages();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.GetUnreadInboxMessages() instead.")]
        public static JSONArray GetUnreadInboxMessages() =>
            CleverTapUnitySDK.CleverTap.GetUnreadInboxMessages();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.GetInboxMessageForId(string) instead.")]
        public static JSONClass GetInboxMessageForId(string messageId) =>
            CleverTapUnitySDK.CleverTap.GetInboxMessageForId(messageId);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.DeleteInboxMessageForID(string) instead.")]
        public static void DeleteInboxMessageForID(string messageId) =>
            CleverTapUnitySDK.CleverTap.DeleteInboxMessageForID(messageId);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.DeleteInboxMessagesForIDs(string[]) instead.")]
        public static void DeleteInboxMessagesForIDs(string[] messageIds) =>
            CleverTapUnitySDK.CleverTap.DeleteInboxMessagesForIDs(messageIds);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.MarkReadInboxMessageForID(string) instead.")]
        public static void MarkReadInboxMessageForID(string messageId) =>
            CleverTapUnitySDK.CleverTap.MarkReadInboxMessageForID(messageId);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.MarkReadInboxMessagesForIDs(string[]) instead.")]
        public static void MarkReadInboxMessagesForIDs(string[] messageIds) =>
            CleverTapUnitySDK.CleverTap.MarkReadInboxMessagesForIDs(messageIds);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.RecordInboxNotificationViewedEventForID(string) instead.")]
        public static void RecordInboxNotificationViewedEventForID(string messageId) =>
            CleverTapUnitySDK.CleverTap.RecordInboxNotificationViewedEventForID(messageId);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.RecordInboxNotificationClickedEventForID(string) instead.")]
        public static void RecordInboxNotificationClickedEventForID(string messageId) =>
            CleverTapUnitySDK.CleverTap.RecordInboxNotificationClickedEventForID(messageId);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.GetAllDisplayUnits() instead.")]
        public static JSONArray GetAllDisplayUnits() =>
            CleverTapUnitySDK.CleverTap.GetAllDisplayUnits();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.GetDisplayUnitForID(string) instead.")]
        public static JSONClass GetDisplayUnitForID(string unitID) =>
            CleverTapUnitySDK.CleverTap.GetDisplayUnitForID(unitID);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.RecordDisplayUnitViewedEventForID(string) instead.")]
        public static void RecordDisplayUnitViewedEventForID(string unitID) =>
            CleverTapUnitySDK.CleverTap.RecordDisplayUnitViewedEventForID(unitID);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.RecordDisplayUnitClickedEventForID(string) instead.")]
        public static void RecordDisplayUnitClickedEventForID(string unitID) =>
            CleverTapUnitySDK.CleverTap.RecordDisplayUnitClickedEventForID(unitID);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.FetchProductConfig() instead.")]
        public static void FetchProductConfig() =>
            CleverTapUnitySDK.CleverTap.FetchProductConfig();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.FetchProductConfigWithMinimumInterval(double) instead.")]
        public static void FetchProductConfigWithMinimumInterval(double minimumInterval) =>
            CleverTapUnitySDK.CleverTap.FetchProductConfigWithMinimumInterval(minimumInterval);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.SetProductConfigMinimumFetchInterval(double) instead.")]
        public static void SetProductConfigMinimumFetchInterval(double minimumFetchInterval) =>
            CleverTapUnitySDK.CleverTap.SetProductConfigMinimumFetchInterval(minimumFetchInterval);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ActivateProductConfig() instead.")]
        public static void ActivateProductConfig() =>
            CleverTapUnitySDK.CleverTap.ActivateProductConfig();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.FetchAndActivateProductConfig() instead.")]
        public static void FetchAndActivateProductConfig() =>
            CleverTapUnitySDK.CleverTap.FetchAndActivateProductConfig();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.SetProductConfigDefaults(Dictionary<string, object>) instead.")]
        public static void SetProductConfigDefaults(Dictionary<string, object> defaults) =>
            CleverTapUnitySDK.CleverTap.SetProductConfigDefaults(defaults);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.SetProductConfigDefaultsFromPlistFileName(string) instead.")]
        public static void SetProductConfigDefaultsFromPlistFileName(string fileName) =>
            CleverTapUnitySDK.CleverTap.SetProductConfigDefaultsFromPlistFileName(fileName);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.GetProductConfigValueFor(string) instead.")]
        public static JSONClass GetProductConfigValueFor(string key) =>
            CleverTapUnitySDK.CleverTap.GetProductConfigValueFor(key);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.GetProductConfigLastFetchTimeStamp() instead.")]
        public static double GetProductConfigLastFetchTimeStamp() =>
            CleverTapUnitySDK.CleverTap.GetProductConfigLastFetchTimeStamp();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ResetProductConfig() instead.")]
        public static void ResetProductConfig() =>
            CleverTapUnitySDK.CleverTap.ResetProductConfig();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.GetFeatureFlag(string, bool) instead.")]
        public static bool GetFeatureFlag(string key, bool defaultValue) =>
            CleverTapUnitySDK.CleverTap.GetFeatureFlag(key, defaultValue);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.PromptPushPrimer(Dictionary<string, object>) instead.")]
        public static void PromptPushPrimer(Dictionary<string, object> json) =>
            CleverTapUnitySDK.CleverTap.PromptPushPrimer(json);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.PromptForPushPermission(bool) instead.")]
        public static void PromptForPushPermission(bool showFallbackSettings) =>
            CleverTapUnitySDK.CleverTap.PromptForPushPermission(showFallbackSettings);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.IsPushPermissionGranted() instead.")]
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

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.SetDebugLevel(int) instead.")]
        public static void SetDebugLevel(int level) =>
            CleverTapUnitySDK.CleverTap.SetDebugLevel(level);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.LaunchWithCredentials(string, string) instead.")]
        public static void Initialize(string accountID, string accountToken) =>
            CleverTapUnitySDK.CleverTap.LaunchWithCredentials(accountID, accountToken);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.LaunchWithCredentialsForRegion(string, string, string) instead.")]
        public static void Initialize(string accountID, string accountToken, string region) =>
            CleverTapUnitySDK.CleverTap.LaunchWithCredentialsForRegion(accountID, accountToken, region);

        [Obsolete("Not supported for Android.")]
        public static void LaunchWithCredentials(string accountID, string token) {
            //no op only supported on ios
        }

        [Obsolete("Not supported for Android.")]
        public static void LaunchWithCredentials(string accountID, string token, string region) {            
            //no op only supported on ios
        }

        [Obsolete("Not supported for Android.")]
        public static void RegisterPush() {
            //no op only supported on ios
        }

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.CreateNotificationChannel(string, string, string, int, bool) instead.")]
        public static void CreateNotificationChannel(string channelId, string channelName, string channelDescription, int importance, bool showBadge) =>
            CleverTapUnitySDK.CleverTap.CreateNotificationChannel(channelId, channelName, channelDescription, importance, showBadge);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.CreateNotificationChannelWithSound(string, string, string, int, bool, string) instead.")]
        public static void CreateNotificationChannelWithSound(string channelId, string channelName, string channelDescription, int importance, bool showBadge, string sound) =>
            CleverTapUnitySDK.CleverTap.CreateNotificationChannelWithSound(channelId, channelName, channelDescription, importance, showBadge, sound);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.CreateNotificationChannelWithGroup(string, string, string, int, string, bool) instead.")]
        public static void CreateNotificationChannelWithGroup(string channelId, string channelName, string channelDescription, int importance, string groupId, bool showBadge) =>
            CleverTapUnitySDK.CleverTap.CreateNotificationChannelWithGroup(channelId, channelName, channelDescription, importance, groupId, showBadge);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.CreateNotificationChannelWithGroupAndSound(string, string, string, int, string, bool, string) instead.")]
        public static void CreateNotificationChannelWithGroupAndSound(string channelId, string channelName, string channelDescription, int importance, string groupId, bool showBadge, string sound) =>
            CleverTapUnitySDK.CleverTap.CreateNotificationChannelWithGroupAndSound(channelId, channelName, channelDescription, importance, groupId, showBadge, sound);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.CreateNotificationChannelGroup(string, string) instead.")]
        public static void CreateNotificationChannelGroup(string groupId, string groupName) =>
            CleverTapUnitySDK.CleverTap.CreateNotificationChannelGroup(groupId, groupName);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.DeleteNotificationChannel(string) instead.")]
        public static void DeleteNotificationChannel(string channelId) =>
            CleverTapUnitySDK.CleverTap.DeleteNotificationChannel(channelId);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.DeleteNotificationChannelGroup(string) instead.")]
        public static void DeleteNotificationChannelGroup(string groupId) =>
            CleverTapUnitySDK.CleverTap.DeleteNotificationChannelGroup(groupId);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.SetOptOut(bool) instead.")]
        public static void SetOptOut(bool value) =>
            CleverTapUnitySDK.CleverTap.SetOptOut(value);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.SetOffline(bool) instead.")]
        public static void SetOffline(bool value) =>
            CleverTapUnitySDK.CleverTap.SetOffline(value);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.EnableDeviceNetworkInfoReporting(bool) instead.")]
        public static void EnableDeviceNetworkInfoReporting(bool value) =>
            CleverTapUnitySDK.CleverTap.EnableDeviceNetworkInfoReporting(value);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.EnablePersonalization() instead.")]
        public static void EnablePersonalization() =>
            CleverTapUnitySDK.CleverTap.EnablePersonalization();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.DisablePersonalization() instead.")]
        public static void DisablePersonalization() =>
            CleverTapUnitySDK.CleverTap.DisablePersonalization();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.SetLocation(double, double) instead.")]
        public static void SetLocation(double lat, double lon) =>
            CleverTapUnitySDK.CleverTap.SetLocation(lat, lon);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.OnUserLogin(Dictionary<string, string>) instead.")]
        public static void OnUserLogin(Dictionary<string, string> properties) =>
            CleverTapUnitySDK.CleverTap.OnUserLogin(properties);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfilePush(Dictionary<string, string>) instead.")]
        public static void ProfilePush(Dictionary<string, string> properties) =>
            CleverTapUnitySDK.CleverTap.ProfilePush(properties);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileGet(string) instead.")]
        public static string ProfileGet(string key) =>
            CleverTapUnitySDK.CleverTap.ProfileGet(key);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileGetCleverTapAttributionIdentifier() instead.")]
        public static string ProfileGetCleverTapAttributionIdentifier() =>
            CleverTapUnitySDK.CleverTap.ProfileGetCleverTapAttributionIdentifier();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileGetCleverTapID() instead.")]
        public static string ProfileGetCleverTapID() =>
            CleverTapUnitySDK.CleverTap.ProfileGetCleverTapID();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.GetCleverTapId() instead.")]
        public static void GetCleverTapId() =>
            CleverTapUnitySDK.CleverTap.GetCleverTapID();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileIncrementValueForKey(string, double) instead.")]
        public static void ProfileIncrementValueForKey(string key, double val) =>
            CleverTapUnitySDK.CleverTap.ProfileIncrementValueForKey(key, val);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileIncrementValueForKey(string, int) instead.")]
        public static void ProfileIncrementValueForKey(string key, int val) =>
            CleverTapUnitySDK.CleverTap.ProfileIncrementValueForKey(key, val);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileDecrementValueForKey(string, double) instead.")]
        public static void ProfileDecrementValueForKey(string key, double val) =>
            CleverTapUnitySDK.CleverTap.ProfileDecrementValueForKey(key, val);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileDecrementValueForKey(string, int) instead.")]
        public static void ProfileDecrementValueForKey(string key, int val) =>
            CleverTapUnitySDK.CleverTap.ProfileDecrementValueForKey(key, val);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.SuspendInAppNotifications() instead.")]
        public static void SuspendInAppNotifications() =>
            CleverTapUnitySDK.CleverTap.SuspendInAppNotifications();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.DiscardInAppNotifications() instead.")]
        public static void DiscardInAppNotifications() =>
            CleverTapUnitySDK.CleverTap.DiscardInAppNotifications();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ResumeInAppNotifications() instead.")]
        public static void ResumeInAppNotifications() =>
            CleverTapUnitySDK.CleverTap.ResumeInAppNotifications();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileRemoveValueForKey(string) instead.")]
        public static void ProfileRemoveValueForKey(string key) =>
            CleverTapUnitySDK.CleverTap.ProfileRemoveValueForKey(key);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileSetMultiValuesForKey(string, List<string>) instead.")]
        public static void ProfileSetMultiValuesForKey(string key, List<string> values) =>
            CleverTapUnitySDK.CleverTap.ProfileSetMultiValuesForKey(key, values);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileAddMultiValuesForKey(string, List<string>) instead.")]
        public static void ProfileAddMultiValuesForKey(string key, List<string> values) =>
            CleverTapUnitySDK.CleverTap.ProfileAddMultiValuesForKey(key, values);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileRemoveMultiValuesForKey(string, List<string>) instead.")]
        public static void ProfileRemoveMultiValuesForKey(string key, List<string> values) =>
            CleverTapUnitySDK.CleverTap.ProfileRemoveMultiValuesForKey(key, values);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileAddMultiValueForKey(string, string) instead.")]
        public static void ProfileAddMultiValueForKey(string key, string val) =>
            CleverTapUnitySDK.CleverTap.ProfileAddMultiValueForKey(key, val);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileRemoveMultiValueForKey(string, string) instead.")]
        public static void ProfileRemoveMultiValueForKey(string key, string val) =>
            CleverTapUnitySDK.CleverTap.ProfileRemoveMultiValueForKey(key, val);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.RecordScreenView(string) instead.")]
        public static void RecordScreenView(string screenName) =>
            CleverTapUnitySDK.CleverTap.RecordScreenView(screenName);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.RecordEvent(string) instead.")]
        public static void RecordEvent(string eventName) =>
            CleverTapUnitySDK.CleverTap.RecordEvent(eventName);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.RecordEvent(string, Dictionary<string, object>) instead.")]
        public static void RecordEvent(string eventName, Dictionary<string, object> properties) =>
            CleverTapUnitySDK.CleverTap.RecordEvent(eventName, properties);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.RecordChargedEventWithDetailsAndItems(Dictionary<string, object>, List<Dictionary<string, object>>) instead.")]
        public static void RecordChargedEventWithDetailsAndItems(Dictionary<string, object> details, List<Dictionary<string, object>> items) =>
            CleverTapUnitySDK.CleverTap.RecordChargedEventWithDetailsAndItems(details, items);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.EventGetFirstTime(string) instead.")]
        public static int EventGetFirstTime(string eventName) =>
            CleverTapUnitySDK.CleverTap.EventGetFirstTime(eventName);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.EventGetLastTime(string) instead.")]
        public static int EventGetLastTime(string eventName) =>
            CleverTapUnitySDK.CleverTap.EventGetLastTime(eventName);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.EventGetOccurrences(string) instead.")]
        public static int EventGetOccurrences(string eventName) =>
            CleverTapUnitySDK.CleverTap.EventGetOccurrences(eventName);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.EventGetDetail(string) instead.")]
        public static JSONClass EventGetDetail(string eventName) =>
            CleverTapUnitySDK.CleverTap.EventGetDetail(eventName);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.UserGetEventHistory() instead.")]
        public static JSONClass UserGetEventHistory() =>
            CleverTapUnitySDK.CleverTap.UserGetEventHistory();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.SessionGetUTMDetails() instead.")]
        public static JSONClass SessionGetUTMDetails() =>
            CleverTapUnitySDK.CleverTap.SessionGetUTMDetails();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.SessionGetTimeElapsed() instead.")]
        public static int SessionGetTimeElapsed() =>
            CleverTapUnitySDK.CleverTap.SessionGetTimeElapsed();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.UserGetTotalVisits() instead.")]
        public static int UserGetTotalVisits() =>
            CleverTapUnitySDK.CleverTap.UserGetTotalVisits();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.UserGetScreenCount() instead.")]
        public static int UserGetScreenCount() =>
            CleverTapUnitySDK.CleverTap.UserGetScreenCount();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.UserGetPreviousVisitTime() instead.")]
        public static int UserGetPreviousVisitTime() =>
            CleverTapUnitySDK.CleverTap.UserGetPreviousVisitTime();

        [Obsolete("Not supported for Android.")]
        public static void SetApplicationIconBadgeNumber(int num) {
            // no-op for Android
        }

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.PushInstallReferrerSource(string, string, string) instead.")]
        public static void PushInstallReferrerSource(string source, string medium, string campaign) =>
            CleverTapUnitySDK.CleverTap.PushInstallReferrerSource(source, medium, campaign);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.InitializeInbox() instead.")]
        public static void InitializeInbox() =>
            CleverTapUnitySDK.CleverTap.InitializeInbox();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ShowAppInbox(string) instead.")]
        public static void ShowAppInbox(string styleConfig) =>
            CleverTapUnitySDK.CleverTap.ShowAppInbox(styleConfig);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.DismissAppInbox() instead.")]
        public static void DismissAppInbox() =>
            CleverTapUnitySDK.CleverTap.DismissAppInbox();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.GetInboxMessageCount() instead.")]
        public static int GetInboxMessageCount() =>
            CleverTapUnitySDK.CleverTap.GetInboxMessageCount();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.DeleteInboxMessagesForIDs(string[]) instead.")]
        public static void DeleteInboxMessagesForIDs(string[] messageIds) =>
            CleverTapUnitySDK.CleverTap.DeleteInboxMessagesForIDs(messageIds);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.DeleteInboxMessageForID(string) instead.")]
        public static void DeleteInboxMessageForID(string messageId) =>
            CleverTapUnitySDK.CleverTap.DeleteInboxMessageForID(messageId);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.MarkReadInboxMessagesForIDs(string[]) instead.")]
        public static void MarkReadInboxMessagesForIDs(string[] messageIds) =>
            CleverTapUnitySDK.CleverTap.MarkReadInboxMessagesForIDs(messageIds);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.MarkReadInboxMessageForID(string) instead.")]
        public static void MarkReadInboxMessageForID(string messageId) =>
            CleverTapUnitySDK.CleverTap.MarkReadInboxMessageForID(messageId);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.GetInboxMessageUnreadCount() instead. ")]
        public static int GetInboxMessageUnreadCount() =>
            CleverTapUnitySDK.CleverTap.GetInboxMessageUnreadCount();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.PromptPushPrimer(Dictionary<string, object>) instead.")]
        public static void PromptPushPrimer(Dictionary<string, object> details) =>
            CleverTapUnitySDK.CleverTap.PromptPushPrimer(details);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.PromptForPushPermission(bool) instead.")]
        public static void PromptForPushPermission(bool showFallbackSettings) =>
            CleverTapUnitySDK.CleverTap.PromptForPushPermission(showFallbackSettings);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.IsPushPermissionGranted() instead.")]
        public static bool IsPushPermissionGranted() =>
            CleverTapUnitySDK.CleverTap.IsPushPermissionGranted();

#else

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.LaunchWithCredentialsForRegion(string, string, string) instead.")]
        public static void LaunchWithCredentials(string accountID, string token, string region) =>
            CleverTapUnitySDK.CleverTap.LaunchWithCredentialsForRegion(accountID, token, region);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.OnUserLogin(Dictionary<string, string>) instead.")]
        public static void OnUserLogin(Dictionary<string, string> properties) =>
            CleverTapUnitySDK.CleverTap.OnUserLogin(properties);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfilePush(Dictionary<string, string>) instead.")]
        public static void ProfilePush(Dictionary<string, string> properties) =>
            CleverTapUnitySDK.CleverTap.ProfilePush(properties);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileGet(string) instead.")]
        public static string ProfileGet(string key) =>
            CleverTapUnitySDK.CleverTap.ProfileGet(key);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileGetCleverTapAttributionIdentifier() instead.")]
        public static string ProfileGetCleverTapAttributionIdentifier() =>
            CleverTapUnitySDK.CleverTap.ProfileGetCleverTapAttributionIdentifier();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileGetCleverTapID() instead.")]
        public static string ProfileGetCleverTapID() =>
            CleverTapUnitySDK.CleverTap.ProfileGetCleverTapID();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileRemoveValueForKey(string) instead.")]
        public static void ProfileRemoveValueForKey(string key) =>
            CleverTapUnitySDK.CleverTap.ProfileRemoveValueForKey(key);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileSetMultiValuesForKey(string, List<string>) instead.")]
        public static void ProfileSetMultiValuesForKey(string key, List<string> values) =>
            CleverTapUnitySDK.CleverTap.ProfileSetMultiValuesForKey(key, values);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileAddMultiValuesForKey(string, List<string>) instead.")]
        public static void ProfileAddMultiValuesForKey(string key, List<string> values) =>
            CleverTapUnitySDK.CleverTap.ProfileAddMultiValuesForKey(key, values);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileRemoveMultiValuesForKey(string, List<string>) instead.")]
        public static void ProfileRemoveMultiValuesForKey(string key, List<string> values) =>
            CleverTapUnitySDK.CleverTap.ProfileRemoveMultiValuesForKey(key, values);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileAddMultiValueForKey(string, string) instead.")]
        public static void ProfileAddMultiValueForKey(string key, string val) =>
            CleverTapUnitySDK.CleverTap.ProfileAddMultiValueForKey(key, val);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileRemoveMultiValueForKey(string, string) instead.")]
        public static void ProfileRemoveMultiValueForKey(string key, string val) =>
            CleverTapUnitySDK.CleverTap.ProfileRemoveMultiValueForKey(key, val);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileIncrementValueForKey(string, double) instead.")]
        public static void ProfileIncrementValueForKey(string key, double val) =>
            CleverTapUnitySDK.CleverTap.ProfileIncrementValueForKey(key, val);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileIncrementValueForKey(string, int) instead.")]
        public static void ProfileIncrementValueForKey(string key, int val) =>
            CleverTapUnitySDK.CleverTap.ProfileIncrementValueForKey(key, val);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileDecrementValueForKey(string, double) instead.")]
        public static void ProfileDecrementValueForKey(string key, double val) =>
            CleverTapUnitySDK.CleverTap.ProfileDecrementValueForKey(key, val);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ProfileDecrementValueForKey(string, int) instead.")]
        public static void ProfileDecrementValueForKey(string key, int val) =>
            CleverTapUnitySDK.CleverTap.ProfileDecrementValueForKey(key, val);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.SuspendInAppNotifications() instead.")]
        public static void SuspendInAppNotifications() =>
            CleverTapUnitySDK.CleverTap.SuspendInAppNotifications();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.DiscardInAppNotifications() instead.")]
        public static void DiscardInAppNotifications() =>
            CleverTapUnitySDK.CleverTap.DiscardInAppNotifications();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ResumeInAppNotifications() instead.")]
        public static void ResumeInAppNotifications() =>
            CleverTapUnitySDK.CleverTap.ResumeInAppNotifications();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.GetCleverTapID() instead.")]
        public static string GetCleverTapID() =>
            CleverTapUnitySDK.CleverTap.GetCleverTapID();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.RecordScreenView(string) instead.")]
        public static void RecordScreenView(string screenName) =>
            CleverTapUnitySDK.CleverTap.RecordScreenView(screenName);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.RecordEvent(string) instead.")]
        public static void RecordEvent(string eventName) =>
            CleverTapUnitySDK.CleverTap.RecordEvent(eventName);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.RecordEvent(string, Dictionary<string, object>) instead.")]
        public static void RecordEvent(string eventName, Dictionary<string, object> properties) =>
            CleverTapUnitySDK.CleverTap.RecordEvent(eventName, properties);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.RecordChargedEventWithDetailsAndItems(Dictionary<string, object>, List<Dictionary<string, object>>) instead.")]
        public static void RecordChargedEventWithDetailsAndItems(Dictionary<string, object> details, List<Dictionary<string, object>> items) =>
            CleverTapUnitySDK.CleverTap.RecordChargedEventWithDetailsAndItems(details, items);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.EventGetFirstTime(string) instead.")]
        public static int EventGetFirstTime(string eventName) =>
            CleverTapUnitySDK.CleverTap.EventGetFirstTime(eventName);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.EventGetLastTime(string) instead.")]
        public static int EventGetLastTime(string eventName) =>
            CleverTapUnitySDK.CleverTap.EventGetLastTime(eventName);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.EventGetOccurrences(string) instead.")]
        public static int EventGetOccurrences(string eventName) =>
            CleverTapUnitySDK.CleverTap.EventGetOccurrences(eventName);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.EventGetDetail(string) instead.")]
        public static JSONClass EventGetDetail(string eventName) =>
            CleverTapUnitySDK.CleverTap.EventGetDetail(eventName);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.UserGetEventHistory() instead.")]
        public static JSONClass UserGetEventHistory() =>
            CleverTapUnitySDK.CleverTap.UserGetEventHistory();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.SessionGetUTMDetails() instead.")]
        public static JSONClass SessionGetUTMDetails() =>
            CleverTapUnitySDK.CleverTap.SessionGetUTMDetails();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.SessionGetTimeElapsed() instead.")]
        public static int SessionGetTimeElapsed() =>
            CleverTapUnitySDK.CleverTap.SessionGetTimeElapsed();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.UserGetTotalVisits() instead.")]
        public static int UserGetTotalVisits() =>
            CleverTapUnitySDK.CleverTap.UserGetTotalVisits();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.UserGetScreenCount() instead.")]
        public static int UserGetScreenCount() =>
            CleverTapUnitySDK.CleverTap.UserGetScreenCount();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.UserGetPreviousVisitTime() instead.")]
        public static int UserGetPreviousVisitTime() =>
            CleverTapUnitySDK.CleverTap.UserGetPreviousVisitTime();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.EnablePersonalization() instead.")]
        public static void EnablePersonalization() =>
            CleverTapUnitySDK.CleverTap.EnablePersonalization();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.DisablePersonalization() instead.")]
        public static void DisablePersonalization() =>
            CleverTapUnitySDK.CleverTap.DisablePersonalization();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.RegisterPush() instead.")]
        public static void RegisterPush() =>
            CleverTapUnitySDK.CleverTap.RegisterPush();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.SetApplicationIconBadgeNumber(int) instead.")]
        public static void SetApplicationIconBadgeNumber(int num) =>
            CleverTapUnitySDK.CleverTap.SetApplicationIconBadgeNumber(num);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.SetDebugLevel(int) instead.")]
        public static void SetDebugLevel(int level) =>
            CleverTapUnitySDK.CleverTap.SetDebugLevel(level);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.SetLocation(double, double) instead.")]
        public static void SetLocation(double lat, double lon) =>
            CleverTapUnitySDK.CleverTap.SetLocation(lat, lon);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.PushInstallReferrerSource(string, string, string) instead.")]
        public static void PushInstallReferrerSource(string source, string medium, string campaign) =>
            CleverTapUnitySDK.CleverTap.PushInstallReferrerSource(source, medium, campaign);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.EnableDeviceNetworkInfoReporting(bool) instead.")]
        public static void EnableDeviceNetworkInfoReporting(bool value) =>
            CleverTapUnitySDK.CleverTap.EnableDeviceNetworkInfoReporting(value);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.SetOptOut(bool) instead.")]
        public static void SetOptOut(bool value) =>
            CleverTapUnitySDK.CleverTap.SetOptOut(value);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.SetOffline(bool) instead.")]
        public static void SetOffline(bool value) =>
            CleverTapUnitySDK.CleverTap.SetOffline(value);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.CreateNotificationChannel(string, string, string, int, bool) instead.")]
        public static void CreateNotificationChannel(string channelId, string channelName, string channelDescription, int importance, bool showBadge) =>
            CleverTapUnitySDK.CleverTap.CreateNotificationChannel(channelId, channelName, channelDescription, importance, showBadge);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.CreateNotificationChannelWithSound(string, string, string, int, bool, string) instead.")]
        public static void CreateNotificationChannelWithSound(string channelId, string channelName, string channelDescription, int importance, bool showBadge, string sound) =>
            CleverTapUnitySDK.CleverTap.CreateNotificationChannelWithSound(channelId, channelName, channelDescription, importance, showBadge, sound);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.CreateNotificationChannelWithGroup(string, string, string, int, string, bool) instead.")]
        public static void CreateNotificationChannelWithGroup(string channelId, string channelName, string channelDescription, int importance, string groupId, bool showBadge) =>
            CleverTapUnitySDK.CleverTap.CreateNotificationChannelWithGroup(channelId, channelName, channelDescription, importance, groupId, showBadge);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.CreateNotificationChannelWithGroupAndSound(string, string, string, int, string, bool, string) instead.")]
        public static void CreateNotificationChannelWithGroupAndSound(string channelId, string channelName, string channelDescription, int importance, string groupId, bool showBadge, string sound) =>
            CleverTapUnitySDK.CleverTap.CreateNotificationChannelWithGroupAndSound(channelId, channelName, channelDescription, importance, groupId, showBadge, sound);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.CreateNotificationChannelGroup(string, string) instead.")]
        public static void CreateNotificationChannelGroup(string groupId, string groupName) =>
            CleverTapUnitySDK.CleverTap.CreateNotificationChannelGroup(groupId, groupName);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.DeleteNotificationChannel(string) instead.")]
        public static void DeleteNotificationChannel(string channelId) =>
            CleverTapUnitySDK.CleverTap.DeleteNotificationChannel(channelId);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.DeleteNotificationChannelGroup(string) instead.")]
        public static void DeleteNotificationChannelGroup(string groupId) =>
            CleverTapUnitySDK.CleverTap.DeleteNotificationChannelGroup(groupId);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.InitializeInbox() instead.")]
        public static void InitializeInbox() =>
            CleverTapUnitySDK.CleverTap.InitializeInbox();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.ShowAppInbox(string) instead.")]
        public static void ShowAppInbox(string styleConfig) =>
            CleverTapUnitySDK.CleverTap.ShowAppInbox(styleConfig);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.DismissAppInbox() instead.")]
        public static void DismissAppInbox() =>
            CleverTapUnitySDK.CleverTap.DismissAppInbox();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.GetInboxMessageCount() instead.")]
        public static int GetInboxMessageCount() =>
            CleverTapUnitySDK.CleverTap.GetInboxMessageCount();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.GetInboxMessageUnreadCount() instead.")]
        public static int GetInboxMessageUnreadCount() =>
            CleverTapUnitySDK.CleverTap.GetInboxMessageUnreadCount();

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.DeleteInboxMessagesForIDs(string[]) instead.")]
        public static void DeleteInboxMessagesForIDs(string[] messageIds) =>
            CleverTapUnitySDK.CleverTap.DeleteInboxMessagesForIDs(messageIds);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.DeleteInboxMessageForID(string) instead.")]
        public static void DeleteInboxMessageForID(string messageId) =>
            CleverTapUnitySDK.CleverTap.DeleteInboxMessageForID(messageId);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.MarkReadInboxMessagesForIDs(string[]) instead.")]
        public static void MarkReadInboxMessagesForIDs(string[] messageIds) =>
            CleverTapUnitySDK.CleverTap.MarkReadInboxMessagesForIDs(messageIds);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.MarkReadInboxMessageForID(string) instead.")]
        public static void MarkReadInboxMessageForID(string messageId) =>
            CleverTapUnitySDK.CleverTap.MarkReadInboxMessageForID(messageId);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.PromptPushPrimer(Dictionary<string, object>) instead.")]
        public static void PromptPushPrimer(Dictionary<string, object> json) =>
            CleverTapUnitySDK.CleverTap.PromptPushPrimer(json);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.PromptForPushPermission(bool) instead.")]
        public static void PromptForPushPermission(bool showFallbackSettings) =>
            CleverTapUnitySDK.CleverTap.PromptForPushPermission(showFallbackSettings);

        [Obsolete("Please use CleverTapUnitySDK.CleverTap.IsPushPermissionGranted() instead.")]
        public static bool IsPushPermissionGranted() =>
            CleverTapUnitySDK.CleverTap.IsPushPermissionGranted();

#endif
    }
}
