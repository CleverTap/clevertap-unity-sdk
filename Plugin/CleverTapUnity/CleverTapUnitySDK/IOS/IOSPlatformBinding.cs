#if UNITY_IOS
using CleverTap.Common;
using CleverTap.Utilities;
using System.Collections.Generic;

namespace CleverTap.IOS {
    internal class IOSPlatformBinding : CleverTapPlatformBindings {
        internal IOSPlatformBinding() {
            CallbackHandler = CreateGameObjectAndAttachCallbackHandler<IOSCallbackHandler>("IOSCallbackHandler");
            CleverTapLogger.Log("Start: CleverTap binding for iOS.");
        }

        public override void ActivateProductConfig() {
            IOSDllImport.CleverTap_activateProductConfig();
        }

        public override void DeleteInboxMessageForID(string messageId) {
            IOSDllImport.CleverTap_deleteInboxMessageForID(messageId);
        }

        public override void DeleteInboxMessagesForIDs(string[] messageIds) {
            IOSDllImport.CleverTap_deleteInboxMessagesForIDs(messageIds, messageIds.Length);
        }

        public override void DisablePersonalization() {
            IOSDllImport.CleverTap_disablePersonalization();
        }

        public override void DiscardInAppNotifications() {
            IOSDllImport.CleverTap_discardInAppNotifications();
        }

        public override void DismissAppInbox() {
            IOSDllImport.CleverTap_dismissAppInbox();
        }

        public override void EnableDeviceNetworkInfoReporting(bool enabled) {
            IOSDllImport.CleverTap_enableDeviceNetworkInfoReporting(enabled);
        }

        public override void EnablePersonalization() {
            IOSDllImport.CleverTap_enablePersonalization();
        }

        public override JSONClass EventGetDetail(string eventName) {
            string jsonString = IOSDllImport.CleverTap_eventGetDetail(eventName);
            JSONClass json;
            try {
                json = (JSONClass)JSON.Parse(jsonString);
            } catch {
                CleverTapLogger.LogError("Unable to parse event detail json");
                json = new JSONClass();
            }
            return json;
        }

        public override int EventGetFirstTime(string eventName) {
            return IOSDllImport.CleverTap_eventGetFirstTime(eventName);
        }

        public override int EventGetLastTime(string eventName) {
            return IOSDllImport.CleverTap_eventGetLastTime(eventName);
        }

        public override int EventGetOccurrences(string eventName) {
            return IOSDllImport.CleverTap_eventGetOccurrences(eventName);
        }

        public override void FetchAndActivateProductConfig() {
            IOSDllImport.CleverTap_fetchAndActivateProductConfig();
        }

        public override void FetchProductConfig() {
            IOSDllImport.CleverTap_fetchProductConfig();
        }

        public override void FetchProductConfigWithMinimumInterval(double minimumInterval) {
            IOSDllImport.CleverTap_fetchProductConfigWithMinimumInterval(minimumInterval);
        }

        public override JSONArray GetAllDisplayUnits() {
            string jsonString = IOSDllImport.CleverTap_getAllDisplayUnits();
            JSONArray json;
            try {
                json = (JSONArray)JSON.Parse(jsonString);
            } catch {
                CleverTapLogger.LogError("Unable to parse native display units json");
                json = new JSONArray();
            }
            return json;
        }

        public override JSONArray GetAllInboxMessages() {
            string jsonString = IOSDllImport.CleverTap_getAllInboxMessages();
            JSONArray json;
            try {
                json = (JSONArray)JSON.Parse(jsonString);
            } catch {
                CleverTapLogger.LogError("Unable to parse app inbox messages json");
                json = new JSONArray();
            }
            return json;
        }

        public override string GetCleverTapID() {
            return IOSDllImport.CleverTap_getCleverTapID();
        }

        public override JSONClass GetDisplayUnitForID(string unitID) {
            string jsonString = IOSDllImport.CleverTap_getDisplayUnitForID(unitID);
            JSONClass json;
            try {
                json = (JSONClass)JSON.Parse(jsonString);
            } catch {
                CleverTapLogger.LogError("Unable to parse native display unit json");
                json = new JSONClass();
            }
            return json;
        }

        public override bool GetFeatureFlag(string key, bool defaultValue) {
            return IOSDllImport.CleverTap_getFeatureFlag(key, defaultValue);
        }

        public override int GetInboxMessageCount() {
            return IOSDllImport.CleverTap_getInboxMessageCount();
        }

        public override JSONClass GetInboxMessageForId(string messageId) {
            string jsonString = IOSDllImport.CleverTap_getInboxMessageForId(messageId);
            JSONClass json;
            try {
                json = (JSONClass)JSON.Parse(jsonString);
            } catch {
                CleverTapLogger.LogError("Unable to parse app inbox message json");
                json = new JSONClass();
            }
            return json;
        }

        public override int GetInboxMessageUnreadCount() {
            return IOSDllImport.CleverTap_getInboxMessageUnreadCount();
        }

        public override double GetProductConfigLastFetchTimeStamp() {
            return IOSDllImport.CleverTap_getProductConfigLastFetchTimeStamp();
        }

        public override JSONClass GetProductConfigValueFor(string key) {
            string jsonString = IOSDllImport.CleverTap_getProductConfigValueFor(key);
            JSONClass json;
            try {
                json = (JSONClass)JSON.Parse(jsonString);
            } catch {
                CleverTapLogger.LogError("Unable to parse product config value");
                json = new JSONClass();
            }
            return json;
        }

        public override JSONArray GetUnreadInboxMessages() {
            string jsonString = IOSDllImport.CleverTap_getUnreadInboxMessages();
            JSONArray json;
            try {
                json = (JSONArray)JSON.Parse(jsonString);
            } catch {
                CleverTapLogger.LogError("Unable to parse unread app inbox messages json");
                json = new JSONArray();
            }
            return json;
        }

        public override void InitializeInbox() {
            IOSDllImport.CleverTap_initializeInbox();
        }

        public override bool IsPushPermissionGranted() {
            IOSDllImport.CleverTap_isPushPermissionGranted();
            // Added for iOS
            return true;
        }

        public override void LaunchWithCredentials(string accountID, string token) {
            IOSDllImport.CleverTap_launchWithCredentials(accountID, token);
        }

        public override void LaunchWithCredentialsForRegion(string accountID, string token, string region) {
            IOSDllImport.CleverTap_launchWithCredentialsForRegion(accountID, token, region);
        }

        public override void MarkReadInboxMessageForID(string messageId) {
            IOSDllImport.CleverTap_markReadInboxMessageForID(messageId);
        }

        public override void MarkReadInboxMessagesForIDs(string[] messageIds) {
            int arrLength = messageIds.Length;
            IOSDllImport.CleverTap_markReadInboxMessagesForIDs(messageIds, arrLength);
        }

        public override void OnUserLogin(Dictionary<string, string> properties) {
            var propertiesString = Json.Serialize(properties);
            IOSDllImport.CleverTap_onUserLogin(propertiesString);
        }

        public override void ProfileAddMultiValueForKey(string key, string val) {
            IOSDllImport.CleverTap_profileAddMultiValueForKey(key, val);
        }

        public override void ProfileAddMultiValuesForKey(string key, List<string> values) {
            IOSDllImport.CleverTap_profileAddMultiValuesForKey(key, values.ToArray(), values.Count);
        }

        public override void ProfileDecrementValueForKey(string key, double val) {
            IOSDllImport.CleverTap_profileDecrementDoubleValueForKey(key, val);
        }

        public override void ProfileDecrementValueForKey(string key, int val) {
            IOSDllImport.CleverTap_profileDecrementIntValueForKey(key, val);
        }

        public override string ProfileGet(string key) {
            return IOSDllImport.CleverTap_profileGet(key);
        }

        public override string ProfileGetCleverTapAttributionIdentifier() {
            return IOSDllImport.CleverTap_profileGetCleverTapAttributionIdentifier(); ;
        }

        public override string ProfileGetCleverTapID() {
            return IOSDllImport.CleverTap_profileGetCleverTapID(); ;
        }

        public override void ProfileIncrementValueForKey(string key, double val) {
            IOSDllImport.CleverTap_profileIncrementDoubleValueForKey(key, val);
        }

        public override void ProfileIncrementValueForKey(string key, int val) {
            IOSDllImport.CleverTap_profileIncrementIntValueForKey(key, val);
        }

        public override void ProfilePush(Dictionary<string, string> properties) {
            var propertiesString = Json.Serialize(properties);
            IOSDllImport.CleverTap_profilePush(propertiesString);
        }

        public override void ProfileRemoveMultiValueForKey(string key, string val) {
            IOSDllImport.CleverTap_profileRemoveMultiValueForKey(key, val);
        }

        public override void ProfileRemoveMultiValuesForKey(string key, List<string> values) {
            IOSDllImport.CleverTap_profileRemoveMultiValuesForKey(key, values.ToArray(), values.Count);
        }

        public override void ProfileRemoveValueForKey(string key) {
            IOSDllImport.CleverTap_profileRemoveValueForKey(key);
        }

        public override void ProfileSetMultiValuesForKey(string key, List<string> values) {
            IOSDllImport.CleverTap_profileSetMultiValuesForKey(key, values.ToArray(), values.Count);
        }

        public override void PromptForPushPermission(bool showFallbackSettings) {
            IOSDllImport.CleverTap_promptForPushPermission(showFallbackSettings);
        }

        public override void PromptPushPrimer(Dictionary<string, object> json) {
            var jsonString = Json.Serialize(json);
            IOSDllImport.CleverTap_promptPushPrimer(jsonString);
        }

        public override void PushInstallReferrerSource(string source, string medium, string campaign) {
            IOSDllImport.CleverTap_pushInstallReferrerSource(source, medium, campaign);
        }

        public override void RecordChargedEventWithDetailsAndItems(Dictionary<string, object> details, List<Dictionary<string, object>> items) {
            var detailsString = Json.Serialize(details);
            var itemsString = Json.Serialize(items);
            IOSDllImport.CleverTap_recordChargedEventWithDetailsAndItems(detailsString, itemsString);
        }

        public override void RecordDisplayUnitClickedEventForID(string unitID) {
            IOSDllImport.CleverTap_recordDisplayUnitClickedEventForID(unitID);
        }

        public override void RecordDisplayUnitViewedEventForID(string unitID) {
            IOSDllImport.CleverTap_recordDisplayUnitViewedEventForID(unitID);
        }

        public override void RecordEvent(string eventName) {
            IOSDllImport.CleverTap_recordEvent(eventName, null);
        }

        public override void RecordEvent(string eventName, Dictionary<string, object> properties) {
            var propertiesString = Json.Serialize(properties);
            IOSDllImport.CleverTap_recordEvent(eventName, propertiesString);
        }

        public override void RecordInboxNotificationClickedEventForID(string messageId) {
            IOSDllImport.CleverTap_recordInboxNotificationClickedEventForID(messageId);
        }

        public override void RecordInboxNotificationViewedEventForID(string messageId) {
            IOSDllImport.CleverTap_recordInboxNotificationViewedEventForID(messageId);
        }

        public override void RecordScreenView(string screenName) {
            IOSDllImport.CleverTap_recordScreenView(screenName);
        }

        public override void RegisterPush() {
            IOSDllImport.CleverTap_registerPush();
        }

        public override void ResetProductConfig() {
            IOSDllImport.CleverTap_resetProductConfig();
        }

        public override void ResumeInAppNotifications() {
            IOSDllImport.CleverTap_resumeInAppNotifications();
        }

        public override int SessionGetTimeElapsed() {
            return IOSDllImport.CleverTap_sessionGetTimeElapsed();
        }

        public override JSONClass SessionGetUTMDetails() {
            string jsonString = IOSDllImport.CleverTap_sessionGetUTMDetails();
            JSONClass json;
            try {
                json = (JSONClass)JSON.Parse(jsonString);
            } catch {
                CleverTapLogger.LogError("Unable to parse session utm details json");
                json = new JSONClass();
            }
            return json;
        }

        public override void SetApplicationIconBadgeNumber(int num) {
            IOSDllImport.CleverTap_setApplicationIconBadgeNumber(num);
        }

        public override void SetDebugLevel(int level) {
            IOSDllImport.CleverTap_setDebugLevel(level);
        }

        public override void SetLocation(double lat, double lon) {
            IOSDllImport.CleverTap_setLocation(lat, lon);
        }

        public override void SetOffline(bool enabled) {
            IOSDllImport.CleverTap_setOffline(enabled);
        }

        public override void SetOptOut(bool enabled) {
            IOSDllImport.CleverTap_setOptOut(enabled);
        }

        public override void SetProductConfigDefaults(Dictionary<string, object> defaults) {
            var defaultsString = Json.Serialize(defaults);
            IOSDllImport.CleverTap_setProductConfigDefaults(defaultsString);
        }

        public override void SetProductConfigDefaultsFromPlistFileName(string fileName) {
            IOSDllImport.CleverTap_setProductConfigDefaultsFromPlistFileName(fileName);
        }

        public override void SetProductConfigMinimumFetchInterval(double minimumFetchInterval) {
            IOSDllImport.CleverTap_setProductConfigMinimumFetchInterval(minimumFetchInterval);
        }

        public override void ShowAppInbox(Dictionary<string, object> styleConfig) {
            var styleConfigString = Json.Serialize(styleConfig);
            IOSDllImport.CleverTap_showAppInbox(styleConfigString);
        }

        public override void ShowAppInbox(string styleConfig) {
            // TODO : Validate if we can use it like this (following Android implemenation) 
            var styleConfigString = Json.Serialize(new Dictionary<string, object> { { "showAppInbox", styleConfig } });
            IOSDllImport.CleverTap_showAppInbox(styleConfigString);
        }

        public override void SuspendInAppNotifications() {
            IOSDllImport.CleverTap_suspendInAppNotifications();
        }

        public override JSONClass UserGetEventHistory() {
            string jsonString = IOSDllImport.CleverTap_userGetEventHistory();
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
            return IOSDllImport.CleverTap_userGetPreviousVisitTime();
        }

        public override int UserGetScreenCount() {
            return IOSDllImport.CleverTap_userGetScreenCount();
        }

        public override int UserGetTotalVisits() {
            return IOSDllImport.CleverTap_userGetTotalVisits();
        }
    }
}
#endif