using CleverTapSDK;
using CleverTapSDK.Common;
using CleverTapSDK.Utilities;
using UnityEngine;

namespace CTExample
{
    public class App : MonoBehaviour
    {
        public string accountName = "ACCOUNT_NAME";
        public string accountId = "ACCOUNT_ID";
        [SerializeField] private string accountToken = "ACCOUNT_TOKEN";
        [SerializeField] private string accountRegion = "";

        void Awake()
        {
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
            Logger.Log($"Setting targetFrameRate to: {(int)Screen.currentResolution.refreshRateRatio.value}");
            Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
#endif

            // Unity internal Logs
            CleverTap.SetLogLevel(LogLevel.Debug);
            // SDK logs
            CleverTap.SetDebugLevel(3);
            // Launch CleverTap
            if (!string.IsNullOrEmpty(accountRegion))
            {
                CleverTap.LaunchWithCredentialsForRegion(accountId, accountToken, accountRegion);
            }
            else
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                Logger.LogError("Account region is required for WebGL builds. Ensure your app is also enabled for WebGL.");
                return;
#endif
                CleverTap.LaunchWithCredentials(accountId, accountToken);
            }
            Logger.Log($"Launching \"{accountName}\" with accountId: {accountId}, accountToken: {accountToken}, accountRegion: {accountRegion}.");

            // Add listeners for events that may be triggered on app launch
            CleverTap.OnCleverTapPushOpenedCallback += CleverTapPushOpenedCallback;
#if UNITY_IOS
            CleverTap.OnCleverTapPushNotificationTappedWithCustomExtrasCallback += CleverTap_OnCleverTapPushNotificationTappedWithCustomExtrasCallback;
#endif
            CleverTap.OnCleverTapDeepLinkCallback += CleverTapDeepLinkCallback;
            CleverTap.OnCleverTapInAppNotificationShowCallback += CleverTapInAppNotificationShowCallback;
            CleverTap.OnCleverTapInAppNotificationButtonTapped += CleverTapInAppNotificationButtonTapped;
            CleverTap.OnCleverTapInAppNotificationDismissedCallback += CleverTapInAppNotificationDismissedCallback;

            // Add listeners for Custom Templates
            CleverTap.OnCustomTemplatePresent += CleverTapCustomTemplatePresent;
            CleverTap.OnCustomTemplateClose += CleverTapCustomTemplateClose;
            CleverTap.OnCustomFunctionPresent += CleverTapCustomFunctionPresent;

#if UNITY_ANDROID
            if (!CleverTap.IsPushPermissionGranted())
            {
                CleverTap.PromptForPushPermission(true);
            }

            CleverTap.GetUserEventLog("App Launched", (UserEventLog userEventLog) =>
            {
                Logger.Log($"UserEventLog: {userEventLog.EventName}, {userEventLog.NormalizedEventName}, {userEventLog.FirstTS}, {userEventLog.LastTS}, {userEventLog.CountOfEvents}, {userEventLog.DeviceID}");
            });
#endif
        }

        #region Callbacks on app launch
        private void CleverTapPushOpenedCallback(string message)
        {
            Logger.Log($"Push Opened callback: {message}");
        }

        private void CleverTap_OnCleverTapPushNotificationTappedWithCustomExtrasCallback(string message)
        {
            Logger.Log($"Push Tapped with Custom extras callback: {message}");
        }

        private void CleverTapDeepLinkCallback(string message)
        {
            Logger.Log($"Deeplink callback: {message}");
        }

        private void CleverTapInAppNotificationShowCallback(string message)
        {
            Logger.Log($"InAppNotification Show callback: {message}");
        }

        private void CleverTapInAppNotificationButtonTapped(string message)
        {
            Logger.Log($"InAppNotification ButtonTapped: {message}");
        }

        private void CleverTapInAppNotificationDismissedCallback(string message)
        {
            Logger.Log($"InAppNotification Dismissed callback: {message}");
        }
        #endregion

        #region Custom Templates
        private void CleverTapCustomTemplatePresent(CleverTapTemplateContext context)
        {
            Logger.Log($"Custom Template present: {context}");
            var model = new CustomTemplateModel(context);
            model.Show();
        }

        private void CleverTapCustomFunctionPresent(CleverTapTemplateContext context)
        {
            Logger.Log($"Custom Function present: {context}");
            var model = new CustomTemplateModel(context, true);
            model.Show();
        }

        private void CleverTapCustomTemplateClose(CleverTapTemplateContext context)
        {
            Logger.Log($"Custom Template close: {context}");
            context.SetDismissed();
        }
        #endregion
    }
}