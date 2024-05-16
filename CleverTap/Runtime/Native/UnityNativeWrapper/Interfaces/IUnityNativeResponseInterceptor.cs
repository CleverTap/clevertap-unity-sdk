#if !UNITY_IOS && !UNITY_ANDROID
namespace CleverTapSDK.Native {
    internal interface IUnityNativeResponseInterceptor {
        internal UnityNativeResponse Intercept(UnityNativeResponse response);
    }
}
#endif