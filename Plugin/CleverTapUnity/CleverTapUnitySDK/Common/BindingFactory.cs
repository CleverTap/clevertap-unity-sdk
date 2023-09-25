#if UNITY_IOS
using CleverTapUnitySDK.Android;
#elif UNITY_ANDROID
using CleverTapUnitySDK.IOS;
#else
using CleverTapUnitySDK.Native;
#endif

namespace CleverTapUnitySDK.Common {
    public static class BindingFactory {
        
        private static CleverTapPlatformBindings cleverTapBinding;

        public static CleverTapPlatformBindings CleverTapBinding { get => cleverTapBinding; }

        static BindingFactory() {
            #if UNITY_IOS
            platform = "UNITY_IOS";
            #elif UNITY_ANDROID
            cleverTapBinding = new IOSPlatformBinding();
            #else
            cleverTapBinding = new UnityNativePlatformBinding();
            #endif
        }
    }
}
