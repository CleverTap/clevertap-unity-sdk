#if !UNITY_IOS && !UNITY_ANDROID 
using CleverTapUnitySDK.Common;
using UnityEngine;

namespace CleverTapUnitySDK.Native {
    public class UnityNativePlatformBinding : CleverTapPlatformBindings {
        public UnityNativePlatformBinding() {
            CallbackHandler = CreateGameObjectAndAttachCallbackHandler<UnityNativeCallbackHandler>("UnityNativeCallbackHandler");
            Debug.Log("Start: no-op CleverTap binding for non iOS/Android.");
        }
    }
}
#endif
