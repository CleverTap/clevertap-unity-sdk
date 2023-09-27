#if UNITY_ANDROID
using CleverTapUnitySDK.Android;
#elif UNITY_IOS
using CleverTapUnitySDK.IOS;
#else
using CleverTapUnitySDK.Native;
#endif

namespace CleverTapUnitySDK.Common {
    public static class BindingFactory {
        
        private static CleverTapPlatformBindings cleverTapBinding;

        public static CleverTapPlatformBindings CleverTapBinding { get => cleverTapBinding; }

        static BindingFactory() {
            #if UNITY_ANDROID
            cleverTapBinding = new AndroidPlatformBinding();
            #elif UNITY_IOS
            cleverTapBinding = new IOSPlatformBinding();
            #else
            cleverTapBinding = new UnityNativePlatformBinding();
            #endif
        }
    }
}
