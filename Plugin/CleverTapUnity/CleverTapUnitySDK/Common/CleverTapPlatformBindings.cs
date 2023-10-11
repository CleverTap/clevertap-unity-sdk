using CleverTap.Constants;
using CleverTap.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace CleverTap.Common {
    internal abstract class CleverTapPlatformBindings {
        public const string VERSION = CleverTapVersion.CLEVERTAP_SDK_VERSION;

        public CleverTapCallbackHandler CallbackHandler { get; protected set; }

        protected T CreateGameObjectAndAttachCallbackHandler<T>(string objectName) where T : CleverTapCallbackHandler {
            var gameObject = new GameObject(objectName);
            gameObject.AddComponent<T>();
            GameObject.DontDestroyOnLoad(gameObject);
            return gameObject.GetComponent<T>();
        }

        #region Default - Platform Bindings

        public virtual void ActivateProductConfig() {
        }

        public virtual void CreateNotificationChannel(string channelId, string channelName, string channelDescription, int importance, bool showBadge) {
        }

        public virtual void CreateNotificationChannelGroup(string groupId, string groupName) {
        }

        public virtual void CreateNotificationChannelWithGroup(string channelId, string channelName, string channelDescription, int importance, string groupId, bool showBadge) {
        }

        public virtual void CreateNotificationChannelWithGroupAndSound(string channelId, string channelName, string channelDescription, int importance, string groupId, bool showBadge, string sound) {
        }

        public virtual void CreateNotificationChannelWithSound(string channelId, string channelName, string channelDescription, int importance, bool showBadge, string sound) {
        }

        public virtual void DeleteInboxMessageForID(string messageId) {
        }

        public virtual void DeleteInboxMessagesForIDs(string[] messageIds) {
        }

        public virtual void DeleteNotificationChannel(string channelId) {
        }

        public virtual void DeleteNotificationChannelGroup(string groupId) {
        }

        public virtual void DisablePersonalization() {
        }

        public virtual void DiscardInAppNotifications() {
        }

        public virtual void DismissAppInbox() {
        }

        public virtual void EnableDeviceNetworkInfoReporting(bool enabled) {
        }

        public virtual void EnablePersonalization() {
        }

        public virtual JSONClass EventGetDetail(string eventName) {
            return new JSONClass();
        }

        public virtual int EventGetFirstTime(string eventName) {
            return -1;
        }

        public virtual int EventGetLastTime(string eventName) {
            return -1;
        }

        public virtual int EventGetOccurrences(string eventName) {
            return -1;
        }

        public virtual void FetchAndActivateProductConfig() {
        }

        public virtual void FetchProductConfig() {
        }

        public virtual void FetchProductConfigWithMinimumInterval(double minimumInterval) {
        }

        public virtual JSONArray GetAllDisplayUnits() {
            return new JSONArray();
        }

        public virtual JSONArray GetAllInboxMessages() {
            return new JSONArray();
        }

        public virtual string GetCleverTapID() {
            return "testCleverTapID";
        }

        public virtual JSONClass GetDisplayUnitForID(string unitID) {
            return new JSONClass();
        }

        public virtual bool GetFeatureFlag(string key, bool defaultValue) {
            // Validate if this is ok?
            return defaultValue;
        }

        public virtual int GetInboxMessageCount() {
            return -1;
        }

        public virtual JSONClass GetInboxMessageForId(string messageId) {
            return new JSONClass();
        }

        public virtual int GetInboxMessageUnreadCount() {
            return -1;
        }

        public virtual double GetProductConfigLastFetchTimeStamp() {
            return -1;
        }

        public virtual JSONClass GetProductConfigValueFor(string key) {
            return new JSONClass();
        }

        public virtual JSONArray GetUnreadInboxMessages() {
            return new JSONArray();
        }

        public virtual void InitializeInbox() {
        }

        public virtual bool IsPushPermissionGranted() {
            return false;
        }

        public virtual void LaunchWithCredentials(string accountID, string token) {
        }

        public virtual void LaunchWithCredentialsForRegion(string accountID, string token, string region) {
        }

        public virtual void MarkReadInboxMessageForID(string messageId) {
        }

        public virtual void MarkReadInboxMessagesForIDs(string[] messageIds) {
        }

        public virtual void OnUserLogin(Dictionary<string, string> properties) {
        }

        public virtual void ProfileAddMultiValueForKey(string key, string val) {
        }

        public virtual void ProfileAddMultiValuesForKey(string key, List<string> values) {
        }

        public virtual void ProfileDecrementValueForKey(string key, double val) {
        }

        public virtual void ProfileDecrementValueForKey(string key, int val) {
        }

        public virtual string ProfileGet(string key) {
            return "test";
        }

        public virtual string ProfileGetCleverTapAttributionIdentifier() {
            return "testAttributionIdentifier";
        }

        public virtual string ProfileGetCleverTapID() {
            return "testCleverTapID";
        }

        public virtual void ProfileIncrementValueForKey(string key, double val) {
        }

        public virtual void ProfileIncrementValueForKey(string key, int val) {
        }

        public virtual void ProfilePush(Dictionary<string, string> properties) {
        }

        public virtual void ProfileRemoveMultiValueForKey(string key, string val) {
        }

        public virtual void ProfileRemoveMultiValuesForKey(string key, List<string> values) {
        }

        public virtual void ProfileRemoveValueForKey(string key) {
        }

        public virtual void ProfileSetMultiValuesForKey(string key, List<string> values) {
        }

        public virtual void PromptForPushPermission(bool showFallbackSettings) {
        }

        public virtual void PromptPushPrimer(Dictionary<string, object> json) {
        }

        public virtual void PushInstallReferrerSource(string source, string medium, string campaign) {
        }

        public virtual void RecordChargedEventWithDetailsAndItems(Dictionary<string, object> details, List<Dictionary<string, object>> items) {
        }

        public virtual void RecordDisplayUnitClickedEventForID(string unitID) {
        }

        public virtual void RecordDisplayUnitViewedEventForID(string unitID) {
        }

        public virtual void RecordEvent(string eventName) {
        }

        public virtual void RecordEvent(string eventName, Dictionary<string, object> properties) {
        }

        public virtual void RecordInboxNotificationClickedEventForID(string messageId) {
        }

        public virtual void RecordInboxNotificationViewedEventForID(string messageId) {
        }

        public virtual void RecordScreenView(string screenName) {
        }

        public virtual void RegisterPush() {
        }

        public virtual void ResetProductConfig() {
        }

        public virtual void ResumeInAppNotifications() {
        }

        public virtual int SessionGetTimeElapsed() {
            return -1;
        }

        public virtual JSONClass SessionGetUTMDetails() {
            return new JSONClass();
        }

        public virtual void SetApplicationIconBadgeNumber(int num) {
        }

        public virtual void SetDebugLevel(int level) {
        }

        public virtual void SetLocation(double lat, double lon) {
        }

        public virtual void SetOffline(bool enabled) {
        }

        public virtual void SetOptOut(bool enabled) {
        }

        public virtual void SetProductConfigDefaults(Dictionary<string, object> defaults) {
        }

        public virtual void SetProductConfigDefaultsFromPlistFileName(string fileName) {
        }

        public virtual void SetProductConfigMinimumFetchInterval(double minimumFetchInterval) {
        }

        public virtual void ShowAppInbox(Dictionary<string, object> styleConfig) {
        }

        public virtual void ShowAppInbox(string styleConfig) {
        }

        public virtual void SuspendInAppNotifications() {
        }

        public virtual JSONClass UserGetEventHistory() {
            return new JSONClass();
        }

        public virtual int UserGetPreviousVisitTime() {
            return -1;
        }

        public virtual int UserGetScreenCount() {
            return -1;
        }

        public virtual int UserGetTotalVisits() {
            return -1;
        }

        #endregion
    }
}
