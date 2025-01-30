using CleverTapSDK;
using CleverTapSDK.Common;
using CleverTapSDK.Utilities;
using UnityEngine;

namespace CTExample
{
    public class App : MonoBehaviour
    {
        public bool promptPushPermissionOnLaunch;

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
            LaunchCleverTap();

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

            // Prompt for Push permissions
            if (promptPushPermissionOnLaunch)
            {
                CleverTap.PromptForPushPermission(true);
            }
        }

        #region Launch CleverTap
        private void LaunchCleverTap()
        {
            var settings = CleverTapSettingsRuntime.Instance;

            if (settings == null)
            {
                Debug.LogError("CleverTapSettings have not been set");
                return;
            }

            // LaunchWithCredentials is not needed on iOS and Android
#if !(UNITY_IOS || UNITY_ANDROID) || UNITY_EDITOR
            if (!string.IsNullOrEmpty(settings.CleverTapAccountToken))
            {
                CleverTap.LaunchWithCredentialsForRegion(settings.CleverTapAccountId, settings.CleverTapAccountToken, settings.CleverTapAccountRegion);
            }
            else
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                Logger.LogError("Account region is required for WebGL builds. Ensure your app is also enabled for WebGL.");
                return;
#endif
                CleverTap.LaunchWithCredentials(settings.CleverTapAccountId, settings.CleverTapAccountToken);
            }
#endif
            Logger.Log($"Launching with accountId: {settings.CleverTapAccountId}," +
                $" accountToken: {settings.CleverTapAccountToken}," +
                $" accountRegion: {settings.CleverTapAccountRegion}.");
        }
        #endregion

        #region Callbacks on App Launch
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
            CleverTap.SuspendInAppNotifications();
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

        private void Reset()
        {
            // Set default value to be true
            promptPushPermissionOnLaunch = true;
        }
    }
}