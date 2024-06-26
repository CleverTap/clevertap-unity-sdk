#if !UNITY_IOS && !UNITY_ANDROID
using System;
using UnityEngine;

namespace CleverTapSDK.Native {
    internal class UnityNativeDeviceInfo {
        private readonly string _sdkVersion;
        private readonly string _appVersion;
        private readonly string _appBuild;
        private readonly string _bundleId;
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
        private readonly string _deviceId;
        private readonly string _library;
        private readonly bool _wifi;
        private readonly string _locale;

//TODO: test and log info for MAC, windows and WEBGl
        internal UnityNativeDeviceInfo() {
            _sdkVersion = UnityNativeConstants.SDK.VERSION;
            _appVersion = Application.version; // Check where to get this
            _appBuild = Application.productName; // Check where to get this
            _bundleId = "123456789"; // Check if we need this and how to get
            _osName = GetOSName(); // Only suppport for Windows and MacOS
            _osVersion = SystemInfo.operatingSystem;
            _manufacturer = SystemInfo.deviceModel; // Check if we need this and is it correct
            _model = SystemInfo.deviceModel; // Check if we need this
            _carrier = null; // Check if we need this and how to get
            _countryCode = null; // Check if we need this and how to get
            _timeZone = null; // Check if we need this and how to get
            _radio = null; // Check if we need this and how to get
            _vendorIdentifier = null; // Check if we need this and how to get
            _deviceWidth = Screen.width.ToString(); // Check if we need this and is it correct
            _deviceHeight = Screen.height.ToString(); // Check if we need this and is it correct
            _deviceId = GetDeviceId();
            _library = null; // Check if we need this and how to get
            _wifi = false; // Check if we need this and how to get
            _locale = "xx_XX"; // Check if we need this and how to get
        }

        internal string SdkVersion => _sdkVersion;

        internal string AppVersion => _appVersion;

        internal string AppBuild => _appBuild;

        internal string BundleId => _bundleId;

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

        internal string DeviceId => _deviceId;

        internal string Library => _library;

        internal bool Wifi => _wifi;

        internal string Locale => _locale;

        private string GetOSName() {
            var operatingSystem = SystemInfo.operatingSystem?.ToLower().Replace(" ", "");

            if (operatingSystem?.ToLower().Contains("windows") == true) {
                return "Windows";
            }

            if (operatingSystem?.ToLower().Contains("macos") == true) {
                return "MacOS";
            }

            return "Unknown";
        }

        private string GetDeviceId() {
            var deviceId = PlayerPrefs.GetString(SystemInfo.deviceUniqueIdentifier);
            if (string.IsNullOrEmpty(deviceId)) {
                deviceId = GenerateDeviceId();
                PlayerPrefs.SetString(SystemInfo.deviceUniqueIdentifier, deviceId);
            }

            return deviceId;
        }

        private string GenerateDeviceId() {
            var guid = Guid.NewGuid().ToString();
            return "-" + guid.Replace("-", "").Trim().ToLower();
        }
    }
}
#endif