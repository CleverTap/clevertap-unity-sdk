using CleverTap.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace CleverTapTest.NewBindings
{
    public class IOSPlatformBinding : CleverTapPlatformBindings
    {
        #region Extern

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_launchWithCredentials(string accountID, string token);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_launchWithCredentialsForRegion(string accountID, string token, string region);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_onUserLogin(string properties);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_profilePush(string properties);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern string CleverTap_profileGet(string key);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern string CleverTap_profileGetCleverTapAttributionIdentifier();

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern string CleverTap_profileGetCleverTapID();

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_profileRemoveValueForKey(string key);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_profileSetMultiValuesForKey(string key, string[] array, int size);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_profileAddMultiValuesForKey(string key, string[] array, int size);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_profileRemoveMultiValuesForKey(string key, string[] array, int size);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_profileAddMultiValueForKey(string key, string val);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_profileRemoveMultiValueForKey(string key, string val);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_profileIncrementDoubleValueForKey(string key, double val);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_profileIncrementIntValueForKey(string key, int val);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_profileDecrementDoubleValueForKey(string key, double val);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_profileDecrementIntValueForKey(string key, int val);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_suspendInAppNotifications();

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_discardInAppNotifications();

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_resumeInAppNotifications();

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern string CleverTap_getCleverTapID();

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_recordScreenView(string screenName);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_recordEvent(string eventName, string properties);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_recordChargedEventWithDetailsAndItems(string details, string items);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_setOffline(bool enabled);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_setOptOut(bool enabled);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_enableDeviceNetworkInfoReporting(bool enabled);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_registerPush();

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_setApplicationIconBadgeNumber(int num);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_setDebugLevel(int level);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_enablePersonalization();

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_disablePersonalization();

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_setLocation(double lat, double lon);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern int CleverTap_eventGetFirstTime(string eventName);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern int CleverTap_eventGetLastTime(string eventName);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern int CleverTap_eventGetOccurrences(string eventName);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern string CleverTap_userGetEventHistory();

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern string CleverTap_sessionGetUTMDetails();

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern int CleverTap_sessionGetTimeElapsed();

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern string CleverTap_eventGetDetail(string eventName);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern int CleverTap_userGetTotalVisits();

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern int CleverTap_userGetScreenCount();

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern int CleverTap_userGetPreviousVisitTime();

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_pushInstallReferrerSource(string source, string medium, string campaign);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_showAppInbox(string styleConfig);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_dismissAppInbox();

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern int CleverTap_getInboxMessageCount();

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern int CleverTap_getInboxMessageUnreadCount();

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern int CleverTap_initializeInbox();

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern string CleverTap_getAllInboxMessages();

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern string CleverTap_getUnreadInboxMessages();

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern string CleverTap_getInboxMessageForId(string messageId);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_deleteInboxMessageForID(string messageId);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_deleteInboxMessagesForIDs(string[] messageIds, int arrLength);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_markReadInboxMessageForID(string messageId);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_markReadInboxMessagesForIDs(string[] messageIds, int arrLength);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_recordInboxNotificationViewedEventForID(string messageId);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_recordInboxNotificationClickedEventForID(string messageId);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern string CleverTap_getAllDisplayUnits();

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern string CleverTap_getDisplayUnitForID(string unitID);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_recordDisplayUnitViewedEventForID(string unitID);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_recordDisplayUnitClickedEventForID(string unitID);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_fetchProductConfig();

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_fetchProductConfigWithMinimumInterval(double minimumInterval);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_setProductConfigMinimumFetchInterval(double minimumFetchInterval);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_activateProductConfig();

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_fetchAndActivateProductConfig();

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_setProductConfigDefaults(string defaults);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_setProductConfigDefaultsFromPlistFileName(string fileName);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern string CleverTap_getProductConfigValueFor(string key);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern double CleverTap_getProductConfigLastFetchTimeStamp();

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_resetProductConfig();

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern bool CleverTap_getFeatureFlag(string key, bool defaultValue);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_promptForPushPermission(bool showFallbackSettings);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_promptPushPrimer(string json);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void CleverTap_isPushPermissionGranted();

        #endregion

        public override void ActivateProductConfig()
        {
            CleverTap_activateProductConfig();
        }

        public override void DeleteInboxMessageForID(string messageId)
        {
            CleverTap_deleteInboxMessageForID(messageId);
        }

        public override void DeleteInboxMessagesForIDs(string[] messageIds)
        {
            CleverTap_deleteInboxMessagesForIDs(messageIds, messageIds.Length);
        }

        public override void DisablePersonalization()
        {
            CleverTap_disablePersonalization();
        }

        public override void DiscardInAppNotifications()
        {
            CleverTap_discardInAppNotifications();
        }

        public override void DismissAppInbox()
        {
            CleverTap_dismissAppInbox();
        }

        public override void EnableDeviceNetworkInfoReporting(bool enabled)
        {
            CleverTap_enableDeviceNetworkInfoReporting(enabled);
        }

        public override void EnablePersonalization()
        {
            CleverTap_enablePersonalization();
        }

        public override JSONClass EventGetDetail(string eventName)
        {
            string jsonString = CleverTap_eventGetDetail(eventName);
            JSONClass json;
            try
            {
                json = (JSONClass)JSON.Parse(jsonString);
            }
            catch
            {
                Debug.LogError("Unable to parse event detail json");
                json = new JSONClass();
            }
            return json;
        }

        public override int EventGetFirstTime(string eventName)
        {
            return CleverTap_eventGetFirstTime(eventName);
        }

        public override int EventGetLastTime(string eventName)
        {
            return CleverTap_eventGetLastTime(eventName);
        }

        public override int EventGetOccurrences(string eventName)
        {
            return CleverTap_eventGetOccurrences(eventName);
        }

        public override void FetchAndActivateProductConfig()
        {
            CleverTap_fetchAndActivateProductConfig();
        }

        public override void FetchProductConfig()
        {
            CleverTap_fetchProductConfig();
        }

        public override void FetchProductConfigWithMinimumInterval(double minimumInterval)
        {
            CleverTap_fetchProductConfigWithMinimumInterval(minimumInterval);
        }

        public override JSONArray GetAllDisplayUnits()
        {
            string jsonString = CleverTap_getAllDisplayUnits();
            JSONArray json;
            try
            {
                json = (JSONArray)JSON.Parse(jsonString);
            }
            catch
            {
                Debug.LogError("Unable to parse native display units json");
                json = new JSONArray();
            }
            return json;
        }

        public override JSONArray GetAllInboxMessages()
        {
            string jsonString = CleverTap_getAllInboxMessages();
            JSONArray json;
            try
            {
                json = (JSONArray)JSON.Parse(jsonString);
            }
            catch
            {
                Debug.LogError("Unable to parse app inbox messages json");
                json = new JSONArray();
            }
            return json;
        }

        public override string GetCleverTapID()
        {
            return CleverTap_getCleverTapID();
        }

        public override JSONClass GetDisplayUnitForID(string unitID)
        {
            string jsonString = CleverTap_getDisplayUnitForID(unitID);
            JSONClass json;
            try
            {
                json = (JSONClass)JSON.Parse(jsonString);
            }
            catch
            {
                Debug.LogError("Unable to parse native display unit json");
                json = new JSONClass();
            }
            return json;
        }

        public override bool GetFeatureFlag(string key, bool defaultValue)
        {
            return CleverTap_getFeatureFlag(key, defaultValue);
        }

        public override int GetInboxMessageCount()
        {
            return CleverTap_getInboxMessageCount();
        }

        public override JSONClass GetInboxMessageForId(string messageId)
        {
            string jsonString = CleverTap_getInboxMessageForId(messageId);
            JSONClass json;
            try
            {
                json = (JSONClass)JSON.Parse(jsonString);
            }
            catch
            {
                Debug.LogError("Unable to parse app inbox message json");
                json = new JSONClass();
            }
            return json;
        }

        public override int GetInboxMessageUnreadCount()
        {
            return CleverTap_getInboxMessageUnreadCount();
        }

        public override double GetProductConfigLastFetchTimeStamp()
        {
            return CleverTap_getProductConfigLastFetchTimeStamp();
        }

        public override JSONClass GetProductConfigValueFor(string key)
        {
            string jsonString = CleverTap_getProductConfigValueFor(key);
            JSONClass json;
            try
            {
                json = (JSONClass)JSON.Parse(jsonString);
            }
            catch
            {
                Debug.LogError("Unable to parse product config value");
                json = new JSONClass();
            }
            return json;
        }

        public override JSONArray GetUnreadInboxMessages()
        {
            string jsonString = CleverTap_getUnreadInboxMessages();
            JSONArray json;
            try
            {
                json = (JSONArray)JSON.Parse(jsonString);
            }
            catch
            {
                Debug.LogError("Unable to parse unread app inbox messages json");
                json = new JSONArray();
            }
            return json;
        }

        public override void InitializeInbox()
        {
            CleverTap_initializeInbox();
        }

        public override bool IsPushPermissionGranted()
        {
            CleverTap_isPushPermissionGranted();
            // Added for iOS
            return true;
        }

        public override void LaunchWithCredentials(string accountID, string token)
        {
            CleverTap_launchWithCredentials(accountID, token);
        }

        public override void LaunchWithCredentialsForRegion(string accountID, string token, string region)
        {
            CleverTap_launchWithCredentialsForRegion(accountID, token, region);
        }

        public override void MarkReadInboxMessageForID(string messageId)
        {
            CleverTap_markReadInboxMessageForID(messageId);
        }

        public override void MarkReadInboxMessagesForIDs(string[] messageIds)
        {
            int arrLength = messageIds.Length;
            CleverTap_markReadInboxMessagesForIDs(messageIds, arrLength);
        }

        public override void OnUserLogin(Dictionary<string, string> properties)
        {
            var propertiesString = Json.Serialize(properties);
            CleverTap_onUserLogin(propertiesString);
        }

        public override void ProfileAddMultiValueForKey(string key, string val)
        {
            CleverTap_profileAddMultiValueForKey(key, val);
        }

        public override void ProfileAddMultiValuesForKey(string key, List<string> values)
        {
            CleverTap_profileAddMultiValuesForKey(key, values.ToArray(), values.Count);
        }

        public override void ProfileDecrementValueForKey(string key, double val)
        {
            CleverTap_profileDecrementDoubleValueForKey(key, val);
        }

        public override void ProfileDecrementValueForKey(string key, int val)
        {
            CleverTap_profileDecrementIntValueForKey(key, val);
        }

        public override string ProfileGet(string key)
        {
            return CleverTap_profileGet(key);
        }

        public override string ProfileGetCleverTapAttributionIdentifier()
        {
            return CleverTap_profileGetCleverTapAttributionIdentifier(); ;
        }

        public override string ProfileGetCleverTapID()
        {
            return CleverTap_profileGetCleverTapID(); ;
        }

        public override void ProfileIncrementValueForKey(string key, double val)
        {
            CleverTap_profileIncrementDoubleValueForKey(key, val);
        }

        public override void ProfileIncrementValueForKey(string key, int val)
        {
            CleverTap_profileIncrementIntValueForKey(key, val);
        }

        public override void ProfilePush(Dictionary<string, string> properties)
        {
            var propertiesString = Json.Serialize(properties);
            CleverTap_profilePush(propertiesString);
        }

        public override void ProfileRemoveMultiValueForKey(string key, string val)
        {
            CleverTap_profileRemoveMultiValueForKey(key, val);
        }

        public override void ProfileRemoveMultiValuesForKey(string key, List<string> values)
        {
            CleverTap_profileRemoveMultiValuesForKey(key, values.ToArray(), values.Count);
        }

        public override void ProfileRemoveValueForKey(string key)
        {
            CleverTap_profileRemoveValueForKey(key);
        }

        public override void ProfileSetMultiValuesForKey(string key, List<string> values)
        {
            CleverTap_profileSetMultiValuesForKey(key, values.ToArray(), values.Count);
        }

        public override void PromptForPushPermission(bool showFallbackSettings)
        {
            CleverTap_promptForPushPermission(showFallbackSettings);
        }

        public override void PromptPushPrimer(Dictionary<string, object> json)
        {
            var jsonString = Json.Serialize(json);
            CleverTap_promptPushPrimer(jsonString);
        }

        public override void PushInstallReferrerSource(string source, string medium, string campaign)
        {
            CleverTap_pushInstallReferrerSource(source, medium, campaign);
        }

        public override void RecordChargedEventWithDetailsAndItems(Dictionary<string, object> details, List<Dictionary<string, object>> items)
        {
            var detailsString = Json.Serialize(details);
            var itemsString = Json.Serialize(items);
            CleverTap_recordChargedEventWithDetailsAndItems(detailsString, itemsString);
        }

        public override void RecordDisplayUnitClickedEventForID(string unitID)
        {
            CleverTap_recordDisplayUnitClickedEventForID(unitID);
        }

        public override void RecordDisplayUnitViewedEventForID(string unitID)
        {
            CleverTap_recordDisplayUnitViewedEventForID(unitID);
        }

        public override void RecordEvent(string eventName)
        {
            CleverTap_recordEvent(eventName, null);
        }

        public override void RecordEvent(string eventName, Dictionary<string, object> properties)
        {
            var propertiesString = Json.Serialize(properties);
            CleverTap_recordEvent(eventName, propertiesString);
        }

        public override void RecordInboxNotificationClickedEventForID(string messageId)
        {
            CleverTap_recordInboxNotificationClickedEventForID(messageId);
        }

        public override void RecordInboxNotificationViewedEventForID(string messageId)
        {
            CleverTap_recordInboxNotificationViewedEventForID(messageId);
        }

        public override void RecordScreenView(string screenName)
        {
            CleverTap_recordScreenView(screenName);
        }

        public override void RegisterPush()
        {
            CleverTap_registerPush();
        }

        public override void ResetProductConfig()
        {
            CleverTap_resetProductConfig();
        }

        public override void ResumeInAppNotifications()
        {
            CleverTap_resumeInAppNotifications();
        }

        public override int SessionGetTimeElapsed()
        {
            return CleverTap_sessionGetTimeElapsed();
        }

        public override JSONClass SessionGetUTMDetails()
        {
            string jsonString = CleverTap_sessionGetUTMDetails();
            JSONClass json;
            try
            {
                json = (JSONClass)JSON.Parse(jsonString);
            }
            catch
            {
                Debug.LogError("Unable to parse session utm details json");
                json = new JSONClass();
            }
            return json;
        }

        public override void SetApplicationIconBadgeNumber(int num)
        {
            CleverTap_setApplicationIconBadgeNumber(num);
        }

        public override void SetDebugLevel(int level)
        {
            CleverTap_setDebugLevel(level);
        }

        public override void SetLocation(double lat, double lon)
        {
            CleverTap_setLocation(lat, lon);
        }

        public override void SetOffline(bool enabled)
        {
            CleverTap_setOffline(enabled);
        }

        public override void SetOptOut(bool enabled)
        {
            CleverTap_setOptOut(enabled);
        }

        public override void SetProductConfigDefaults(Dictionary<string, object> defaults)
        {
            var defaultsString = Json.Serialize(defaults);
            CleverTap_setProductConfigDefaults(defaultsString);
        }

        public override void SetProductConfigDefaultsFromPlistFileName(string fileName)
        {
            CleverTap_setProductConfigDefaultsFromPlistFileName(fileName);
        }

        public override void SetProductConfigMinimumFetchInterval(double minimumFetchInterval)
        {
            CleverTap_setProductConfigMinimumFetchInterval(minimumFetchInterval);
        }

        public override void ShowAppInbox(Dictionary<string, object> styleConfig)
        {
            var styleConfigString = Json.Serialize(styleConfig);
            CleverTap_showAppInbox(styleConfigString);
        }

        public override void ShowAppInbox(string styleConfig)
        {
            // TODO : Validate if we can use it like this (following Android implemenation) 
            var styleConfigString = Json.Serialize(new Dictionary<string, object> { { "showAppInbox", styleConfig } });
            CleverTap_showAppInbox(styleConfigString);
        }

        public override void SuspendInAppNotifications()
        {
            CleverTap_suspendInAppNotifications();
        }

        public override JSONClass UserGetEventHistory()
        {
            string jsonString = CleverTap_userGetEventHistory();
            JSONClass json;
            try
            {
                json = (JSONClass)JSON.Parse(jsonString);
            }
            catch
            {
                Debug.LogError("Unable to parse user event history json");
                json = new JSONClass();
            }
            return json;
        }

        public override int UserGetPreviousVisitTime()
        {
            return CleverTap_userGetPreviousVisitTime();
        }

        public override int UserGetScreenCount()
        {
            return CleverTap_userGetScreenCount();
        }

        public override int UserGetTotalVisits()
        {
            return CleverTap_userGetTotalVisits();
        }
    }
}
