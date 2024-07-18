#if !UNITY_IOS && !UNITY_ANDROID
using System;
using UnityEngine;

namespace CleverTapSDK.Native
{
    internal class UnityNativeDeviceInfo
    {
        private readonly object deviceIDLock = new object();
        private string _deviceId;

        public UnityNativeDeviceInfo()
        {
            InitializeDeviceId();
        }

        internal string SdkVersion => UnityNativeConstants.SDK.VERSION;
        internal string AppVersion => Application.version;
        internal string AppBuild => Application.productName;
        internal string BundleId => Application.identifier;
        internal string OsName => GetOSName();
        internal string OsVersion => SystemInfo.operatingSystem;
        internal string Manufacturer => SystemInfo.deviceModel;
        internal string Model => SystemInfo.deviceModel;
        internal string Carrier => null; // Not applicable for non-mobile platforms
        internal string CountryCode => GetCountryCode();
        internal string TimeZone => System.TimeZoneInfo.Local.StandardName;
        internal string Radio => null; // Not applicable for non-mobile platforms
        internal string VendorIdentifier => SystemInfo.deviceUniqueIdentifier;
        internal string DeviceWidth => Screen.width.ToString();
        internal string DeviceHeight => Screen.height.ToString();
        internal string Library => null; // Not applicable or define as needed
        internal bool Wifi => GetNetworkConnectionType();
        internal string Locale => System.Globalization.CultureInfo.CurrentCulture.Name;

        internal string DeviceId
        {
            get
            {
                return GetDeviceId() ?? GenerateFallbackDeviceId();
            }
        }

        private string GetDeviceId()
        {
            lock (deviceIDLock)
            {
                
                {
                    return PlayerPrefs.GetString(GetDeviceIdStorageKey(), null);
                }
            }
        }

        public void ForceNewDeviceID()
        {
            string newDeviceID = GenerateGuid();
            ForceUpdateDeviceId(newDeviceID);
        }

        public void ForceUpdateCustomCleverTapID(string cleverTapID)
        {
            //if (ValidateCleverTapID(cleverTapID))
            //{
              //  ForceUpdateDeviceId(UnityNativeConstants.CUSTOM_CLEVERTAP_ID_PREFIX + cleverTapID);
            //}
            //else
            {
                GenerateFallbackDeviceId();
            }
        }

        public void ForceUpdateDeviceId(string id)
        {
            lock (deviceIDLock)
            {
                PlayerPrefs.SetString(GetDeviceIdStorageKey(), id);
            }
        }

        private string GenerateFallbackDeviceId()
        {
            string fallbackDeviceId = UnityNativeConstants.SDK.ERROR_PROFILE_PREFIX + Guid.NewGuid().ToString().Replace("-", "");
            if (!string.IsNullOrEmpty(fallbackDeviceId))
            {
                UpdateFallbackId(fallbackDeviceId);
            }
            return fallbackDeviceId;
        }

        private void UpdateFallbackId(string fallbackId)
        {
            PlayerPrefs.SetString(GetFallbackIdStorageKey(), fallbackId);
        }

        private string GenerateGuid()
        {
            return UnityNativeConstants.SDK.GUID_PREFIX + Guid.NewGuid().ToString().Replace("-", "");
        }

        private string GetDeviceIdStorageKey()
        {
            return UnityNativeConstants.SDK.DEVICE_ID_TAG + ":" + UnityNativeAccountManager.Instance.AccountInfo.AccountId;
        }

        private string GetFallbackIdStorageKey()
        {
            return UnityNativeConstants.SDK.FALLBACK_ID_TAG + ":" + UnityNativeAccountManager.Instance.AccountInfo.AccountId;
        }

        private bool ValidateCleverTapID(string cleverTapID)
        {
            // Add your validation logic here
            return !string.IsNullOrEmpty(cleverTapID);
        }

        public bool IsErrorDeviceId() {
            return GetDeviceId() != null && GetDeviceId().StartsWith(UnityNativeConstants.SDK.ERROR_PROFILE_PREFIX);
        }
        private void InitializeDeviceId()
        {
            // Initialize the Device ID logic similar to Java code
            string storedDeviceId = GetDeviceId();
            if (string.IsNullOrEmpty(storedDeviceId))
            {
                // Handle custom CleverTap ID initialization
                ForceNewDeviceID();
            }
        }

        private string GetOSName()
        {
            var operatingSystem = SystemInfo.operatingSystem?.ToLower().Replace(" ", "");

            if (operatingSystem?.ToLower().Contains("windows") == true)
            {
                return "Windows";
            }

            if (operatingSystem?.ToLower().Contains("macos") == true)
            {
                return "MacOS";
            }

            return "Unknown";
        }

        private string GetCountryCode()
        {
            return System.Globalization.RegionInfo.CurrentRegion.TwoLetterISORegionName;
        }

        private bool GetNetworkConnectionType()
        {
            if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                return true;
            }
            return false;
        }
    }
}
#endif
