#if UNITY_ANDROID
using CleverTap.Android;
#elif UNITY_IOS
using CleverTap.IOS;
#else
using CleverTap.Native;
#endif

namespace CleverTap.Common {
    internal static class BindingFactory {
        
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
