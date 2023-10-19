#if UNITY_ANDROID
using CleverTap.Android;
#elif UNITY_IOS
using CleverTap.IOS;
#else
using CleverTap.Native;
#endif

namespace CleverTap.Common {
    internal static class VariableFactory {
        private static CleverTapPlatformVariable cleverTapVariable;

        public static CleverTapPlatformVariable CleverTapVariable { get => cleverTapVariable; }

        static VariableFactory() {
            #if UNITY_ANDROID
            cleverTapVariable = new AndroidPlatformVariable();
            #elif UNITY_IOS
            cleverTapVariable = new IOSPlatformVariable();
            #else
            cleverTapVariable = new UnityNativePlatformVariable();
            #endif
        }
    }
}
