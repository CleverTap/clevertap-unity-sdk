#if !UNITY_IOS && !UNITY_ANDROID 
using CleverTap.Common;
using CleverTap.Utilities;

namespace CleverTap.Native {
    internal class UnityNativePlatformBinding : CleverTapPlatformBindings {
        public UnityNativePlatformBinding() {
            CallbackHandler = CreateGameObjectAndAttachCallbackHandler<UnityNativeCallbackHandler>("UnityNativeCallbackHandler");
            CleverTapLogger.Log("Start: no-op CleverTap binding for non iOS/Android.");
        }
    }
}
#endif
