#if !UNITY_IOS && !UNITY_ANDROID 
using CleverTapUnitySDK.Common;

namespace CleverTapUnitySDK.Native {
    public class UnityNativePlatformBinding : CleverTapPlatformBindings {
        public UnityNativePlatformBinding() {
            CallbackHandler = CreateGameObjectAndAttachCallbackHandler<UnityNativeCallbackHandler>();
        }
    }
}
#endif
