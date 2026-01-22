using System.IO;
using CleverTapSDK.Utilities;
using UnityEngine;

namespace CleverTapSDK.Private
{
    [System.Serializable]
    public class CleverTapSettings : ScriptableObject
    {
        #region Project Settings
        public CleverTapEnvironmentKey DefaultEnvironment = CleverTapEnvironmentKey.PROD;

        public SerializableDictionary<CleverTapEnvironmentKey, CleverTapEnvironmentCredential> Environments = new SerializableDictionary<CleverTapEnvironmentKey, CleverTapEnvironmentCredential>();
        #endregion

        #region IOS Specific Settings
        /// <summary>
        /// Disable IDFV on iOS.
        /// </summary>
        public bool CleverTapDisableIDFV;

        /// <summary>
        /// Boolean whether to use auto integrate on iOS.
        /// Default value is true.
        /// </summary>
        public bool CleverTapIOSUseAutoIntegrate = true;

        /// <summary>
        /// Boolean whether to set UNUserNotificationCenter delegate on iOS.
        /// Default value is true.
        /// If set to false, you must implement the center delegate
        /// methods yourself and call the CleverTap methods.
        /// </summary>
        public bool CleverTapIOSUseUNUserNotificationCenter = true;

        /// <summary>
        /// Boolean whether to present remote notifications while app is on foreground on iOS.
        /// Default value is false. The UNNotificationPresentationOptionNone is used.
        /// If changed to true, notification will be presented using UNNotificationPresentationOptionBanner/Alert |
        /// UNNotificationPresentationOptionBadge | UNNotificationPresentationOptionSound.
        /// </summary>
        public bool CleverTapIOSPresentNotificationOnForeground;
        #endregion

        #region Other Settings
        /// <summary>
        /// Boolean whether the CleverTap settings should be saved to the streaming assets as JSON.
        /// Default value is false.
        /// Set this to true if you need to use the CleverTapSettings runtime.
        /// </summary>
        public bool CleverTapSettingsSaveToJSON;
        #endregion

        public override string ToString()
        {
            return $"CleverTapSettings:\n" +
                   $"CleverTap Default Environment : {DefaultEnvironment}\n" +
                   $"CleverTap Environments : {JsonUtility.ToJson(Environments)}\n" +
                   $"CleverTapDisableIDFV: {CleverTapDisableIDFV}\n" +
                   $"CleverTapIOSUseAutoIntegrate: {CleverTapIOSUseAutoIntegrate}\n" +
                   $"CleverTapIOSUseUNUserNotificationCenter: {CleverTapIOSUseUNUserNotificationCenter}\n" +
                   $"CleverTapIOSPresentNotificationOnForeground: {CleverTapIOSPresentNotificationOnForeground}\n" +
                   $"CleverTapSettingsSaveToJSON: {CleverTapSettingsSaveToJSON}";
        }

        internal static readonly string settingsPath = Path.Combine("Assets", "CleverTapSettings.asset");
        internal static readonly string jsonPath = Path.Combine(Application.streamingAssetsPath, "CleverTapSettings.json");
    }
}