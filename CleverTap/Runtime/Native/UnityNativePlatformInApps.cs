#if !UNITY_IOS && !UNITY_ANDROID 
using CleverTapSDK.Common;
using CleverTapSDK.Utilities;

namespace CleverTapSDK.Native {
    internal class UnityNativePlatformInApps : CleverTapPlatformInApps {
        internal override void FetchInApps(int callbackId) =>
            CleverTapLogger.LogError("CleverTap Error: FetchInApps is not supported for this platform.");
    }
}
#endif
