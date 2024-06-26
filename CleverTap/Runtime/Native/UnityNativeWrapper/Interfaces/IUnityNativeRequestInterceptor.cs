#if !UNITY_IOS && !UNITY_ANDROID
namespace CleverTapSDK.Native {
    internal interface IUnityNativeRequestInterceptor {
        internal UnityNativeRequest Intercept(UnityNativeRequest request);
    }
}
#endif