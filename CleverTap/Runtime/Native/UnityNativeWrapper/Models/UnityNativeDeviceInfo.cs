#if !UNITY_IOS && !UNITY_ANDROID
using System;
using UnityEngine;
using CleverTapSDK.Constants;

namespace CleverTapSDK.Native {

    internal class UnityNativeDeviceInfo {
        private readonly string _sdkVersion;
        private readonly string _appVersion;
        private readonly string _appBuild;
        private readonly string _osName;
        private readonly string _osVersion;
        private readonly string _manufacturer;
        private readonly string _model;
        private readonly string _carrier;
        private readonly string _countryCode;
        private readonly string _timeZone;
        private readonly string _radio;
        private readonly string _vendorIdentifier;
        private readonly string _deviceWidth;
        private readonly string _deviceHeight;
        private readonly string _library;
        private readonly int _wifi;
        private readonly string _locale;
        private readonly UnityNativePreferenceManager _preferenceManager;

        internal UnityNativeDeviceInfo(string accountId) {
            _preferenceManager = UnityNativePreferenceManager.GetPreferenceManager(accountId);
            _sdkVersion = CleverTapVersion.CLEVERTAP_SDK_REVISION; // Use the SDK Version Revision
            _appVersion = Application.version;
            _appBuild = Application.productName;
            _osName = GetOSName(); // Only suppport for Windows and MacOS
            _osVersion = SystemInfo.operatingSystem;
            _manufacturer = SystemInfo.deviceModel;
            _model = SystemInfo.deviceModel;
            _carrier = null;
            _countryCode = null;
            _timeZone = null;
            _radio = null;
            _vendorIdentifier = null;
            _deviceWidth = Screen.width.ToString();
            _deviceHeight = Screen.height.ToString();
            _library = null;
            _wifi = -1;
            _locale = null;
        }

        internal string SdkVersion => _sdkVersion;

        internal string AppVersion => _appVersion;

        internal string AppBuild => _appBuild;

        internal string OsName => _osName;

        internal string OsVersion => _osVersion;

        internal string Manufacturer => _manufacturer;

        internal string Model => _model;

        internal string Carrier => _carrier;

        internal string CountryCode => _countryCode;

        internal string TimeZone => _timeZone;

        internal string Radio => _radio;

        internal string VendorIdentifier => _vendorIdentifier;

        internal string DeviceWidth => _deviceWidth;

        internal string DeviceHeight => _deviceHeight;

        internal string DeviceId => GetDeviceId();

        internal string Library => _library;

        internal int Wifi => _wifi;

        internal string Locale => _locale;

        internal bool EnableNetworkInfoReporting {
            get
            {
                return _preferenceManager.GetInt(UnityNativeConstants.Profile.NETWORK_INFO_KEY, 0) == 1;
            }
            set
            {
                _preferenceManager.SetInt(UnityNativeConstants.Profile.NETWORK_INFO_KEY, value ? 1 : 0);
            }
        }

        private string GetOSName() {
            var operatingSystem = SystemInfo.operatingSystem?.ToLower().Replace(" ", "");

            if (operatingSystem?.ToLower().Contains(UnityNativeConstants.OSNames.WINDOWS.ToLower()) == true) {
                return UnityNativeConstants.OSNames.WINDOWS;
            }

            if (operatingSystem?.ToLower().Contains(UnityNativeConstants.OSNames.MACOS.ToLower()) == true) {
                return UnityNativeConstants.OSNames.MACOS;
            }

            return UnityNativeConstants.OSNames.UNKNOWN;
        }

        private string GetDeviceId() {
            string id = _preferenceManager.GetString(UnityNativeConstants.SDK.DEVICE_ID_KEY, null);
            if (string.IsNullOrEmpty(id))
            {
                ForceNewDeviceID();
            }
            return _preferenceManager.GetString(UnityNativeConstants.SDK.DEVICE_ID_KEY, null);
        }

        internal void ForceNewDeviceID() {
            string newDeviceID = GenerateGuid();
            ForceUpdateDeviceId(newDeviceID);
        }

        internal void ForceUpdateDeviceId(string id) {
            _preferenceManager.SetString(UnityNativeConstants.SDK.DEVICE_ID_KEY, id);
        }

        private string GenerateGuid() {
            string id = Guid.NewGuid().ToString().Replace("-", "");
            return $"{UnityNativeConstants.SDK.UNITY_GUID_PREFIX}{id}";
        }
    }
}
#endif