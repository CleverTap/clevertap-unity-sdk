using CleverTapSDK;
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

            // add listeners for events that may be triggered on app launch so they can be logged
            CleverTap.OnCleverTapPushOpenedCallback += CleverTapCallback;
            CleverTap.OnCleverTapDeepLinkCallback += CleverTapCallback;
            CleverTap.OnCleverTapInAppNotificationShowCallback += CleverTapCallback;
            CleverTap.OnCleverTapInAppNotificationButtonTapped += CleverTapCallback;
            CleverTap.OnCleverTapInAppNotificationDismissedCallback += CleverTapCallback;

            // todo: add UI for custom templates
            CleverTap.OnCustomTemplatePresent += CleverTapCustomTemplatePresent;
            CleverTap.OnCustomTemplateClose += CleverTapCustomTemplateClose;
            CleverTap.OnCustomFunctionPresent += CleverTapCustomFunctionPresent;

#if UNITY_ANDROID
            if (!CleverTap.IsPushPermissionGranted())
            {
                CleverTap.PromptForPushPermission(true);
            }
#endif
        }

        private void CleverTapCallback(string message)
        {
        }

        private void CleverTapCustomTemplatePresent(CleverTapSDK.Common.CleverTapTemplateContext context)
        {
            context.SetPresented();
            context.SetDismissed();
        }

        private void CleverTapCustomTemplateClose(CleverTapSDK.Common.CleverTapTemplateContext context)
        {
            context.SetDismissed();
        }

        private void CleverTapCustomFunctionPresent(CleverTapSDK.Common.CleverTapTemplateContext context)
        {
            context.SetPresented();
            context.SetDismissed();
        }
    }
}