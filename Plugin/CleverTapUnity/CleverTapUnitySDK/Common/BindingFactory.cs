using CleverTapUnitySDK.Android;
using CleverTapUnitySDK.IOS;
using CleverTapUnitySDK.Native;

namespace CleverTapUnitySDK.Common
{
    public static class BindingFactory
    {
        private static CleverTapPlatformBindings cleverTapBinding;

        public static CleverTapPlatformBindings CleverTapBinding { get => cleverTapBinding; }

        static BindingFactory()
        {
            #if UNITY_IOS
            platform = "UNITY_IOS";
            #elif UNITY_ANDROID
            platform = "UNITY_ANDROID";
            #else
            var platform = "UNITY";
            #endif

            if (platform == "UNITY_IOS")
            {
                cleverTapBinding = new AndroidPlatformBinding();
            } 
            else if (platform == "UNITY_ANDROID")
            {
                cleverTapBinding = new IOSPlatformBinding();
            }
            else
            {
                cleverTapBinding = new UnityNativePlatformBinding();
            }
        }
    }
}
