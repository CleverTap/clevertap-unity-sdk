using System.Collections.Generic;
using CleverTap.Utilities;

namespace CleverTapTest.NewBindings
{
    public static class CleverTap
    {
        public static void ActivateProductConfig()
        {
            BindingFactory.CleverTapBinding.ActivateProductConfig();
        }

        public static void CreateNotificationChannel(string channelId, string channelName, string channelDescription, int importance, bool showBadge)
        {
            BindingFactory.CleverTapBinding.CreateNotificationChannel(channelId, channelName, channelDescription, importance, showBadge);
        }

        public static void CreateNotificationChannelGroup(string groupId, string groupName)
        {
            BindingFactory.CleverTapBinding.CreateNotificationChannelGroup(groupId, groupName);
        }

        public static void CreateNotificationChannelWithGroup(string channelId, string channelName, string channelDescription, int importance, string groupId, bool showBadge)
        {
            BindingFactory.CleverTapBinding.CreateNotificationChannelWithGroup(channelId, channelName, channelDescription, importance, groupId, showBadge);
        }

        public static void CreateNotificationChannelWithGroupAndSound(string channelId, string channelName, string channelDescription, int importance, string groupId, bool showBadge, string sound)
        {
            BindingFactory.CleverTapBinding.CreateNotificationChannelWithGroupAndSound(channelId, channelName, channelDescription, importance, groupId, showBadge, sound);
        }

        public static void CreateNotificationChannelWithSound(string channelId, string channelName, string channelDescription, int importance, bool showBadge, string sound)
        {
            BindingFactory.CleverTapBinding.CreateNotificationChannelWithSound(channelId, channelName, channelDescription, importance, showBadge, sound);
        }

        public static void DeleteInboxMessageForID(string messageId)
        {
            BindingFactory.CleverTapBinding.DeleteInboxMessageForID(messageId);
        }

        public static void DeleteInboxMessagesForIDs(string[] messageIds)
        {
            BindingFactory.CleverTapBinding.DeleteInboxMessagesForIDs(messageIds);
        }

        public static void DeleteNotificationChannel(string channelId)
        {
            BindingFactory.CleverTapBinding.DeleteNotificationChannel(channelId);
        }

        public static void DeleteNotificationChannelGroup(string groupId)
        {
            BindingFactory.CleverTapBinding.DeleteNotificationChannelGroup(groupId);
        }

        public static void DisablePersonalization()
        {
            BindingFactory.CleverTapBinding.DisablePersonalization();
        }

        public static void DiscardInAppNotifications()
        {
            BindingFactory.CleverTapBinding.DiscardInAppNotifications();
        }

        public static void DismissAppInbox()
        {
            BindingFactory.CleverTapBinding.DismissAppInbox();
        }

        public static void EnableDeviceNetworkInfoReporting(bool enabled)
        {
            BindingFactory.CleverTapBinding.EnableDeviceNetworkInfoReporting(enabled);
        }

        public static void EnablePersonalization()
        {
            BindingFactory.CleverTapBinding.EnablePersonalization();
        }

        public static JSONClass EventGetDetail(string eventName)
        {
            return BindingFactory.CleverTapBinding.EventGetDetail(eventName);
        }

        public static int EventGetFirstTime(string eventName)
        {
            return BindingFactory.CleverTapBinding.EventGetFirstTime(eventName);
        }

        public static int EventGetLastTime(string eventName)
        {
            return BindingFactory.CleverTapBinding.EventGetLastTime(eventName);
        }

        public static int EventGetOccurrences(string eventName)
        {
            return BindingFactory.CleverTapBinding.EventGetOccurrences(eventName);
        }

        public static void FetchAndActivateProductConfig()
        {
            BindingFactory.CleverTapBinding.FetchAndActivateProductConfig();
        }

        public static void FetchProductConfig()
        {
            BindingFactory.CleverTapBinding.FetchProductConfig();
        }

        public static void FetchProductConfigWithMinimumInterval(double minimumInterval)
        {
            BindingFactory.CleverTapBinding.FetchProductConfigWithMinimumInterval(minimumInterval);
        }

        public static JSONArray GetAllDisplayUnits()
        {
            return BindingFactory.CleverTapBinding.GetAllDisplayUnits();
        }

        public static JSONArray GetAllInboxMessages()
        {
            return BindingFactory.CleverTapBinding.GetAllInboxMessages();
        }

        public static string GetCleverTapID()
        {
            return BindingFactory.CleverTapBinding.GetCleverTapID();
        }

        public static JSONClass GetDisplayUnitForID(string unitID)
        {
            return BindingFactory.CleverTapBinding.GetDisplayUnitForID(unitID);
        }

        public static bool GetFeatureFlag(string key, bool defaultValue)
        {
            return BindingFactory.CleverTapBinding.GetFeatureFlag(key, defaultValue);
        }

        public static int GetInboxMessageCount()
        {
            return BindingFactory.CleverTapBinding.GetInboxMessageCount();
        }

        public static JSONClass GetInboxMessageForId(string messageId)
        {
            return BindingFactory.CleverTapBinding.GetInboxMessageForId(messageId);
        }

        public static int GetInboxMessageUnreadCount()
        {
            return BindingFactory.CleverTapBinding.GetInboxMessageUnreadCount();
        }

        public static double GetProductConfigLastFetchTimeStamp()
        {
            return BindingFactory.CleverTapBinding.GetProductConfigLastFetchTimeStamp();
        }

        public static JSONClass GetProductConfigValueFor(string key)
        {
            return BindingFactory.CleverTapBinding.GetProductConfigValueFor(key);
        }

        public static JSONArray GetUnreadInboxMessages()
        {
            return BindingFactory.CleverTapBinding.GetUnreadInboxMessages();
        }

        public static void InitializeInbox()
        {
            BindingFactory.CleverTapBinding.InitializeInbox();
        }

        public static bool IsPushPermissionGranted()
        {
            return BindingFactory.CleverTapBinding.IsPushPermissionGranted();
        }

        public static void LaunchWithCredentials(string accountID, string token)
        {
            BindingFactory.CleverTapBinding.LaunchWithCredentials(accountID, token);
        }

        public static void LaunchWithCredentialsForRegion(string accountID, string token, string region)
        {
            BindingFactory.CleverTapBinding.LaunchWithCredentialsForRegion(accountID, token, region);
        }

        public static void MarkReadInboxMessageForID(string messageId)
        {
            BindingFactory.CleverTapBinding.MarkReadInboxMessageForID(messageId);
        }

        public static void MarkReadInboxMessagesForIDs(string[] messageIds)
        {
            BindingFactory.CleverTapBinding.MarkReadInboxMessagesForIDs(messageIds);
        }

        public static void OnUserLogin(Dictionary<string, string> properties)
        {
            BindingFactory.CleverTapBinding.OnUserLogin(properties);
        }

        public static void ProfileAddMultiValueForKey(string key, string val)
        {
            BindingFactory.CleverTapBinding.ProfileAddMultiValueForKey(key, val);
        }

        public static void ProfileAddMultiValuesForKey(string key, List<string> values)
        {
            BindingFactory.CleverTapBinding.ProfileAddMultiValuesForKey(key, values);
        }

        public static void ProfileDecrementValueForKey(string key, double val)
        {
            BindingFactory.CleverTapBinding.ProfileDecrementValueForKey(key, val);
        }

        public static void ProfileDecrementValueForKey(string key, int val)
        {
            BindingFactory.CleverTapBinding.ProfileDecrementValueForKey(key, val);
        }

        public static string ProfileGet(string key)
        {
            return BindingFactory.CleverTapBinding.ProfileGet(key);
        }

        public static string ProfileGetCleverTapAttributionIdentifier()
        {
            return BindingFactory.CleverTapBinding.ProfileGetCleverTapAttributionIdentifier();
        }

        public static string ProfileGetCleverTapID()
        {
            return BindingFactory.CleverTapBinding.ProfileGetCleverTapID();
        }

        public static void ProfileIncrementValueForKey(string key, double val)
        {
            BindingFactory.CleverTapBinding.ProfileIncrementValueForKey(key, val);
        }

        public static void ProfileIncrementValueForKey(string key, int val)
        {
            BindingFactory.CleverTapBinding.ProfileIncrementValueForKey(key, val);
        }

        public static void ProfilePush(Dictionary<string, string> properties)
        {
            BindingFactory.CleverTapBinding.ProfilePush(properties);
        }

        public static void ProfileRemoveMultiValueForKey(string key, string val)
        {
            BindingFactory.CleverTapBinding.ProfileRemoveMultiValueForKey(key, val);
        }

        public static void ProfileRemoveMultiValuesForKey(string key, List<string> values)
        {
            BindingFactory.CleverTapBinding.ProfileRemoveMultiValuesForKey(key, values);
        }

        public static void ProfileRemoveValueForKey(string key)
        {
            BindingFactory.CleverTapBinding.ProfileRemoveValueForKey(key);
        }

        public static void ProfileSetMultiValuesForKey(string key, List<string> values)
        {
            BindingFactory.CleverTapBinding.ProfileSetMultiValuesForKey(key, values);
        }

        public static void PromptForPushPermission(bool showFallbackSettings)
        {
            BindingFactory.CleverTapBinding.PromptForPushPermission(showFallbackSettings);
        }

        public static void PromptPushPrimer(Dictionary<string, object> json)
        {
            BindingFactory.CleverTapBinding.PromptPushPrimer(json);
        }

        public static void PushInstallReferrerSource(string source, string medium, string campaign)
        {
            BindingFactory.CleverTapBinding.PushInstallReferrerSource(source, medium, campaign);
        }

        public static void RecordChargedEventWithDetailsAndItems(Dictionary<string, object> details, List<Dictionary<string, object>> items)
        {
            BindingFactory.CleverTapBinding.RecordChargedEventWithDetailsAndItems(details, items);
        }

        public static void RecordDisplayUnitClickedEventForID(string unitID)
        {
            BindingFactory.CleverTapBinding.RecordDisplayUnitClickedEventForID(unitID);
        }

        public static void RecordDisplayUnitViewedEventForID(string unitID)
        {
            BindingFactory.CleverTapBinding.RecordDisplayUnitViewedEventForID(unitID);
        }

        public static void RecordEvent(string eventName)
        {
            BindingFactory.CleverTapBinding.RecordEvent(eventName);
        }

        public static void RecordEvent(string eventName, Dictionary<string, object> properties)
        {
            BindingFactory.CleverTapBinding.RecordEvent(eventName, properties);
        }

        public static void RecordInboxNotificationClickedEventForID(string messageId)
        {
            BindingFactory.CleverTapBinding.RecordInboxNotificationClickedEventForID(messageId);
        }

        public static void RecordInboxNotificationViewedEventForID(string messageId)
        {
            BindingFactory.CleverTapBinding.RecordInboxNotificationViewedEventForID(messageId);
        }

        public static void RecordScreenView(string screenName)
        {
            BindingFactory.CleverTapBinding.RecordScreenView(screenName);
        }

        public static void RegisterPush()
        {
            BindingFactory.CleverTapBinding.RegisterPush();
        }

        public static void ResetProductConfig()
        {
            BindingFactory.CleverTapBinding.ResetProductConfig();
        }

        public static void ResumeInAppNotifications()
        {
            BindingFactory.CleverTapBinding.ResumeInAppNotifications();
        }

        public static int SessionGetTimeElapsed()
        {
            return BindingFactory.CleverTapBinding.SessionGetTimeElapsed();
        }

        public static JSONClass SessionGetUTMDetails()
        {
            return BindingFactory.CleverTapBinding.SessionGetUTMDetails();
        }

        public static void SetApplicationIconBadgeNumber(int num)
        {
            BindingFactory.CleverTapBinding.SetApplicationIconBadgeNumber(num);
        }

        public static void SetDebugLevel(int level)
        {
            BindingFactory.CleverTapBinding.SetDebugLevel(level);
        }

        public static void SetLocation(double lat, double lon)
        {
            BindingFactory.CleverTapBinding.SetLocation(lat, lon);
        }

        public static void SetOffline(bool enabled)
        {
            BindingFactory.CleverTapBinding.SetOffline(enabled);
        }

        public static void SetOptOut(bool enabled)
        {
            BindingFactory.CleverTapBinding.SetOptOut(enabled);
        }

        public static void SetProductConfigDefaults(Dictionary<string, object> defaults)
        {
            BindingFactory.CleverTapBinding.SetProductConfigDefaults(defaults);
        }

        public static void SetProductConfigDefaultsFromPlistFileName(string fileName)
        {
            BindingFactory.CleverTapBinding.SetProductConfigDefaultsFromPlistFileName(fileName);
        }

        public static void SetProductConfigMinimumFetchInterval(double minimumFetchInterval)
        {
            BindingFactory.CleverTapBinding.SetProductConfigMinimumFetchInterval(minimumFetchInterval);
        }

        public static void ShowAppInbox(Dictionary<string, object> styleConfig)
        {
            BindingFactory.CleverTapBinding.ShowAppInbox(styleConfig);
        }

        public static void ShowAppInbox(string styleConfig)
        {
            BindingFactory.CleverTapBinding.ShowAppInbox(styleConfig);
        }

        public static void SuspendInAppNotifications()
        {
            BindingFactory.CleverTapBinding.SuspendInAppNotifications();
        }

        public static JSONClass UserGetEventHistory()
        {
            return BindingFactory.CleverTapBinding.UserGetEventHistory();
        }

        public static int UserGetPreviousVisitTime()
        {
            return BindingFactory.CleverTapBinding.UserGetPreviousVisitTime();
        }

        public static int UserGetScreenCount()
        {
            return BindingFactory.CleverTapBinding.UserGetScreenCount();
        }

        public static int UserGetTotalVisits()
        {
            return BindingFactory.CleverTapBinding.UserGetTotalVisits();
        }
    }
}
