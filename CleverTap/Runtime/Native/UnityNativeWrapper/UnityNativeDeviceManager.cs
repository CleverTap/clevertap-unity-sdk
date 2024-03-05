#if !UNITY_IOS && !UNITY_ANDROID
using System;

namespace CleverTapSDK.Native {
    internal class UnityNativeDeviceManager {
        private static readonly Lazy<UnityNativeDeviceManager> instance = new Lazy<UnityNativeDeviceManager>(() => new UnityNativeDeviceManager());

        private readonly UnityNativeDeviceInfo _deviceInfo;

        private UnityNativeDeviceManager() { 
            _deviceInfo = new UnityNativeDeviceInfo();
        }

        internal static UnityNativeDeviceManager Instance => instance.Value;

        internal UnityNativeDeviceInfo DeviceInfo => _deviceInfo;
    }
}
#endif