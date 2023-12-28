#if UNITY_ANDROID
using CleverTapSDK.Common;

namespace CleverTapSDK.Android {
    internal class AndroidPlatformInApps : CleverTapPlatformInApps {
        internal override void FetchInApps(int callbackId) =>
            CleverTapAndroidJNI.CleverTapJNIInstance.Call("fetchInApps", callbackId);
    }
}
#endif