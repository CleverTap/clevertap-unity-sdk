﻿using CleverTap.Utilities;
using CleverTapUnitySDK.Common;
using System.Collections.Generic;

namespace CleverTapUnitySDK {
    public static class CleverTap {
        
        private static CleverTapPlatformBindings cleverTapBinding = BindingFactory.CleverTapBinding;
        private static CleverTapCallbackHandler cleverTapCallbackHandler = BindingFactory.CleverTapBinding.CallbackHandler;

        #region Events - CleverTap Callback Handler

        public static event CleverTapCallbackWithMessageDelegate OnCleverTapDeepLinkCallback {
            add => cleverTapCallbackHandler.OnCleverTapDeepLinkCallback += value;
            remove => cleverTapCallbackHandler.OnCleverTapDeepLinkCallback -= value;
        }

        public static event CleverTapCallbackWithMessageDelegate OnCleverTapProfileInitializedCallback {
            add => cleverTapCallbackHandler.OnCleverTapProfileInitializedCallback += value;
            remove => cleverTapCallbackHandler.OnCleverTapProfileInitializedCallback -= value;
        }

        public static event CleverTapCallbackWithMessageDelegate OnCleverTapProfileUpdatesCallback {
            add => cleverTapCallbackHandler.OnCleverTapProfileUpdatesCallback += value;
            remove => cleverTapCallbackHandler.OnCleverTapProfileUpdatesCallback -= value;
        }

        public static event CleverTapCallbackWithMessageDelegate OnCleverTapPushOpenedCallback {
            add => cleverTapCallbackHandler.OnCleverTapPushOpenedCallback += value;
            remove => cleverTapCallbackHandler.OnCleverTapPushOpenedCallback -= value;
        }

        public static event CleverTapCallbackWithMessageDelegate OnCleverTapInitCleverTapIdCallback {
            add => cleverTapCallbackHandler.OnCleverTapInitCleverTapIdCallback += value;
            remove => cleverTapCallbackHandler.OnCleverTapInitCleverTapIdCallback -= value;
        }

        public static event CleverTapCallbackWithMessageDelegate OnCleverTapInAppNotificationDismissedCallback {
            add => cleverTapCallbackHandler.OnCleverTapInAppNotificationDismissedCallback += value;
            remove => cleverTapCallbackHandler.OnCleverTapInAppNotificationDismissedCallback -= value;
        }

        public static event CleverTapCallbackWithMessageDelegate OnCleverTapInAppNotificationShowCallback {
            add => cleverTapCallbackHandler.OnCleverTapInAppNotificationShowCallback += value;
            remove => cleverTapCallbackHandler.OnCleverTapInAppNotificationShowCallback -= value;
        }

        public static event CleverTapCallbackWithMessageDelegate OnCleverTapOnPushPermissionResponseCallback {
            add => cleverTapCallbackHandler.OnCleverTapOnPushPermissionResponseCallback += value;
            remove => cleverTapCallbackHandler.OnCleverTapOnPushPermissionResponseCallback -= value;
        }

        public static event CleverTapCallbackWithMessageDelegate OnCleverTapInAppNotificationButtonTapped {
            add => cleverTapCallbackHandler.OnCleverTapInAppNotificationButtonTapped += value;
            remove => cleverTapCallbackHandler.OnCleverTapInAppNotificationButtonTapped -= value;
        }

        public static event CleverTapCallbackDelegate OnCleverTapInboxDidInitializeCallback {
            add => cleverTapCallbackHandler.OnCleverTapInboxDidInitializeCallback += value;
            remove => cleverTapCallbackHandler.OnCleverTapInboxDidInitializeCallback -= value;
        }

        public static event CleverTapCallbackDelegate OnCleverTapInboxMessagesDidUpdateCallback {
            add => cleverTapCallbackHandler.OnCleverTapInboxMessagesDidUpdateCallback += value;
            remove => cleverTapCallbackHandler.OnCleverTapInboxMessagesDidUpdateCallback -= value;
        }

        public static event CleverTapCallbackWithMessageDelegate OnCleverTapInboxCustomExtrasButtonSelect {
            add => cleverTapCallbackHandler.OnCleverTapInboxCustomExtrasButtonSelect += value;
            remove => cleverTapCallbackHandler.OnCleverTapInboxCustomExtrasButtonSelect -= value;
        }

        public static event CleverTapCallbackWithMessageDelegate OnCleverTapInboxItemClicked {
            add => cleverTapCallbackHandler.OnCleverTapInboxItemClicked += value;
            remove => cleverTapCallbackHandler.OnCleverTapInboxItemClicked -= value;
        }

        public static event CleverTapCallbackWithMessageDelegate OnCleverTapNativeDisplayUnitsUpdated {
            add => cleverTapCallbackHandler.OnCleverTapNativeDisplayUnitsUpdated += value;
            remove => cleverTapCallbackHandler.OnCleverTapNativeDisplayUnitsUpdated -= value;
        }

        public static event CleverTapCallbackWithMessageDelegate OnCleverTapProductConfigFetched {
            add => cleverTapCallbackHandler.OnCleverTapProductConfigFetched += value;
            remove => cleverTapCallbackHandler.OnCleverTapProductConfigFetched -= value;
        }

        public static event CleverTapCallbackWithMessageDelegate OnCleverTapProductConfigActivated {
            add => cleverTapCallbackHandler.OnCleverTapProductConfigActivated += value;
            remove => cleverTapCallbackHandler.OnCleverTapProductConfigActivated -= value;
        }

        public static event CleverTapCallbackWithMessageDelegate OnCleverTapProductConfigInitialized {
            add => cleverTapCallbackHandler.OnCleverTapProductConfigInitialized += value;
            remove => cleverTapCallbackHandler.OnCleverTapProductConfigInitialized -= value;
        }

        public static event CleverTapCallbackWithMessageDelegate OnCleverTapFeatureFlagsUpdated {
            add => cleverTapCallbackHandler.OnCleverTapFeatureFlagsUpdated += value;
            remove => cleverTapCallbackHandler.OnCleverTapFeatureFlagsUpdated -= value;
        }

        #endregion

        #region Methods - CleverTap Platform Bindings

        public static void ActivateProductConfig() =>
            cleverTapBinding.ActivateProductConfig();

        public static void CreateNotificationChannel(string channelId, string channelName, string channelDescription, int importance, bool showBadge) =>
            cleverTapBinding.CreateNotificationChannel(channelId, channelName, channelDescription, importance, showBadge);

        public static void CreateNotificationChannelGroup(string groupId, string groupName) =>
            cleverTapBinding.CreateNotificationChannelGroup(groupId, groupName);

        public static void CreateNotificationChannelWithGroup(string channelId, string channelName, string channelDescription, int importance, string groupId, bool showBadge) =>
            cleverTapBinding.CreateNotificationChannelWithGroup(channelId, channelName, channelDescription, importance, groupId, showBadge);

        public static void CreateNotificationChannelWithGroupAndSound(string channelId, string channelName, string channelDescription, int importance, string groupId, bool showBadge, string sound) =>
            cleverTapBinding.CreateNotificationChannelWithGroupAndSound(channelId, channelName, channelDescription, importance, groupId, showBadge, sound);

        public static void CreateNotificationChannelWithSound(string channelId, string channelName, string channelDescription, int importance, bool showBadge, string sound) =>
            cleverTapBinding.CreateNotificationChannelWithSound(channelId, channelName, channelDescription, importance, showBadge, sound);

        public static void DeleteInboxMessageForID(string messageId) =>
            cleverTapBinding.DeleteInboxMessageForID(messageId);

        public static void DeleteInboxMessagesForIDs(string[] messageIds) =>
            cleverTapBinding.DeleteInboxMessagesForIDs(messageIds);

        public static void DeleteNotificationChannel(string channelId) =>
            cleverTapBinding.DeleteNotificationChannel(channelId);

        public static void DeleteNotificationChannelGroup(string groupId) =>
            cleverTapBinding.DeleteNotificationChannelGroup(groupId);

        public static void DisablePersonalization() =>
            cleverTapBinding.DisablePersonalization();

        public static void DiscardInAppNotifications() =>
            cleverTapBinding.DiscardInAppNotifications();

        public static void DismissAppInbox() =>
            cleverTapBinding.DismissAppInbox();

        public static void EnableDeviceNetworkInfoReporting(bool enabled) =>
            cleverTapBinding.EnableDeviceNetworkInfoReporting(enabled);

        public static void EnablePersonalization() =>
            cleverTapBinding.EnablePersonalization();

        public static JSONClass EventGetDetail(string eventName) =>
            cleverTapBinding.EventGetDetail(eventName);

        public static int EventGetFirstTime(string eventName) =>
            cleverTapBinding.EventGetFirstTime(eventName);

        public static int EventGetLastTime(string eventName) =>
            cleverTapBinding.EventGetLastTime(eventName);

        public static int EventGetOccurrences(string eventName) =>
            cleverTapBinding.EventGetOccurrences(eventName);

        public static void FetchAndActivateProductConfig() =>
            cleverTapBinding.FetchAndActivateProductConfig();

        public static void FetchProductConfig() =>
            cleverTapBinding.FetchProductConfig();

        public static void FetchProductConfigWithMinimumInterval(double minimumInterval) =>
            cleverTapBinding.FetchProductConfigWithMinimumInterval(minimumInterval);

        public static JSONArray GetAllDisplayUnits() =>
            cleverTapBinding.GetAllDisplayUnits();

        public static JSONArray GetAllInboxMessages() =>
            cleverTapBinding.GetAllInboxMessages();

        public static string GetCleverTapID() =>
            cleverTapBinding.GetCleverTapID();

        public static JSONClass GetDisplayUnitForID(string unitID) =>
            cleverTapBinding.GetDisplayUnitForID(unitID);

        public static bool GetFeatureFlag(string key, bool defaultValue) =>
            cleverTapBinding.GetFeatureFlag(key, defaultValue);

        public static int GetInboxMessageCount() =>
            cleverTapBinding.GetInboxMessageCount();

        public static JSONClass GetInboxMessageForId(string messageId) =>
            cleverTapBinding.GetInboxMessageForId(messageId);

        public static int GetInboxMessageUnreadCount() =>
            cleverTapBinding.GetInboxMessageUnreadCount();

        public static double GetProductConfigLastFetchTimeStamp() =>
            cleverTapBinding.GetProductConfigLastFetchTimeStamp();

        public static JSONClass GetProductConfigValueFor(string key) =>
            cleverTapBinding.GetProductConfigValueFor(key);

        public static JSONArray GetUnreadInboxMessages() =>
            cleverTapBinding.GetUnreadInboxMessages();

        public static void InitializeInbox() =>
            cleverTapBinding.InitializeInbox();

        public static bool IsPushPermissionGranted() =>
            cleverTapBinding.IsPushPermissionGranted();

        public static void LaunchWithCredentials(string accountID, string token) =>
            cleverTapBinding.LaunchWithCredentials(accountID, token);

        public static void LaunchWithCredentialsForRegion(string accountID, string token, string region) =>
            cleverTapBinding.LaunchWithCredentialsForRegion(accountID, token, region);

        public static void MarkReadInboxMessageForID(string messageId) =>
            cleverTapBinding.MarkReadInboxMessageForID(messageId);

        public static void MarkReadInboxMessagesForIDs(string[] messageIds) =>
            cleverTapBinding.MarkReadInboxMessagesForIDs(messageIds);

        public static void OnUserLogin(Dictionary<string, string> properties) =>
            cleverTapBinding.OnUserLogin(properties);

        public static void ProfileAddMultiValueForKey(string key, string val) =>
            cleverTapBinding.ProfileAddMultiValueForKey(key, val);

        public static void ProfileAddMultiValuesForKey(string key, List<string> values) =>
            cleverTapBinding.ProfileAddMultiValuesForKey(key, values);

        public static void ProfileDecrementValueForKey(string key, double val) =>
            cleverTapBinding.ProfileDecrementValueForKey(key, val);

        public static void ProfileDecrementValueForKey(string key, int val) =>
            cleverTapBinding.ProfileDecrementValueForKey(key, val);

        public static string ProfileGet(string key) =>
            cleverTapBinding.ProfileGet(key);

        public static string ProfileGetCleverTapAttributionIdentifier() =>
            cleverTapBinding.ProfileGetCleverTapAttributionIdentifier();

        public static string ProfileGetCleverTapID() =>
            cleverTapBinding.ProfileGetCleverTapID();

        public static void ProfileIncrementValueForKey(string key, double val) =>
            cleverTapBinding.ProfileIncrementValueForKey(key, val);

        public static void ProfileIncrementValueForKey(string key, int val) =>
            cleverTapBinding.ProfileIncrementValueForKey(key, val);

        public static void ProfilePush(Dictionary<string, string> properties) =>
            cleverTapBinding.ProfilePush(properties);

        public static void ProfileRemoveMultiValueForKey(string key, string val) =>
            cleverTapBinding.ProfileRemoveMultiValueForKey(key, val);

        public static void ProfileRemoveMultiValuesForKey(string key, List<string> values) =>
            cleverTapBinding.ProfileRemoveMultiValuesForKey(key, values);

        public static void ProfileRemoveValueForKey(string key) =>
            cleverTapBinding.ProfileRemoveValueForKey(key);

        public static void ProfileSetMultiValuesForKey(string key, List<string> values) =>
            cleverTapBinding.ProfileSetMultiValuesForKey(key, values);

        public static void PromptForPushPermission(bool showFallbackSettings) =>
            cleverTapBinding.PromptForPushPermission(showFallbackSettings);

        public static void PromptPushPrimer(Dictionary<string, object> json) =>
            cleverTapBinding.PromptPushPrimer(json);

        public static void PushInstallReferrerSource(string source, string medium, string campaign) =>
            cleverTapBinding.PushInstallReferrerSource(source, medium, campaign);

        public static void RecordChargedEventWithDetailsAndItems(Dictionary<string, object> details, List<Dictionary<string, object>> items) =>
            cleverTapBinding.RecordChargedEventWithDetailsAndItems(details, items);

        public static void RecordDisplayUnitClickedEventForID(string unitID) =>
            cleverTapBinding.RecordDisplayUnitClickedEventForID(unitID);

        public static void RecordDisplayUnitViewedEventForID(string unitID) =>
            cleverTapBinding.RecordDisplayUnitViewedEventForID(unitID);

        public static void RecordEvent(string eventName) =>
            cleverTapBinding.RecordEvent(eventName);

        public static void RecordEvent(string eventName, Dictionary<string, object> properties) =>
            cleverTapBinding.RecordEvent(eventName, properties);

        public static void RecordInboxNotificationClickedEventForID(string messageId) =>
            cleverTapBinding.RecordInboxNotificationClickedEventForID(messageId);

        public static void RecordInboxNotificationViewedEventForID(string messageId) =>
            cleverTapBinding.RecordInboxNotificationViewedEventForID(messageId);

        public static void RecordScreenView(string screenName) =>
            cleverTapBinding.RecordScreenView(screenName);

        public static void RegisterPush() =>
            cleverTapBinding.RegisterPush();

        public static void ResetProductConfig() =>
            cleverTapBinding.ResetProductConfig();

        public static void ResumeInAppNotifications() =>
            cleverTapBinding.ResumeInAppNotifications();

        public static int SessionGetTimeElapsed() =>
            cleverTapBinding.SessionGetTimeElapsed();

        public static JSONClass SessionGetUTMDetails() =>
            cleverTapBinding.SessionGetUTMDetails();

        public static void SetApplicationIconBadgeNumber(int num) =>
            cleverTapBinding.SetApplicationIconBadgeNumber(num);

        public static void SetDebugLevel(int level) =>
            cleverTapBinding.SetDebugLevel(level);

        public static void SetLocation(double lat, double lon) =>
            cleverTapBinding.SetLocation(lat, lon);

        public static void SetOffline(bool enabled) =>
            cleverTapBinding.SetOffline(enabled);

        public static void SetOptOut(bool enabled) =>
            cleverTapBinding.SetOptOut(enabled);

        public static void SetProductConfigDefaults(Dictionary<string, object> defaults) =>
            cleverTapBinding.SetProductConfigDefaults(defaults);

        public static void SetProductConfigDefaultsFromPlistFileName(string fileName) =>
            cleverTapBinding.SetProductConfigDefaultsFromPlistFileName(fileName);

        public static void SetProductConfigMinimumFetchInterval(double minimumFetchInterval) =>
            cleverTapBinding.SetProductConfigMinimumFetchInterval(minimumFetchInterval);

        public static void ShowAppInbox(Dictionary<string, object> styleConfig) =>
            cleverTapBinding.ShowAppInbox(styleConfig);

        public static void ShowAppInbox(string styleConfig) =>
            cleverTapBinding.ShowAppInbox(styleConfig);

        public static void SuspendInAppNotifications() =>
            cleverTapBinding.SuspendInAppNotifications();

        public static JSONClass UserGetEventHistory() =>
            cleverTapBinding.UserGetEventHistory();

        public static int UserGetPreviousVisitTime() =>
            cleverTapBinding.UserGetPreviousVisitTime();

        public static int UserGetScreenCount() =>
            cleverTapBinding.UserGetScreenCount();

        public static int UserGetTotalVisits() =>
            cleverTapBinding.UserGetTotalVisits();

        #endregion
    }
}