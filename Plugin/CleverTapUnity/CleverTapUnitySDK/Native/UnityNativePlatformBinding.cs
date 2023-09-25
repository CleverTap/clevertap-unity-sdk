using CleverTapUnitySDK.Common;

namespace CleverTapUnitySDK.Native
{
    public class UnityNativePlatformBinding : CleverTapPlatformBindings
    {
        public UnityNativePlatformBinding()
        {
            CallbackHandler = new UnityNativeCallbackHandler();
        }
    }
}
